using System.Collections.Generic;
using System.Data;

using Zen.DBMS;
using Zen.DBMS.Schema;
using Zen.DBMS.Oracle;
using Zen.Utilities.Algorithm;
using Zen.Utilities.Generics;

namespace Zen.DBAToolbox
{
    using Cell = Zen.Common.Def.Pair<int, int>;

    public sealed class OracleTblViewTree : DBTableViewTree
    {
        public static readonly string[] OracleTableGroupbyColumns = new string[] { "OWNER", "TYPE" };

        public OracleTblViewTree(IDataProvider<DataTable> tblProvider)
            : this(tblProvider, OracleTableGroupbyColumns, TableNameColumn)
        {
        }

        public OracleTblViewTree(IDataProvider<DataTable> tblProvider, IEnumerable<string> indexColumns, string leafColumn)
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
            id.Owner = row["OWNER"].ToString();
            id.SubType = row["TYPE"].ToString();

            return id;
        }

        public override DataTable Export(Cell location)
        {
            DataTable tbl = null;
            DBObjectIdentifier id = GetIdentifier(location);
            _server.Execute(string.Format("SELECT * FROM {0}", id.FullName), out tbl);
            tbl.TableName = id.Name;
            return tbl;
        }
    }

}
