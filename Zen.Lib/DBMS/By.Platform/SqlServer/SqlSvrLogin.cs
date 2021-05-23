using Zen.Utilities.Generics;
using System.Collections.Generic;

namespace Zen.DBMS.SqlServer
{
    /// <summary>
    /// Data Source=myServerAddress;Initial Catalog=myDataBase;User ID=myUsername;Password=myPassword;
    /// </summary>
    public sealed class SqlSvrLogin : DBLoginInfo
    {
        public override string DataSource
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
        protected override void AdjustConnProperties()
        {
            if (string.IsNullOrEmpty(_connProperties[ConnKWD.UserId]))
                Macros.SafeAdd(_connProperties, "Integrated Security", "true");
        }

        protected override void LoadAliasMap()
        {
            if (_aliasMap != null)
                return;

            _aliasMap = new Dictionary<string, string>();
            _aliasMap.Add("server", ConnKWD.DataSource);
            _aliasMap.Add("hostname", ConnKWD.DataSource);
            _aliasMap.Add("host", ConnKWD.DataSource);
            _aliasMap.Add("database", "Initial Catalog");
            _aliasMap.Add("uid", ConnKWD.UserId);
            _aliasMap.Add("pwd", ConnKWD.Password);
        }
        #endregion
    }

}
