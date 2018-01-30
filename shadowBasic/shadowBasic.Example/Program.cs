using shadowBasic.Components.Chat;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace shadowBasic.Example
{
    class Program
    {
        private static Random _random = new Random();

        static void Main(string[] args)
        {
            using (KeybinderCore core = new KeybinderCore("rgn_ac_gta"))
            {
                core.AddComponent(new ChatComponent(core));
                core.Start();

                while(true)
                    Thread.Sleep(50);
            }
        }

        [Chat(".*")]
        public static async Task ChatCommandAsync(string[] parameters)
        {
            await Task.Delay(_random.Next(0, 100));
            Debug.WriteLine(parameters[0]);
        }
    }
}
