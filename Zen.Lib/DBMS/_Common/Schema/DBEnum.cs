
// Enums for DBMS platform and data type, etc

using System;
namespace Zen.DBMS.Schema
{
	#region platform, data type, etc...
	public enum DBMSPlatformEnum
	{
		Unknown   = 0,
		Generic   = 1,	// cross platform
        ODBC,
        OleDB,
		SqlServer,
        SqlCe,
		Oracle,
        Sybase,
        DB2,
        MySql,
        Sqlite,
        Access,
	}

    public enum DBObjectEnum
    {
        Unknown = 0,
        Database,
        Tablespace,
        Table,
        View,
        StoredProcedure,
        Function,
    }

	public enum DataTypeEnum
	{
		Unspecified = 0,
		Int32       = 1,
		AnsiString  = 2,
		WideString  = 3,
		AnsiCLOB    = 4,
		WideCLOB    = 5,
		BLOB        = 6,
		Bit         = 7,
		GUID        = 8,
		DATETIME    = 9,
		NUMERIC     = 10,
		VarBinary	= 11,
		MAX_VALUE 
	}
	#endregion

	#region miscellaneous
	[Flags]
	public enum ParameterDirectionEnum	
	{
		// Note: "IN OUT " parameter is not supported on SQLServer
		INPUT  = 0x1,
		OUTPUT = 0x2,
	}

	public enum SortingOrderEnum
	{
		UNSPECIFIED = 0,
		ASCENDING	= 1,
		DESCENDING	= 2,
	}
	#endregion
}

