using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Zen.UIControls.Layout;
using Zen.Utilities.Generics;
using Zen.Common.Def;

namespace Zen.UIControls
{
    using CellBorderStyle = TableLayoutPanelCellBorderStyle;

    /// <summary>
    /// Responsibility: 
    ///   Display Header (optional) + Dictionary Content
    ///   Allow set lable(header + key) / value hint
    ///   Allow add / delete items (optional)
    /// </summary>
    public sealed class DictionaryCtrl : UserControl
    {
        const int ColCount = 3; // | Key | ==> | Value |

        public const string KeyLabel = "Key";
        public const string ValueLabel = "Value";
        public const string ConnectorLabel = "ConnectorLabel";

        public DictionaryCtrl()
        {
            InitializeComponent();
        }

        public int SetObject(Dictionary<string, string> dictionary)
        {
            return SetObject(dictionary, null, CellBorderStyle.None);
        }

        // To do: test same type / data reload / etc
        /// <summary>
        /// <param name="valueHints">Key => hints or/and Value => hints</param>
        /// <param name="lableHints">Control label</param>
        /// <param name="borderStyle"></param>
        /// </summary>
        public int SetObject(Dictionary<string, string> dictionary, DictionaryHint hint, CellBorderStyle borderStyle)
        {
            _dictionary = dictionary;
            _hint = (hint != null) ? hint : new DictionaryHint();

            _overarchingPanel.SuspendLayout();
            _overarchingPanel.CellBorderStyle = borderStyle;

            int rowIndex = 0;
            DrawHeader(ref rowIndex);
            DrawData(ref rowIndex);

            if (_allowAddNew)
                DrawStar(ref rowIndex);

            _overarchingPanel.ResumeLayout();
            return rowIndex;
        }

        /// <summary>
        /// Doing data exchange: control ==> object
        /// </summary>
        public bool OnOk()
        {
            foreach (Pair<Label, ITypeBinder> kv in _controls)
                _dictionary[kv.X.Text] = kv.Y.Value.ToString();

            return true;
        }
        public bool OnCancel()
        {
            return true;
        }

        #region Initialization
        private void InitializeComponent()
        {
            this._overarchingPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // _overarchingPanel
            // 
            this._overarchingPanel.AutoSize = true;
            this._overarchingPanel.ColumnCount = ColCount;
            this._overarchingPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.AutoSize));
            this._overarchingPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.AutoSize));
            this._overarchingPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.AutoSize));
            this._overarchingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._overarchingPanel.Location = new System.Drawing.Point(0, 0);
            this._overarchingPanel.Margin = new System.Windows.Forms.Padding(0);
            this._overarchingPanel.Padding = new System.Windows.Forms.Padding(0);
            this._overarchingPanel.Name = "_overarchingPanel";
            this._overarchingPanel.RowCount = 1;
            this._overarchingPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._overarchingPanel.Size = new System.Drawing.Size(100, 40);
            this._overarchingPanel.TabIndex = 0;
            // 
            // DictionaryCtrl
            // 
            this.Controls.Add(this._overarchingPanel);
            this.Name = "DictionaryCtrl";
            this.Size = new System.Drawing.Size(100, 40);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void OnLoad(object sender, EventArgs e)
        {
        }

        private void OnNew(object sender, EventArgs e)
        {
            _overarchingPanel.SuspendLayout();

            _overarchingPanel.Controls.Remove(_btnNew);

            TextBoxBinder keyBinder = new TextBoxBinder("...", null);
            _overarchingPanel.Controls.Add(keyBinder.Control, 0, _newRowIndex);

            if (!string.IsNullOrEmpty(_hint.ConnectorLabel))
                AddLabel(_hint.ConnectorLabel, _newRowIndex, 1);

            TextBoxBinder valueBinder = new TextBoxBinder();
            _overarchingPanel.Controls.Add(valueBinder.Control, 2, _newRowIndex);

            _newRowIndex++;
            _overarchingPanel.Controls.Add(_btnNew, 0, _newRowIndex);

            _overarchingPanel.ResumeLayout();
        }

        private void DrawHeader(ref int rowIndex)
        {
            if (_hint.ShowHeader)
            {
                _overarchingPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                // Add "Key" + ":" + "Value"
                AddLabel(_hint.KeyLabel, rowIndex, 0);
                AddLabel(_hint.ValueLabel, rowIndex, 2);
                rowIndex++;
            }

            // Draw separator. If no header defined, still add it to make layout better.
            _overarchingPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            HorizontalLineCtrl line = new HorizontalLineCtrl();
            line.Padding = new Padding(0, 0, 0, 0);
            line.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            line.Visible = (_hint.ShowHeader);

            _overarchingPanel.SetColumnSpan(line, ColCount);
            _overarchingPanel.Controls.Add(line, 0, rowIndex);

            rowIndex++;
        }

        private void DrawData(ref int rowIndex)
        {
            _controls = new List<Pair<Label, ITypeBinder>>();

            //TextBoxHint keyHint = new TextBoxHint();
            //keyHint.IsReadonly = true;

            bool showConnector = (!string.IsNullOrEmpty(_hint.ConnectorLabel));
            Dictionary<string, IEnumerable<string>> kvHints = _hint.KeyValueHints;
            ITypeBinder valueBinder = null;
            foreach (KeyValuePair<string, string> kv in _dictionary)
            {
                _overarchingPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                //TextBoxBinder keyBinder = new TextBoxBinder(kv.Key, keyHint);
                //_overarchingPanel.Controls.Add(keyBinder.Control, 0, rowIndex);
                Label lbl = AddLabel(kv.Key, rowIndex, 0);

                if (showConnector)
                    AddLabel(_hint.ConnectorLabel, rowIndex, 1);

                if (kvHints != null && kvHints.ContainsKey(kv.Key))
                    valueBinder = new ComboBoxBinder(kv.Value, kvHints[kv.Key], ComboBoxStyle.DropDown);
                else
                    valueBinder = new TextBoxBinder(kv.Value, null);
                _overarchingPanel.Controls.Add(valueBinder.Control, 2, rowIndex);

                _controls.Add(new Pair<Label, ITypeBinder>(lbl, valueBinder));
                rowIndex++;
            }
        }

        private void DrawStar(ref int rowIndex)
        {
            _overarchingPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            _btnNew = new Button();
            _btnNew.AutoSize = true;
            _btnNew.Text = "*";
            _btnNew.Padding = new Padding(0, 0, 0, 0);
            _btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _btnNew.Click += new System.EventHandler(OnNew);

            _overarchingPanel.Controls.Add(_btnNew, 0, rowIndex);
            _newRowIndex = rowIndex;

            rowIndex++;
        }

        private Label AddLabel(string labelText, int rowIndex, int colIndex)
        {
            Label lblMappingSign = new Label();
            lblMappingSign.AutoSize = true;
            lblMappingSign.Text = labelText;
            lblMappingSign.Padding = new Padding(0, 3, 0, 0);
            lblMappingSign.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _overarchingPanel.Controls.Add(lblMappingSign, colIndex, rowIndex);

            return lblMappingSign;
        }

        #endregion

        #region private data
        private TableLayoutPanel _overarchingPanel;
        private List<Pair<Label, ITypeBinder>> _controls;

        private Dictionary<string, string> _dictionary;
        private DictionaryHint _hint;

        private bool _allowAddNew = false;
        private Button _btnNew;
        private int _newRowIndex = -1;
        #endregion

    }
}
