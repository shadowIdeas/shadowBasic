using shadowBasic.Interop;
using System;

namespace shadowBasic
{
    public static class Util
    {
        public static bool IsGameActive(this KeybinderCore core)
        {
            return core.GameRunning && !IsDesktopActive();
        }

        public static bool IsDesktopActive()
        {
            var gta = UtilInterop.FindWindow(null, "GTA:SA:MP");
            if (gta != IntPtr.Zero)
                return gta != UtilInterop.GetForegroundWindow();

            return false;
        }
    }
}
