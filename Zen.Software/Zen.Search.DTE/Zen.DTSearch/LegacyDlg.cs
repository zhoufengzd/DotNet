using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using dtSearch.Engine;
using Zen.Search.Util;
using Zen.Search.Util.DTS;

namespace Zen.DTSearch
{
    /// <summary>
    /// The main application form
    /// </summary>
    public partial class LegacyDlg : Form
    {
        const string AccessConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=northwind.mdb";
        const string OleDBLocalConn = @"Provider=SQLOLEDB;Server=LocalHost;Integrated Security=SSPI;";
        const string OleDBVMLabConn = @"Provider=SQLOLEDB;Server=Ent-Sql;Database=SSEDemo;UID=sa;Pwd=pa$$w0rd";
        const string OleDBCustomSearchConn = @"Provider=SQLOLEDB;Server=qa-perfsql2k\sql2k5;Database=CUSTOM_SEARCH;UID=admin;Pwd=admin";
        const string OleDBEnron2K5Conn = @"Provider=SQLOLEDB;Server=qa-perfsql2k\sql2k5;Database=NEWENRON;UID=admin;Pwd=admin";
        const string OleDB100kDBConn = @"Provider=SQLOLEDB;Server=qa-perfsql2k;Database=100KDB;UID=sa;Pwd=summation";
        const string OleDB500KConn = @"Provider=SQLOLEDB;Server=qa-perfsql2k\sql2k5;Database=DT_500K_DB;UID=admin;Pwd=admin";
        const string VersionFmt = "{0}.{1}({2})";   // "<Major>.<Minor>(<Build>)"

        const string DemoSearchText = "Summary:Photo;Docdate:199802??;DocID:NT-9*;Witness:Peter Wool;";
        const string EnronSearchText = "xtranote1:VK-SF-Stanford-3-14-01;:ECd-00052*;DocID:1000016-001;*2736*;subject:meeting;doclink:";

        public LegacyDlg()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (_search != null)
            {
                _search.Reset();
                _search = null;
            }

            if (_logger != null)
            {
                _logger.Flush();
                _logger.Close();
                _logger = null;
            }

            base.Dispose(disposing);
        }

        #region event handlers
        private void OnLoad(object sender, EventArgs e)
        {
            InitEngine();

            _numericUpDownBatchSize.Minimum = 10;
            _numericUpDownBatchSize.Maximum = 50000;
            _numericUpDownBatchSize.Increment = 1000;
            _numericUpDownBatchSize.Value = 10000;

            _indexRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.StartupPath), "Indexes"));
            _textBoxIndexerRoot.Text = Path.Combine(_indexRoot, "ssedemo");

            string[] connectionStrings = new string[] { OleDBLocalConn, OleDBVMLabConn, OleDBCustomSearchConn, OleDBEnron2K5Conn, OleDB100kDBConn, OleDB500KConn };
            _comboBoxConnectString.DataSource = connectionStrings;
            Text = string.Format("{0} -- DT Engine Version: {1}", Text, _dtVersion);

            // 'Verify' page
            _comboBoxCompareType.DataSource = Enum.GetNames(typeof(CompareType));
            _comboBoxCompareType.SelectedIndex = 1;

            string[] searchStrings = new string[] { DemoSearchText, EnronSearchText };
            _comboBoxBatchSearch.DataSource = searchStrings;

            _textBoxCreateView.Text = LoadDeploySql();

            _textBoxResultFile.Text = "60000RowGuid.txt";
        }

        private void InitEngine()
        {
            // Create a new Server object
            Server dtServer = new Server();

            // Display the Engine version
            _dtVersion = string.Format(VersionFmt,
                dtServer.MajorVersion.ToString(), dtServer.MinorVersion.ToString(), dtServer.Build.ToString());
            dtServer.Dispose();
            dtServer = null;

            _factory = new DTFactory(Path.Combine(Application.StartupPath, "Config"));
        }

        #region 'Index' page
        private void OnIndexIt(object sender, EventArgs e)
        {
            if (_connString != _comboBoxConnectString.Text)
            {
                OnConnectionChanged(this, EventArgs.Empty);
                return;
            }

            _textBoxIndexStatus.Text = string.Empty;
            _stopRequested = false;
            _totalRowProcessed = 0;

            if (_targetTbl == null)
            {
                MessageBox.Show("Please select target table.");
                return;
            }

            if (_totalRowCount < 1)
                return;

            PrepareTable(_targetTbl);
            StartWatchIt("Indexer, " + _targetTbl);
            {
                long endRow = (long)_numericUpDownEndRow.Value;
                if (endRow >= _totalRowCount)
                    endRow = _totalRowCount + 1;

                string indexRootDir = Path.Combine(_textBoxIndexerRoot.Text, _targetTbl);
                if (Directory.Exists(indexRootDir))
                    Directory.Delete(indexRootDir, true);
                List<IndexJob> jobs = BuildIndexJobs(_targetTbl, (long)(_numericUpDownStartingRow.Value), (long)(_numericUpDownBatchSize.Value), endRow, indexRootDir);
                _textBoxMergeRoot.Text = indexRootDir;

                foreach (IndexJob job in jobs)
                {
                    ExecuteIndexJob(job);
                    _totalRowProcessed += ((DTTableSource)job.DataSourceToIndex).RecordProcessed;
                    job.Dispose();

                    if (_stopRequested)
                        break;
                }
            }
            StopWatchIt("Indexer, " + _targetTbl);

            _textBoxProcessed.Text = _totalRowProcessed.ToString();
            _textBoxIndexStatus.Text = (_stopRequested) ? "Indexing Stopped" : "Indexing Complete";
        }

        private void OnStop(object sender, EventArgs e)
        {
            _stopRequested = true;
        }

        private void OnConnectionChanged(object sender, EventArgs e)
        {
            if (_connString == _comboBoxConnectString.Text)
                return;

            _targetTbl = null;
            _totalRowCount = -1;

            if (_tableRowCountTbl != null)
            {
                _tableRowCountTbl.Clear();
                _tableRowCountTbl = null;
            }

            _connString = _comboBoxConnectString.Text;
            try
            {
                if (_conn != null)
                    _conn.Close();

                _conn = new OleDbConnection(_connString); //+ ";Connect Timeout=0;"
                _conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _conn = null;
                return;
            }

            string database = GetDatabaseName(_connString);
            _textBoxIndexerRoot.Text = (database == null) ? _indexRoot : Path.Combine(_indexRoot, database);

            if (_tableCountTimer == null)
            {
                _tableCountTimer = new Timer();
                _tableCountTimer.Interval = 500;
                _tableCountTimer.Tick += new EventHandler(OnRefreshTableList);
            }

            if (!_tableCountTimer.Enabled)
                _tableCountTimer.Start();
        }

        private void OnSelectedTableChanged(object sender, EventArgs e)
        {
            _textBoxProcessed.Text = string.Empty;
            if (_comboBoxTargetTbl.SelectedIndex < 0)
            {
                _targetTbl = null;
                _totalRowCount = -1;
                _numericUpDownStartingRow.Maximum = 0;
                _numericUpDownEndRow.Maximum = 0;
                _numericUpDownStartingRow.Value = 0;

                _textBoxRowCount.Text = string.Empty;
            }
            else
            {
                _targetTbl = _comboBoxTargetTbl.SelectedItem.ToString();
                _textBoxSearchTableName.Text = _targetTbl;
                _totalRowCount = GetRowCount(_targetTbl);

                _numericUpDownStartingRow.Maximum = _totalRowCount + 1;
                _numericUpDownStartingRow.Increment = _totalRowCount / 10;
                if (_numericUpDownStartingRow.Increment < 1)
                    _numericUpDownStartingRow.Increment = 1;
                _numericUpDownStartingRow.Value = 0;

                _numericUpDownEndRow.Maximum = _numericUpDownStartingRow.Maximum;
                _numericUpDownEndRow.Increment = _numericUpDownStartingRow.Increment;
                _numericUpDownEndRow.Value = (_totalRowCount > -1) ? _totalRowCount : 0;

                _textBoxRowCount.Text = _totalRowCount.ToString("0,0");

                // sync with 'Verify' page
                _textBoxSearchIndexPath.Text = Path.Combine(_textBoxIndexerRoot.Text, _targetTbl);
            }
        }

        private void OnBatchSizeChanged(object sender, EventArgs e)
        {
            _numericUpDownEndRow.Increment = _numericUpDownBatchSize.Value;
        }

        private void OnRefreshTableList(object sender, EventArgs eArgs)
        {
            _tableCountTimer.Stop();
            _comboBoxTargetTbl.DataSource = null;
            try
            {
                _comboBoxTargetTbl.DataSource = GetTableNames();
                GetAllTableRowCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 'Merge' page
        private void OnMergeIndex(object sender, EventArgs e)
        {
            string indexRoot = _textBoxMergeRoot.Text;
            StringCollection indexDirs = GetIndexDirectories(indexRoot, false);
            if (indexDirs == null)
                return;

            bool compressIndex = _checkBoxCompressIndex.Checked;
            _buttonMergeIndex.Enabled = false;

            StartWatchIt("Merge, " + _targetTbl);
            {
                using (IndexJob job = new IndexJob())
                {
                    job.IndexPath = indexRoot;
                    job.CreateRelativePaths = false;
                    job.IndexesToMerge = indexDirs;
                    job.ActionMerge = true;
                    job.ActionCompress = compressIndex;
                    job.ExecuteInThread();
                    DoExecution(job, _textBoxMergeStatus);
                }

                if (_checkBoxCleanUp.Checked)
                {
                    foreach (string subDir in indexDirs)
                        Directory.Delete(subDir, true);
                }
            }
            StopWatchIt("Merge, " + _targetTbl);

            _buttonMergeIndex.Enabled = true;
        }
        #endregion

        #region 'Verify' page
        private void OnRefreshWords(object sender, EventArgs e)
        {
            _comboBoxSearchValue.DataSource = null;
            StringCollection indexDirs = GetIndexDirectories(_textBoxSearchIndexPath.Text, true);
            if (indexDirs == null)
                return;

            string searchWord = _comboBoxSearchValue.Text;
            if (string.IsNullOrEmpty(searchWord))
                searchWord = "a";

            List<string> words = new List<string>();
            using (WordListBuilder wordsBuilder = new WordListBuilder())
            {
                Dictionary<string, int> wordsMap = new Dictionary<string, int>();
                foreach (string dir in indexDirs)
                {
                    if (wordsBuilder.OpenIndex(dir))
                    {
                        wordsBuilder.ListWords(searchWord, 5);
                        for (int i = 0; i < wordsBuilder.Count; ++i)
                        {
                            wordsMap[wordsBuilder.GetNthWord(i)] = 1;
                        }
                    }
                }

                foreach (KeyValuePair<string, int> kv in wordsMap)
                    words.Add(kv.Key);
            }

            _comboBoxSearchValue.DataSource = words;
        }

        private void OnSearch(object sender, System.EventArgs e)
        {
            string resultTable = null;
            StartWatchIt("Search, " + _targetTbl);
            {
                //try
                {
                    SearchFactory sf = new SearchFactory();
                    if (_search == null)
                        sf.GetSearchProvider(out _search);

                    _search.SetOption("connectionString", _connString);
                    _search.SetOption("skipLoadResult", _checkBoxSkipLoad.Checked ? "true" : "false");
                    _search.SetOption("IndexPath", _textBoxSearchIndexPath.Text);

                    if (_numericUpDownMaxResult.Value > 0)
                        _search.SetOption("MaxResultCount", _numericUpDownMaxResult.Value.ToString());

                    ILeafItem qi = null;
                    sf.GetLeafItem(out qi);
                    qi.TableName = _textBoxSearchTableName.Text;
                    qi.Fieldname = _textBoxTargetField.Text;
                    qi.QueryValue = _comboBoxSearchValue.Text;
                    qi.CompareType = (CompareType)Enum.Parse(typeof(CompareType), _comboBoxCompareType.SelectedItem.ToString());
                    resultTable = _search.AddQueryItem(qi);

                    _search.RunSearch();
                }
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}
            }
            StopWatchIt("Search, " + _targetTbl);

            _textBoxOnFoundDoc.Text = _search.GetQueryResult(resultTable).HitCount.ToString();
            _search.Reset();
        }

        private void OnSearchOptionChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_comboBoxSearchValue.Text))
                return;

            CompareType ct = (CompareType)Enum.Parse(typeof(CompareType), _comboBoxCompareType.SelectedItem.ToString());
            string fldMapped = MapTableFieldName(_textBoxSearchTableName.Text, _textBoxTargetField.Text);
            _textBoxSearchRequest.Text = DtSearchProvider.BuildFieldRequest(fldMapped, EscapeChars(_comboBoxSearchValue.Text), ct);
        }
        #endregion

        #region 'Info' page

        private void OnBatchIndex(object sender, EventArgs e)
        {
            if (_tableRowCountTbl == null)
                return;

            DataGridViewSelectedRowCollection selected = _dataGridViewTableRowCount.SelectedRows;
            if (selected.Count < 1)
                return;

            _tabControl.SelectedTab = _tabPageIndex;
            foreach (DataGridViewRow row in selected)
            {
                string tableName = row.Cells[0].Value.ToString();
                int found = _comboBoxTargetTbl.FindStringExact(tableName);
                if (found > -1)
                {
                    _comboBoxTargetTbl.SelectedIndex = found;
                    OnSelectedTableChanged(this, EventArgs.Empty);
                    OnIndexIt(this, EventArgs.Empty);
                    OnMergeIndex(this, EventArgs.Empty);
                }
            }
        }

        private void OnBatchSearch(object sender, EventArgs e)
        {
            List<LeafItem> items = LoadDefaultQueryItems();
            StartWatchIt("Search, " + _targetTbl);
            {
                //_search = new DtSearchProvider();
                SearchFactory sf = new SearchFactory();
                sf.GetSearchProvider(out _search);
                _search.SetOption("connectionString", _connString);
                _search.SetOption("IndexPath", _textBoxSearchIndexPath.Text);
                _search.SetOption("MaxResultCount", _numericUpDownMaxResult.Value.ToString());

                foreach (LeafItem qi in items)
                {
                    _search.SetOption("skipLoadResult", "true");
                    _search.AddQueryItem(qi);
                    _search.RunSearch();

                    _search.SetOption("skipLoadResult", "false");
                    _search.AddQueryItem(qi);
                    _search.RunSearch();
                }
            }
            StopWatchIt("Search, " + _targetTbl);
        }

        private void OnCurrentRowChanged(object sender, EventArgs e)
        {
            if (_tableRowCountTbl == null || _dataGridViewTableRowCount.CurrentRow == null)
                return;

            string table = _dataGridViewTableRowCount.CurrentRow.Cells[0].Value.ToString();
            int found = _comboBoxTargetTbl.FindStringExact(table);
            if (found > -1)
                _comboBoxTargetTbl.SelectedIndex = found;
        }

        private void OnPrepareTables(object sender, EventArgs e)
        {
            _buttonPrepareTables.Enabled = false;

            foreach (string table in _tableNames)
                PrepareTable(table);

            _buttonPrepareTables.Enabled = true;
        }

        private void OnCleanUpTempTables(object sender, EventArgs e)
        {
            if (_tempRulstTableNames == null)
                return;

            const string DropTableFmt = @"DROP TABLE [{0}]";
            foreach (string tbl in _tempRulstTableNames)
                ExecuteNonQuery(string.Format(DropTableFmt, tbl), true);
        }

        private void OnListFullTextColumns(object sender, EventArgs e)
        {
            const string ListFulltextColumnFmt = @"exec sp_help_fulltext_columns [{0}]";

            string query = null;
            StringBuilder buffer = new StringBuilder();
            buffer.Append("\"Table Name\"\t\"Short Name\"\t\"FT Column Count\"\t\"FT Columns\"");
            buffer.AppendLine();

            foreach (string tableName in _tableNames)
            {
                string shortTableName = (tableName.Replace("SLTDS_C1_", string.Empty)).Replace("SLT_C1_", string.Empty);
                query = string.Format(ListFulltextColumnFmt, tableName);

                buffer.AppendFormat("\"{0}\"\t", tableName);
                buffer.AppendFormat("\"{0}\"\t", shortTableName);
                int colCount = 0;
                StringBuilder colNameBuffer = new StringBuilder();
                using (OleDbDataReader reader = ExecuteReader(query, true))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            if (colCount++ > 0)
                                colNameBuffer.AppendFormat(", [{0}]", reader[3].ToString());
                            else
                                colNameBuffer.AppendFormat("[{0}]", reader[3].ToString());
                        }
                    }
                }

                buffer.AppendFormat("\"{0}\"\t", colCount);
                buffer.AppendFormat("\"{0}\"", colNameBuffer);
                buffer.AppendLine();
            }

            try
            {
                using (StreamWriter sw = new StreamWriter("FullTextColumns.txt", false))
                {
                    sw.Write(buffer.ToString());
                    sw.Flush();
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion

        #region 'Deploy' page
        private void OnDeploy(object sender, EventArgs e)
        {
            string sql = _textBoxCreateView.SelectedText;
            if (sql.Length < 1)
                sql = _textBoxCreateView.Text;

            ExecuteNonQuery(sql, false);
        }
        #endregion

        #region 'Result' page
        private void OnRefresh(object sender, EventArgs e)
        {
            GetTempResultTableRowCount();
        }

        private void OnExportData(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selected = _dataGridViewSearchResult.SelectedRows;
            if (selected.Count < 1)
                return;

            foreach (DataGridViewRow row in selected)
            {
                string tableName = row.Cells[0].Value.ToString();
                string query = string.Format("SELECT * FROM [{0}]", tableName);

                try
                {
                    StartWatchIt("Export, " + tableName);

                    int count = 0;
                    StreamWriter logger = new StreamWriter("exportLog.txt", true);
                    using (OleDbDataReader reader = ExecuteReader(query, true))
                    {
                        if (reader != null)
                        {
                            using (StreamWriter sw = new StreamWriter(tableName + ".txt", false))
                            {
                                while (reader.Read())
                                {
                                    sw.WriteLine(reader[0].ToString());
                                    count++;

                                    if (count % 500 == 0)
                                        sw.Flush();
                                }

                                sw.Flush();
                                sw.Close();

                                logger.WriteLine("{0},{1}", tableName, count);
                                logger.Close();
                                logger.Dispose();
                            }
                        }
                    }

                    StopWatchIt("Export, " + tableName);
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        private void OnLoadData(object sender, EventArgs e)
        {
            string dataFile = _textBoxResultFile.Text;
            if (!File.Exists(dataFile))
            {
                MessageBox.Show("Invalid data file: " + dataFile);
                return;
            }

            DataGridViewSelectedRowCollection selected = _dataGridViewSearchResult.SelectedRows;
            if (selected.Count < 1)
                return;

            string tableName = selected[0].Cells[0].Value.ToString();

            DataTable resultTbl = new DataTable();
            resultTbl.Columns.Add("RowGuids", typeof(Guid));

            resultTbl.BeginLoadData();
            DataRowCollection rows = resultTbl.Rows;

            const int BatchSize = 5000; // Expose to config if needed
            int lineCount = 0;

            string connString = _connString.ToLower().Replace("provider=sqloledb;", string.Empty);
            connString = connString.Replace("provider=sqloledb.1;", string.Empty);
            connString = connString.Replace("provider=sqloledb.2;", string.Empty);

            StartWatchIt("Loader, " + tableName);
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            using (StreamReader file = new StreamReader(dataFile))
            {
                string line = null;
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Length < 1) // Skip empty line
                        continue;

                    DataRow dtRowTmp = resultTbl.NewRow();
                    dtRowTmp.ItemArray = new object[] { line };
                    rows.Add(dtRowTmp);

                    if (lineCount++ > BatchSize)
                        UploadQueryResult(conn, tableName, ref resultTbl);
                }

                UploadQueryResult(conn, tableName, ref resultTbl);
            }
            conn.Close();

            StopWatchIt("Loader, " + tableName);
        }

        /// <summary>
        /// Bulk insert into Temporary table
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="resultTableName"></param>
        /// <param name="resultTbl"></param>
        private void UploadQueryResult(SqlConnection con, string resultTableName, ref DataTable resultTbl)
        {
            if (resultTbl.Rows.Count > 0)
            {
                SqlBulkCopy bulkcopy = null;

                try
                {
                    bulkcopy = new SqlBulkCopy(con, SqlBulkCopyOptions.TableLock, null);
                    bulkcopy.BatchSize = resultTbl.Rows.Count;
                    bulkcopy.DestinationTableName = string.Format("[{0}]", resultTableName);
                    bulkcopy.BulkCopyTimeout = 0;
                    bulkcopy.WriteToServer(resultTbl);

                    resultTbl.Clear();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    bulkcopy.Close();
                }
            }
        }

        #endregion

        #endregion

        #region support functions
        private void ExecuteIndexJob(IndexJob job)
        {
            _buttonIndexIt.Enabled = false;

            // Start index job execution in a separate thread
            job.ActionCreate = !IndexExists(job.IndexPath);
            job.ExecuteInThread();
            DoExecution(job, _textBoxIndexStatus);

            // Reset flags and controls
            _buttonIndexIt.Enabled = true;
        }

        private void DoExecution(IndexJob job, TextBox textBoxStatus)
        {
            // Monitor the job execution thread as it progresses
            IndexProgressInfo status = new IndexProgressInfo();
            while (job.IsThreadDone(500, status) == false)
            {
                // Set the status text based on the current indexing step
                switch (status.Step)
                {
                    case IndexingStep.ixStepBegin:
                        textBoxStatus.Text = "Opening index";
                        break;

                    case IndexingStep.ixStepCheckingFiles:
                        textBoxStatus.Text = "Checking files";
                        break;

                    case IndexingStep.ixStepCompressing:
                        textBoxStatus.Text = "Compressing index";
                        break;

                    case IndexingStep.ixStepCreatingIndex:
                        textBoxStatus.Text = "Creating index";
                        break;

                    case IndexingStep.ixStepDone:
                        textBoxStatus.Text = "Indexing Complete";
                        break;
                    case IndexingStep.ixStepMerging:
                        textBoxStatus.Text = "Merging words into index";
                        break;

                    case IndexingStep.ixStepNone:
                        textBoxStatus.Text = string.Empty;
                        break;

                    case IndexingStep.ixStepReadingFiles:
                        textBoxStatus.Text = status.File.Name;
                        break;

                    case IndexingStep.ixStepStoringWords:
                        textBoxStatus.Text = status.File.Name + " (storing words)";
                        break;

                    default:
                        textBoxStatus.Text = string.Empty;
                        break;
                }

                // Let other form events be handled while we're looping
                Application.DoEvents();

                DTTableSource ds = (DTTableSource)job.DataSourceToIndex;
                if (ds != null) // Only applies to Indexing job
                {
                    _textBoxProcessed.Text = ds.RecordProcessed.ToString();
                    if (_stopRequested)
                        ds.StopRequested = true;
                }
            }

            // If there were errors, display the errors as additions to the
            // status text
            JobErrorInfo err = job.Errors;
            for (int i = 0; i < err.Count; i++)
            {
                textBoxStatus.Text = textBoxStatus.Text + " " + err.Message(i);
            }
        }

        private string EscapeChars(string rawRequest)
        {
            StringBuilder tmp = new StringBuilder(rawRequest);
            tmp.Replace(@"\t", "\t");
            tmp.Replace(@"\n", "\n");

            return tmp.ToString();
        }

        private List<string> GetTableNames()
        {
            if (_conn == null)
                return null;

            _tableNames = new List<string>();
            _tempRulstTableNames = new List<string>();

            // Retrieve the database schema containing user table names
            DataTable dtTableList = _conn.GetSchema("Tables");
            //DataTable dtViewList = _conn.GetOleDbSchemaTable(OleDbSchemaGuid.Views, new object[] { null, null, null, "VIEW" });

            _tableNames.Add("SLT_SecurityPrincipals");
            AddTableNames(dtTableList.Rows);
            //AddTableNames(dtViewList.Rows);

            return _tableNames;
        }

        private void AddTableNames(DataRowCollection rows)
        {
            int tableCount = rows.Count;

            Regex rgx = new Regex("_[c|C][1-9]+[0-9]*_");
            for (int i = 0; i < tableCount; i++)
            {
                string tableName = (string)rows[i]["TABLE_NAME"];
                if (rgx.IsMatch(tableName))
                    _tableNames.Add(tableName);

                if (TryParseGuid(tableName))
                    _tempRulstTableNames.Add(tableName);
            }
        }

        private long GetRowCount(string tableName)
        {
            try
            {
                if (_conn == null)
                    return -1;

                string select = string.Format("SELECT COUNT_BIG(1) FROM [{0}]", tableName);
                OleDbCommand cmd = new OleDbCommand(select, _conn);

                long count = (long)cmd.ExecuteScalar();
                return count;
            }
            catch (Exception)
            {
                //System.Windows.Forms.MessageBox.Show(e.Message);
                return -1;
            }
        }

        private bool ExecuteNonQuery(string sql, bool ignoreError)
        {
            try
            {
                if (_conn == null)
                    return false;

                OleDbCommand cmd = new OleDbCommand(sql, _conn);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception e)
            {
                if (!ignoreError)
                    System.Windows.Forms.MessageBox.Show(e.Message);

                return false;
            }
        }

        private OleDbDataReader ExecuteReader(string sql, bool ignoreError)
        {
            try
            {
                if (_conn == null)
                    return null;

                OleDbCommand cmd = new OleDbCommand(sql, _conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                return reader;
            }
            catch (Exception e)
            {
                if (!ignoreError)
                    System.Windows.Forms.MessageBox.Show(e.Message);

                return null;
            }
        }

        private bool IndexExists(string indexDir)
        {
            if (!Directory.Exists(indexDir))
                return false;

            IndexInfo indexInfo = IndexJob.GetIndexInfo(indexDir);
            return (indexInfo.IndexSize != 0);
        }

        private List<IndexJob> BuildIndexJobs(string tableName, long startRow, long batchSize, long endRow, string indexFilePath)
        {
            const string BatchFmt = "{0} WHERE [__RowNumber] >= {1} AND [__RowNumber] < {2} ";

            StringBuilder selectSqlBase = new StringBuilder();
            selectSqlBase.AppendFormat("SELECT TOP {0} * FROM [{1}] WITH (NOLOCK)", endRow, tableName);

            bool useBatch = (endRow > batchSize);
            string selectSql = null;
            List<IndexJob> jobs = new List<IndexJob>();
            while (startRow < endRow)
            {
                if (useBatch)
                {
                    selectSql = string.Format(BatchFmt,
                        selectSqlBase.ToString(), startRow, startRow + batchSize > endRow ? endRow : startRow + batchSize);
                }
                else
                {
                    selectSql = selectSqlBase.ToString();
                }

                DTTableSource dataSource = new DTTableSource(null);
                dataSource.TableInfo = new TableInfo(tableName, selectSql, null);

                string indexPath = startRow > 1 ? Path.Combine(indexFilePath, startRow.ToString()) : indexFilePath;
                IndexJob indexJob = _factory.GetCreateIndexJob(indexPath, dataSource);
                jobs.Add(indexJob);

                startRow += batchSize;
            }

            return jobs;
        }

        private StringCollection GetIndexDirectories(string rootDir, bool includeRoot)
        {
            if (string.IsNullOrEmpty(rootDir) || !Directory.Exists(rootDir))
                return null;

            StringCollection indexDirs = new StringCollection();
            if (includeRoot && IndexExists(rootDir))
                indexDirs.Add(rootDir);

            string[] subDirs = Directory.GetDirectories(rootDir);
            if (subDirs != null)
            {
                foreach (string indexDir in subDirs)
                {
                    if (IndexExists(indexDir))
                        indexDirs.Add(indexDir);
                }
            }

            return indexDirs.Count < 1 ? null : indexDirs;
        }

        private string BuildSearchRequest()
        {
            if (string.IsNullOrEmpty(_comboBoxSearchValue.Text))
                return string.Empty;

            CompareType ct = (CompareType)Enum.Parse(typeof(CompareType), _comboBoxCompareType.SelectedItem.ToString());
            return DtSearchProvider.BuildFieldRequest(_textBoxTargetField.Text, EscapeChars(_comboBoxSearchValue.Text), ct);
        }

        private void StartWatchIt(string category)
        {
            _stopWatch.Start();
            _textBoxStartTime.Text = DateTime.Now.ToLongTimeString();
            _textBoxEndTime.Text = string.Empty;
            _textBoxDuration.Text = string.Empty;

            try
            {
                if (_logger == null)
                    _logger = new StreamWriter("Zen.DTSearch.log", true);
            }
            catch (Exception)
            {
                return;
            }

            _logger.Write(string.Format("{0}, {1}", category, _textBoxStartTime.Text));
            _logger.Flush();
        }

        private void StopWatchIt(string category)
        {
            _stopWatch.Stop();
            _textBoxEndTime.Text = DateTime.Now.ToLongTimeString();
            _textBoxDuration.Text = String.Format("{0:0.00}", (_stopWatch.ElapsedMilliseconds / 1000.0));
            _stopWatch.Reset();

            if (_logger == null)
                return;
            _logger.Write(string.Format(", {0}, {1}\r\n", _textBoxEndTime.Text, _textBoxDuration.Text));
            _logger.Flush();
        }

        private void GetAllTableRowCount()
        {
            if (_tableRowCountTbl != null)
                return;

            _tableRowCountTbl = new DataTable();
            _tableRowCountTbl.Columns.Add("Table Name", typeof(string));
            _tableRowCountTbl.Columns.Add("Record Count", typeof(long));

            if (_tableNames == null)
                return;

            string tableRowCountFile = GetDatabaseName(_connString);
            if (tableRowCountFile == null)
                tableRowCountFile = "Default.csv";

            StreamWriter sw = null;
            if (!File.Exists(tableRowCountFile))
            {
                sw = new StreamWriter(tableRowCountFile);
                sw.WriteLine("\"Table Name\",\"Record Count\"");
            }

            List<string> nonEmptyTables = new List<string>();
            DataRowCollection rows = _tableRowCountTbl.Rows;
            foreach (string table in _tableNames)
            {
                long rowCount = GetRowCount(table);
                if (rowCount < 1)
                    continue;

                nonEmptyTables.Add(table);

                DataRow dtRowTmp = _tableRowCountTbl.NewRow();
                dtRowTmp.ItemArray = new object[] { table, rowCount };
                rows.Add(dtRowTmp);

                if (sw != null)
                    sw.WriteLine(string.Format("\"{0}\",\"{1}\"", table, rowCount));
            }

            if (sw != null)
            {
                sw.Flush();
                sw.Dispose();
            }

            _comboBoxTargetTbl.DataSource = nonEmptyTables;
            _dataGridViewTableRowCount.DataSource = _tableRowCountTbl;
        }

        private void PrepareTable(string tableName)
        {
            const string AddIdentifyColumnFmt =
                "ALTER TABLE [<TableName>] ADD [__RowNumber] int Identity(1,1)\r\n" +
                "CREATE UNIQUE INDEX [IX_<TableName>_rownumber] ON [<TableName>]([__RowNumber])";
            ExecuteNonQuery(AddIdentifyColumnFmt.Replace("<TableName>", tableName), true);
        }

        private List<LeafItem> LoadDefaultQueryItems()
        {
            char[] fieldDelimter = new char[] { ';' };
            char[] fldValuedelimter = new char[] { ':' };
            CompareType[] compareTypes = new CompareType[] { CompareType.Equals, CompareType.IsNotNull, CompareType.IsNull, CompareType.GreaterThan };

            string[] searchTargets = _comboBoxBatchSearch.Text.Split(fieldDelimter, StringSplitOptions.RemoveEmptyEntries);
            if (searchTargets.Length < 1)
            {
                _comboBoxBatchSearch.Text = DemoSearchText;
                searchTargets = _comboBoxBatchSearch.Text.Split(fieldDelimter, StringSplitOptions.RemoveEmptyEntries);
            }

            List<LeafItem> qItems = new List<LeafItem>(searchTargets.Length);
            string searchField, searchValue;
            foreach (string fs in searchTargets)
            {
                string[] searchPair = fs.Split(fldValuedelimter);
                if (searchPair.Length < 2)
                    continue;

                searchField = searchPair[0].Trim();
                searchValue = searchPair[1].Trim();
                for (int ind = 0; ind < compareTypes.Length; ind++)
                {
                    CompareType ct = compareTypes[ind];
                    if (searchField.Length < 1 && !(ct == CompareType.Equals || ct == CompareType.NotEqual))
                        continue;

                    if (searchValue.Length < 1 && !(ct == CompareType.IsNotNull || ct == CompareType.IsNull))
                        continue;

                    LeafItem qi = new LeafItem();
                    qi.TableName = _targetTbl;
                    qi.Fieldname = searchField;
                    qi.QueryValue = searchValue;
                    qi.CompareType = ct;

                    qItems.Add(qi);
                }
            }

            return qItems;
        }

        private string LoadDeploySql()
        {
            const string DeploySql = "Deploy.sql";
            if (!File.Exists(DeploySql))
                return null;

            using (StreamReader file = new StreamReader(DeploySql))
            {
                return file.ReadToEnd();
            }
        }

        private bool TryParseGuid(string input)
        {
            const string GuidFmt = @"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$";
            return Regex.IsMatch(input, GuidFmt);
        }

        private string GetDatabaseName(string conString)
        {
            string conStringLower = conString.ToLower();

            Regex rgx = new Regex(@"(database\s*=)(\s*)(\w+);*");
            Match match = rgx.Match(conStringLower);
            return (match.Success) ? match.Groups[3].Value : null;
        }

        private string MapTableFieldName(string tableName, string rawFieldname)
        {
            if (string.IsNullOrEmpty(rawFieldname))
                return rawFieldname;

            string cleanedTableName = tableName;

            int bracketLeft = tableName.LastIndexOf('[');
            int bracketRight = tableName.LastIndexOf(']');
            if (bracketLeft > -1 && bracketRight > -1)
                cleanedTableName = tableName.Substring(bracketLeft + 1, bracketRight - bracketLeft - 1);

            int ind = cleanedTableName.LastIndexOf('_');
            if (ind > -1)
            {
                string tableNameShort = cleanedTableName.Substring(ind + 1);
                return tableNameShort.Replace('-', '_') + "_" + rawFieldname;
            }

            return rawFieldname;
        }

        private void GetTempResultTableRowCount()
        {
            if (_tmpResultTableRowCountTbl != null)
                return;

            if (_tempRulstTableNames == null || _tempRulstTableNames.Count < 1)
                return;

            _tmpResultTableRowCountTbl = new DataTable();
            _tmpResultTableRowCountTbl.Columns.Add("Table Name", typeof(string));
            _tmpResultTableRowCountTbl.Columns.Add("Record Count", typeof(long));

            DataRowCollection rows = _tmpResultTableRowCountTbl.Rows;
            foreach (string table in _tempRulstTableNames)
            {
                long rowCount = GetRowCount(table);

                DataRow dtRowTmp = _tmpResultTableRowCountTbl.NewRow();
                dtRowTmp.ItemArray = new object[] { table, rowCount };
                rows.Add(dtRowTmp);
            }

            _dataGridViewSearchResult.DataSource = _tmpResultTableRowCountTbl;
        }

        #endregion

        #region private data
        private string _dtVersion = null;
        private DTFactory _factory;
        private DTStatusHandler _dtStatusHandler = new DTStatusHandler();
        private string _indexRoot;

        private string _connString;
        private OleDbConnection _conn;
        private string _targetTbl;
        private long _totalRowCount = -1;
        private long _totalRowProcessed = -1;
        private bool _stopRequested = false;

        private ISearchProvider _search;
        private Stopwatch _stopWatch = new Stopwatch();
        private DataTable _tableRowCountTbl;
        private DataTable _tmpResultTableRowCountTbl;
        private List<string> _tableNames;
        private List<string> _tempRulstTableNames;

        private StreamWriter _logger;
        private Timer _tableCountTimer;
        #endregion

    }
}
