using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowBasic.Components.Settings
{
    public class DefaultProvider : ISettingsProvider
    {
        public async Task<Dictionary<Tuple<Type, string>, object>> LoadAsync()
        {
            var settings = new Dictionary<Tuple<Type, string>, object>();
            if (!File.Exists("Settings.dat"))
                return settings;

            using (var reader = new StreamReader("Settings.dat"))
            {
                var content = await reader.ReadToEndAsync();
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                foreach (var item in dictionary)
                {
                    var tuple = JsonConvert.DeserializeObject<Tuple<Type, string>>(item.Key);
                    settings.Add(tuple, item.Value);
                }
            }

            return settings;
        }

        public async Task SaveAsync(Dictionary<Tuple<Type, string>, object> settings)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (var item in settings)
            {
                var key = JsonConvert.SerializeObject(item.Key);
                var value = JsonConvert.SerializeObject(item.Value);
                dictionary.Add(key, value);
            }

            using (var writer = new StreamWriter(File.OpenWrite("Settings.dat")))
                await writer.WriteAsync(JsonConvert.SerializeObject(dictionary));
        }
    }
}
