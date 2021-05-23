using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Zen.Utilities.Service;

namespace Zen.Scheduler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            SvcHost host = new SvcHost(SchedulerInstaller.ServiceName, new TaskDispatcher());
            ServiceBase.Run(host);
        }
    }
}
