using shadowBasic.BasicAPI;
using shadowBasic.Components;
using shadowBasic.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using shadowBasic.Components.Settings;

namespace shadowBasic
{
    public class KeybinderCore : IDisposableEx
    {
        private bool _disposed;

        private bool _started;
        private bool _stopped;
        private bool _paused;
        private bool _gameRunning;
        private readonly List<Component> _components;

        private Task _moduleWaitTask;
        private volatile bool _moduleWaitRunning;

        private readonly ProcessWatcher _processWatcher;

        public bool Paused
        {
            get { return _paused; }
            set { _paused = value; }
        }

        public bool GameRunning
        {
            get { return _gameRunning; }
        }

        /// <summary>
        /// Initalize the core.
        /// </summary>
        public KeybinderCore(string executeableName, API api)
        {
            _disposed = false;

            _started = false;
            _stopped = false;
            _paused = false;
            _gameRunning = false;
            _components = new List<Component>();

            _moduleWaitRunning = false;

            API.Instance = api;

            _processWatcher = new ProcessWatcher(executeableName);
            _processWatcher.ProcessStarted += ProcessStarted;
            _processWatcher.ProcessStopped += ProcessStopped;
        }

        ~KeybinderCore()
        {
            Dispose(false);
        }

        public void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            Stop();
            DisposeComponents();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Start the core.
        /// </summary>
        public void Start()
        {
            if (_started)
                return;

            if (_stopped)
                throw new InvalidOperationException("The core cannot be restarted again. Please create a new instance.");

            _started = true;

            SearchAssemblies();
            
            foreach (var component in _components.OrderByDescending(c => c is SettingComponent))
                component.Start();
            _processWatcher.Start();
        }

        /// <summary>
        /// Stop the core.
        /// </summary>
        public void Stop()
        {
            if (_stopped)
                return;

            _started = false;
            _stopped = true;

            foreach (var component in _components.OrderBy(c => c is SettingComponent))
                component.Stop();

            _processWatcher.Stop();
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

        public T GetComponent<T>() where T : Component
        {
            return (T)_components.Find(c => c.GetType() == typeof(T));
        }

        private void ProcessStarted(object sender, EventArgs e)
        {
            if(!_moduleWaitRunning && (_moduleWaitTask == null || _moduleWaitTask.IsCanceled || _moduleWaitTask.IsFaulted || _moduleWaitTask.IsCompleted))
            {
                _moduleWaitRunning = true;
                _moduleWaitTask = new Task(ModuleWaitProcedure, TaskCreationOptions.LongRunning);
                _moduleWaitTask.Start();
            }
        }

        private void ProcessStopped(object sender, EventArgs e)
        {
            foreach (var component in _components)
                component.ProcessStopped();

            API.Instance.Uninitialize();
            _gameRunning = false;
        }

        private void InitializeAPI()
        {
            API.Instance.Initialize();
        }

        private void SearchAssemblies()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var component in _components)
                    component.Initialize(assembly);
            }
        }

        private void DisposeComponents()
        {
            foreach (var component in _components.OrderBy(c => c is SettingComponent))
            {
                if (component is IDisposable obj)
                    obj.Dispose();
            }
        }

        private void ModuleWaitProcedure()
        {
            while(_moduleWaitRunning)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(50.0));

                if (_processWatcher.WatchedProcess != null)
                {
                    try
                    {
                        var gotModule = false;

                        var moduleCollection = _processWatcher.WatchedProcess.Modules;
                        foreach (ProcessModule item in moduleCollection)
                        {
                            if (item.ModuleName == "samp.dll" && UtilInterop.FindWindow(null, "GTA:SA:MP") != IntPtr.Zero)
                            {
                                InitializeAPI();
                                foreach (var component in _components)
                                    component.ProcessStarted();

                                _moduleWaitRunning = false;
                                _gameRunning = true;
                                gotModule = true;
                                break;
                            }
                        }

                        if (!gotModule)
                        {
                            _processWatcher.WatchedProcess.Refresh();
                            Thread.Sleep(500);
                        }
                    }
                    catch (System.ComponentModel.Win32Exception)
                    {
                    }

                }
                else
                {
                    _moduleWaitRunning = false;
                }
            }
        }
    }
}
