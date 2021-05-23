using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Zen.DBMS.Schema;
using System.Collections.Generic;

namespace Zen.DBMS
{
    /// <summary>
    /// DBMSContext / DBMSFactory 
    /// Design thought: 
    ///   Use OleDB as much as possible.
    ///   Use platform dependent ADO.Net as needed, e.g., SqlServer bulk load.
    /// </summary>
    public sealed class DBMSFactory
    {
        public static DBMSContext CreateDBMSContext(DBMSPlatformEnum platform)
        {
            switch (platform)
            {
                case DBMSPlatformEnum.Access:
                    return new Zen.DBMS.Access.AccessContext();
                case DBMSPlatformEnum.DB2:
                    return new Zen.DBMS.DB2.DB2Context();
                case DBMSPlatformEnum.MySql:
                    return new Zen.DBMS.MySql.MySqlContext();
                case DBMSPlatformEnum.SqlServer:
                    return new Zen.DBMS.SqlServer.SqlSvrContext();
                case DBMSPlatformEnum.SqlCe:
                    return new Zen.DBMS.SqlCe.SqlCeContext();
                case DBMSPlatformEnum.Sqlite:
                    return new Zen.DBMS.Sqlite.SqliteContext();
                case DBMSPlatformEnum.Oracle:
                    return new Zen.DBMS.Oracle.OracleContext();
                case DBMSPlatformEnum.OleDB:
                    return new OleDBContext();
                case DBMSPlatformEnum.ODBC:
                    return new ODBCContext();

                default:
                    return null;
            }
        }

        public static DataTable GetDBMSProviders()
        {
            return DbProviderFactories.GetFactoryClasses();
        }
    }

    /// <summary>
    /// DBMSContext: Static context per dbms. Extends beyond provider factory.
    ///   Platform Type
    ///   DbProviderFactory
    ///   Additional Services:
    ///     Quote Name
    /// </summary>
    public class DBMSContext : IDBQuoter
    {
        public const string StringLiteralFmt = "'{0}'";
        public const string IdentifierFmt = "\"{0}\"";

        public DBMSContext()
        {
        }

        public DBMSPlatformEnum Platform
        {
            get { return _dbmsPlatform; }
        }

        public virtual DbProviderFactory ProviderFactory
        {
            get
            {
                if (_providerFactory == null)
                    _providerFactory = DbProviderFactories.GetFactory(_providerName);
                return _providerFactory;
            }
        }

        public virtual List<string> GetDataSources()
        {
            return null;
        }

        public virtual DBLoginInfo CreateDBLoginInfo()
        {
            return null;
        }

        public virtual IDataSource CreateDatasource(string connString)
        {
            return new DataSourceInstance(this, connString);
        }

        public IEnumerable<string> ConnPropertyNames
        {
            get
            {
                if (_conPropertyKeys == null)
                {
                    _conPropertyKeys = new List<string>();
                    foreach (string key in ProviderFactory.CreateConnectionStringBuilder().Keys)
                        _conPropertyKeys.Add(key);
                }

                return _conPropertyKeys;
            }
        }

        public virtual string QuoteName(string strRawName)
        {
            return string.Format(IdentifierFmt, strRawName.Replace("\"", "\"\""));
        }

        public virtual string QuoteValue(string strRawValue)
        {
            return string.Format(DBMSContext.StringLiteralFmt, strRawValue.Replace("'", "''"));
        }

        #region protected data
        protected DBMSPlatformEnum _dbmsPlatform = DBMSPlatformEnum.Generic;
        protected string _providerName;
        protected DbProviderFactory _providerFactory;
        #endregion

        #region private data
        private List<string> _conPropertyKeys;
        #endregion
    }


    /// <summary>
    /// OleDB Context
    /// </summary>
    public class OleDBContext : DBMSContext
    {
        public OleDBContext()
        {
            _dbmsPlatform = DBMSPlatformEnum.OleDB;
            _providerName = "System.Data.OleDb";
        }
    }

    /// <summary>
    /// ODBC Context
    /// </summary>
    public class ODBCContext : DBMSContext
    {
        public ODBCContext()
        {
            _dbmsPlatform = DBMSPlatformEnum.ODBC;
            _providerName = "System.Data.ODBC";
        }
    }
}
