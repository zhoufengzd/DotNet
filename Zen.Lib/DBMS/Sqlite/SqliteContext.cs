using Zen.DBMS.Schema;
using System.Data.Common;

namespace Zen.DBMS.Sqlite
{
    /// <summary>
    /// SqlServer DBMSContext
    /// </summary>
    public sealed class SqliteContext : DBMSContext
    {
        public SqliteContext()
        {
            _dbmsPlatform = DBMSPlatformEnum.SqlServer;
            _providerName = "System.Data.Sqlite";
        }

        public override DbProviderFactory ProviderFactory
        {
            get { return new System.Data.SQLite.SQLiteFactory(); }
        }

        public override DBLoginInfo CreateDBLoginInfo()
        {
            return new SqliteLogin();
        }
    }
}
