using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Zen.DBMS.Schema
{
    using Delimiter = Zen.Utilities.Text.Delimiter;
    using Macros = Zen.Utilities.Generics.Macros;


    #region IDDLCreator implementor
    //////////////////////////////////////////////////////////////////////////////

    /// <summary>
    ///  Base meta data database object. 
    ///    Applies to the object that could be created by 'CREATE typeName ObjectName ..."
    ///    Design goal: keep meta object as platform independent
    /// </summary>
    public class DBObjMeta : IDDLBuilder
    {
        public DBObjMeta(string strObjName, SchemaContext context)
        {
            _identifier = new DBObjectIdentifier(strObjName, DBObjectEnum.Unknown, context.DBMSContext);
            _context = context;

            _strTypeName = null;			    // must be initialized in derived class
            _strTypeModifier = string.Empty;	// optional
        }

        public SchemaContext Context
        {
            get { return Macros.SafeGet(ref _context); }
            set { _context = value; }
        }
        public DBObjectIdentifier Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }
        public string FullName
        {
            get { return _identifier.FullName; }
        }

        public string BuildCreateDDL()
        {
            return (BuildCoreDDL() + BuildExtendedDDL());
        }

        public virtual string BuildDropDDL()
        {
            StringBuilder ddlBuffer = new StringBuilder();
            ddlBuffer.AppendFormat("{0} {1} {2}", KWD.DROP, _strTypeName, FullName);
            return ddlBuffer.ToString();
        }

        /// <summary> CREATE ... ()</summary>
        public virtual string BuildCoreDDL()
        {
            StringBuilder ddlBuffer = new StringBuilder();
            ddlBuffer.AppendFormat("{0} {1}", GetDDLHeader(), GetDDLBody());
            return ddlBuffer.ToString();
        }

        public string BuildAlterDDL(int nAction)
        {
            Debug.Assert(false, "must be implemented in subclass!");
            return string.Empty;
        }

        /// <summary> index, check constraint, privilege, etc </summary>
        public virtual string BuildExtendedDDL()
        {
            return string.Empty;
        }

        #region protected functions
        protected virtual string GetDDLHeader()
        {
            string strDDLHeader = KWD.CREATE + " ";
            if (_strTypeModifier.Length > 0)
                strDDLHeader += _strTypeModifier + " ";
            strDDLHeader += _strTypeName + " " + FullName + " ";

            return strDDLHeader;
        }

        protected virtual string GetDDLBody()
        {
            return string.Empty;
        }

        protected string TypeModifier
        {
            get { return _strTypeModifier; }
            set { _strTypeModifier = value; }
        }
        #endregion

        #region protected data section
        protected string _strTypeName;
        protected string _strTypeModifier;		// Optional: UNIQUE, CLUSTERED, BITMAP, etc
        protected DBObjectIdentifier _identifier;
        protected SchemaContext _context;
        #endregion
    }

    /// <summary> 
    /// A structure to hold data type information. 
    /// no support for UDT yet. 
    /// </summary>
    public sealed class DBDataType
    {
        public DBDataType() { }

        public DataTypeEnum TypeEnum
        {
            get { return _dataType; }
            set { _dataType = value; }
        }
        public int StringLength
        {
            get { return _stringLength; }
            set { _stringLength = value; }
        }
        public int Precision
        {
            get { return _precision; }
            set { _precision = value; }
        }
        public int Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// Platform dependent data type
        /// </summary>
        public int NativeType
        {
            get { return _nativeDataType; }
            set { _nativeDataType = value; }
        }

        public string ToString(IDataTypeDictionary typeMap)
        {
            string strTypeDDL = typeMap.ToString(_dataType) + " ";
            Debug.Assert((_stringLength == 0) || (_precision == 0));

            if (_stringLength > 0)
                strTypeDDL += "( " + _stringLength.ToString() + " ) ";
            if (_precision > 0)
                strTypeDDL += "( " + _precision.ToString() + ", " + _scale.ToString() + " ) ";

            return strTypeDDL;
        }

        #region private data
        private DataTypeEnum _dataType = DataTypeEnum.Unspecified;
        private int _nativeDataType = 0;
        private int _stringLength = 0;
        private int _precision = 0;
        private int _scale = 0;
        #endregion
    }

    /// <summary> 
    ///   Class to hold typename + type + default value 
    ///     Inherited by column definition and function/procedure parameter definition
    /// </summary>
    public class DataTypeMeta
    {
        // typeMap: platform dependent data type dictionary. Used when building DDL.
        public DataTypeMeta(string strName, DataTypeEnum enumDataType, IDataTypeDictionary typeMap)
        {
            _dbDataType = new DBDataType();
            _dbDataType.TypeEnum = enumDataType;

            _strName = strName;
            _typeMap = typeMap;

            _strDefaultValue = null;
        }

        public string Name
        {
            get { return _strName; }
        }
        public DBDataType DataType
        {
            get { return _dbDataType; }
        }
        public IDataTypeDictionary TypeDictionary
        {
            get { return _typeMap; }
        }
        public string Default
        {
            get { return _strDefaultValue; }
            set { _strDefaultValue = value; }
        }

        #region protected data section
        protected string _strName;
        protected DBDataType _dbDataType;
        protected IDataTypeDictionary _typeMap;
        protected string _strDefaultValue;	// optional
        #endregion
    }

    /// <summary> Structure to hold column meta. </summary>
    public class ColumnMeta : DataTypeMeta
    {
        public ColumnMeta(string strColName, DataTypeEnum enumDataType, IDataTypeDictionary typeMap)
            : base(strColName, enumDataType, typeMap)
        {
        }

        /// <summary> 
        /// name + type [ (precision, scale) | (length) ] + [not null]
        /// "dtColumn datetime DEFAULT getdate() NOT NULL"
        /// </summary>
        public override string ToString()
        {
            string strDDL = _strName + " " + _dbDataType.ToString(_typeMap);
            if (_strDefaultValue != null)
            {
                _strDefaultValue.Trim();
                if (_strDefaultValue.Length > 0)
                    strDDL += KWD.DEFAULT + " " + _strDefaultValue + " ";
            }

            if (_typeMap.DBMSPlatform == DBMSPlatformEnum.SqlServer)
            {
                if (_bIdentity)
                    strDDL += KWD.IDENTITY + " ";	// only support default seed, increment (1,1)
            }

            strDDL += (_bNullable ? KWD.NULL : KWD.NOT_NULL);

            return strDDL;
        }

        /// <summary> optional attributes </summary>
        public bool Nullable
        {
            get { return _bNullable; }
            set { _bNullable = value; }
        }
        public bool Identity
        {
            get { return _bIdentity; }
            set { _bIdentity = value; }
        }

        #region protected data
        protected bool _bNullable = false;
        protected bool _bIdentity = false;	// SQL Server only
        #endregion
    }

    public class TableMeta : DBObjMeta
    {
        public TableMeta(string strObjName, SchemaContext context)
            : base(strObjName, context)
        {
            _strTypeName = KWD.TABLE;
            _colMetaList = new List<ColumnMeta>();
        }

        public List<ColumnMeta> ColMetaList
        {
            get { return _colMetaList; }
        }

        public ColumnMeta GetColMeta(int nIndex)
        {
            return (_colMetaList[nIndex]);
        }
        public ColumnMeta GetColMeta(string strColName)
        {
            ColumnMeta colMetaTmp = null;
            for (int nInd = 0; nInd < _colMetaList.Count; nInd++)
            {
                colMetaTmp = _colMetaList[nInd];
                if (colMetaTmp.Name == strColName)
                    return colMetaTmp;
            }
            return null;
        }

        #region protected memeber function
        protected override string GetDDLBody()
        {
            return "( " + GetColumnDDL() + " ) ";
        }

        protected string GetColumnDDL()
        {
            return Delimiter.ToString(_colMetaList);
        }
        #endregion

        #region protected data section
        private List<ColumnMeta> _colMetaList;
        private bool _bFullTextIndexed = false;	// SQL Server only
        #endregion
    }

    public class ViewMeta : DBObjMeta
    {
        public ViewMeta(string strObjName, SchemaContext context)
            : base(strObjName, context)
        {
            _strTypeName = KWD.VIEW;
            _strDDLBodyText = string.Empty;
        }

        public string DDLBodyText
        {
            get { return _strDDLBodyText; }
            set { _strDDLBodyText = value; }
        }

        // expose columns to client
        public List<string> GetColumnList()
        {
            if (_columnNames == null)
                _columnNames = new List<string>();
            return _columnNames;
        }

        #region protected memeber function
        protected override string GetDDLBody()
        {
            string strDDL = string.Empty;
            if ((_columnNames != null) && (_columnNames.Count > 0))
                strDDL = "( " + Delimiter.ToString(_columnNames) + " ) ";
            strDDL += " AS \n" + _strDDLBodyText;

            return strDDL;
        }
        #endregion

        #region protected data section
        protected List<string> _columnNames;		// optional 
        protected string _strDDLBodyText;
        #endregion
    }

    public class IndexMeta : DBObjMeta
    {
        public IndexMeta(string strObjName, string strTableName, SchemaContext context)
            : base(strObjName, context)
        {
            _strTypeName = KWD.INDEX;
            _columnNames = new List<string>();

            _identifier.Parent = strTableName;
        }

        public override string BuildDropDDL()
        {
            string strDropDDL = KWD.DROP + " " + _strTypeName + " " + _identifier.FullName;
            return strDropDDL;
        }

        public List<string> GetColumnList()
        {
            return _columnNames;
        }

        #region protected memeber function
        protected override string GetDDLBody()
        {
            Debug.Assert(_columnNames.Count > 0);
            string strDDL = "ON " + _identifier.Parent + " ( ";
            strDDL += Delimiter.ToString(_columnNames);
            strDDL += " ) ";

            return strDDL;
        }
        #endregion

        #region protected data section
        protected List<string> _columnNames;	// only column names
        #endregion
    }

    /// <summary> Structure to hold parameter data. </summary>
    public class ParameterMeta : DataTypeMeta
    {
        public ParameterMeta(string strParamName,
                              DataTypeEnum enumDataType,
                              ParameterDirectionEnum enumDirection,
                              IDataTypeDictionary typeMap)
            : base(strParamName, enumDataType, typeMap)
        {
            _eDirection = enumDirection;
        }

        // name + type [ (precision, scale) | (length) ] + [OUTPUT]
        // SQLServer: "@mesg1 VARCHAR, @mesg2 VARCHAR OUTPUT, @mesg3 VARCHAR OUTPUT"
        // Oracle: "mesg1 IN VARCHAR2, mesg2 OUT VARCHAR2, mesg3 IN OUT VARCHAR2"
        public override string ToString()
        {
            // Pre: check default value
            bool bHasDefaultValue = false;
            if (_strDefaultValue != null)
            {
                _strDefaultValue.Trim();
                bHasDefaultValue = (_strDefaultValue.Length > 0);
                Debug.Assert((!bHasDefaultValue) || (_eDirection == ParameterDirectionEnum.INPUT));
            }

            // Build it up
            string strDDL = _strName + " ";
            if (_typeMap.DBMSPlatform == DBMSPlatformEnum.SqlServer)
            {
                strDDL += _dbDataType.ToString(_typeMap);

                if (_eDirection == ParameterDirectionEnum.OUTPUT)
                    strDDL += KWD.OUTPUT;

                if (bHasDefaultValue)
                    strDDL += " = " + _strDefaultValue + " ";
            }
            else if (_typeMap.DBMSPlatform == DBMSPlatformEnum.Oracle)
            {
                strDDL += (_eDirection == ParameterDirectionEnum.OUTPUT) ? KWD.OUTPUT : KWD.IN;
                strDDL += _dbDataType.ToString(_typeMap);

                if (bHasDefaultValue)
                    strDDL += KWD.DEFAULT + " " + _strDefaultValue + " ";
            }

            return strDDL;
        }

        #region protected data section
        protected ParameterDirectionEnum _eDirection;
        #endregion
    }

    public class StoredProcMeta : DBObjMeta
    {
        public StoredProcMeta(string strObjName, SchemaContext context)
            : base(strObjName, context)
        {
            _strTypeName = KWD.PROCEDURE;
            _paramList = new List<ParameterMeta>();
            _strDDLBodyText = string.Empty;
        }

        public List<ParameterMeta> GetParameterList()
        {
            return _paramList;
        }
        public string DDLBodyText
        {
            get { return _strDDLBodyText; }
            set { _strDDLBodyText = value; }
        }

        #region protected memeber function
        protected override string GetDDLHeader()
        {
            string strDDLHeader = base.GetDDLHeader();

            string strParameters = GetParametersDDL();
            if (strParameters.Length > 0)
                strDDLHeader += "( " + strParameters + " ) ";

            strDDLHeader += KWD.AS + " \n";

            return strDDLHeader;
        }

        protected string GetParametersDDL()
        {
            return Delimiter.ToString(_paramList);
        }

        protected override string GetDDLBody()
        {
            return _strDDLBodyText;
        }
        #endregion

        #region protected data section
        protected List<ParameterMeta> _paramList;	// parameter list
        protected string _strDDLBodyText;
        #endregion
    }

    public class FunctionMeta : StoredProcMeta
    {
        // CREATE FUNCTION [schema.]function [arguments_clause]
        // RETURN datatype [invoke_clause]
        public FunctionMeta(string strObjName, DataTypeEnum enumRetDataType, SchemaContext context)
            : base(strObjName, context)
        {
            _strTypeName = KWD.FUNCTION;
            _returnType.TypeEnum = enumRetDataType;
        }

        public DBDataType DataType
        {
            get { return _returnType; }
        }

        #region protected memeber function

        // "CREATE FUNCTION" + 
        // Oracle:    "get_su_major( inmajor varchar2   ) RETURN NUMBER AS su_paid number "
        // SQLServer: "get_su_major(@inmajor varchar(40)) RETURNS money AS "
        protected override string GetDDLHeader()
        {
            StringBuilder strDDLHeader = new StringBuilder();
            strDDLHeader.AppendFormat("{0}( {1} ) {2} {3} {4} \n", base.GetDDLHeader(), GetParametersDDL(),
                (_context.DBMSContext.Platform == DBMSPlatformEnum.SqlServer)? KWD.RETURN: KWD.RETURNS, _returnType.ToString(), KWD.AS);

            return strDDLHeader.ToString();
        }
        #endregion

        #region protected data section
        protected DBDataType _returnType;
        #endregion
    }

    #endregion IDDLCreator implementor


    #region IDDLAlterer implementor
    //////////////////////////////////////////////////////////////////////////////

    // Alter Table....
    public class TableAlter : IDDLAlterer
    {
        #region public interface
        public TableAlter(TableMeta tableObj)
        {
            _table = tableObj;
        }

        public string BuildAlterDDL(int nAction)
        {
            string strAlterDDL = "ALTER TABLE " + _table.FullName + " ";
            strAlterDDL += BuildAlterDDLBody(nAction);

            return strAlterDDL;
        }

        public string BuildAlterDDL()
        {
            return BuildAlterDDL(_nAction);
        }
        public int AlterAction
        {
            get { return _nAction; }
            set { _nAction = value; }
        }
        #endregion

        #region protected memeber function
        protected virtual string BuildAlterDDLBody(int nAction)
        {
            return string.Empty;
        }
        #endregion

        #region protected data section
        protected TableMeta _table;
        protected int _nAction;
        #endregion
    }

    public class ConstraintAlter : TableAlter
    {
        #region public interface
        public ConstraintAlter(TableMeta tableObj, string strObjName)
            : base(tableObj)
        {
            _strObjName = strObjName;
            _strTypeModifier = string.Empty;
            _columnNames = new List<string>();
        }

        public List<string> GetColumnList()
        {
            return _columnNames;
        }
        #endregion

        #region protected memeber function
        protected override string BuildAlterDDLBody(int nAction)
        {
            string strDDL = string.Empty;
            if ((DDLActionEnum)nAction == DDLActionEnum.DROP)
            {
                strDDL = KWD.DROP + " " + KWD.CONSTRAINT + " " + _strObjName;
            }
            else if ((DDLActionEnum)nAction == DDLActionEnum.CREATE)
            {
                strDDL = KWD.ADD + " " + KWD.CONSTRAINT + " " + _strObjName + " " + _strTypeModifier + " ( ";
                strDDL += Delimiter.ToString(_columnNames);
                strDDL += " ) ";
            }
            else
            {
                Debug.Assert(false, "Invalid alter constrainter action requested!");
            }

            return strDDL;
        }
        #endregion

        #region protected data section
        protected string _strTypeModifier;	// Primary Key, Foreign Key, Unique Key
        protected string _strObjName;
        protected List<string> _columnNames;
        #endregion
    }

    public class PK_ConstraintAlter : ConstraintAlter
    {
        public PK_ConstraintAlter(TableMeta tableObj, string strObjName)
            : base(tableObj, strObjName)
        {
            _strTypeModifier = KWD.PRIMARY_KEY;
        }
    }

    public class UK_ConstraintAlter : ConstraintAlter
    {
        public UK_ConstraintAlter(TableMeta tableObj, string strObjName)
            : base(tableObj, strObjName)
        {
            _strTypeModifier = KWD.UNIQUE;
        }
    }

    public class FK_ConstraintAlter : ConstraintAlter
    {
        #region public interface
        public FK_ConstraintAlter(TableMeta targetTableObj, TableMeta refTable, string strObjName)
            : base(targetTableObj, strObjName)
        {
            _refTable = refTable;
            _strTypeModifier = KWD.FOREIGN_KEY;
            _refColumnNames = new List<string>();
        }

        public List<string> GetRefColumnList()
        {
            return _refColumnNames;
        }
        #endregion

        #region protected memeber function
        protected override string BuildAlterDDLBody(int nAction)
        {
            if ((DDLActionEnum)nAction == DDLActionEnum.DROP)
            {
                return base.BuildAlterDDLBody(nAction);
            }
            else	// Add
            {
                Debug.Assert((DDLActionEnum)nAction == DDLActionEnum.CREATE);
                string strDDL = base.BuildAlterDDLBody(nAction);
                strDDL += KWD.REFERENCES + " " + _refTable.FullName + " ( ";
                strDDL += Delimiter.ToString(_refColumnNames);
                strDDL += " ) ";

                return strDDL;
            }
        }
        #endregion

        #region private data section
        private TableMeta _refTable;
        private List<string> _refColumnNames;	// columns referenced in refTable
        #endregion
    }

    /// <summary> One target column per alteration </summary>
    public class ColumnAlter : TableAlter
    {
        #region public interface
        public ColumnAlter(TableMeta tableObj, ColumnMeta tgtColMeta)
            : base(tableObj)
        {
            _targetColMeta = tgtColMeta;
        }

        #endregion

        #region protected memeber function
        protected override string BuildAlterDDLBody(int nAction)
        {
            string strDDL = string.Empty;
            if ((DDLActionEnum)nAction == DDLActionEnum.DROP)
            {
                strDDL = KWD.DROP + " " + KWD.COLUMN + " " + _targetColMeta.Name;
            }
            else if ((DDLActionEnum)nAction == DDLActionEnum.CREATE)
            {
                strDDL = KWD.ADD + " " + _targetColMeta.ToString();
            }
            else if ((DDLActionEnum)nAction == DDLActionEnum.MODIFY)
            {
                if (_table.Context.DBMSContext.Platform == DBMSPlatformEnum.SqlServer)
                    strDDL = KWD.ALTER + " " + KWD.COLUMN + " " + _targetColMeta.ToString();
                else
                    strDDL = KWD.MODIFY + " " + KWD.COLUMN + " " + _targetColMeta.ToString();
            }
            return strDDL;
        }
        #endregion

        #region protected data section
        protected ColumnMeta _targetColMeta;
        #endregion
    }

    #endregion IDDLAlterer implementor

}

