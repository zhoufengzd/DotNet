using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

using Zen.DBMS;
using Zen.DBMS.Schema;
using Zen.DBMS.Security;
using Zen.DBMS.SQLExecution;

namespace Zen.DBAToolbox
{
    using ByteUnit = Zen.Common.Def.ByteUnit;

    /// <summary>
    ///  CaseSchema Manager: global object holder / manager
    ///    . Build SQL DDL 
    ///    . Inspect / Deploy(Update) / Drop CaseSchema 
    ///         dataType mapping 
    ///    Note: back-end only, no UI.
    /// </summary>
    public class SchemaManager
    {
        static SqlsvrDTDictionary SqlSvrDictionary = new SqlsvrDTDictionary();
        static OracleDTDictionary OracleDictionary = new OracleDTDictionary();

        public SchemaManager()
        {
            _context = new SchemaContext();
        }

        public bool DeploySchema(CaseSchema currentSchema, Version lowVersion, Version highVersion, ref string strErrMsg)
        {
            Debug.Assert((currentSchema != null), "Invalid CaseSchema");
            if (currentSchema == null)
                return false;

            StringBuilder strCreateDDL = null;
            StringBuilder strDropDDL = null;
            BuildDDL(currentSchema, lowVersion, highVersion, ref strCreateDDL, ref strDropDDL);

            //return RunSQL(strCreateDDL.ToString(), ref strErrMsg);
            return true;
        }

        public bool DropSchema(CaseSchema currentSchema, ref string strErrMsg)
        {
            Debug.Assert((currentSchema != null), "Invalid CaseSchema");
            if (currentSchema == null)
                return false;

            StringBuilder strCreateDDL = null;
            StringBuilder strDropDDL = null;
            BuildDDL(currentSchema, Version.MINVERSION, Version.MAXVERSION, ref strCreateDDL, ref strDropDDL);

            //return RunSQL(strDropDDL.ToString(), ref strErrMsg);
            return true;
        }

        public Version InspectSchema(SchemaTypeEnum schemaType, ref string strErrorMsg)
        {
            //string strDDL = "SELECT MajorVer, MinorVer FROM ";
            //if (schemaType == SchemaTypeEnum.CASE)
            //    strDDL += "[dbo].[SLT_CoreSchemaVersion] " + KWD.SQLSVR_BD;
            //else if (schemaType == SchemaTypeEnum.ULM)
            //    strDDL += "[dbo].[SLT_LIC_SchemaVersion] " + KWD.SQLSVR_BD;

            //TableRowSet resultRow = new TableRowSet();
            //if (!RunSQL(strDDL, ref resultRow, ref strErrorMsg))
            //    return Version.INVALID;

            //Version ver = new Version(Convert.ToInt32(resultRow.At(0, 0).ToString()), Convert.ToInt32(resultRow.At(0, 1).ToString()));
            //return ver;
            return null;
        }

        public void BuildDDL(CaseSchema currentSchema, Version lowVersion, Version highVersion,
            ref StringBuilder strCreateDDL, ref StringBuilder strDropDDL)
        {
            if (currentSchema == null)
                return;

            SchemaBuilder builder = new SchemaBuilder(DBMSPlatformEnum.SqlServer, SqlSvrDictionary);
            builder.SetSchemaVersion(lowVersion, highVersion);
            builder.BuildDDL(currentSchema, ref strCreateDDL, ref strDropDDL);
        }

        #region private data...
        private SchemaContext _context;
        #endregion
    }

    public sealed class TableRowSQLBuilder
    {
        // INSERT INTO MyTable (PriKey, Description)
        // VALUES (123, 'A description of part 123.')
        public static string BuildInsertLine(TableRowCollection rowCollection, TableMeta tableObj,
            Version lowVersion, Version highVersion)
        {
            string strTableName = tableObj.FullName;
            StringBuilder strInsertBatch = new StringBuilder(1024);	// at least 1K
            string strInsertSQL = string.Empty;
            strInsertSQL += "INSERT INTO ";
            strInsertSQL += strTableName;

            foreach (TableRow tblRow in rowCollection)
            {
                DDLActionEnum action = Anonymous.ValidateVersion(tblRow, lowVersion, highVersion);
                if (action == DDLActionEnum.NONE)	// not in range!
                    continue;

                StringBuilder strInsertLineTmp = new StringBuilder(strInsertSQL);
                StringBuilder strColumnData = new StringBuilder();
                strInsertLineTmp.Append(" ( ");
                TableFieldValueCollection fields = tblRow.FieldValues;
                for (int nInd = 0; nInd < fields.Count; nInd++)
                {
                    TableFieldValue field = fields[nInd];
                    //if ( field.IsNull == false )
                    //	continue;

                    ColumnMeta colMetaTmp = tableObj.GetColMeta(field.Name);
                    if (colMetaTmp == null)
                    {
                        Debug.Assert(false, "Column " + field.Name + " not found!");
                        continue;
                    }

                    if (strColumnData.Length > 0)
                    {
                        strInsertLineTmp.Append(",");
                        strColumnData.Append(",");
                    }

                    strInsertLineTmp.Append(field.Name);
                    if (IsNumeric(colMetaTmp.DataType.TypeEnum))
                    {
                        strColumnData.Append(field.FieldValue);
                    }
                    else	// Quote the value
                    {
                        strColumnData.Append(CHAR.SINGLEQUOTE);
                        strColumnData.Append(field.FieldValue);
                        strColumnData.Append(CHAR.SINGLEQUOTE);
                    }
                }

                strInsertLineTmp.Append(" ) \n");
                strInsertLineTmp.Append("VALUES (");
                strInsertLineTmp.Append(strColumnData);
                strInsertLineTmp.Append(")");

                strInsertBatch.Append(strInsertLineTmp);
                strInsertBatch.Append(KWD.SQLSVR_BD);
            }

            return strInsertBatch.ToString();
        }

        public static bool IsNumeric(DataTypeEnum dtType)
        {
            return ((dtType == DataTypeEnum.Int32) || (dtType == DataTypeEnum.NUMERIC));
        }
    }

    public class Version
    {
        #region public interface
        public Version()
        {
            _lMajor = 1; _lMinor = 0;
        }
        public Version(Int32 lMajor, Int32 lMinor)
        {
            this.Major = lMajor; this.Minor = lMinor;
        }
        public Version(Version verRef)
        {
            this.Major = verRef.Major; this.Minor = verRef.Minor;
        }

        public Int32 Major
        {
            get { return _lMajor; }
            set { Debug.Assert((value >= -1) && (value <= MAXVALUE)); _lMajor = value; }
        }
        public Int32 Minor
        {
            get { return _lMinor; }
            set { Debug.Assert((value >= -1) && (value <= MAXVALUE)); _lMinor = value; }
        }

        public override string ToString()
        {
            if (this == g_MinVersion)
                return "Minimum";
            else if (this == g_MaxVersion)
                return "Maximum";
            else if (this == g_InvalidVersion)
                return "Invalid";
            else
                return (_lMajor.ToString() + "." + _lMinor.ToString());
        }
        public override bool Equals(object obj)
        {
            Version verRight = obj as Version;
            return (verRight == null) ? false : (this == verRight);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region public static interface
        public static bool operator <(Version verLeft, Version verRight)
        {
            if (verLeft.Major < verRight.Major)
                return true;
            else if (verLeft.Major == verRight.Major)
                return (verLeft.Minor < verRight.Minor);
            else
                return false;
        }
        public static bool operator >(Version verLeft, Version verRight)
        {
            return verRight < verLeft;
        }
        public static bool operator ==(Version verLeft, Version verRight)
        {
            return (verLeft.Major == verRight.Major) && (verLeft.Minor == verRight.Minor);
        }
        public static bool operator !=(Version verLeft, Version verRight)
        {
            return !(verLeft == verRight);
        }
        public static bool operator <=(Version verLeft, Version verRight)
        {
            return (verLeft < verRight) || (verLeft == verRight);
        }
        public static bool operator >=(Version verLeft, Version verRight)
        {
            return (verLeft > verRight) || (verLeft == verRight);
        }
        public static Version MINVERSION
        {
            get { return g_MinVersion; }
        }
        public static Version MAXVERSION
        {
            get { return g_MaxVersion; }
        }
        public static Version INVALID
        {
            get { return g_InvalidVersion; }
        }
        #endregion

        #region private data...
        private Int32 _lMajor;
        private Int32 _lMinor;

        private static readonly Int32 MAXVALUE = 9999;
        private static readonly Version g_MinVersion = new Version(0, 0);
        private static readonly Version g_MaxVersion = new Version(MAXVALUE, MAXVALUE);
        private static readonly Version g_InvalidVersion = new Version(-1, -1);
        #endregion
    }

    internal class SchemaBuilder
    {
        public SchemaBuilder(DBMSPlatformEnum dbmsPlatform, IDataTypeDictionary dtMap)
        {
            _dbmsPlatform = dbmsPlatform;
            _dtMap = dtMap;
            _LowVersion = Version.MINVERSION;	// no version filter by default
            _HighVersion = Version.MAXVERSION;

            _context = new SchemaContext();
        }

        public void SetSchemaVersion(Version lowVersion, Version highVersion)
        {
            _LowVersion = lowVersion;
            _HighVersion = highVersion;
        }

        public void BuildDDL(CaseSchema currentSchema, ref StringBuilder strCreateDDL, ref StringBuilder strDropDDL)
        {
            if (currentSchema == null)
                return;

            if (strCreateDDL == null)
                strCreateDDL = new StringBuilder();
            strCreateDDL.Capacity = 36 * ByteUnit.KiloByte;	// reserve 36K

            if (strDropDDL == null)
                strDropDDL = new StringBuilder();
            strDropDDL.Capacity = 12 * ByteUnit.KiloByte;	// reserve 12K

            //////////////////////////////
            // Build objects

            // Initialize...
            ArrayList roleList = new ArrayList();
            ArrayList grantList = new ArrayList();
            ArrayList denyList = new ArrayList();

            ArrayList tableList = new ArrayList();
            ArrayList indexList = new ArrayList();
            ArrayList foreignKeyList = new ArrayList();
            ArrayList primaryKeyList = new ArrayList();
            ArrayList uniqkeyList = new ArrayList();
            ArrayList colAlterList = new ArrayList();

            StringBuilder strRowDataSql = new StringBuilder(4 * ByteUnit.KiloByte);

            // Build!
            GlobalTableCollection globalTables = currentSchema.GlobalTables;
            foreach (GlobalTableDef tableDef in globalTables)
            {
                Anonymous.SafeAdd(ref tableList, BuildTable(tableDef, ref indexList, ref primaryKeyList,
                                ref uniqkeyList, ref foreignKeyList, ref strRowDataSql, ref colAlterList));
            }

            CaseTableDefCollection caseTables = currentSchema.CaseTables;
            foreach (CaseTableDef caseTableDef in caseTables)
            {
                Anonymous.SafeAdd(ref tableList, BuildTable(caseTableDef, ref indexList, ref primaryKeyList,
                                ref uniqkeyList, ref foreignKeyList, ref strRowDataSql, ref colAlterList));
            }

            ArrayList procedureList = new ArrayList();
            ArrayList functionList = new ArrayList();
            SQLStoredProcedureCollection spList = currentSchema.SQLStoredProcedures;
            foreach (SQLStoredProcedure spDef in spList)
            {
                Anonymous.SafeAdd(ref procedureList, BuildStoredProc(spDef, ref grantList, ref denyList));
            }

            ArrayList viewList = new ArrayList();
            ViewCollection globalViews = currentSchema.Views;
            foreach (View viewDef in globalViews)
            {
                Anonymous.SafeAdd(ref viewList, BuildView(viewDef));
            }
            BuildRoleList(ref roleList, grantList, denyList);

            //////////////////////////////
            // Build SQL

            // initialize temp stringBuilder...
            StringBuilder strPKConstraint = new StringBuilder(1 * ByteUnit.KiloByte);
            StringBuilder strUKConstraint = new StringBuilder(1 * ByteUnit.KiloByte);
            StringBuilder strFKConstraint = new StringBuilder(1 * ByteUnit.KiloByte);
            StringBuilder strAlterColumn = new StringBuilder(1 * ByteUnit.KiloByte);
            StringBuilder strDropIndex = new StringBuilder(1 * ByteUnit.KiloByte);
            StringBuilder strDropProc = new StringBuilder(1 * ByteUnit.KiloByte);
            StringBuilder strDropView = new StringBuilder(1 * ByteUnit.KiloByte);
            StringBuilder strDropTable = new StringBuilder(1 * ByteUnit.KiloByte);
            StringBuilder strDropRole = new StringBuilder(1 * ByteUnit.KiloByte);

            // build...
            BuildSQL(ref strCreateDDL, ref strDropRole, roleList);
            BuildSQL(ref strCreateDDL, ref strDropTable, tableList);
            BuildSQL(ref strCreateDDL, ref strDropIndex, indexList);

            BuildAlterSQL(ref strCreateDDL, ref strPKConstraint, primaryKeyList);
            BuildAlterSQL(ref strCreateDDL, ref strUKConstraint, uniqkeyList);
            BuildAlterSQL(ref strCreateDDL, ref strFKConstraint, foreignKeyList);
            BuildAlterSQL(ref strCreateDDL, ref strAlterColumn, colAlterList);

            BuildSQL(ref strCreateDDL, ref strDropProc, procedureList);
            BuildSQL(ref strCreateDDL, ref strDropView, viewList);

            ObjPermSQLBuilder permBuilder = new ObjPermSQLBuilder();
            foreach (GranteeMeta granteeTmp in grantList)
                AppendBatch(ref strCreateDDL, permBuilder.BuildGrantSQL(granteeTmp), _dbmsPlatform);
            strCreateDDL.Append(strRowDataSql);

            strDropDDL.Append(strFKConstraint);
            strDropDDL.Append(strUKConstraint);
            strDropDDL.Append(strPKConstraint);
            strDropDDL.Append(strDropIndex);
            strDropDDL.Append(strDropProc);
            strDropDDL.Append(strDropView);
            strDropDDL.Append(strDropTable);
            strDropDDL.Append(strDropRole);
        }

        #region protected memeber function
        protected void AppendBatch(ref StringBuilder strTarget, string strSrc, DBMSPlatformEnum dbmsPlatform)
        {
            Anonymous.AppendBatch(ref strTarget, strSrc, dbmsPlatform);
        }

        protected void BuildRoleList(ref ArrayList roleList, ArrayList grantList, ArrayList denyList)
        {
            Hashtable tmpRoleMap = new Hashtable();
            foreach (GranteeMeta roleGrantee in grantList)
            {
                foreach (string strRoleNameTmp in roleGrantee.GranteeList)
                {
                    if (tmpRoleMap[strRoleNameTmp] != null)
                        continue;
                    tmpRoleMap.Add(strRoleNameTmp, 0);
                }
            }
            foreach (GranteeMeta roleDenied in denyList)
            {
                foreach (string strRoleNameTmp in roleDenied.GranteeList)
                {
                    if (tmpRoleMap[strRoleNameTmp] != null)
                        continue;
                    tmpRoleMap.Add(strRoleNameTmp, 0);
                }
            }

            foreach (DictionaryEntry roleEntry in tmpRoleMap)
            {
                SQLServerDBRole roleTmp = new SQLServerDBRole(roleEntry.Key.ToString());
                roleList.Add(roleTmp);
            }
        }

        protected TableMeta BuildTable(TableDef tableDefTmp,
            ref ArrayList indexList,
            ref ArrayList primaryKeyList,
            ref ArrayList uniqkeyList,
            ref ArrayList foreignKeyList,
            ref StringBuilder strRowDataSql,
            ref ArrayList colAlterList)
        {
            DDLActionEnum tblAction = ValidateVersion(tableDefTmp);
            if (tblAction == DDLActionEnum.NONE)	// not in range!
                return null;

            TableMeta tableMetaTmp = new TableMeta(tableDefTmp.Name, _context);

            ColumnCollection columns = tableDefTmp.Columns;
            foreach (Column col in columns)
            {
                ColumnMeta colMeta = new ColumnMeta(col.ColumnName, col.DataType, _dtMap);
                colMeta.Default = col.ExplicitDefaultValue;
                colMeta.Nullable = col.IsNullable;			// more...
                colMeta.DataType.StringLength = col.MaxStringLength;

                if (tblAction == DDLActionEnum.MODIFY)
                {
                    DDLActionEnum colAlterAction = ValidateVersion(col);
                    if (colAlterAction != DDLActionEnum.NONE)
                    {
                        ColumnAlter colAlterTmp = new ColumnAlter(tableMetaTmp, colMeta);
                        colAlterTmp.AlterAction = (int)colAlterAction;
                        colAlterList.Add(colAlterTmp);
                    }
                }

                tableMetaTmp.ColMetaList.Add(colMeta);
            }

            BuildIndexAndConstraints(tableDefTmp, tableMetaTmp, ref indexList,
                ref primaryKeyList, ref uniqkeyList, ref foreignKeyList);

            strRowDataSql.Append(TableRowSQLBuilder.BuildInsertLine(tableDefTmp.TableRows, tableMetaTmp, _LowVersion, _HighVersion));

            if (tblAction == DDLActionEnum.MODIFY)	// do not build a new table
                tableMetaTmp = null;

            return tableMetaTmp;
        }

        protected void BuildIndexAndConstraints(TableDef tableDefTmp,
            TableMeta tableMetaTmp,
            ref ArrayList indexList,
            ref ArrayList primaryKeyList,
            ref ArrayList uniqkeyList,
            ref ArrayList foreignKeyList)
        {
            // 	indexes, primaryKeyList, uniqkeyList;
            IndexDefCollection indexes = tableDefTmp.Indexes;
            foreach (IndexDef indxMixed in indexes)
            {
                if (ValidateVersion(indxMixed) == DDLActionEnum.NONE)	// not in range!
                    continue;

                if (indxMixed.Category == IndexCategoryEnum.NORMAL)
                {
                    IndexMeta indexMetaTmp = new IndexMeta(indxMixed.IndexName, tableDefTmp.Name, _context);
                    foreach (IndexColumn indCol in indxMixed.ColumnNames)
                        indexMetaTmp.GetColumnList().Add(indCol.ColumnName);
                    indexList.Add(indexMetaTmp);
                }
                else if (indxMixed.Category == IndexCategoryEnum.PRIMARYKEY)
                {
                    PK_ConstraintAlter pkTmp = new PK_ConstraintAlter(tableMetaTmp, indxMixed.IndexName);
                    foreach (IndexColumn indCol in indxMixed.ColumnNames)
                        pkTmp.GetColumnList().Add(indCol.ColumnName);
                    pkTmp.AlterAction = (int)DDLActionEnum.CREATE;
                    primaryKeyList.Add(pkTmp);
                }
                else if (indxMixed.Category == IndexCategoryEnum.UNIQUEKEY)
                {
                    UK_ConstraintAlter ukTmp = new UK_ConstraintAlter(tableMetaTmp, indxMixed.IndexName);
                    foreach (IndexColumn indCol in indxMixed.ColumnNames)
                        ukTmp.GetColumnList().Add(indCol.ColumnName);
                    ukTmp.AlterAction = (int)DDLActionEnum.CREATE;
                    uniqkeyList.Add(ukTmp);
                }
            }

            GlobalForeignKeyCollection gFrnKeys = tableDefTmp.GlobalTableForeignKeys;
            foreach (GlobalForeignKey fk in gFrnKeys)
            {
                if (ValidateVersion(fk) == DDLActionEnum.NONE)	// not in range!
                    continue;

                TableMeta refTable = new TableMeta(fk.ForeignTable, _context);
                FK_ConstraintAlter foreignKeyTmp = new FK_ConstraintAlter(tableMetaTmp, refTable, fk.Name);
                foreach (ForeignKeyColumn fkCol in fk.Columns)
                {
                    foreignKeyTmp.GetColumnList().Add(fkCol.LocalColumn);
                    foreignKeyTmp.GetRefColumnList().Add(fkCol.ForeignColumn);
                }
                foreignKeyTmp.AlterAction = (int)DDLActionEnum.CREATE;
                foreignKeyList.Add(foreignKeyTmp);
            }
        }

        protected ViewMeta BuildView(View viewDef)
        {
            if (ValidateVersion(viewDef) == DDLActionEnum.NONE)	// not in range!
                return null;

            // Always treated as rebuild or create...
            ViewMeta viewTmp = new ViewMeta(viewDef.ViewName, _context);
            viewTmp.DDLBodyText = viewDef.ViewText.ToString();
            return viewTmp;
        }

        protected StoredProcMeta BuildStoredProc(SQLStoredProcedure spDef, ref ArrayList grantList, ref ArrayList denyList)
        {
            if (ValidateVersion(spDef) == DDLActionEnum.NONE)	// not in range!
                return null;

            // Always treated as rebuild or create...
            StoredProcMeta spTmp = new StoredProcMeta(spDef.ProcName, _context);
            spTmp.DDLBodyText = spDef.ProcedureText.ToString();
            foreach (SQLStoredProcedureParameter param in spDef.Parameters)
            {
                ParameterMeta pramMeta = new ParameterMeta(param.Name, param.DataType, param.ParameterDirection, _dtMap);
                pramMeta.DataType.StringLength = param.StringLength;

                spTmp.GetParameterList().Add(pramMeta);
            }

            SQLRoleAssignmentCollection roles = spDef.Roles;
            foreach (SQLRoleAssignment roleAssign in roles)
            {
                GranteeMeta roleGrantee = new GranteeMeta(spDef.ProcName);
                roleGrantee.GranteeList.Add(roleAssign.RoleName);
                roleGrantee.ObjPermissions.Add(roleAssign.GrantAssignments);
                grantList.Add(roleGrantee);

                GranteeMeta roleDenied = new GranteeMeta(spDef.ProcName);
                roleDenied.GranteeList.Add(roleAssign.RoleName);
                roleDenied.ObjPermissions.Add(roleAssign.DenyAssignments);
                denyList.Add(roleDenied);
            }

            return spTmp;
        }

        protected FunctionMeta BuildFunction(SQLUserDefinedFunction udfDef, ref ArrayList grantList, ref ArrayList denyList)
        {
            if (ValidateVersion(udfDef) == DDLActionEnum.NONE)	// not in range!
                return null;

            // Always treated as rebuild or create...
            //			FunctionMeta funcTmp = new FunctionMeta( udfDef.FunctionName, udfDef.ReturnValue.ToString() );
            FunctionMeta funcTmp = new FunctionMeta(udfDef.FunctionName, DataTypeEnum.NUMERIC, _context);
            //funcTmp.DDLBodyText = udfDef..ToString();
            foreach (SQLStoredProcedureParameter param in udfDef.Parameters)
            {
                ParameterMeta pramMeta = new ParameterMeta(param.Name, param.DataType, param.ParameterDirection, _dtMap);
                pramMeta.DataType.StringLength = param.StringLength;

                funcTmp.GetParameterList().Add(pramMeta);
            }

            SQLRoleAssignmentCollection roles = udfDef.Roles;
            foreach (SQLRoleAssignment roleAssign in roles)
            {
                GranteeMeta roleGrantee = new GranteeMeta(udfDef.FunctionName);
                roleGrantee.GranteeList.Add(roleAssign.RoleName);
                roleGrantee.ObjPermissions.Add(roleAssign.GrantAssignments);
                grantList.Add(roleGrantee);

                GranteeMeta roleDenied = new GranteeMeta(udfDef.FunctionName);
                roleDenied.GranteeList.Add(roleAssign.RoleName);
                roleDenied.ObjPermissions.Add(roleAssign.DenyAssignments);
                denyList.Add(roleDenied);
            }

            return funcTmp;
        }

        protected void BuildSQL(ref StringBuilder strCreateSQL, ref StringBuilder strDropSQL, ArrayList ddlBuilderList)
        {
            bool AddLabel = true;
            foreach (IDDLBuilder ddlBuilderTmp in ddlBuilderList)
            {
                if (AddLabel)
                {
                    strCreateSQL.Append("\n/* ***************************** */\n");
                    strDropSQL.Append("\n/* ***************************** */\n");
                    AddLabel = false;
                }
                AppendBatch(ref strCreateSQL, ddlBuilderTmp.BuildCreateDDL(), _dbmsPlatform);
                AppendBatch(ref strDropSQL, ddlBuilderTmp.BuildDropDDL(), _dbmsPlatform);
            }
        }

        protected void BuildAlterSQL(ref StringBuilder strCreateSQL, ref StringBuilder strDropSQL, ArrayList constraintAlterList)
        {
            bool AddLabel = true;
            foreach (TableAlter tblAlter in constraintAlterList)
            {
                if (AddLabel)
                {
                    strCreateSQL.Append("\n/* ***************************** */\n");
                    strDropSQL.Append("\n/* ***************************** */\n");
                    AddLabel = false;
                }
                AppendBatch(ref strCreateSQL, tblAlter.BuildAlterDDL(), _dbmsPlatform);
                AppendBatch(ref strDropSQL, tblAlter.BuildAlterDDL((int)DDLActionEnum.DROP), _dbmsPlatform);
            }
        }

        protected DDLActionEnum ValidateVersion(SchemaObject schemaObj)
        {
            return Anonymous.ValidateVersion(schemaObj, _LowVersion, _HighVersion);
        }
        #endregion

        #region private data section
        private DBMSPlatformEnum _dbmsPlatform;
        private IDataTypeDictionary _dtMap;

        // version control. _LowVersion <= version selected < _HighVersion
        private Version _LowVersion;
        private Version _HighVersion;

        private SchemaContext _context;
        #endregion
    }

    // Serves as C++ anonymous namespace
    internal class Anonymous
    {
        public static void AppendBatch(ref StringBuilder strTarget, string strSrc, DBMSPlatformEnum dbmsPlatform)
        {
            strTarget.Append(strSrc);
            if (dbmsPlatform == DBMSPlatformEnum.SqlServer)
                strTarget.Append(KWD.SQLSVR_BD);
            else
                strTarget.Append(KWD.ORACLE_BD);
        }

        public static void SafeAdd(ref ArrayList arrList, object obj)
        {
            if (obj != null)
                arrList.Add(obj);
        }

        public static DDLActionEnum ValidateVersion(SchemaObject schemaObj, Version lowVersion, Version highVersion)
        {
            Debug.Assert(lowVersion <= highVersion);

            if (lowVersion == Version.MINVERSION && highVersion == Version.MAXVERSION)
                return DDLActionEnum.CREATE;

            Version objLowVersion = new Version(schemaObj.FirstMajorVersion, schemaObj.FirstMinorVersion);
            Version objHighVersion = new Version(schemaObj.LastMajorVersion, schemaObj.LastMinorVersion);	// current version
            Debug.Assert(objLowVersion <= objHighVersion);

            if ((lowVersion > objHighVersion) || (objHighVersion >= highVersion))
                return DDLActionEnum.NONE;

            if (objLowVersion == objHighVersion)
                return DDLActionEnum.CREATE;
            else
                return DDLActionEnum.MODIFY;
        }
    }
}

