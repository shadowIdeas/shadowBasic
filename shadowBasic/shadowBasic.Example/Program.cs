using shadowBasic.BasicAPI;
using shadowBasic.BasicAPI.SAPI;
using shadowBasic.Components.Chat;
using shadowBasic.Components.Key;
using shadowBasic.Components.Settings;
using shadowBasic.Components.Text;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static shadowBasic.Interop.UtilInterop;

namespace shadowBasic.Example
{
    class Program
    {
        private static Random _random = new Random();

        static void Main(string[] args)
        {
            using (KeybinderCore core = new KeybinderCore("rgn_ac_gta", new SAPI()))
            {
                var testCollection = new TestCollection(core);

                core.AddComponent(new ChatComponent(core, testCollection));
                core.AddComponent(new KeyComponent(core, testCollection));
                core.AddComponent(new TextComponent(core, testCollection));
                core.AddComponent(new SettingComponent(core, new DefaultProvider()));
                core.Start();

                Console.ReadLine();
            }
        }

        [Chat(".*")]
        public static async Task ChatCommandAsync(string[] parameters)
        {
            await Task.Delay(_random.Next(0, 100));
            Debug.WriteLine($"async: {parameters[0]}");
        }
    }

    class TestCollection : IChatCollection, IKeyCollection, ITextCollection
    {
        private KeybinderCore _core;

        public TestCollection(KeybinderCore core)
        {
            _core = core;
        }

        [Chat(".*")]
        public void ChatCommand(string[] parameters)
        {
            _core.GetComponent<SettingComponent>().SetPermanentSetting("LastChatMessage", parameters[0]);
        }

        [Key(Keys.VK_I)]
        public void KeyCommand()
        {
            var lastChatMessage = _core.GetComponent<SettingComponent>().GetPermanentSetting<string>("LastChatMessage");
            API.Instance.Chat.AddMessage($"Last Message was \"{lastChatMessage}\"");
        }

        [Text("/hello")]
        public void TextCommand(string[] parameters)
        {
            Debug.WriteLine(String.Join(" ", parameters));
        }
    }
}
