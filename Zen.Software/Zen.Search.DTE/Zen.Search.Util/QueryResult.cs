using System;
using System.Collections.Generic;
using System.Text;

namespace Zen.Search.Util
{
    /// <summary>
    /// Join query results (row guids from multiple source tables as needed)
    ///   May do an external file sort.
    /// </summary>
    public sealed class QueryResult : MarshalByRefObject, IQueryResult
    {
        public QueryResult()
        {
        }

        public string SourceTable
        {
            get { return _sourceTable; }
            set { _sourceTable = value; }
        }
        public string ResultTable
        {
            get { return _resultTable; }
            set { _resultTable = value; }
        }
        public int HitCount
        {
            get { return _hitCount; }
            set { _hitCount = value; }
        }

        private string _sourceTable;
        private string _resultTable = Guid.NewGuid().ToString();
        private int _hitCount;
    }
}
