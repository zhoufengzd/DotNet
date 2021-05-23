

namespace Zen.DBMS.SQLExecution
{
    using IRowSet = Zen.DBMS.IRowSet;
    using IRowSetList = Zen.DBMS.IRowSetList;

	public interface ISQLExecutor
	{
		// optional: user may set server options before executing sql statement
		void SetOptions( string strXmlOption );

		// Create, insert, etc. no recordset returned
		bool Execute( string strSQL );

		// Select or fetch recordset
		bool Execute( string strSQL, IRowSet     rowSet     );		// if only one row set returned
		bool Execute( string strSQL, IRowSetList rowSetList );		// if multiple row set returned

		string GetErrorMsg();
	}

}

