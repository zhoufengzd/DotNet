using System.Xml.Serialization;
using Zen.Utilities.Generics;

namespace Zen.DBMS.Sqlite
{
    public sealed class SqliteLogin : DBLoginInfo
    {
        const int SqliteVersion = 3;
        public const string SqliteDBFile = "Sqlite Database File (*.db3)|*.db3";

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

        #region override
        protected override void PreGetConnectionString()
        {
            Macros.SafeAdd(_connProperties, "Version", SqliteVersion.ToString());
        }
        #endregion
    }
}
