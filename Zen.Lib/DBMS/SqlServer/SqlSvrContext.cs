using System.Collections.Generic;
using System.Data.Common;
using Zen.DBMS.Schema;
using System.Data;
using System.Data.Sql;

namespace Zen.DBMS.SqlServer
{
    /// <summary>
    /// SqlServer DBMSContext
    /// </summary>
    public sealed class SqlSvrContext : DBMSContext
    {
        public SqlSvrContext()
        {
            _dbmsPlatform = DBMSPlatformEnum.SqlServer;
            _providerName = "System.Data.SqlClient";
        }

        public override DBLoginInfo CreateDBLoginInfo()
        {
            return new SqlSvrLogin();
        }

        public override string QuoteName(string strRawName)
        {
            return CHAR.LEFTBRACKET + strRawName + CHAR.RIGHTBRACKET;
        }

        public override List<string> GetDataSources()
        {
            SqlDataSourceEnumerator enumerator = (SqlDataSourceEnumerator)(base.ProviderFactory).CreateDataSourceEnumerator();

            // ServerName, InstanceName, IsClustered, Version
            DataTable dt = enumerator.GetDataSources();
            DataRowCollection rows = dt.Rows;
            int rowCount = rows.Count;

            List<string> dataSource = new List<string>(rowCount);
            for (int i = 0; i < rowCount; i++)
            {
                string svr = (string)rows[i]["ServerName"];
                object instance = rows[i]["InstanceName"];
                if (instance != System.DBNull.Value)
                    dataSource.Add(svr + "\\" + instance.ToString());
                else
                    dataSource.Add(svr);
            }

            return dataSource;
        }
    }
}
