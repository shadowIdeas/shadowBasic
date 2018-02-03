using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowBasic.Components.Settings
{
    internal class DefaultProvider : ISettingsProvider
    {
        public async Task<Dictionary<Tuple<Type, string>, object>> LoadAsync()
        {
            var settings = new Dictionary<Tuple<Type, string>, object>();
            if (!File.Exists("Settings.dat"))
                return settings;

            using (var reader = new StreamReader("Settings.dat"))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                    ProcessSettingLine(line);
            }

            return settings;
        }

        public async Task SaveAsync(Dictionary<Tuple<Type, string>, object> settings)
        {

        }
    }
}
