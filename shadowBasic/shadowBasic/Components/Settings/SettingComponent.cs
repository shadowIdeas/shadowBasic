using System;
using System.Collections.Generic;
using System.Reflection;

namespace shadowBasic.Components.Settings
{
    public class SettingComponent : Component
    {
        private readonly ISettingsProvider _provider;

        private Dictionary<Tuple<Type, string>, object> _permanentSettings;
        private Dictionary<Tuple<Type, string>, object> _temporarySettings;

        public SettingComponent(KeybinderCore core, ISettingsProvider provider) 
            : base(core)
        {
            _provider = provider;

            _permanentSettings = new Dictionary<Tuple<Type, string>, object>();
            _temporarySettings = new Dictionary<Tuple<Type, string>, object>();
        }

        public override void Start()
        {

        }

        public override void Stop()
        {

        }

        protected override void InitializeAssembly(Assembly assembly)
        {
        }

        public virtual void LoadSettings()
        {

        }

        public virtual T GetPermanentSetting<T>(string name)
        {
            var tuple = new Tuple<Type, string>(typeof(T), name);
            if (_permanentSettings.ContainsKey(tuple))
            {
                var value = _permanentSettings[tuple];
                if (value is long)
                    return (T)Convert.ChangeType(value, typeof(T));
                else
                    return (T)value;
            }
            return default(T);
        }

        public virtual T GetTemporarySetting<T>(string name)
        {
            var tuple = new Tuple<Type, string>(typeof(T), name);
            if (_temporarySettings.ContainsKey(tuple))
                return (T)_temporarySettings[tuple];

            return default(T);
        }

        public virtual void SetPermanentSetting<T>(string name, T obj)
        {
            var tuple = new Tuple<Type, string>(typeof(T), name);
            _permanentSettings[tuple] = obj;
        }

        public virtual void SetTemporarySetting<T>(string name, T obj)
        {
            var tuple = new Tuple<Type, string>(typeof(T), name);
            _temporarySettings[tuple] = obj;
        }
    }
}
