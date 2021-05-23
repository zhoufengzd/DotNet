using System.Collections.Generic;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Zen.DBMS.Schema;

namespace Zen.DBMS.MySql
{
    /// <summary>
    /// MySql DBMSContext
    /// </summary>
    public sealed class MySqlContext : DBMSContext
    {
        public new const string IdentifierFmt = "`{0}`";

        public MySqlContext()
        {
            _dbmsPlatform = DBMSPlatformEnum.MySql;
            _providerName = "MySql.Data.MySqlClient";
        }

        public override DBLoginInfo CreateDBLoginInfo()
        {
            return new MySqlLogin();
        }

        public override DbProviderFactory ProviderFactory
        {
            get { return new MySqlClientFactory(); }
        }

        public override string QuoteName(string strRawName)
        {
            return string.Format(MySqlContext.IdentifierFmt, strRawName.Replace("`", "``"));
        }

        // Could be extended to read from a known location / file
        public override List<string> GetDataSources()
        {
            List<string> dataSource = new List<string>();
            return dataSource;
        }
    }
}
