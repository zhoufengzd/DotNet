using System.Data.Common;
using System.Data.SqlClient;

namespace Zen.DBMS.Schema
{
    using Macros = Zen.Utilities.Generics.Macros;

    public sealed class SchemaContext
    {
        public SchemaContext()
        {
        }

        public DBMSContext DBMSContext
        {
            get { return _dbmsCtxt; }
            set { _dbmsCtxt = value; }
        }
        public IDataTypeDictionary DataTypeDictionary
        {
            get { return _dictionary; }
            set { _dictionary = value; }
        }

        private DBMSContext _dbmsCtxt;
        private IDataTypeDictionary _dictionary;
    }

}
