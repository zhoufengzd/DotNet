using System;

namespace Zen.DBMS
{
    public class TempSqlObjNameGenerator
    {
        static readonly string Prefix = "Zen";
        
        static readonly string SpNameGlobalFmt = "[##" + Prefix + "Sp_{0}]";
        static readonly string SpNameLocalFmt = "[#" + Prefix + "Sp_{0}]";
        static readonly string TableNameGlobalFmt = "[##" + Prefix + "Tbl_{0}]";
        static readonly string TableNameLocalFmt = "[#" + Prefix + "Tbl_{0}]";

        public static string GetSpName()
        {
            return GetSpName(true);
        }
       
        /// <param name="isGlobal">
        /// isGlobal == true, the name providuced visiable to anyone 
        ///   as long as the connection is still alive
        /// isGlobal == false, the name providuced visiable to
        ///   local connection only that builds the object (table or sp)
        /// </param>
        public static string GetSpName(bool isGlobal)
        {
            return string.Format(isGlobal ? SpNameGlobalFmt : SpNameLocalFmt, Guid.NewGuid());
        }

        public static string GetTableName()
        {
            return GetTableName(true);
        }
        public static string GetTableName(bool isGlobal)
        {
            return string.Format(isGlobal ? TableNameGlobalFmt : TableNameLocalFmt, Guid.NewGuid());
        }
    }
}
