using Zen.Utilities.Generics;

namespace Zen.DBMS.SqlServer
{
    /// <summary>
    /// Main purpose: 
    ///   Exchange info as:
    ///     DBLoginInfo <==> DB Login Dialog
    ///   Serialization  
    /// </summary>
    public sealed class SqlSvrLogin : DBLoginInfo
    {
        public SqlSvrLogin()
        {
        }

        public string DataSource
        {
            get { return Macros.SafeGet(_connProperties, ConnKWD.DataSource, null); }
            set { Macros.SafeAdd(_connProperties, ConnKWD.DataSource, value); }
        }
        public string Database
        {
            get { return Macros.SafeGet(_connProperties, ConnKWD.InitialCatalog, null); }
            set { Macros.SafeAdd(_connProperties, ConnKWD.InitialCatalog, value); }
        }
        public string UserId
        {
            get { return Macros.SafeGet(_connProperties, ConnKWD.UserId, null); }
            set { Macros.SafeAdd(_connProperties, ConnKWD.UserId, value); }
        }
        public string Password
        {
            get { return Macros.SafeGet(_connProperties, ConnKWD.Password, null); }
            set { Macros.SafeAdd(_connProperties, ConnKWD.Password, value); }
        }

        #region override
        protected override void PreGetConnectionString()
        {
            if (string.IsNullOrEmpty(_connProperties[ConnKWD.UserId]))
                Macros.SafeAdd(_connProperties, "Integrated Security", "true");
        }
        #endregion
    }

}
