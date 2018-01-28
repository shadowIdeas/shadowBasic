using System.Reflection;

namespace shadowBasic.Components
{
    public abstract class Component
    {
        private readonly KeybinderCore _core;

        protected KeybinderCore Core
        {
            get { return _core; }
        }

        public Component(KeybinderCore core)
        {
            _core = core;
        }

        public abstract void InitializeAssembly(Assembly assembly);
    }
}
