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
    public class DBObjectIdentifier
    {
        public DBObjectIdentifier()
            :this(null, DBObjectEnum.Unknown, null)
        {
        }
        public DBObjectIdentifier(string strObjName, DBObjectEnum type, DBMSContext context)
        {
            _strObjName = strObjName;
            _objType = type;
            _context = context;
        }

        public string Name
        {
            get { return _strObjName; }
            set { _strObjName = value; }
        }

        public DBObjectEnum Type
        {
            get { return _objType; }
            set { _objType = value; }
        }
        public string SubType
        {
            get { return _objSubTypeName; }
            set { _objSubTypeName = value; }
        }

        public string Parent
        {
            get { return _strParentName; }
            set { _strParentName = value; }
        }
        public string Owner
        {
            get { return _strOwnerName; }
            set { _strOwnerName = value; }
        }

        public DBMSContext Context
        {
            get { return Macros.SafeGet<DBMSContext>(ref _context); }
            set { _context = value; }
        }

        /// <summary>
        /// Returns quoted fully qualified name
        ///  == [database] + [owner] + [parent] + [object name]
        /// </summary>
        public virtual string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(_strObjName))
                {
                    Debug.Assert(false);
                    return string.Empty;
                }

                StringBuilder fullName = new StringBuilder();
                if (!string.IsNullOrEmpty(_strOwnerName))
                    fullName.AppendFormat("{0}.", Context.QuoteName(_strOwnerName));

                if (!string.IsNullOrEmpty(_strParentName))
                    fullName.AppendFormat("{0}.", Context.QuoteName(_strParentName));
                fullName.Append(Context.QuoteName(_strObjName));

                return fullName.ToString();
            }
        }

        #region internal data section
        protected string _strObjName;
        protected DBObjectEnum _objType = DBObjectEnum.Unknown;
        protected string _objSubTypeName; 	// e.g., Base Table

        // optional...
        protected string _strParentName; 	// e.g., table name for index object
        protected string _strOwnerName;

        protected DBMSContext _context;
        #endregion
    }

}
