using shadowBasic.BasicAPI;
using shadowBasic.Components.Text;
using shadowBasic.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using static shadowBasic.Interop.HookInterop;

namespace shadowBasic.Components.Key
{
    public class KeyComponent : Component, IDisposableEx
    {
        private const int KeyCount = 256;

        private readonly List<IKeyCollection> _collections;
        private readonly List<Tuple<IKeyCollection, MethodInfo>> _keyBindMethods;

        private bool _disposed;

        private IntPtr _keyboardHookHandle;
        private HookProcedure _keyboardHookProcedure;

        private uint _threadId;
        private Thread _keyboardHookThread;
        private volatile bool _keyboardHookRunning;

        private BitArray _keyStates;
        private BitArray _keyUsed;
        private byte[] _keyPressedCount;
        private bool _menuPressed;

        public KeyComponent(KeybinderCore core, params IKeyCollection[] collections)
            : base(core)
        {
            _collections = new List<IKeyCollection>(collections);
            _keyBindMethods = new List<Tuple<IKeyCollection, MethodInfo>>();

            _disposed = false;
            _keyboardHookHandle = IntPtr.Zero;
            _keyboardHookThread = new Thread(HookMessageProcedure);
            _keyboardHookRunning = false;
            _keyStates = new BitArray(KeyCount);
            _keyUsed = new BitArray(KeyCount);
            _keyPressedCount = new byte[KeyCount];
        }

        public override void Start()
        {
            foreach (var collection in _collections)
            {
                _keyBindMethods.AddRange(new List<MethodInfo>(collection
                    .GetType()
                    .GetMethods()
                    .Where(method => !method.IsStatic && method.CustomAttributes
                    .Where(attribute => attribute.AttributeType == typeof(KeyAttribute))
                    .Count() != 0))
                    .Select(method => new Tuple<IKeyCollection, MethodInfo>(collection, method)));
            }

            _keyboardHookRunning = true;
            _keyboardHookThread.Start();
        }

        public override void Stop()
        {
            _keyboardHookRunning = false;
            PostThreadMessage(_threadId, (int)UtilInterop.WindowMessage.WM_QUIT, IntPtr.Zero, IntPtr.Zero);
            _keyboardHookThread.Join();
        }

        protected override void InitializeAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                _keyBindMethods.AddRange(new List<MethodInfo>(type
                    .GetMethods()
                    .Where(method => method.IsStatic && method.CustomAttributes
                    .Where(attribute => attribute.AttributeType == typeof(KeyAttribute))
                    .Count() != 0))
                    .Select(method => new Tuple<IKeyCollection, MethodInfo>(null, method)));
            }
        }

        public void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (_keyboardHookHandle != IntPtr.Zero)
                Stop();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void HookMessageProcedure()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            _threadId = (uint)AppDomain.GetCurrentThreadId();
#pragma warning restore CS0618 // Type or member is obsolete

            _keyboardHookProcedure = new HookProcedure(LowLevelKeyboardProcedure);
            _keyboardHookHandle = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, _keyboardHookProcedure, UtilInterop.GetModuleHandle(null), 0);

            var message = new MSG();

            while (_keyboardHookRunning)
            {
                var result = GetMessage(out message, IntPtr.Zero, 0, 0);
                if (result == -1)
                    break;

                TranslateMessage(ref message);

                if (message.message == (int)UtilInterop.WindowMessage.WM_QUIT)
                    break;
            }

            UnhookWindowsHookEx(_keyboardHookHandle);
            _keyboardHookHandle = IntPtr.Zero;
        }

        private bool ProceedKey(UtilInterop.WindowMessage action, LowLevelKeyboardInput input)
        {
            if (action == UtilInterop.WindowMessage.WM_KEYDOWN || action == UtilInterop.WindowMessage.WM_KEYUP || action == UtilInterop.WindowMessage.WM_SYSKEYDOWN || action == UtilInterop.WindowMessage.WM_SYSKEYUP)
            {
                SyncronizeKeyStates(action, input);
                var key = (int)input.VirtualKeyCode;

                if (action == UtilInterop.WindowMessage.WM_KEYDOWN || action == UtilInterop.WindowMessage.WM_SYSKEYDOWN)
                {
                    if (!API.Instance.Chat.IsOpen() && !API.Instance.Dialog.IsOpen())
                    {
                        if (_keyStates.Get(key) && _keyPressedCount[key] < 2)
                        {
                            if (_keyPressedCount[key] != 2)
                                _keyPressedCount[key]++;
                        }
                        else if (!_keyStates.Get(key) && _keyPressedCount[key] != 0)
                            _keyPressedCount[key] = 0;
                    }
                }
                else
                {
                    if (action == UtilInterop.WindowMessage.WM_KEYUP && input.VirtualKeyCode == UtilInterop.Keys.VK_MENU && API.Instance.Chat.IsOpen())
                        _menuPressed = true;
                    else
                        _menuPressed = false;

                    if (_keyStates.Get(key) && _keyPressedCount[key] == 0)
                    {
                        if (_keyPressedCount[key] != 2)
                            _keyPressedCount[key]++;
                    }
                    else if (!_keyStates.Get(key) && _keyPressedCount[key] != 0)
                    {
                        _keyPressedCount[key] = 0;
                        _keyUsed.Set(key, false);
                    }
                }
            }
            else if (action == UtilInterop.WindowMessage.WM_CHAR && _menuPressed && API.Instance.Chat.IsOpen())
            {
                _menuPressed = false;
                return true;
            }

            return false;
        }

        private Tuple<UtilInterop.Keys, UtilInterop.Keys> GetTranslatedKey(UtilInterop.WindowMessage action, LowLevelKeyboardInput input)
        {
            var key = input.VirtualKeyCode;
            var modifierKey = UtilInterop.Keys.VK_NONE;

            if (_keyStates[(int)UtilInterop.Keys.VK_SHIFT] && key != UtilInterop.Keys.VK_SHIFT)
                modifierKey |= UtilInterop.Keys.VK_SHIFT;

            if (_keyStates[(int)UtilInterop.Keys.VK_CONTROL] && key != UtilInterop.Keys.VK_CONTROL)
                modifierKey |= UtilInterop.Keys.VK_CONTROL;

            if (_keyStates[(int)UtilInterop.Keys.VK_MENU] && key != UtilInterop.Keys.VK_MENU)
                modifierKey |= UtilInterop.Keys.VK_MENU;

            if (_keyStates[(int)UtilInterop.Keys.VK_APPS] && key != UtilInterop.Keys.VK_APPS)
                modifierKey |= UtilInterop.Keys.VK_APPS;

            if (_keyStates[(int)UtilInterop.Keys.VK_LWIN] && key != UtilInterop.Keys.VK_LWIN)
                modifierKey |= UtilInterop.Keys.VK_LWIN;

            if (_keyStates[(int)key])
            {
                if (_keyPressedCount[(int)key] < 2)
                    return new Tuple<UtilInterop.Keys, UtilInterop.Keys>(key, modifierKey);
            }

            return new Tuple<UtilInterop.Keys, UtilInterop.Keys>(UtilInterop.Keys.VK_NONE, UtilInterop.Keys.VK_NONE);
        }

        private void SyncronizeKeyStates(UtilInterop.WindowMessage action, LowLevelKeyboardInput input)
        {
            for (int i = 0; i < _keyStates.Count; i++)
                _keyStates.Set(i, (UInt16)UtilInterop.GetKeyState(i) >> 15 == 1 ? true : false);

            _keyStates.Set((int)input.VirtualKeyCode, (action == UtilInterop.WindowMessage.WM_KEYDOWN || action == UtilInterop.WindowMessage.WM_SYSKEYDOWN) ? true : false);
        }

        private bool CheckForKeybinds(UtilInterop.Keys key, UtilInterop.Keys modifierKey)
        {
            foreach (var tuple in _keyBindMethods)
            {
                var collection = tuple.Item1;
                var method = tuple.Item2;
                var keyAttributes = method.GetCustomAttributes<KeyAttribute>();
                var conditionalAttributes = method.GetCustomAttributes<ConditionalAttribute>();

                foreach (var keyAttribute in keyAttributes)
                {
                    if (keyAttribute.Key == key && keyAttribute.ModifierKey == modifierKey)
                    {
                        if (keyAttribute.IsActive(Core) && ConditionalAttribute.CanExecute(conditionalAttributes))
                        {
                            if (keyAttribute.IsAsync(method))
                                Task.Run(async () => await method.InvokeAsync(collection, null));
                            else
                                Task.Run(() => method.Invoke(collection, null));

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private IntPtr LowLevelKeyboardProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
                return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);

            if (lParam == null)
                return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);

            if (Core.IsGameActive())
            {
                bool proceedKey = false;
                bool forceProceedKey = false;
                bool disableKeyUsed = false;
                var input = Marshal.PtrToStructure<LowLevelKeyboardInput>(lParam);

                ProceedKey((UtilInterop.WindowMessage)wParam, input);
                var keys = GetTranslatedKey((UtilInterop.WindowMessage)wParam, input);
                var key = keys.Item1;
                var modifierKey = keys.Item2;

                if (!API.Instance.Chat.IsOpen() && !API.Instance.Dialog.IsOpen() && _keyUsed.Get((int)input.VirtualKeyCode))
                    return new IntPtr(1);

                if (input.VirtualKeyCode == UtilInterop.Keys.VK_RETURN && (input.Flags & (0x2000 >> 8)) != 0)
                    forceProceedKey = true;

                if (!API.Instance.Chat.IsOpen() && !API.Instance.Dialog.IsOpen() && Core.IsGameActive())
                {
                    if (input.VirtualKeyCode != UtilInterop.Keys.VK_NONE)
                    {
                        if (!API.Instance.Chat.IsOpen() && !API.Instance.Chat.IsOpen())
                            proceedKey = CheckForKeybinds(key, modifierKey);
                    }
                }


                if (input.VirtualKeyCode == UtilInterop.Keys.VK_RETURN && (((UtilInterop.WindowMessage)wParam == UtilInterop.WindowMessage.WM_KEYUP) || ((UtilInterop.WindowMessage)wParam == UtilInterop.WindowMessage.WM_SYSKEYDOWN)) && API.Instance.Chat.IsOpen())
                    proceedKey = InvokeTextComponent();

                if (forceProceedKey || proceedKey)
                {
                    if (!disableKeyUsed)
                        _keyUsed[(int)key] = true;

                    return new IntPtr(1);
                }
            }

            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private bool InvokeTextComponent()
        {
            var textComponent = Core.GetComponent<TextComponent>();
            if (textComponent == null)
                return false;

            var text = API.Instance.Chat.GetText();

            if (textComponent.CheckCall(text))
            {
                API.Instance.Chat.Clear();

                if (text.Length <= 128)
                    API.Instance.Chat.AddBufferMessage(text);
                else
                    API.Instance.Chat.AddBufferMessage(text.Substring(0, 127));

                API.Instance.Chat.Toggle(false);
                return true;
            }

            return false;
        }
    }
}
