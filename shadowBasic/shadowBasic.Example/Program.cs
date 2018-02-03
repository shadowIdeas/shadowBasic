using shadowBasic.BasicAPI;
using shadowBasic.BasicAPI.SAPI;
using shadowBasic.Components.Chat;
using shadowBasic.Components.Key;
using shadowBasic.Components.Overlay;
using shadowBasic.Components.Overlay.Items;
using shadowBasic.Components.Settings;
using shadowBasic.Components.Text;
using System;
using System.Diagnostics;
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
                core.AddComponent(new OverlayComponent(core, true));
                core.AddComponent(new SettingComponent(core, new DefaultProvider()));
                core.Start();

                var testOverlay = core.GetComponent<OverlayComponent>().GetOverlay<TestOverlay>();
                testOverlay.Overlay.Active = true;
                testOverlay.Overlay.Color = 0xFFFFFFFF;
                testOverlay.Overlay.Size = 10;
                testOverlay.Overlay.UseMaxWidth = true;
                testOverlay.Overlay.UseMaxHeight = true;
                testOverlay.Overlay.MaxWidth = 100;
                testOverlay.Overlay.MaxHeight = 100;
                testOverlay.Overlay.X = 200;
                testOverlay.Overlay.Y = 400;
                testOverlay.Overlay.ExampleValue = "This get displayed when the value of this text is empty and edit mode is activated.";

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

        [Text("/toggleedit")]
        [Text("/togedit")]
        public void ToggleEditCommand(string[] parameters)
        {
            var overlayComponent = _core.GetComponent<OverlayComponent>();
            overlayComponent.EditActive = !overlayComponent.EditActive;
        }
    }

    class TestOverlay : OverlayItemText
    {
        public TestOverlay(KeybinderCore core)
            : base(core, "Test")
        {
            
        }

        public override void Update()
        {
            var now = DateTimeOffset.Now;
            Overlay.Value = now.ToString("HH:mm:ss");
        }
    }
}
