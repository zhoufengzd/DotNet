using Zen.Utilities.Generics;

namespace Zen.DBMS.SqlCe
{
    public sealed class SqlServerCeLogin : DBLoginInfo
    {
        const string KwdEncrypt = "Encrypt";
        const string SqlceDBFile = "SQL Server Compact Edition Database File (*.sdf)|*.sdf";

        [Zen.Common.Def.Layout.FileBrowserHint(Label = "Database File", Filter = SqlceDBFile)]
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
        protected override void AdjustConnProperties()
        {
            Macros.SafeAdd(_connProperties, "Persist Security Info", "False");
        }
        #endregion
    }
}
