using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

using dtSearch.Engine;

namespace Zen.Search.Util
{
    public class ColValueDefs
    {
        public const string NULLValue = "A0B6CE5A4E0541c28DDE9B132A65F705";
        public const string Zero = "0", One = "1";
        public const string DateTimeFmt = @"yyyyMMddhhmmss";
    }

    /// <summary>
    /// DT Search provider.
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("A0B6CE5A-4E05-41c2-8DDE-9B132A65F705")]
    public class DtSearchProvider : MarshalByRefObject, ISearchProvider, IDisposable
    {
        const string CreateTableDDLFmt =
            "CREATE TABLE [<TABLE-NAME>] (RowGUID [uniqueidentifier]) ";
        const string CreateTableIndexDDLFmt =
            "CREATE INDEX [<TABLE-NAME>-IDX] ON [<TABLE-NAME>]([RowGUID]) ";

        const string CreateLognameDDLFmt = "CREATE TABLE [<TABLE-NAME>] (PrincipalID [int] )";
        const string DropTableDDLFmt = "DROP TABLE [<TABLE-NAME>]";

        static readonly char[] RowGuidDelimiter = { '#', '=' };

        #region static service functions
        public static string BuildFieldRequest(string fieldName, string rawSearchValue, CompareType compareType)
        {
            const string FieldNameXFilterFmt = "{0}::";

            string processedValue = ProcessSearchValue(fieldName, rawSearchValue, compareType);
            if (!string.IsNullOrEmpty(fieldName))
                fieldName = string.Format(FieldNameXFilterFmt, fieldName); ;

            string[] searchValues = processedValue.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder fieldRequestBuffer = new StringBuilder();
            int index = 0;
            foreach (string sv in searchValues)
            {
                if (index++ > 0)
                    fieldRequestBuffer.Append(" and ");

                fieldRequestBuffer.Append('(');
                switch (compareType)
                {
                    case CompareType.Equals:
                        fieldRequestBuffer.Append(string.Format("xfilter ( word \"{0}{1}\" )", fieldName, sv)); break;
                    case CompareType.NotEqual:
                        fieldRequestBuffer.Append(string.Format("not (xfilter ( word \"{0}{1}\" ))", fieldName, sv)); break;

                    case CompareType.GreaterThan:
                        fieldRequestBuffer.Append(string.Format("(xfilter (word \"{0}{1}~~ZZZZZZZZZZZZZZ\")) and not (xfilter (word \"{0}{1}\"))", fieldName, sv)); break;

                    case CompareType.GreaterThanEqual:
                        fieldRequestBuffer.Append(string.Format("xfilter (word \"{0}{1}~~ZZZZZZZZZZZZZZ\")", fieldName, sv)); break;

                    case CompareType.LessThan:
                        fieldRequestBuffer.Append(string.Format("(xfilter (word \"{0}0~~{1}\")) and not (xfilter (word \"{0}{1}\"))", fieldName, sv)); break;

                    case CompareType.LessThanEqual:
                        fieldRequestBuffer.Append(string.Format("xfilter (word \"{0}0~~{1}\")", fieldName, sv)); break;

                    case CompareType.IsNotNull:
                        fieldRequestBuffer.Append(string.Format("not (xfilter ( word \"{0}{1}\" ))", fieldName, ColValueDefs.NULLValue)); break;

                    case CompareType.IsNull:
                        fieldRequestBuffer.Append(string.Format("xfilter ( word \"{0}{1}\" )", fieldName, ColValueDefs.NULLValue)); break;

                    default:
                        fieldRequestBuffer.Append(string.Format("xfilter (word \"{0}{1}\")", fieldName, sv)); break;
                }
                fieldRequestBuffer.Append(')');
            }

            return fieldRequestBuffer.ToString();
        }
        #endregion

        #region constructor ~destructor
        public DtSearchProvider()
        {
            LoadOptions();
        }

        ~DtSearchProvider()
        {
            Dispose();
        }

        public void Dispose()
        {
            Reset();

            // Only clean cache after Dispose called
            if (_cache != null)
            {
                _cache.Dispose();
                _cache = null;
            }
        }
        #endregion

        public void SetOption(string optionName, string optionValue)
        {
            if (string.IsNullOrEmpty(optionValue))
                return;

            if (string.Compare(optionName, "connectionString", true) == 0)
            {
                // To do: convert old DB connection string to sql server connection string
                _connString = optionValue.ToLower().Replace("provider=sqloledb;", string.Empty);
                _connString = _connString.Replace("provider=sqloledb.1;", string.Empty);
                _connString = _connString.Replace("provider=sqloledb.2;", string.Empty);

                _indexDBSubRoot = null;
            }
            else if (string.Compare(optionName, "MaxResultCount", true) == 0)
            {
                if (!Int32.TryParse(optionValue, out _maxHitCount))
                    _maxHitCount = 0;
            }
            else if (string.Compare(optionName, "PageSize", true) == 0)
            {
                if (!Int32.TryParse(optionValue, out _pageSize))
                    _pageSize = 0;
            }
            else if (string.Compare(optionName, "skipLoadResult", true) == 0)
            {
                if (!Boolean.TryParse(optionValue, out _skipLoadResult))
                    _skipLoadResult = false;
            }
            else if (string.Compare(optionName, "IndexPath", true) == 0)
            {
                _indexDir = optionValue;
            }
            else if (string.Compare(optionName, "IndexRoot", true) == 0)
            {
                _indexRoot = optionValue;
            }
            else if (string.Compare(optionName, "CaseId", true) == 0)
            {
                if (!Int32.TryParse(optionValue, out _caseId))
                    _caseId = -1;
            }
            else if (string.Compare(optionName, "ActiveForm", true) == 0)
            {
                _mainTable = optionValue;
            }
            else if (string.Compare(optionName, "OrderBy", true) == 0)
            {
                if (!Boolean.TryParse(optionValue, out _orderBy))
                    _orderBy = false;
            }
            else if (string.Compare(optionName, "EnableDTIndexCache", true) == 0)
            {
                if (!Boolean.TryParse(optionValue, out _enableDtIndexCache))
                    _enableDtIndexCache = false;
            }
            else if (string.Compare(optionName, "EnableResultTableIndex", true) == 0)
            {
                if (!Boolean.TryParse(optionValue, out _enableResultTableIndex))
                    _enableResultTableIndex = true;
            }
        }

        /// <summary>
        /// Running and return temp cached result table for now.
        /// To do: Just return result table name, invoke search later when user calls RunSearch()
        /// </summary>
        public string AddQueryItem(IQueryItem item)
        {
            if (_logger == null)
                _logger = new SearchLogger();
            _logger.AddQueryItemCalled();

            _queryNodes.Add(item.NodeId, item);

            LeafItem qi = item as LeafItem;
            if (qi == null)
                return string.Empty;

            MapTableFieldName(qi);

            if (_results.Count < 1)
            {
                QueryResult result = new QueryResult();
                result.SourceTable = qi.TableName;
                _resultMap.Add(result.ResultTable, result);
                _results.Add(result);
            }

            return _results[0].ResultTable;
        }

        /// <summary> 
        /// To do: Optimize sql queries later
        /// 1. Query against dtSearch
        /// 2. Upload result to result table
        /// 3. Return true if search successful
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="resultTableName"></param>
        /// <returns></returns>
        public bool RunSearch()
        {
            if (_queryNodes.Count > 0)
            {
                _logger.BuildRequestStarted();
                string dtRequest = TraverseTree(_queryNodes[0]);
                _logger.BuildRequestStopped();

                DoSearch(dtRequest, _results[0]);
            }

            return true;
        }

        public void Reset()
        {
            if (_resultMap.Count < 1)
                return;

            _queryNodes.Clear();

            CleanUp(_resultMap, _connString);
            _resultMap.Clear();
            _results.Clear();

            if (_logger != null)
            {
                _logger.Dispose();
                _logger = null;
            }
        }

        /// <summary>
        /// Get query result by temp result table name
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IQueryResult GetQueryResult(string tableName)
        {
            QueryResult result = null;
            if (_resultMap.TryGetValue(tableName, out result))
                return result;

            return null;
        }

        #region private functions
        private static string ProcessSearchValue(string fieldName, string searchValue, CompareType compareType)
        {
            const string AnyValue = "*";

            StringBuilder rawValue = new StringBuilder(searchValue.Length);
            foreach (char c in searchValue)
                rawValue.Append((c == '-') ? ' ' : c);

            int length = rawValue.Length;
            if (compareType == CompareType.GreaterThan || compareType == CompareType.GreaterThanEqual ||
                compareType == CompareType.LessThan || compareType == CompareType.LessThanEqual)
            {
                if (length > 0 && rawValue[length - 1] == '*')
                {
                    rawValue.Remove(length - 1, 1);
                    length = rawValue.Length;
                }
            }

            return length < 1 ? AnyValue : rawValue.ToString();
        }

        private void BuildIndexRoot()
        {
            if (!string.IsNullOrEmpty(_indexDBSubRoot))
                return;

            if (_indexRoot == null)
            {
                // To do: Go to IndexRootManager to get the index root settings
                Module md = Assembly.GetExecutingAssembly().ManifestModule;
                string workingDir = Path.GetDirectoryName(md.FullyQualifiedName);
                _indexRoot = Path.Combine(Path.GetDirectoryName(workingDir), "Indexes");
            }

            string database = GetDatabaseName(_connString);
            _indexDBSubRoot = (database == null) ? _indexRoot : Path.Combine(_indexRoot, database);
        }

        /// Prepare temp table and add column defintion
        private DataTable PrepareDataTable(SqlConnection con, string resultTableName, string sourceTableName)
        {
            DataTable resultTbl = new DataTable();

            string sqlStmt = null;
            if (sourceTableName.ToLower().IndexOf("slt_securityprincipals") > -1)
            {
                sqlStmt = CreateLognameDDLFmt;
                resultTbl.Columns.Add("PrincipalID", typeof(int));
            }
            else
            {
                sqlStmt = CreateTableDDLFmt + " " + CreateTableIndexDDLFmt;
                resultTbl.Columns.Add("RowGuids", typeof(Guid));
            }

            sqlStmt = sqlStmt.Replace("<TABLE-NAME>", resultTableName);
            ExceuteNonQuery(con, sqlStmt);

            return resultTbl;
        }

        /// Prepare temp table and add column defintion
        private void PostLoad(SqlConnection con, string resultTableName)
        {
            //if (_enableResultTableIndex)
            //{
            //    string sqlStmt = CreateTableIndexDDLFmt.Replace("<TABLE-NAME>", resultTableName);
            //    ExceuteNonQuery(con, sqlStmt);
            //}
        }

        /// <summary>
        /// Execute the Query
        /// Return the status
        /// </summary>
        /// <param name="connectionSring"></param>
        /// <param name="query"></param>
        private int ExceuteNonQuery(SqlConnection conn, string query)
        {
            int result = 0;
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return result;
        }

        private void ProcessResults(QueryResult resultInfo, SearchResults dtResult)
        {
            const int BatchSize = 5000; // Expose to config if needed

            if (!_skipLoadResult)
                _logger.LoadStarted();

            if (_orderBy)
            {
                _logger.SortStarted();
                dtResult.Sort(SortFlags.dtsSortBySortKey, "E-table_Docid");
                _logger.SortStopped();
            }

            SqlConnection con = null;
            try
            {
                con = new SqlConnection(_connString);
                con.Open();
                DataTable resultTbl = PrepareDataTable(con, resultInfo.ResultTable, resultInfo.SourceTable);

                resultTbl.BeginLoadData();
                DataRowCollection rows = resultTbl.Rows;

                bool returnFirstPageOnly = (_pageSize > 0);
                for (int i = 0; i < resultInfo.HitCount; ++i)
                {
                    if (returnFirstPageOnly && i == _pageSize)
                        break;

                    dtResult.GetNthDoc(i);
                    SearchResultsItem item = dtResult.CurrentItem;

                    DataRow dtRowTmp = resultTbl.NewRow();
                    dtRowTmp.ItemArray = new object[] { item.ShortName.Split(RowGuidDelimiter, StringSplitOptions.RemoveEmptyEntries)[0] };
                    rows.Add(dtRowTmp);

                    if (!_skipLoadResult && i > BatchSize)
                        UploadQueryResult(con, resultInfo.ResultTable, ref resultTbl);
                }

                if (!_skipLoadResult)
                    UploadQueryResult(con, resultInfo.ResultTable, ref resultTbl);

                resultTbl.EndLoadData();
                PostLoad(con, resultInfo.ResultTable);
            }
            finally
            {
                con.Close();
            }

            if (!_skipLoadResult)
                _logger.LoadStopped();
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

        private void CleanUp(Dictionary<string, QueryResult> resultTables, string connectionString)
        {
            using (SqlConnection con = new SqlConnection(_connString))
            {
                con.Open();
                foreach (KeyValuePair<string, QueryResult> kv in resultTables)
                {
                    DropTable(con, kv.Value.ResultTable);
                }

                con.Close();
            }
        }

        private void DropTable(SqlConnection con, string tableName)
        {
            try
            {
                ExceuteNonQuery(con, DropTableDDLFmt.Replace("<TABLE-NAME>", tableName));
            }
            catch (Exception)
            {
            }
        }

        // To do: build up index path by case id / table name
        //    index path = relative path + case id + table name
        private string BuildIndexPath(string tableName)
        {
            string tableNameCleaned = tableName.ToUpper().Replace("[DBO].", string.Empty);
            tableNameCleaned = (tableNameCleaned.Replace("[", string.Empty)).Replace("]", string.Empty);
            string indexPath = null;
            if (tableName.IndexOf("dummy", StringComparison.CurrentCultureIgnoreCase) > -1)
            {
                indexPath = Path.Combine(_indexDBSubRoot, @"_dummyData");
            }
            else
            {
                indexPath = Path.Combine(_indexDBSubRoot, tableNameCleaned);
            }

            return indexPath;

        }

        private void LoadOptions()
        {
            string optionDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Config");
            using (Options options = new Options())
            {
                options.AlphabetFile = Path.Combine(optionDir, "English.abc");
                options.BinaryFiles = BinaryFilesSettings.dtsoIndexSkipBinary;
                options.FieldFlags = FieldFlags.dtsoFfSkipFilenameField;
                options.FileTypeTableFile = Path.Combine(optionDir, "FileType.xml");
                options.Hyphens = HyphenSettings.dtsoHyphenAll;
                options.IndexNumbers = true;
                options.MaxWordLength = 128;
                options.MaxWordsToRetrieve = 8000;
                options.NoiseWordFile = Path.Combine(optionDir, "Noise.dat");
                options.TitleSize = 0;
                options.TextFlags = TextFlags.dtsoTfAutoBreakCJK;
                options.Save();
            }

            string iniFile = Path.Combine(optionDir, "search.ini");
            if (!File.Exists(iniFile))
                return;

            using (StreamReader file = new StreamReader(iniFile))
            {
                string line = null;
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Length < 1 || line[0] == '#') // Skip empty or comment line
                        continue;

                    string[] splitted = line.Split(new char[] { '=', });
                    if (splitted.Length == 2 && !string.IsNullOrEmpty(splitted[0]))
                        SetOption(splitted[0], splitted[1]);
                }
            }
        }

        private void DoSearch(string searchRequest, QueryResult result)
        {
            Debug.Assert(result.HitCount == 0);

            SearchJob search = BuildSearchJob(result.SourceTable);
            search.Request = searchRequest;

            _logger.DTSearchStarted(result.SourceTable, search.Request);
            {
                search.Execute();
                result.HitCount = search.Results.Count;

                ProcessResults(result, search.Results);
            }
            _logger.DTSearchStopped(result.SourceTable, result.ResultTable, result.HitCount);
        }

        /// <summary>
        /// Map table name to a view (for combined index)
        /// </summary>
        private void MapTableFieldName(LeafItem item)
        {
            string tableName = item.TableName;
            string cleanedTableName = tableName;

            int bracketLeft = tableName.LastIndexOf('[');
            int bracketRight = tableName.LastIndexOf(']');
            if (bracketLeft > -1 && bracketRight > -1)
                cleanedTableName = tableName.Substring(bracketLeft + 1, bracketRight - bracketLeft - 1);

            int ind = cleanedTableName.LastIndexOf('_');
            if (ind < 0)
            {
                item.TableName = cleanedTableName;
            }
            else
            {
                string prefix = cleanedTableName.Substring(0, ind + 1);
                string tableNameShort = cleanedTableName.Substring(ind + 1);

                //item.TableName = prefix + _mainTable + "View";
                item.TableName = prefix + "mainETableView";
                if (!string.IsNullOrEmpty(item.Fieldname))
                    item.Fieldname = tableNameShort.Replace('-', '_') + "_" + item.Fieldname;
            }
        }

        private string BuildSearchString(ILeafItem qi)
        {
            //  This handles multiple column / single value search efficiently.
            //  It also handles single column / single value
            if (!string.IsNullOrEmpty(qi.Fieldname))
            {
                string[] fieldNames = qi.Fieldname.Split(',');
                StringBuilder requestBuffer = new StringBuilder();
                int nameIndex = 0;
                foreach (string fldName in fieldNames) // Handled full text vectors here
                {
                    if (nameIndex++ > 0)
                        requestBuffer.Append(" or ");
                    requestBuffer.Append(BuildFieldRequest(fldName.Trim(), qi.QueryValue, qi.CompareType));
                }

                return requestBuffer.ToString();
            }
            else
            {
                return BuildFieldRequest(string.Empty, qi.QueryValue, qi.CompareType);
            }
        }

        /// <param name="targetTable"> target table or view</param>
        /// <returns></returns>
        private SearchJob BuildSearchJob(string targetTable)
        {
            const int IndexCacheCount = 3;

            SearchJob search = new SearchJob();
            BuildIndexRoot();

            string indexPath = _indexDir;
            if (string.IsNullOrEmpty(indexPath))
                indexPath = BuildIndexPath(targetTable);

            if (!IndexExists(indexPath))
                throw new Exception("Invalid index path: " + indexPath);

            search.IndexesToSearch.Add(indexPath);
            search.SearchFlags = search.SearchFlags | SearchFlags.dtsSearchDelayDocInfo;
            search.MaxFilesToRetrieve = _maxHitCount;

            if (_enableDtIndexCache)
            {
                if (_cache == null)
                    _cache = new IndexCache(IndexCacheCount);
                search.SetIndexCache(_cache);
            }

            return search;
        }

        private string TraverseTree(IQueryItem item)
        {
            if (item is IConnectorItem)
            {
                IConnectorItem conn = item as IConnectorItem;
                StringBuilder buffer = new StringBuilder();
                string leftRequest = TraverseTree(_queryNodes[conn.LeftChild]);
                string rightRequest = TraverseTree(_queryNodes[conn.RightChild]);
                buffer.AppendFormat("({0} {1} {2})", leftRequest, conn.FilterType.ToString(), rightRequest);

                return buffer.ToString();
            }
            else
            {
                ILeafItem leaf = item as ILeafItem;
                return BuildSearchString(leaf);
            }
        }

        private bool IndexExists(string indexDir)
        {
            if (!Directory.Exists(indexDir))
                return false;

            IndexInfo indexInfo = IndexJob.GetIndexInfo(indexDir);
            return (indexInfo.IndexSize != 0);
        }

        private string GetDatabaseName(string conString)
        {
            string conStringLower = conString.ToLower();

            Regex rgx = new Regex(@"(database\s*=)(\s*)(\w+);*");
            Match match = rgx.Match(conStringLower);
            return (match.Success) ? match.Groups[3].Value : null;
        }
        #endregion

        #region private data
        private string _indexRoot;
        private string _indexDBSubRoot;
        private string _connString;
        //private List<TableIndexInfo> _tablesInfo;

        private Dictionary<int, IQueryItem> _queryNodes = new Dictionary<int, IQueryItem>();
        private Dictionary<string, QueryResult> _resultMap = new Dictionary<string, QueryResult>();
        private List<QueryResult> _results = new List<QueryResult>();
        private IndexCache _cache = null;

        private string _indexDir;
        private int _caseId = -1;
        private string _mainTable = "E-table";
        private int _maxHitCount = 0;
        private bool _skipLoadResult = false;
        private bool _orderBy = false;
        private bool _enableDtIndexCache = false;
        private bool _enableResultTableIndex = true;
        private int _pageSize = 0;

        private SearchLogger _logger;
        #endregion
    }
}
