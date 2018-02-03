using shadowBasic.Components.Overlay.Items;
using shadowBasic.Interop;
using System;
using System.Threading;
using System.Windows.Interop;
using static shadowBasic.Interop.HookInterop;

namespace shadowBasic.Components.Overlay
{
    internal class MouseHook
    {
        private readonly KeybinderCore _core;
        private readonly OverlayComponent _overlayComponent;

        private HookProcedure _mouseHookProcedure;
        private IntPtr _mouseHookHandle;

        private volatile bool _running;
        private Thread _messageThread;
        private uint _threadId;

        private bool _buttonDown;
        private Point _startPoint;
        private Point _overlayPoint;
        private OverlayEditState _editState;
        private Natives.Text _selectedOverlayText;

        public MouseHook(KeybinderCore core)
        {
            _core = core;
            _overlayComponent = core.GetComponent<OverlayComponent>();
            _mouseHookHandle = IntPtr.Zero;
            _buttonDown = false;
            _startPoint = new Point() { X = 0, Y = 0 };
            _overlayPoint = new Point() { X = 0, Y = 0 };
            _editState = OverlayEditState.None;
            _selectedOverlayText = null;
            _running = false;
            _threadId = 0;
        }

        public void Start()
        {
            _mouseHookProcedure = new HookProcedure(LowLevelMouseProcedure);

            _running = true;
            _messageThread = new Thread(MessageThreadRoutine);
            _messageThread.Start();
        }

        public void Stop()
        {
            _running = false;
            PostThreadMessage(_threadId, (int)UtilInterop.WindowMessage.WM_QUIT, IntPtr.Zero, IntPtr.Zero);
            _messageThread?.Join();

            _mouseHookHandle = IntPtr.Zero;
        }

        private void MessageThreadRoutine()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            _threadId = (uint)AppDomain.GetCurrentThreadId();
#pragma warning restore CS0618 // Type or member is obsolete
            _mouseHookHandle = SetWindowsHookEx(HookType.WH_MOUSE_LL, _mouseHookProcedure, Interop.UtilInterop.GetModuleHandle(null), 0);
            var message = new MSG();
            while (_running)
            {
                var result = GetMessage(out message, IntPtr.Zero, 0, 0);
                if (result == -1)
                    break;

                TranslateMessage(ref message);

                if (message.message == (int)UtilInterop.WindowMessage.WM_QUIT)
                    break;
            }

            UnhookWindowsHookEx(_mouseHookHandle);
        }

        private IntPtr LowLevelMouseProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
                return Interop.HookInterop.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);

            if (lParam != null && _overlayComponent.EditActive && _core.IsGameActive())
            {
                var action = (Interop.UtilInterop.WindowMessage)wParam;
                var input = System.Runtime.InteropServices.Marshal.PtrToStructure<LowLevelMouseInput>(lParam);

                if (action == Interop.UtilInterop.WindowMessage.WM_LBUTTONDOWN)
                {
                    MouseButtonDown(input);
                    return new IntPtr(1);
                }
                else if (action == Interop.UtilInterop.WindowMessage.WM_LBUTTONUP)
                {
                    MouseButtonUp(input);
                    return new IntPtr(1);
                }
                else if (action == Interop.UtilInterop.WindowMessage.WM_MOUSEMOVE)
                {
                    MouseMove(input);
                }
            }

            return Interop.HookInterop.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private void MouseButtonDown(LowLevelMouseInput input)
        {
            Point mousePosition = input.Point;

            if (!_buttonDown)
            {
                _buttonDown = true;

                foreach (var item in _overlayComponent.Overlays)
                {
                    if (item.GetType().IsSubclassOf(typeof(OverlayItemText)))
                    {
                        var textOverlay = (item as OverlayItemText).Overlay;
                        if (textOverlay.IsValidID && textOverlay.Active)
                        {
                            var useMaxWidth = textOverlay.UseMaxWidth;
                            var useMaxHeight = textOverlay.UseMaxHeight;
                            var x = textOverlay.X;
                            var y = textOverlay.Y;
                            var width = useMaxWidth ? textOverlay.MaxWidth : textOverlay.Width;
                            var height = useMaxHeight ? textOverlay.MaxHeight : textOverlay.Height;

                            if (mousePosition.X >= x && mousePosition.X <= (x + width) && mousePosition.Y >= y && mousePosition.Y <= (y + height))
                            {
                                _selectedOverlayText = textOverlay;
                                _startPoint = mousePosition;

                                if ((useMaxWidth || useMaxHeight) && mousePosition.X >= (x + width - 10) && mousePosition.X <= (x + width) && mousePosition.Y >= (y + height - 10) && mousePosition.Y <= (y + height))
                                {
                                    _overlayPoint.X = width;
                                    _overlayPoint.Y = height;

                                    _editState = OverlayEditState.Size;
                                }
                                else
                                {
                                    _overlayPoint.X = _selectedOverlayText.X;
                                    _overlayPoint.Y = _selectedOverlayText.Y;
                                    _editState = OverlayEditState.Position;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void MouseButtonUp(LowLevelMouseInput input)
        {
            if (_buttonDown)
            {
                _buttonDown = false;
                _startPoint = new Point() { X = 0, Y = 0 };
                _overlayPoint = new Point() { X = 0, Y = 0 };
                _editState = OverlayEditState.None;
                _selectedOverlayText = null;
            }
        }

        private void MouseMove(LowLevelMouseInput input)
        {
            if (_buttonDown)
            {
                Point mousePosition = input.Point;

                Point delta = new Point() { X = 0, Y = 0 };
                delta.X = mousePosition.X - _startPoint.X;
                delta.Y = mousePosition.Y - _startPoint.Y;

                if (_selectedOverlayText != null)
                {
                    if (_editState == OverlayEditState.Position)
                    {
                        _selectedOverlayText.X = _overlayPoint.X + delta.X;
                        _selectedOverlayText.Y = _overlayPoint.Y + delta.Y;
                    }
                    else if (_editState == OverlayEditState.Size)
                    {
                        if (_selectedOverlayText.UseMaxWidth)
                            _selectedOverlayText.MaxWidth = _overlayPoint.X + delta.X;
                        if (_selectedOverlayText.UseMaxHeight)
                            _selectedOverlayText.MaxHeight = _overlayPoint.Y + delta.Y;
                    }
                }
            }
        }
    }
}
