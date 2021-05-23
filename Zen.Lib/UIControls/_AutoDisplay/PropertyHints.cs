using System.Collections.Generic;

namespace Zen.UIControls
{
    public class ObjValueLabelHint
    {
        public ObjValueLabelHint()
        {
        }
        public ObjValueLabelHint(string category, Dictionary<string, IEnumerable<string>> valueHints, Dictionary<string, string> LabelHints, DictionaryHint dictionaryHint)
        {
            _category = category;
            _valueHints = valueHints;
            _LabelHints = LabelHints;
            _dictionaryHint = dictionaryHint;
        }

        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }
        public Dictionary<string, IEnumerable<string>> ValueHints
        {
            get { return _valueHints; }
            set { _valueHints = value; }
        }
        public Dictionary<string, string> LabelHints
        {
            get { return _LabelHints; }
            set { _LabelHints = value; }
        }
        public DictionaryHint DictionaryHint
        {
            get { return _dictionaryHint; }
            set { _dictionaryHint = value; }
        }

        #region private data
        private string _category;
        private Dictionary<string, IEnumerable<string>> _valueHints;
        private Dictionary<string, string> _LabelHints;
        private DictionaryHint _dictionaryHint;
        #endregion
    }

    public sealed class DictionaryHint
    {
        const string Key = "Key";
        const string Value = "Value";
        const string Colon = ":";

        public DictionaryHint()
        {
        }

        public bool ShowHeader
        {
            get { return _showHeader; }
            set { _showHeader = value; }
        }
        public string KeyLabel
        {
            get { return _keyLabel; }
            set { _keyLabel = value; }
        }
        public string ValueLabel
        {
            get { return _valueLabel; }
            set { _valueLabel = value; }
        }
        public string ConnectorLabel
        {
            get { return _connectorLabel; }
            set { _connectorLabel = value; }
        }
        public Dictionary<string, IEnumerable<string>> KeyValueHints
        {
            get { return _keyValueHints; }
            set { _keyValueHints = value; }
        }

        #region private data
        private bool _showHeader = true;
        private string _keyLabel = Key;
        private string _valueLabel = Value;
        private string _connectorLabel;
        private Dictionary<string, IEnumerable<string>> _keyValueHints;
        #endregion
    }

}
