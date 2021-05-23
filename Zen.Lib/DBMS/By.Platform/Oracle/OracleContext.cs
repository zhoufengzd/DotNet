using System.Collections.Generic;
using Zen.DBMS.Schema;

namespace Zen.DBMS.Oracle
{
    /// <summary>
    /// Oracle Context
    /// </summary>
    public class OracleContext : DBMSContext
    {
        const string MSProvider = "System.Data.OracleClient";
        const string OracleProvider = "Oracle.DataAccess.Client";

        public OracleContext()
        {
            _dbmsPlatform = DBMSPlatformEnum.Oracle;
            _providerName = OracleProvider;
        }

        public override DBLoginInfo CreateDBLoginInfo()
        {
            return new OracleLogin();
        }

        public override IDataSource CreateDatasource(string connString)
        {
            return new OracleInstance(this, connString);
        }

        // Read .ora file or...?
        public override List<string> GetDataSources()
        {
            return null;
        }

    }
}
