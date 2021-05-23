using System;
using System.Collections.Generic;
using System.Text;

namespace Zen.DBMS.Schema
{
    public interface IDBQuoter
    {
        string QuoteName(string strRawName);
        string QuoteValue(string strRawValue);
    }
}
