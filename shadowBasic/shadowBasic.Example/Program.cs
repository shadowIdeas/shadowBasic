using shadowBasic.Components.Chat;
using shadowBasic.Components.Key;
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
            using (KeybinderCore core = new KeybinderCore("rgn_ac_gta"))
            {
                var testCollection = new TestCollection();

                core.AddComponent(new ChatComponent(core, testCollection));
                core.AddComponent(new KeyComponent(core, testCollection));
                core.AddComponent(new TextComponent(core, testCollection));
                core.Start();

                while(true)
                    Thread.Sleep(50);
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
        [Chat(".*")]
        public void ChatCommand(string[] parameters)
        {
            Debug.WriteLine(parameters[0]);
        }

        [Key(Keys.VK_I)]
        public void KeyCommand()
        {
            Debug.WriteLine("I was pressed!");
        }

        [Text("/hello")]
        public void TextCommand(string[] parameters)
        {
            Debug.WriteLine(String.Join(" ", parameters));
        }
    }
}
