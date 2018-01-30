using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace shadowBasic.Interop
{
    internal static class HookInterop
    {
        public struct Point
        {
            public int X;
            public int Y;
        }

        public struct LowLevelKeyboardInput
        {
            public UtilInterop.Keys VirtualKeyCode;
            public UInt32 ScanCode;
            public UInt32 Flags;
            public UInt32 Time;
            public UIntPtr ExtraInfo;
        }

        public struct LowLevelMouseInput
        {
            public Point Point;
            public UInt32 Data;
            public UInt32 Flags;
            public UInt32 Time;
            public UIntPtr ExtraInfo;
        }

        public enum HookType : int
        {
            WH_CALLWNDPROC = 4,
            WH_CALLWNDPROCRET = 12,
            WH_CBT = 5,
            WH_DEBUG = 6,
            WH_FOREGROUNDIDLE = 11,
            WH_GETMESSAGE = 3,
            WH_JOURNALPLAYBACK = 1,
            WH_JOURNALRECORD = 0,
            WH_KEYBOARD = 2,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE = 7,
            WH_MOUSE_LL = 14,
            WH_MSGFILTER = -1,
            WH_SHELL = 10,
            WH_SYSMSGFILTER = 6
        }

        public delegate IntPtr HookProcedure(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern IntPtr SetWindowsHookEx(HookType hookType, HookProcedure hookProcedure, IntPtr moduleHandle, UInt32 threadID);

        [DllImport("User32.dll")]
        public static extern IntPtr UnhookWindowsHookEx(IntPtr hook);

        [DllImport("User32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hookHandle, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetMessage(out MSG lpMsg, IntPtr handleWindow, uint filterMin, uint filterMax);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostThreadMessage(uint threadID, uint message, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage(ref MSG message);

        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage(ref MSG message);
    }
}
