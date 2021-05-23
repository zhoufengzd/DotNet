using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Zen.Utilities.Proc
{
    /// <summary>
    /// Help to execute external process
    /// 1.	Auto build full path for executable (%Path% + %AssemblyDir% + %DotNetFrameWorkDir%)
    /// 2.	Hide output window by default
    /// </summary>
    public sealed class Executor
    {
        public Executor()
        {
        }

        // Kick off the process then forget about it
        public bool RunProcess(string cmd, string cmdArguments)
        {
            return RunProcess(cmd, cmdArguments, false, false);
        }
        public bool RunProcess(string cmd, string cmdArguments, bool waitForExit, bool hideWindow)
        {
            LoadControllers();
            _output = null;
            _error = null;

            string cmdFullPath = FileUtil.PathBuilder.BuildFullPath(cmd, _defaultPathList);
            if (cmdFullPath == null)
                return false;

            _startInfo.FileName = cmdFullPath;
            _startInfo.Arguments = cmdArguments;
            if (hideWindow)
                _startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process proc = Process.Start(_startInfo);
            _output = proc.StandardOutput.ReadToEnd();
            _error = proc.StandardError.ReadToEnd();

            if (waitForExit)
            {
                proc.WaitForExit();
                return (proc.ExitCode > -1);
            }
            else
            {
                return true;
            }
        }

        public bool OpenFile(string filePath)
        {
            if (!Zen.Utilities.FileUtil.FileValidator.IsValid(filePath, System.IO.FileAccess.Read))
                return false;

            ProcessStartInfo startInfo = new ProcessStartInfo(filePath);
            startInfo.UseShellExecute = true;
            Process.Start(startInfo);

            return true;
        }

        public string Output
        {
            get { return _output; }
        }
        public string Error
        {
            get { return _error; }
        }

        #region Optional exposed accessors.
        /// <summary> ProcessStartInfo. User could configure process start info here.</summary>
        public ProcessStartInfo ProcessStartInfo
        {
            get { LoadControllers(); return _startInfo; }
        }

        public string WorkingDir
        {
            get { LoadControllers(); return _env.WorkingDir; }
        }

        /// <summary>
        /// Exetended executable path: %WorkingDir% + %Path% + %CLRRunTime%
        /// </summary>
        public List<string> ExePathList
        {
            get { LoadControllers(); return _defaultPathList; }
        }
        #endregion

        #region private functions
        private void LoadControllers()
        {
            if (_startInfo != null)
                return;

            _env = new ProcEnvironInfo();

            _defaultPathList = new List<string>();
            _defaultPathList.Add(_env.WorkingDir);
            _defaultPathList.AddRange(_env.SysDir.ExePathList);
            _defaultPathList.Add(_env.SysDir.CLRRunTimeDir);

            _startInfo = new ProcessStartInfo();
            _startInfo.UseShellExecute = false;
            _startInfo.WorkingDirectory = _env.WorkingDir;
            _startInfo.RedirectStandardOutput = true;
            _startInfo.RedirectStandardError = true;
        }
        #endregion

        #region private data
        private ProcEnvironInfo _env;
        private List<string> _defaultPathList;
        private ProcessStartInfo _startInfo;
        private string _output;
        private string _error;
        #endregion
    }
}
