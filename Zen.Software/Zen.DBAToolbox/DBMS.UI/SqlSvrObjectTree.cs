using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Zen.DBMS;
using Zen.DBMS.Schema;
using Zen.DBMS.SqlServer;
using Zen.Utilities.Algorithm;
using Zen.Utilities.Generics;

namespace Zen.DBAToolbox
{
    using Cell = Zen.Common.Def.Pair<int, int>;

    /// <summary>
    /// Combined DBObjectTree
    /// </summary>
    class SqlSvrTree : IDataTree<string, Cell>
    {
        public SqlSvrTree(IDataSource svr)
            : this(svr, DBTableViewTree.TableGroupbyColumns, DBTableViewTree.TableNameColumn)
        {
        }

        public SqlSvrTree(IDataSource svr, IEnumerable<string> indexColumns, string leafColumn)
        {
            _server = (SqlSvrInstance)svr;
            _indexColumns = indexColumns;
            _leafColumn = leafColumn;
        }

        public DataNode<string, Cell> Root
        {
            get { BuildTree(); return _root; }
        }

        public IEnumerable<IDataTableTree> SubTrees
        {
            get { BuildTree(); return _databaseTrees; }
        }

        #region private functions
        private void BuildTree()
        {
            if (_root != null)
                return;

            IEnumerable<string> dbNames = _server.Databases;
            _root = new DataNode<string, Cell>(_server.DataSource);
            _databaseTrees = new List<IDataTableTree>();
            foreach (string db in dbNames)
            {
                SqlSvrTblViewTree tree = new SqlSvrTblViewTree(db, _server.GetTableLister(db), _indexColumns, _leafColumn);
                tree.DataSource = _server;

                DataNode<string, Cell> dbRoot = tree.Root;
                if (dbRoot.Children.Count < 1)
                    continue;

                dbRoot.Id = db;
                _root.Children.Add(db, dbRoot);
                dbRoot.Parent = _root;

                _databaseTrees.Add(tree);
            }
        }
        #endregion

        #region private data
        private SqlSvrInstance _server;
        private IEnumerable<string> _indexColumns;
        private string _leafColumn;

        private DataNode<string, Cell> _root;
        private List<IDataTableTree> _databaseTrees;
        #endregion
    }

    public sealed class SqlSvrTblViewTree : DBTableViewTree
    {
        public SqlSvrTblViewTree(string database, IDataProvider<DataTable> tblProvider)
            : this(database, tblProvider, TableGroupbyColumns, TableNameColumn)
        {
        }

        public SqlSvrTblViewTree(string database, IDataProvider<DataTable> tblProvider, IEnumerable<string> indexColumns, string leafColumn)
            : base(tblProvider, indexColumns, leafColumn)
        {
            _database = database;
        }

        public override DBObjectIdentifier GetIdentifier(Cell location)
        {
            DataRow row = _tblViewer.DataTable.Rows[location.Row];

            SqlSvrIdentifier id = new SqlSvrIdentifier();
            id.Context = _server.DBMSContext;
            id.Type = DBObjectEnum.Table;
            id.Name = row["TABLE_NAME"].ToString();
            id.Database = row["TABLE_CATALOG"].ToString();
            id.Owner = row["TABLE_SCHEMA"].ToString();
            id.SubType = row["TABLE_TYPE"].ToString();

            return id;
        }

        public override DataTable Export(Cell location)
        {
            //SqlConnection conn = (SqlConnection)_server.OpenNewConnection();
            //conn.ChangeDatabase(_database);

            //TableSchema ts = new TableSchema(conn, GetIdentifier(location).FullName);
            //TableAdapterContext ctx = new TableAdapterContext(ts, conn);
            //TableAdapterEditor adapter = new TableAdapterEditor(ctx);
            //adapter.Fill();
            //return adapter.DataTable;

            DataTable tbl = null;
            SqlSvrIdentifier id = (SqlSvrIdentifier)GetIdentifier(location);
            //((SqlSvrInstance)_server).ChangeDatabase(_database);
            _server.Execute(string.Format("SELECT * FROM {0}", id.FullName), out tbl);
            tbl.TableName = id.Name;
            return tbl;
        }

        #region private functions
        #endregion

        #region private data
        private string _database;
        #endregion
    }

}
