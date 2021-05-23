using System;
using System.Collections.Generic;
using System.Text;

namespace Zen.DBMS.Schema
{
    /// <summary> Mark characters used in SQL statement </summary>
    public class CHAR
    {
        // internally used string instead to ease its use when format a string
        public static readonly string AT = "@";
        public static readonly string COMMA = ",";
        public static readonly string DOUBLEQUOTE = "\"";
        public static readonly string LEFTBRACKET = "[";
        public static readonly string LEFTPAREN = "(";
        public static readonly string RIGHTPAREN = ")";
        public static readonly string RIGHTBRACKET = "]";
        public static readonly string SEMICOLON = ";";
        public static readonly string SINGLEQUOTE = "\'";
        public static readonly string TAB = "\t";
    }

    /// <summary> SQL Keywords holder </summary>
    public class KWD	// keywordlist
    {
        // DDL
        public static readonly string CREATE = "CREATE";
        public static readonly string ADD = "ADD";
        public static readonly string DROP = "DROP";
        public static readonly string ALTER = "ALTER";
        public static readonly string MODIFY = "MODIFY";

        // DML
        public static readonly string INSERT = "INSERT";
        public static readonly string DELETE = "DELETE";
        public static readonly string TRUNCATE = "TRUNCATE";
        public static readonly string UPDATE = "UPDATE";
        public static readonly string EXECUTE = "EXECUTE";
        public static readonly string SELECT = "SELECT";
        public static readonly string FROM = "FROM";
        public static readonly string WHERE = "WHERE";

        // Permission
        public static readonly string GRANT = "GRANT";
        public static readonly string REVOKE = "REVOKE";
        public static readonly string NOAUDIT = "NOAUDIT";			// Oracle
        public static readonly string DENY = "DENY";				// SQLServer
        public static readonly string TO = "TO";
        public static readonly string ON = "ON";
        public static readonly string ALL = "ALL";

        // Transaction Control
        public static readonly string COMMIT = "COMMIT";
        public static readonly string ROLLBACK = "ROLLBACK";
        public static readonly string SAVEPOINT = "SAVEPOINT";		// Oracle
        public static readonly string SV_TRANSACT = "SAVE TRANSACTION";	// SQLServer

        // Object 
        public static readonly string SCHEMA = "SCHEMA";
        public static readonly string TABLE = "TABLE";
        public static readonly string COLUMN = "COLUMN";
        public static readonly string CONSTRAINT = "CONSTRAINT";
        public static readonly string INDEX = "INDEX";
        public static readonly string PRIMARY_KEY = "PRIMARY KEY";
        public static readonly string FOREIGN_KEY = "FOREIGN KEY";
        public static readonly string VIEW = "VIEW";
        public static readonly string FUNCTION = "FUNCTION";
        public static readonly string PROCEDURE = "PROCEDURE";
        public static readonly string TRIGGER = "TRIGGER";
        public static readonly string TYPE = "TYPE";
        public static readonly string DATABASE = "DATABASE";
        public static readonly string TABLESPACE = "TABLESPACE";
        public static readonly string CLUSTER = "CLUSTER";
        public static readonly string LOGIN = "LOGIN";
        public static readonly string ROLE = "ROLE";
        public static readonly string USER = "USER";
        public static readonly string GROUP = "GROUP";
        public static readonly string RULE = "RULE";

        // NULL, DEFAULT
        public static readonly string NULL = "NULL";
        public static readonly string NOT_NULL = "NOT NULL";
        public static readonly string DEFAULT = "DEFAULT";
        public static readonly string REFERENCES = "REFERENCES";
        public static readonly string IDENTITY = "IDENTITY";
        public static readonly string UNIQUE = "UNIQUE";

        public static readonly string ASC = "ASC";
        public static readonly string DESC = "DESC";

        //  Function and Procedure
        public static readonly string RETURN = "RETURN";	// Oracle. 
        public static readonly string RETURNS = "RETURNS";	// SQLServer
        public static readonly string AS = "AS";
        public static readonly string IN = "IN";
        public static readonly string OUT = "OUT";		    // Oracle. 
        public static readonly string OUTPUT = "OUTPUT";	// SQLServer

        // Batch Delimiter
        public static readonly string SQLSVR_BD = "\nGO\n";	// SQLServer
        public static readonly string ORACLE_BD = "\n;\n";	// Oracle
    }


}
