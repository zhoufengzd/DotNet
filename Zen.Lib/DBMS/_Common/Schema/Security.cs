using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Zen.DBMS.Schema;
using Zen.Utilities.Text;

namespace Zen.DBMS.Security
{
    #region Security Enum
    enum PermissionActionEnum
    {
        NONE = 0x0,
        GRANT,
        REVOKE,
        DENY,			// Not supported on Oracle
    }

    // System Permission
    public enum OracleSysPermissionEnum
    {
        NONE = 0x0,
        CREATE_SESSION,
        UNLIMITED_TABLESPACE,
    }

    public enum SqlSvrSysPermissionEnum
    {
        NONE = 0x0,
        CREATE_DATABASE,
        CREATE_DEFAULT,
        CREATE_FUNCTION,
        CREATE_PROCEDURE,
        CREATE_RULE,
        CREATE_TABLE,
        CREATE_VIEW,
        BACKUP_DATABASE,
        BACKUP_LOG_,
    }

    // Object Permission
    [System.Flags]
    public enum ObjectPermissionEnum
    {
        NONE = 0x0,
        SELECT = 0x1,
        INSERT = 0x2,
        UPDATE = 0x3,
        DELETE = 0x4,
        DRI = 0x8,	// reference
        EXEC = 0x10,
        ALL = SELECT | INSERT | UPDATE | DELETE | DRI | EXEC
    }
    #endregion

    #region Security Object
    public interface IGrantee
    {
        string Name { get; }
    }

    public class SecurityObjBase : IGrantee
    {
        public SecurityObjBase(string objName)
        {
            _objName = objName;
        }

        public string Name
        {
            get { return _objName; }
        }

        #region protected data section
        protected string _objName;
        #endregion
    }

    // Create / Drop / Alter...
    public class User : SecurityObjBase
    {
        public User(string objectName)
            : base(objectName)
        {
        }
    }

    public class Role : SecurityObjBase
    {
        public Role(string objName)
            : base(objName)
        {
        }
    }

    public class Group : SecurityObjBase
    {
        public Group(string objName)
            : base(objName)
        {
        }
    }

    // SQLServer Security
    public class Login : SecurityObjBase
    {
        public Login(string objName)
            : base(objName)
        {
        }
    }

    public class SQLServerDBRole : Role, IDDLBuilder
    {
        public SQLServerDBRole(string objName)
            : base(objName)
        {
            Debug.Assert((objName.Length > 0), "Invalid Object Name");
        }

        public string BuildCreateDDL()
        {
            string strCreateDDL = "EXEC SP_ADDROLE " + _objName;
            return strCreateDDL;
        }

        public string BuildDropDDL()
        {
            string strDropDDL = "EXEC SP_DROPROLE " + _objName;
            return strDropDDL;
        }

        public string BuildAlterDDL(int nAction)
        {
            Debug.Assert(false, "not supported!");
            return string.Empty;
        }
    }
    #endregion

    #region Permission/Privileges Action Object
    /// <summary>
    /// Grantor{1}
    ///  -- Action{1}( Grant | Revoke | [Deny]... )
    ///  --  Permissions{N}
    ///  --  + [ scope: system{N} | ON object{1} ]
    ///	 -- TO Grantee{N}( User | Role | Group )
    ///	  Note: 'DENY' action is not available on Oracle	
    /// </summary>

    public sealed class GranteeMeta
    {
        public GranteeMeta(string strTargetObj)
        {
            _strstrTargetObj = strTargetObj;
            _granteeList = new List<string>();
            _sysPermissions = new List<string>();
            _ObjPermissions = new List<ObjectPermissionEnum>();
        }

        public List<string> GranteeList
        {
            get { return _granteeList; }
        }
        public List<string> SysPermissions
        {
            get { return _sysPermissions; }
        }
        public List<ObjectPermissionEnum> ObjPermissions
        {
            get { return _ObjPermissions; }
        }
        public string Object
        {
            get { return _strstrTargetObj; }
        }

        #region protected data section
        private List<string> _granteeList;
        private List<string> _sysPermissions;
        private List<ObjectPermissionEnum> _ObjPermissions;
        private string _strstrTargetObj;	// Fully qualified object name
        #endregion
    }

    public interface IPermSQLBuilder
    {
        string BuildGrantSQL(GranteeMeta gtMeta);
        string BuildRevokeSQL(GranteeMeta gtMeta);
    }

    /// <summary> Object Permission SQL Builder </summary>
    public class ObjPermSQLBuilder : IPermSQLBuilder
    {
        #region public interface
        public ObjPermSQLBuilder()
        {
        }

        // "GRANT SELECT ON dbo.V_xxx_Fields TO xxxUsers"
        // "GRANT EXECUTE ON dbo.ssp_IsRowHotFactedByUser TO xxxTrustedUsers"
        public string BuildGrantSQL(GranteeMeta gtMeta)
        {
            return BuildActionSQL(KWD.GRANT, gtMeta);
        }

        public string BuildRevokeSQL(GranteeMeta gtMeta)
        {
            return BuildActionSQL(KWD.DENY, gtMeta);
        }

        #endregion

        #region private
        private static string BuildActionSQL(string strAction, GranteeMeta gtMeta)
        {
            StringBuilder strActionSQL = new StringBuilder();
            strActionSQL.Append(strAction);
            strActionSQL.Append(' ');

            strActionSQL.Append(Delimiter.ToString(gtMeta.ObjPermissions));
            strActionSQL.Append(' ');
            strActionSQL.Append(KWD.ON);
            strActionSQL.Append(' ');
            strActionSQL.Append(gtMeta.Object);
            strActionSQL.Append(' ');
            strActionSQL.Append(KWD.TO);
            strActionSQL.Append(' ');
            strActionSQL.Append(Delimiter.ToString(gtMeta.GranteeList));

            return strActionSQL.ToString();
        }
        #endregion
    }
    #endregion
}

