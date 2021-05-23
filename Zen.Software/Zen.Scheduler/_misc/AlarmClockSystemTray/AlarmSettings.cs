using System;
using System.Collections.Generic;
using System.Text;

namespace AlarmClockSystemTray
{
    public sealed class AlarmSettings
    {
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }
        public string SoundFile
        {
            get { return _soundFile; }
            set { _soundFile = value; }
        }
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        private DateTime _time;
        private string _soundFile;
        private string _message;
        private string _action;
        private bool _enabled;
    }
}
