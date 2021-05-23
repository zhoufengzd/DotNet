using System.Collections.Generic;
using System.Data;

using Zen.DBMS.Schema;
using Zen.Utilities.Algorithm;
using System.Data.Common;

namespace Zen.DBAToolbox
{
    using Cell = Zen.Common.Def.Pair<int, int>;

    public sealed class SqliteObjectTree : DBTableViewTree
    {
        public SqliteObjectTree(IDataProvider<DataTable> tblProvider)
            :this(tblProvider, TableGroupbyColumns, TableNameColumn)
        {
        }

        public SqliteObjectTree(IDataProvider<DataTable> tblProvider, IEnumerable<string> indexColumns, string leafColumn)
            : base(tblProvider, indexColumns, leafColumn)
        {
        }

        public override DBObjectIdentifier GetIdentifier(Cell location)
        {
            DataRow row = _tblViewer.DataTable.Rows[location.Row];

            DBObjectIdentifier id = new DBObjectIdentifier();
            id.Context = _server.DBMSContext;
            id.Type = DBObjectEnum.Table;
            id.Name = row["TABLE_NAME"].ToString();
            id.Owner = row["TABLE_SCHEMA"].ToString();
            id.SubType = row["TABLE_TYPE"].ToString();

            return id;
        }

        public override DataTable Export(Cell location)
        {
            DataTable tbl = null;
            DBObjectIdentifier id = GetIdentifier(location);
            _server.Execute(string.Format("SELECT * FROM {0}", id.Name), out tbl);
            tbl.TableName = id.Name;
            return tbl;
        }

        #region private functions
        #endregion

        #region private data
        #endregion
    }

}
