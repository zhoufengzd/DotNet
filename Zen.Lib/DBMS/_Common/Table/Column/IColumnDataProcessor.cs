using System;
using System.Collections.Generic;
using System.Text;

namespace Zen.DBMS
{
    /// <summary>
    /// Generic Field Data Processor Interface.
    /// </summary>
    public interface IColumnDataProcessor<ProcessResultT>
    {
        bool Process(int colIndex, ref object colValue, ref ProcessResultT result);
        bool Process(string colName, ref object colValue, ref ProcessResultT result);
    }
}
