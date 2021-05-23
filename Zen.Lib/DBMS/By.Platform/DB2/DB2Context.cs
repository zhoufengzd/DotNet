using System.Collections.Generic;
using System.Data.Common;
using Zen.DBMS.Schema;

namespace Zen.DBMS.DB2
{
    /// <summary>
    /// DB2 DBMSContext
    /// </summary>
    public sealed class DB2Context : DBMSContext
    {
        public new const string StringLiteralFmt = "\"{0}\"";

        public DB2Context()
        {
            _dbmsPlatform = DBMSPlatformEnum.DB2;
            _providerName = "IBM.Data.DB2";
        }

        public override DBLoginInfo CreateDBLoginInfo()
        {
            return new DB2Login();
        }

        public override DbProviderFactory ProviderFactory
        {
            get { return IBM.Data.DB2.DB2Factory.Instance; }
        }

        public override IDataSource CreateDatasource(string connString)
        {
            return new DB2Instance(this, connString);
        }

        // Could be extended to read from a known location / file
        public override List<string> GetDataSources()
        {
            List<string> dataSource = new List<string>();
            return dataSource;
        }
    }
}
