using System.Collections.Generic;
using System.Data;
using Zen.Common.Def;
using Zen.DBMS;
using Zen.DBMS.Schema;
using Zen.Utilities.Algorithm;

namespace Zen.DBAToolbox
{
    using Cell = Pair<int, int>;

    interface IDBObjectTree : IDataTableTree
    {
        IDataSource DataSource { get; set; }
        DBObjectIdentifier GetIdentifier(Cell location);

        DataTable Export(Cell location);
    }

    /// <summary>
    /// Model: table / view list (provided in DataTable format)
    /// View: TableViewExplorer
    /// Controller: DBObjectTree
    /// </summary>
    public class DBTableViewTree : DataTableTree, IDBObjectTree
    {
        public static readonly string[] TableGroupbyColumns = new string[] { "TABLE_SCHEMA", "TABLE_TYPE" };
        public static readonly string TableNameColumn = "TABLE_NAME";

        public DBTableViewTree(IDataProvider<DataTable> tblProvider)
            : this(tblProvider, TableGroupbyColumns, TableNameColumn)
        {
        }

        public DBTableViewTree(IDataProvider<DataTable> tblProvider, IEnumerable<string> indexColumns, string leafColumn)
            : base(tblProvider, indexColumns, leafColumn)
        {
        }

        public virtual DBObjectIdentifier GetIdentifier(Cell location)
        {
            DBObjectIdentifier id = new DBObjectIdentifier();
            id.Name = ToString(location);
            id.Context = _server.DBMSContext;
            return id;
        }

        public IDataSource DataSource
        {
            get { return _server; }
            set { _server = value; }
        }

        public virtual DataTable Export(Cell location)
        {
            return null;
        }

        #region private data
        protected IDataSource _server;
        #endregion
    }

}
