using System.Collections.Generic;
using Zen.DBMS.Schema;

namespace Zen.DBMS
{
    /// May not need them in .Net since DataTable and DataSet have been defined already.

    /// <summary>
    /// A simple and generic structure to describe the table data
    /// It could be implemented as a wrapper on SQLDMO QueryResult, 
    ///   ADO RecordSet, ..., etc
    /// </summary>
    public interface ISimpleContainer<T> : IEnumerable<T>
    {
        int Size();						// Returns the number of objects in container 
        void Clear();					// Erases all the objects in container
        void Reserve(int lCount);		// Reserves a minimum length of storage 
    }

    public interface ICell
    {
        void FromString(string strValue);	// sets internal value from string
        string ToString();					// Returns a String that represents the current 
    }

    public interface IRow : ISimpleContainer<ICell>
    {
        void Push_back(ICell element);	// Adds an element to the end 
        ICell At(int lColIndex);	    // Returns a reference to the element at a specified location 
    }

    /// <summary>
    /// Similar to DataTable
    /// </summary>
    public interface IRowSet : ISimpleContainer<IRow>
    {
        void Push_back(IRow dataRow);	// Adds a dataRow to the end 
        IRow At(int lRowIndex);	        // Returns a reference to the dataRow at a specified row index

        void SetMetaData(int lColIndex, ColumnMeta colMeta);	// set column definitions
    }

    /// <summary>
    /// Similar to DataSet
    /// </summary>
    public interface IRowSetList : ISimpleContainer<IRowSet>
    {
        void Push_back(IRowSet IRowSet);	// Adds a rowset to the end 
        IRowSet At(int lRowSetIndex);	        // Returns a reference to the rowset at a specified rowSet index
    }
}
