using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Zen.DBMS
{
    //public class ConnectionInfo
    //{
    //    public string DataSource;
    //    public string InitialCatalog;

    //    public string UserID;
    //    public string Password;
    //}

    public sealed class ConnectionInfo
    {
        public ConnectionInfo()
        {
        }
        public ConnectionInfo(string connString)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(connString);
            _dataSource = sqlBuilder.DataSource;
            if (!String.IsNullOrEmpty(sqlBuilder.InitialCatalog))
                this.InitialCatalog = sqlBuilder.InitialCatalog;

            _userId = sqlBuilder.UserID;
            _password = sqlBuilder.Password;
        }
        public ConnectionInfo(ConnectionInfo logIn)
        {
            _dataSource = logIn.DataSource;
            _initialCatalog = logIn.InitialCatalog;

            _userId = logIn._userId;
            _password = logIn.Password;
        }

        public string DataSource
        {
            get { return _dataSource; }
            set { _dataSource = value; }
        }
        public string InitialCatalog
        {
            get { return _initialCatalog; }
            set { _initialCatalog = value; }
        }
        public string UserID
        {
            get { return _userId; }
            set { _userId = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public override string ToString()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.DataSource = _dataSource;
            if (this.InitialCatalog != null)
                sqlBuilder.InitialCatalog = this.InitialCatalog;
            if (_userId != null)
                sqlBuilder.UserID = _userId;
            if (_password != null)
                sqlBuilder.Password = _password;

            if (sqlBuilder.UserID.Length < 1)
                sqlBuilder.IntegratedSecurity = true;

            return sqlBuilder.ConnectionString;
        }

        #region private data
        private string _dataSource;
        private string _initialCatalog;
        private string _userId;
        private string _password;
        #endregion
    }
}
