using System;
using Zen.Utilities.Generics;

namespace Zen.DBMS.MySql
{
    /// <summary>
    /// Server=myServerAddress;Port=3306;Database=myDataBase;Uid=myUsername;Pwd=myPassword;
    /// </summary>
    public sealed class MySqlLogin : DBLoginInfo
    {
        public const string DefaultPort = "3306";

        [Zen.Common.Def.Layout.LayoutHint(Label = "Server")]
        public override string DataSource
        {
            get { return Macros.SafeGet(_connProperties, ConnKWD.Server, null); }
            set { Macros.SafeAdd(_connProperties, ConnKWD.Server, value); }
        }
        public int Port
        {
            get { return Int32.Parse(Macros.SafeGet(_connProperties, ConnKWD.Port, DefaultPort)); }
            set { Macros.SafeAdd(_connProperties, ConnKWD.Port, value.ToString()); }
        }
        public string UserId
        {
            get { return Macros.SafeGet(_connProperties, ConnKWD.UID, null); }
            set { Macros.SafeAdd(_connProperties, ConnKWD.UID, value); }
        }
        public string Password
        {
            get { return Macros.SafeGet(_connProperties, ConnKWD.PWD, null); }
            set { Macros.SafeAdd(_connProperties, ConnKWD.PWD, value); }
        }
    }
}
