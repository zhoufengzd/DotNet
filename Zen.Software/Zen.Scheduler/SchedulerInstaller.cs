using System.ComponentModel;
using Zen.Utilities.Service;

namespace Zen.Scheduler
{
    [RunInstaller(true)]
    public partial class SchedulerInstaller : SvcInstallerBase
    {
        public static readonly string ServiceName = "Zen Scheduler";
        public static readonly string DisplayName = "Zen Schedule Service";
        public static readonly string Description = "Scheduler based services. ";

        public SchedulerInstaller()
            :base(ServiceName)
        {
            SvcInstallerSettings svcSettings = base.SvcInstallerSettings;
            svcSettings.DisplayName = DisplayName;
            svcSettings.Description = Description;
        }
    }
}
