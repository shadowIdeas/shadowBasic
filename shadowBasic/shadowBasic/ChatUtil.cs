using shadowBasic.BasicAPI;

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
            API.Instance.Chat.AddMessage("{" + InformationColor + "}" + message);
        }

        public static void ShowWarning(string message)
        {
            API.Instance.Chat.AddMessage("{" + WarningColor + "}" + message);
        }

        public static void ShowError(string message)
        {
            API.Instance.Chat.AddMessage("{" + ErrorColor + "}" + message);
        }

        public static void ShowUsage(string message)
        {
            API.Instance.Chat.AddMessage("{" + UsageColor + "}" + message);
        }
    }
}
