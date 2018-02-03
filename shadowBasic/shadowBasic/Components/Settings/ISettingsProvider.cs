using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadowBasic.Components.Settings
{
    public interface ISettingsProvider
    {
        Task<Dictionary<Tuple<Type, string>, object>> LoadAsync();
        Task SaveAsync(Dictionary<Tuple<Type, string>, object> settings);
    }
}
