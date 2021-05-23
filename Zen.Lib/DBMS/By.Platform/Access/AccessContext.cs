using System.Collections.Generic;
using Zen.DBMS.Schema;

namespace Zen.DBMS.Access
{
    /// <summary>
    /// Access DBMSContext
    /// </summary>
    public sealed class AccessContext : OleDBContext
    {
        public new const string StringLiteralFmt = "\"{0}\"";

        public AccessContext()
        {
            _dbmsPlatform = DBMSPlatformEnum.Access;
        }

        public override DBLoginInfo CreateDBLoginInfo()
        {
            return new AccessLogin();
        }

        //public override IDataSource CreateDatasource(string connString)
        //{
        //    return new AccessInstance(this, connString);
        //}

        public override string QuoteName(string strRawName)
        {
            return CHAR.LEFTBRACKET + strRawName + CHAR.RIGHTBRACKET;
        }

        public override string QuoteValue(string strRawValue)
        {
            return string.Format(AccessContext.StringLiteralFmt, strRawValue.Replace("\"", "\"\""));
        }


        // Could be extended to read from a known location / file
        public override List<string> GetDataSources()
        {
            List<string> dataSource = new List<string>();
            return dataSource;
        }
    }
}
