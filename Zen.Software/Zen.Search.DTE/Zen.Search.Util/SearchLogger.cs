using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Zen.Search.Util
{
    class SearchLogger : IDisposable
    {
        public SearchLogger()
        {
        }
        public void Dispose()
        {
            CloseLogFile();
        }

        public void AddQueryItemCalled()
        {
            OpenLogFile();
            if (string.IsNullOrEmpty(_addQueryItemStarted))
                _addQueryItemStarted = DateTime.Now.ToLongTimeString();
        }

        public void DTSearchStarted(string tableName, string searchRequest)
        {
            OpenLogFile();

            if (_logger == null)
                return;
            _searchWatch.Reset();
            _searchWatch.Start();

            _logger.Write(string.Format("\r\n{0},{1},{2},{3}", tableName, _addQueryItemStarted, searchRequest, DateTime.Now.ToLongTimeString()));
            _logger.Flush();
        }
        public void DTSearchStopped(string tableName, string resultTableName, int hitCount)
        {
            if (_logger == null)
                return;

            _searchWatch.Stop();
            string duration = String.Format("{0:0.00}", (_searchWatch.ElapsedMilliseconds / 1000.0));

            _logger.Write(string.Format(",{0},{1},{2},{3},{4},{5},{6}",
                DateTime.Now.ToLongTimeString(), duration, hitCount, resultTableName, _loadDuration, _sortDuration, _buildRequestDuration));

            CloseLogFile();
        }

        public void LoadStarted()
        {
            _loadResultWatch.Reset();
            _loadResultWatch.Start();
            _loadDuration = string.Empty;
        }
        public void LoadStopped()
        {
            _loadResultWatch.Stop();
            _loadDuration = String.Format("{0:0.00}", (_loadResultWatch.ElapsedMilliseconds / 1000.0));
        }

        public void SortStarted()
        {
            _sortResultWatch.Reset();
            _sortResultWatch.Start();
            _sortDuration = string.Empty;
        }
        public void SortStopped()
        {
            _sortResultWatch.Stop();
            _sortDuration = String.Format("{0:0.00}", (_sortResultWatch.ElapsedMilliseconds / 1000.0));
        }

        public void BuildRequestStarted()
        {
            _buildDtRequestWatch.Reset();
            _buildDtRequestWatch.Start();
            _buildRequestDuration = string.Empty;
        }
        public void BuildRequestStopped()
        {
            _buildDtRequestWatch.Stop();
            _buildRequestDuration = String.Format("{0}", (_buildDtRequestWatch.ElapsedMilliseconds));
        }

        #region private functions
        private void OpenLogFile()
        {
            try
            {
                if (_logger == null)
                {
                    const string logFile = "Zen.Search.Util.log";
                    FileInfo fileInfo = new FileInfo(logFile);

                    bool appendHeader = !(fileInfo.Exists && fileInfo.Length >= 10);
                    _logger = new StreamWriter(logFile, true);
                    _logger.AutoFlush = true;
                    if (appendHeader)
                        _logger.WriteLine("tableName,AddQueryItemStarted,searchRequest,startTime,endTime,duration,hitCount,resultTableName,loadDuration,SortDuration,buildRequest(ms)");
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void CloseLogFile()
        {
            try
            {
                if (_logger != null)
                {
                    _logger.Close();
                    _logger = null;
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region private data
        private Stopwatch _searchWatch = new Stopwatch();
        private Stopwatch _loadResultWatch = new Stopwatch();
        private Stopwatch _sortResultWatch = new Stopwatch();
        private Stopwatch _buildDtRequestWatch = new Stopwatch();
        private string _loadDuration;
        private string _sortDuration;
        private string _buildRequestDuration;
        private string _addQueryItemStarted;
        private StreamWriter _logger;
        #endregion
    }
}
