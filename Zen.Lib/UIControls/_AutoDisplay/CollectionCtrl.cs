using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Zen.Common.Def;
using Zen.Utilities.Generics;

namespace Zen.UIControls.Layout
{
    using ButtonInfo = Triple<string, EventHandler, Button>;
    using CellBorderStyle = TableLayoutPanelCellBorderStyle;
    using Zen.Utilities;

    /// <summary>
    /// Single item collection control: sequentially displayed, one per page
    /// </summary>
    public sealed class CollectionCtrl : UserControl, IAutoControl<IEnumerable>
    {
        public CollectionCtrl()
        {
            InitializeComponent();
        }

        public int SetObject(IEnumerable objCollection)
        {
            return SetObject(objCollection, CellBorderStyle.None);
        }
        public int SetObject(IEnumerable objCollection, CellBorderStyle borderStyle)
        {
            _borderStyle = borderStyle;

            Type objType = objCollection.GetType();
            if (TypeInterrogator.IsDictionaryType(objType))
                _iter = new Traverser(CollConverter.ToList(objCollection, GenBuilder.BuildKeyValueType(objType)));
            else
                _iter = new Traverser(CollConverter.ToList(objCollection));

            _objControl.SetObject(_iter.First, null, _borderStyle);

            return 0;
        }

        #region private functions

        #region Initialization
        private void InitializeComponent()
        {
            SuspendLayout();
            _overarchingPanel = new TableLayoutPanel();
            _overarchingPanel.SuspendLayout();

            _overarchingPanel.Anchor = UIConst.AutoSize;
            _overarchingPanel.AutoSize = true;
            _overarchingPanel.Dock = DockStyle.Fill;
            _overarchingPanel.Location = new System.Drawing.Point(0, 0);
            _overarchingPanel.Margin = new Padding(0);
            _overarchingPanel.Padding = new Padding(0);
            _overarchingPanel.Name = "_overarchingPanel";
            _overarchingPanel.Size = new System.Drawing.Size(100, 20);
            _overarchingPanel.ColumnCount = 1;
            _overarchingPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _overarchingPanel.RowCount = 2;
            _overarchingPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            _overarchingPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            int rowIndex = 0;
            _overarchingPanel.Controls.Add(BuildButtonPanel(), 0, rowIndex++);
            _overarchingPanel.Controls.Add(BuildPropertyCtrl(), 0, rowIndex++);

            Controls.Add(_overarchingPanel);
            this.Size = new System.Drawing.Size(100, 40);

            _overarchingPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void OnLoad(object sender, EventArgs e)
        {
        }

        #region header panel
        private FlowLayoutPanel BuildButtonPanel()
        {
            int tabIndex = 0;

            FlowLayoutPanel headPanel = new FlowLayoutPanel();
            headPanel.SuspendLayout();

            headPanel.Anchor = UIConst.AutoSize;
            headPanel.Dock = DockStyle.Fill;
            headPanel.FlowDirection = FlowDirection.LeftToRight;
            headPanel.AutoSize = true;
            AddCounterBox(headPanel, tabIndex++);
            AddButtons(LoadButtonInfoList(), headPanel, tabIndex++);

            headPanel.ResumeLayout(false);
            return headPanel;
        }
        private void AddCounterBox(Panel container, int tabIndex)
        {
            container.SuspendLayout();

            _textBoxCounter = new TextBox();
            _textBoxCounter.Anchor = UIConst.AutoSize;
            _textBoxCounter.Margin = new Padding(0, 3, 6, 3);
            _textBoxCounter.Size = new Size(40, 20);
            _textBoxCounter.TabIndex = tabIndex;
            container.Controls.Add(_textBoxCounter);

            container.ResumeLayout(false);
        }

        private ButtonInfo[] LoadButtonInfoList()
        {
            if (_btnFirst == null)
            {
                _btnFirst = new ButtonInfo("|<<", OnFirst, null);
                _btnPrevious = new ButtonInfo("<", OnPrevious, null);
                _btnNext = new ButtonInfo(">", OnNext, null);
                _btnLast = new ButtonInfo(">>|", OnLast, null);
                _btnNew = new ButtonInfo("&New", OnNew, null);
                _btnDelete = new ButtonInfo("&Delete", OnDelete, null);
            }

            return new ButtonInfo[] { _btnFirst, _btnPrevious, _btnNext, _btnLast, _btnNew, _btnDelete };
        }
        private void AddButtons(IEnumerable<ButtonInfo> btnInfoList, Panel container, int tabIndex)
        {
            container.SuspendLayout();

            foreach (ButtonInfo btnInfo in btnInfoList)
                container.Controls.Add(BuildButton(btnInfo, tabIndex++));

            container.ResumeLayout(false);
        }
        private Button BuildButton(ButtonInfo btnInfo, int tabIndex)
        {
            Button btn = new Button();
            btn.Name = btnInfo.X;
            btn.Size = new System.Drawing.Size(48, 20);
            btn.TabIndex = tabIndex;
            btn.Text = btn.Name;
            btn.UseVisualStyleBackColor = true;

            btn.Click += new System.EventHandler(btnInfo.Y);
            btnInfo.Z = btn;

            return btn;
        }

        private PropertyCtrl BuildPropertyCtrl()
        {
            _objControl = new PropertyCtrl();
            _objControl.Anchor = UIConst.AutoSize;
            _objControl.Dock = DockStyle.Fill;
            _objControl.Margin = new Padding(0);
            _objControl.Padding = new Padding(0);
            _objControl.Load += new EventHandler(OnLoad);

            return _objControl;
        }
        #endregion

        #endregion

        #region Event handler
        private void OnFirst(object sender, System.EventArgs e)
        {
            UpdateCtrl(_iter.First);
        }
        private void OnNext(object sender, System.EventArgs e)
        {
            UpdateCtrl(_iter.Next);
        }
        private void OnPrevious(object sender, System.EventArgs e)
        {
            UpdateCtrl(_iter.Previous);
        }
        private void OnLast(object sender, System.EventArgs e)
        {
            UpdateCtrl(_iter.Last);
        }
        private void OnNew(object sender, System.EventArgs e)
        {
        }
        private void OnDelete(object sender, System.EventArgs e)
        {
        }
        #endregion

        private void UpdateCtrl(object obj)
        {
            _objControl.OnOk();
            _objControl.SetObject(obj, null, _borderStyle);
        }
        #endregion

        #region private data
        private TableLayoutPanel _overarchingPanel;
        private PropertyCtrl _objControl;
        private TextBox _textBoxCounter;
        private ButtonInfo _btnFirst, _btnPrevious, _btnNext, _btnLast, _btnNew, _btnDelete;
        private CellBorderStyle _borderStyle = CellBorderStyle.None;

        private Traverser _iter;
        #endregion
    }

    /// <summary>
    /// Prefer use it with collections with limited count (disconnected)
    /// </summary>
    internal sealed class Traverser
    {
        public Traverser(IList collection)
        {
            _list = collection;
            Debug.Assert(_list.Count > 0);
        }

        public object First
        {
            get { return TryMove(0); }
        }
        public object Next
        {
            get {return TryMove(_currentInd + 1); }
        }
        public object Previous
        {
            get { return TryMove(_currentInd - 1); }
        }
        public object Last
        {
            get {return TryMove(_list.Count - 1); }
        }
        public object Current
        {
            get { return _list[_currentInd]; }
        }

        public object this[int index]
        {
            get
            {
                if (IsValid(index))
                    _currentInd = index;
                return _list[index];
            }
            set
            {
                if (IsValid(index))
                    _currentInd = index;
                _list[index] = value;
            }
        }

        #region private functions
        private object TryMove(int intendedIndex)
        {
            if (_currentInd != intendedIndex && IsValid(intendedIndex))
                _currentInd = intendedIndex;

            return _list[_currentInd];
        }

        private bool IsValid(int index)
        {
            return (index > -1 && index <= _list.Count - 1);
        }
        #endregion

        #region private data
        private IList _list;
        private int _currentInd = -1;
        #endregion
    }

}
