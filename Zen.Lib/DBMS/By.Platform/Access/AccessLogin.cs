using Zen.Utilities.Generics;

namespace Zen.DBMS.Access
{
    /// <summary>
    /// Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\mydatabase.mdb;User Id=admin;Password=;
    /// </summary>
    public sealed class AccessLogin : DBLoginInfo
    {
        const string AccessDBFile = "Access Database File (*.mdb)|*.mdb";

        public AccessLogin()
        {
            Macros.SafeAdd(_connProperties, "Provider", "Microsoft.Jet.OLEDB.4.0");
        }

        [Zen.Common.Def.Layout.FileBrowserHint(Label = "Database File", Filter = AccessDBFile)]
        public override string DataSource
        {
            get { return Macros.SafeGet(_connProperties, ConnKWD.DataSource, null); }
            set { Macros.SafeAdd(_connProperties, ConnKWD.DataSource, value); }
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
    }
}
