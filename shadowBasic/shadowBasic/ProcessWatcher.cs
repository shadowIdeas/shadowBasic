using System;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace shadowBasic
{
    internal class ProcessWatcher
    {
        private readonly string _watchedProcessName;

        private Process _watchedProcess;
        private ManagementEventWatcher _managementEventWatcher;

        public delegate void ProcessStartedEventHandler(object sender, EventArgs e);
        public delegate void ProcessStoppedEventHandler(object sender, EventArgs e);

        public event ProcessStartedEventHandler ProcessStarted;
        public event ProcessStartedEventHandler ProcessStopped;

        public string WatchedProcessName
        {
            get { return _watchedProcessName; }
        }

        public Process WatchedProcess
        {
            get { return _watchedProcess; }
        }

        public ProcessWatcher(string processName)
        {
            _watchedProcessName = processName;
            InitializeWatcher();
        }

        public void Start()
        {
            FindProcess();
            _managementEventWatcher.Start();
        }

        public void Stop()
        {
            _managementEventWatcher.Stop();
        }

        private void InitializeWatcher()
        {
            var scope = @"\\.\root\CIMV2";
            var queryString =
            "SELECT TargetInstance" +
            "  FROM __InstanceCreationEvent " +
            "WITHIN  10 " +
            " WHERE TargetInstance ISA 'Win32_Process' " +
            "   AND TargetInstance.Name = '" + _watchedProcessName + ".exe'";

            _managementEventWatcher = new ManagementEventWatcher(scope, queryString);
            _managementEventWatcher.EventArrived += WatchedProcessStarted;
        }

        private void FindProcess()
        {
            if (RefreshProcess())
                ProcessStarted?.Invoke(this, new EventArgs());
        }

        private bool RefreshProcess()
        {
            _watchedProcess = Process.GetProcesses().FirstOrDefault(i => i.ProcessName == _watchedProcessName);
            if (_watchedProcess != default(Process))
            {
                _watchedProcess.Exited += WatchedProcessStopped;
                _watchedProcess.EnableRaisingEvents = true;
                return true;
            }

            return false;
        }

        private void WatchedProcessStarted(object sender, EventArrivedEventArgs e)
        {
            FindProcess();
        }

        private void WatchedProcessStopped(object sender, EventArgs e)
        {
            _watchedProcess = null;
            ProcessStopped?.Invoke(this, new EventArgs());
        }
    }
}
