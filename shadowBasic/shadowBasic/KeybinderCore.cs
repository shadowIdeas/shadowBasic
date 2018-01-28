using shadowBasic.Components;
using System;
using System.Collections.Generic;

namespace shadowBasic
{
    public class KeybinderCore
    {
        private bool _started;
        private List<Component> _components;

        /// <summary>
        /// Initalize the core.
        /// </summary>
        public KeybinderCore()
        {
            _started = false;
            _components = new List<Component>();
        }

        /// <summary>
        /// Start the core.
        /// </summary>
        public void Start()
        {
            _started = true;

            SearchAssemblies();
        }

        /// <summary>
        /// Stop the core.
        /// </summary>
        public void Stop()
        {
            _started = false;
        }

        /// <summary>
        /// Add an component to the core.
        /// If an component is already registered a <see cref="ArgumentException"/> get thrown.
        /// </summary>
        /// <param name="component">The component to be added.</param>
        public void AddComponent(Component component)
        {
            if (_components.Exists(c => c == component))
                throw new ArgumentException("Component is already registered.", nameof(component));

            _components.Add(component);
        }

        private void SearchAssemblies()
        {

        }
    }
}
