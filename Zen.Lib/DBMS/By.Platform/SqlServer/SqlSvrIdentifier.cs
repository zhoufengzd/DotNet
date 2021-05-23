using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Zen.DBMS.Schema
{
    using Macros = Zen.Utilities.Generics.Macros; 

    /// <summary>
    ///  structure to identify database object
    /// </summary>
    public class SqlSvrIdentifier: DBObjectIdentifier
    {
        public SqlSvrIdentifier()
            :this(null, DBObjectEnum.Unknown, null)
        {
        }
        public SqlSvrIdentifier(string strObjName, DBObjectEnum type, DBMSContext context)
            :base(strObjName, type, context)
        {
        }

        public string Database
        {
            get { return _strDatabaseName; }
            set { _strDatabaseName = value; }
        }

        /// <summary>
        /// Returns quoted fully qualified name
        ///  == [database] + [owner] + [parent] + [object name]
        /// </summary>
        public override string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(_strObjName))
                {
                    Debug.Assert(false);
                    return string.Empty;
                }

                StringBuilder fullName = new StringBuilder();
                if (!string.IsNullOrEmpty(_strDatabaseName))
                    fullName.AppendFormat("{0}.", Context.QuoteName(_strDatabaseName));
                if (!string.IsNullOrEmpty(_strOwnerName))
                    fullName.AppendFormat("{0}.", Context.QuoteName(_strOwnerName));

                if (!string.IsNullOrEmpty(_strParentName))
                    fullName.AppendFormat("{0}.", Context.QuoteName(_strParentName));
                fullName.Append(Context.QuoteName(_strObjName));

                return fullName.ToString();
            }
        }

        #region private data section
        private string _strDatabaseName;
        #endregion
    }

}
