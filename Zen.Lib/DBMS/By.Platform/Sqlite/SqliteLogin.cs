using System.Xml.Serialization;
using Zen.Utilities.Generics;

namespace Zen.DBMS.Sqlite
{
    /// <summary>
    /// Data Source=filename;Version=3;Password=myPassword;
    /// </summary>
    public sealed class SqliteLogin : DBLoginInfo
    {
        const string SqliteVersion = "3";
        const string SqliteDBFile = "Sqlite Database File (*.db3)|*.db3";

        [Zen.Common.Def.Layout.FileBrowserHint(Label = "Database File", Filter = SqliteDBFile)]
        public override string DataSource
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
        protected override void AdjustConnProperties()
        {
            Macros.SafeAdd(_connProperties, "Version", SqliteVersion);
        }
        #endregion
    }
}
