using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Zen.Utilities.Proc
{
    using Assembly = System.Reflection.Assembly;
    using Module = System.Reflection.Module;

    public sealed class SystemDirectories
    {
        static readonly char[] PathSeparator = { ';', };

        public SystemDirectories()
        {
            _winDir = System.Environment.GetEnvironmentVariable("windir");
            _system32 = Path.Combine(_winDir, "system32");
            _clrRunTime = RuntimeEnvironment.GetRuntimeDirectory();

            string paths = System.Environment.GetEnvironmentVariable("path");
            _exePathList = new List<string>(paths.Split(PathSeparator, StringSplitOptions.RemoveEmptyEntries));
        }

        public string System32
        {
            get { return _system32; }
        }
        public string WinDir
        {
            get { return _winDir; }
        }
        public string CLRRunTimeDir
        {
            get { return _clrRunTime; }
        }
        public List<string> ExePathList
        {
            get { return _exePathList; }
        }

        #region private data
        private string _winDir;
        private string _system32;
        private string _clrRunTime;
        private List<string> _exePathList;
        #endregion
    }

    public sealed class ProcEnvironInfo
    {
        public ProcEnvironInfo()
        {
            _sysDir = new SystemDirectories();

            Module md = Assembly.GetCallingAssembly().ManifestModule;
            _workingDir = Path.GetDirectoryName(md.FullyQualifiedName);
            _exeName = md.Name;
        }

        public SystemDirectories SysDir
        {
            get { return _sysDir; }
        }
        public string WorkingDir
        {
            get { return _workingDir; }
        }

        #region private data
        private SystemDirectories _sysDir;
        private string _workingDir;
        private string _exeName;
        #endregion
    }
}
