using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Zen.DBMS
{
    using Macros = Zen.Utilities.Generics.Macros;

    public interface IDBConnBuilder
    {
        DbConnection OpenNewConnection();
    }

    /// <summary>
    /// Connection Manager:
    ///    Single server instance connection pool manager.
    ///    To do: handle time out and other connection error issue
    /// </summary>
    public sealed class ConnPoolMgr
    {
        public ConnPoolMgr(IDBConnBuilder conBuilder)
        {
            _connectionBlder = conBuilder;
            _conStack = new Stack<DbConnection>();
        }

        public void CheckIn(DbConnection item)
        {
            _conStack.Push(item);
        }

        public DbConnection CheckOut()
        {
            DbConnection conn = null;
            if (_conStack.Count < 1)
            {
                conn = _connectionBlder.OpenNewConnection();
            }
            else
            {
                conn = _conStack.Pop();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
            }

            return conn;
        }

        public void Reset()
        {
            foreach (DbConnection conn in _conStack)
                conn.Close();

            _conStack.Clear();
        }

        #region private data
        private IDBConnBuilder _connectionBlder;
        private Stack<DbConnection> _conStack;
        #endregion
    }

    /// <summary>
    /// Parse OleDB / SQL connection string 
    /// </summary>
    //internal sealed class ConnStringHelper
    //{
    //    public static DBLoginInfo ToLoginInfo(string connString, DBMSContext context)
    //    {
    //        DBLoginInfo conInfo = new DBLoginInfo();
    //        DbConnectionStringBuilder blder = context.ProviderFactory.CreateConnectionStringBuilder();

    //        conInfo.DataSource = SafeGetValue(blder, "Data Source");
    //        if (string.IsNullOrEmpty(conInfo.DataSource))
    //            conInfo.DataSource = SafeGetValue(blder, "Server");
    //        if (string.IsNullOrEmpty(conInfo.DataSource))
    //            conInfo.DataSource = SafeGetValue(blder, "Hostname");
    //        if (string.IsNullOrEmpty(conInfo.DataSource))// couldn't find Data Source
    //            return null;

    //        conInfo.InitialCatalog = SafeGetValue(blder, "Initial Catalog");
    //        if (string.IsNullOrEmpty(conInfo.InitialCatalog))
    //            conInfo.InitialCatalog = SafeGetValue(blder, "Database");

    //        conInfo.UserName = SafeGetValue(blder, "User ID");
    //        if (string.IsNullOrEmpty(conInfo.UserName))
    //            conInfo.UserName = SafeGetValue(blder, "UID");

    //        conInfo.Password = SafeGetValue(blder, "Password");
    //        if (string.IsNullOrEmpty(conInfo.Password))
    //            conInfo.Password = SafeGetValue(blder, "PWD");

    //        return conInfo;
    //    }

    //    public static string ToConnString(DBLoginInfo conInfo, DBMSContext context)
    //    {
    //        return ToConnString(conInfo, context, null);
    //    }

    //    public static string ToConnString(DBLoginInfo conInfo, DBMSContext context, Dictionary<string, string> conProperties)
    //    {
    //        if (conInfo == null || string.IsNullOrEmpty(conInfo.DataSource))
    //        {
    //            System.Diagnostics.Debug.Assert(false);
    //            return null;
    //        }

    //        DbConnectionStringBuilder blder = context.ProviderFactory.CreateConnectionStringBuilder();
    //        Macros.SafeAdd(blder, "Data Source", conInfo.DataSource);

    //        if (!string.IsNullOrEmpty(conInfo.InitialCatalog))
    //        {
    //            Macros.SafeAdd(blder, "Initial Catalog", conInfo.InitialCatalog);
    //        }

    //        if (string.IsNullOrEmpty(conInfo.UserName))
    //        {
    //            Macros.SafeAdd(blder, "Integrated Security", "true");
    //        }
    //        else
    //        {
    //            Macros.SafeAdd(blder, "User ID", conInfo.UserName);
    //            Macros.SafeAdd(blder, "Password", conInfo.Password);
    //        }

    //        if (conProperties != null)
    //        {
    //            foreach (KeyValuePair<string, string> kv in conProperties)
    //                Macros.SafeAdd(blder, kv.Key, kv.Value);
    //        }

    //        return blder.ConnectionString;
    //    }

    //    #region private functions
    //    private static string SafeGetValue(DbConnectionStringBuilder builder, string keyName)
    //    {
    //        object objValue = Macros.SafeGet(builder, keyName, string.Empty);
    //        if (objValue == null)
    //            return string.Empty;

    //        return objValue.ToString();
    //    }
    //    #endregion
    //}
}
