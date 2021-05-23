using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Zen.Search.Util
{
    public enum FilterType
    {
        None = 0,
        And = 1,
        Or = 2,
        AndNot = 3,
        OrNot = 4
    }

    public enum CompareType
    {
        Unknown = 0,
        Equals = 1,
        LessThan = 2,
        LessThanEqual = 3,
        GreaterThanEqual = 4,
        GreaterThan = 5,
        NotEqual = 6,
        IsNotNull = 7,
        IsNull = 8,
        TimeGreaterThan = 9
    }

    public interface IQueryItem
    {
        int NodeId { get; set; }
        int Parent { get; set; }
    }

    public interface IConnectorItem : IQueryItem
    {
        FilterType FilterType { get; set; }
        int LeftChild { get; set; }
        int RightChild { get; set; }
    }

    public interface ILeafItem : IQueryItem
    {
        string TableName { get; set; }
        string Fieldname { get; set; }
        string QueryValue { get; set; }
        CompareType CompareType { get; set; }
    }

    public interface ISearchProvider    
    {
        void SetOption(string optionName, string optionValue);

        string AddQueryItem(IQueryItem item); // return temp result table name
        bool RunSearch();
        void Reset();

        IQueryResult GetQueryResult(string tableName);
    }

    public interface IQueryResult
    {
        string ResultTable { get; set; }
        int HitCount { get; set; }
    }

    public interface ISearchFactory
    {
        void GetSearchProvider(out ISearchProvider searchExecutor);
        void GetConnectorItem(out IConnectorItem item);
        void GetLeafItem(out ILeafItem item);
    }
}
