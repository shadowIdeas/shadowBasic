using System;
using System.Reflection;

namespace shadowBasic.Components
{
    public abstract class Component
    {
        private readonly KeybinderCore _core;
        private bool _isInitialized;

        protected KeybinderCore Core
        {
            get { return _core; }
        }

        public bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public Component(KeybinderCore core)
        {
            _core = core;
            _isInitialized = false;
        }

        public void Initialize(Assembly assembly)
        {
            _isInitialized = true;
            InitializeAssembly(assembly);
        }

        public virtual void ProcessStarted()
        {

        }

        public virtual void ProcessStopped()
        {

        }

        public abstract void Start();
        public abstract void Stop();

        protected abstract void InitializeAssembly(Assembly assembly);
    }
}
