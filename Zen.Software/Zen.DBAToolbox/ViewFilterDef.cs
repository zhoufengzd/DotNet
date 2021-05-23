using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zen.DBAToolbox
{
    class ViewFilterDef
    {
        private string _filterOut;

        public string FilterOut
        {
            get { return _filterOut; }
            set { _filterOut = value; }
        }
    }
}
