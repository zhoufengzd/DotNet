using System;
using System.Collections.Generic;

namespace Zen.LaunchPad
{
    using Macros = Zen.Utilities.Generics.Macros;

    public class CmdInfo
    {
        public CmdInfo()
        {
        }

        public CmdInfo(string command, string parameters)
        {
            _command = command;
            _parameters = parameters;
        }

        public string Command
        {
            get { return _command; }
            set { _command = value; }
        }

        public string Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        #region private data
        private string _command = null;
        private string _parameters = null;
        #endregion
    }

    [Serializable]
    public sealed class CommandList
    {
        public List<CmdInfo> Commands
        {
            get { return Macros.SafeGet(ref _cmdList); }
            set { _cmdList = value; }
        }

        public Dictionary<string, string> ShellSettings
        {
            get { return Macros.SafeGet(ref _shellSettings); }
            set { _shellSettings = value; }
        }

        #region private data
        private List<CmdInfo> _cmdList;
        private Dictionary<string, string> _shellSettings;
        #endregion
    }
}
