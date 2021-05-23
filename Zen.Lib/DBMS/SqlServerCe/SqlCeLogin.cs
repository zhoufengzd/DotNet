using System.Xml.Serialization;
using Zen.Utilities.Generics;

namespace Zen.DBMS.SqlCe
{
    public sealed class SqlServerCeLogin : DBLoginInfo
    {
        const int SqliteVersion = 3;
        public const string KwdEncrypt = "Encrypt";
        public const string SqliteDBFile = "SQL Server Compact Edition Database File (*.sdf)|*.sdf";

        //[FileDirBrowserHint(BrowserMode.File, null, SqliteDBFile)]
        public string DatabaseFile
        {
            get { return Macros.SafeGet(_connProperties, ConnKWD.DataSource, null); }
            set { Macros.SafeAdd(_connProperties, ConnKWD.DataSource, value); }
        }
        public string Password
        {
            get { return Macros.SafeGet(_connProperties, ConnKWD.Password, null); }
            set { Macros.SafeAdd(_connProperties, ConnKWD.Password, value); }
        }
        public bool Encrypted
        {
            get 
            {
                bool encrypted = false;
                bool.TryParse(Macros.SafeGet(_connProperties, KwdEncrypt, "false"), out encrypted); 
                return encrypted;
            }
            set
            {
                Macros.SafeAdd(_connProperties, KwdEncrypt, value ? "true" : "false"); 
            }
        }

        #region override
        protected override void PreGetConnectionString()
        {
            Macros.SafeAdd(_connProperties, "Persist Security Info", "False");
        }
        #endregion
    }
}
