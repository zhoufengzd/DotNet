using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zen.Scheduler
{
    /// <summary>
    /// Task: from user's point of view
    /// </summary>
    public sealed class Task
    {
        private string _exeCommand;
        private string _parameters;
    }

    public sealed class ScheduleItem
    {
        private DateTime _startTime;
        private DateTime _stopTime;
        private TimeSpan _repeatTimeSpan;
    }

    public sealed class ScheduledTask
    {
        private ScheduleItem _scheduleInfo;
        private Task _task;
    }

    public sealed class ScheduledTaskLog
    {
        private ScheduledTask _taskInfo;
        private int _result;
        private string _message;
    }
}
