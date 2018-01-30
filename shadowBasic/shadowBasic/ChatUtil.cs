using SAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowBasic
{
    public static class ChatUtil
    {
        public static string InformationColor { get; set; } = "888888";
        public static string WarningColor { get; set; } = "3366CC";
        public static string ErrorColor { get; set; } = "F05424";
        public static string UsageColor { get; set; } = "777777";

        public static void ShowInformation(string message)
        {
            ChatAPI.Instance.AddMessage("{" + InformationColor + "}" + message);
        }

        public static void ShowWarning(string message)
        {
            ChatAPI.Instance.AddMessage("{" + WarningColor + "}" + message);
        }

        public static void ShowError(string message)
        {
            ChatAPI.Instance.AddMessage("{" + ErrorColor + "}" + message);
        }

        public static void ShowUsage(string message)
        {
            ChatAPI.Instance.AddMessage("{" + UsageColor + "}" + message);
        }
    }
}
