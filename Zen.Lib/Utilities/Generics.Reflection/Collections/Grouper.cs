using System.Collections;
using System.Collections.Generic;
using System.Data;
using Zen.Utilities.FileUtil;
using Zen.Utilities.Text;
using Zen.Utilities.Defs;
using Zen.Common.Def;

namespace Zen.Utilities.Generics
{
    using Cell = Pair<int, int>;

    /// <summary>
    /// Word -> Row Index
    ///   No duplicates on parent level
    ///   Leaf value duplicated are allowed
    /// </summary>
    public sealed class TableGrouper
    {
        public TableGrouper(DataTable tbl)
        {
            _dataTbl = tbl;
            _indexes = new Dictionary<int, Dictionary<string, Set<int>>>();
        }

        public DataNode<string, Cell> GroupBy(IEnumerable<string> indexColumns, string leafColumn)
        {
            LoadColumnMap();

            Set<int> groupbySet = PrepareIndex(indexColumns, leafColumn);
            return BuildTree(groupbySet, leafColumn);
        }

        public Set<int> Find()
        {
            return null;
        }

        #region private functions
        private void LoadColumnMap()
        {
            if (_colMap != null)
                return;

            _colMap = new TwowayMap<string, int>();
            foreach (DataColumn cl in _dataTbl.Columns)
                _colMap.Add(cl.ColumnName, cl.Ordinal);
        }

        private Set<int> PrepareIndex(IEnumerable<string> indexColumns, string leafColumn)
        {
            Set<int> indexColumnSet = new Set<int>();
            int leafColOrdinal = _colMap.FindT2(leafColumn);
            indexColumnSet.Add(leafColOrdinal);
            foreach (string col in indexColumns)
                indexColumnSet.Add(_colMap.FindT2(col));

            BuildIndex(indexColumnSet);

            Set<int> groupbySetCleaned = new Set<int>();
            foreach (int colOrdinal in indexColumnSet)
            {
                if (colOrdinal != leafColOrdinal && _indexes[colOrdinal].Count > 0)
                    groupbySetCleaned.Add(colOrdinal);
            }
            return groupbySetCleaned;
        }

        private void BuildIndex(Set<int> indexColumns)
        {
            Dictionary<int, Dictionary<string, Set<int>>> targets = new Dictionary<int, Dictionary<string, Set<int>>>();
            foreach (int colOrdinal in indexColumns)
            {
                if (!_indexes.ContainsKey(colOrdinal))
                    targets.Add(colOrdinal, new Dictionary<string, Set<int>>());
            }
            if (targets.Count < 1) // every column indexed already
                return;

            DataRowCollection rows = _dataTbl.Rows;
            object[] items = null;
            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                items = rows[rowIndex].ItemArray;

                foreach (KeyValuePair<int, Dictionary<string, Set<int>>> kv in targets)
                {
                    Dictionary<string, Set<int>> colIndex = kv.Value;
                    object cell = items[kv.Key];
                    string indexWord = (cell == null) ? null : cell.ToString();
                    if (string.IsNullOrEmpty(indexWord))
                        continue;

                    if (!colIndex.ContainsKey(indexWord))
                        colIndex[indexWord] = new Set<int>();
                    colIndex[indexWord].Add(rowIndex);
                }
            }

            foreach (KeyValuePair<int, Dictionary<string, Set<int>>> kv in targets)
            {
                foreach (Set<int> leaves in kv.Value.Values)
                    leaves.Sort();

                _indexes.Add(kv.Key, kv.Value);
            }
        }

        private DataNode<string, Cell> BuildTree(Set<int> groupbySet, string leafColumn)
        {
            EnumeratorMgr em = new EnumeratorMgr();
            foreach (int colOrdinal in groupbySet)
                em.AddEnumerable(_indexes[colOrdinal]);

            int leafColOrdinal = _colMap.FindT2(leafColumn);
            DataNode<string, Cell> root = new DataNode<string, Cell>("root");
            while (em.MoveNext())
            {
                // Get current leaf set
                // All the leaves share the same parent / parents
                Set<int> leafSet = ProduceLeafSet(em, leafColOrdinal);
                if (leafSet == null || leafSet.Count < 1)
                    continue;

                // Build parent nodes for these leaves
                DataRow row = _dataTbl.Rows[leafSet[0]];
                DataNode<string, Cell> currentParent = root;
                foreach (int colOrdinal in groupbySet)
                {
                    string nodeName = row[colOrdinal].ToString();
                    DataNode<string, Cell> nodeTmp = null;
                    if (!currentParent.Children.TryGetValue(nodeName, out nodeTmp))
                    {
                        nodeTmp = AddChild(currentParent, nodeName);
                        nodeTmp.Data = new Cell(leafSet[0], colOrdinal);
                    }

                    System.Diagnostics.Debug.Assert(nodeTmp.Data != null);
                    currentParent = nodeTmp; // Move one level down
                }

                // Add these leaves
                DataRowCollection rows = _dataTbl.Rows;
                foreach (int rowIndex in leafSet)
                {
                    row = _dataTbl.Rows[rowIndex];
                    DataNode<string, Cell> leafNode = AddChild(currentParent, rowIndex.ToString());
                    leafNode.Data = new Cell(rowIndex, -1);
                }
            }

            return root;
        }

        /// <summary> All the leaves share the same parent / parents </summary>
        private Set<int> ProduceLeafSet(EnumeratorMgr em, int leafColOrdinal)
        {
            // Get current leaf set
            Set<int> leafSet = null;
            foreach (IEnumerator<KeyValuePair<string, Set<int>>> colEnum in em.Current)
            {
                if (leafSet == null)
                    leafSet = colEnum.Current.Value;
                else
                    leafSet = leafSet.Intersect(colEnum.Current.Value);
            }

            // To do: sort the leafSet by leaf column
            return leafSet;
        }

        /// <summary> Return child node </summary>
        private DataNode<string, Cell> AddChild(DataNode<string, Cell> parent, string label)
        {
            DataNode<string, Cell> node = new DataNode<string, Cell>(label);
            parent.Children.Add(label, node);
            node.Parent = parent;

            return node;
        }
        #endregion

        #region private data
        private DataTable _dataTbl;
        private TwowayMap<string, int> _colMap; // name <==> Ordinal
        private Dictionary<int, Dictionary<string, Set<int>>> _indexes; // column ordinal -> word -> row index set
        #endregion
    }
}
