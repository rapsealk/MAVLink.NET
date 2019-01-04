using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Management;

namespace MAVLink.NET
{
    public sealed class WMI : IDisposable
    {
        //private ManagementEventWatcher _watcher;
        private TaskScheduler _taskScheduler;

        public ObservableCollection<string> ComPorts { get; private set; }

        public WMI()
        {
            _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            
        }

        public static void WatchSerialPorts()
        {
            // TODO("Not implemented")

        }

        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion
    }
}
