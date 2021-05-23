using System.Xml.Serialization;
using Zen.Utilities.Generics;

namespace Zen.DBMS.DB2
{
    /// <summary>
    /// Server=myAddress:myPortNumber;Database=myDataBase;UID=myUsername;PWD=myPassword;
    /// </summary>
    public sealed class DB2Login : DBLoginInfo
    {
        public DB2Login()
        {
        }

        public override string DataSource
        {
            get { return Macros.SafeGet(_connProperties, ConnKWD.Server, null); }
            set { Macros.SafeAdd(_connProperties, ConnKWD.Server, value); }
        }
        public string Database
        {
            get { return Macros.SafeGet(_connProperties, ConnKWD.Database, null); }
            set { Macros.SafeAdd(_connProperties, ConnKWD.Database, value); }
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
