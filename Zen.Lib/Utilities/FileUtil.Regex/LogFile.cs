using System;
using System.Collections.Generic;
using System.IO;
using Zen.Utilities.Text;

namespace Zen.Utilities.FileUtil
{
    /// <summary>
    /// Logging utilities
    /// </summary>
    public sealed class LogFile : IDisposable
    {
        public LogFile(string filePath)
        {
            Open(filePath);
        }
        ~LogFile()
        {
            Close();
        }

        public void Dispose()
        {
            Close();
        }

        public void Write(string line)
        {
            _writer.WriteLine(line);
        }

        #region private functions
        private void Open(string filePath)
        {
            if (_writer != null)
                return;

            _writer = new StreamWriter(filePath, true);
            _writer.AutoFlush = true;
        }

        private void Close()
        {
            if (_writer != null)
            {
                _writer.Flush();
                _writer.Close();
                _writer = null;
            }
        }
        #endregion

        #region private data
        private StreamWriter _writer;
        #endregion
    }

    public sealed class LogFileInvoker
    {
        public static void LogIt(string filePath, string line)
        {
            using (LogFile log = new LogFile(filePath))
            {
                log.Write(line);
            }
        }

        public static void LogIt<T>(string filePath, IEnumerable<T> collection)
        {
            using (LogFile log = new LogFile(filePath))
            {
                log.Write(Delimiter.ToString<T>(collection));
            }
        }

    }
}
