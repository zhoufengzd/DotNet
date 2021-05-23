using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Zen.Search.Util
{
    public class SearchFactory : ISearchFactory
    {
        public SearchFactory()
        {
        }

        public void GetSearchProvider(out ISearchProvider provider)
        {
            provider = new DtSearchProvider();
        }

        public void GetConnectorItem(out IConnectorItem item)
        {
            item = new ConnectorItem();
            item.NodeId = _itemCounter++;
        }
        public void GetLeafItem(out ILeafItem item)
        {
            item = new LeafItem();
            item.NodeId = _itemCounter++;
        }

        private int _itemCounter = 0;
    }
}
