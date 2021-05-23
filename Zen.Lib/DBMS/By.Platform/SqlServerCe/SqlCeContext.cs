using Zen.DBMS.Schema;

namespace Zen.DBMS.SqlCe
{
    /// <summary>
    /// SqlServer DBMSContext
    /// </summary>
    public sealed class SqlCeContext : DBMSContext
    {
        public SqlCeContext()
        {
            _dbmsPlatform = DBMSPlatformEnum.SqlCe;
            _providerName = "System.Data.SqlServerCe.3.5";
        }

        public override DBLoginInfo CreateDBLoginInfo()
        {
            return new SqlServerCeLogin();
        }

        public override IDataSource CreateDatasource(string connString)
        {
            return new SqlCeInstance(this, connString);
        }
    }
}
