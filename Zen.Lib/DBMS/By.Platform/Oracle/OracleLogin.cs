using System.Xml.Serialization;
using Zen.Utilities.Generics;

namespace Zen.DBMS.Oracle
{
    public sealed class OracleLogin : DBLoginInfo
    {
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
