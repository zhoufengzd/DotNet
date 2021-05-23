using System.Collections.Generic;
using System.Data;

using Zen.DBMS;
using Zen.DBMS.Schema;
using Zen.DBMS.DB2;
using Zen.Utilities.Algorithm;
using Zen.Utilities.Generics;

namespace Zen.DBAToolbox
{
    using Cell = Zen.Common.Def.Pair<int, int>;

    public sealed class DB2TblViewTree : DBTableViewTree
    {
        public DB2TblViewTree(IDataProvider<DataTable> tblProvider)
            : this(tblProvider, TableGroupbyColumns, TableNameColumn)
        {
        }

        public DB2TblViewTree(IDataProvider<DataTable> tblProvider, IEnumerable<string> indexColumns, string leafColumn)
            : base(tblProvider, indexColumns, leafColumn)
        {
        }

        public override DBObjectIdentifier GetIdentifier(Cell location)
        {
            DataRow row = _tblViewer.DataTable.Rows[location.Row];

            DB2Identifier id = new DB2Identifier();
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
            DataTable tbl = null;
            DB2Identifier id = (DB2Identifier)GetIdentifier(location);
            _server.Execute(string.Format("SELECT * FROM {0}", id.FullName), out tbl);
            tbl.TableName = id.Name;
            return tbl;
        }
    }

}
