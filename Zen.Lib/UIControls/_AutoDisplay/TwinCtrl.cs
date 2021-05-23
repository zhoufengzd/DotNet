using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Zen.Common.Def;

namespace Zen.UIControls
{
    using CellBorderStyle = TableLayoutPanelCellBorderStyle;

    public sealed class TwinCtrl : UserControl, IAutoControl<Twin<object>>
    {
        public TwinCtrl()
        {
            InitializeComponent();
        }

        public int SetObject(Twin<object> twinObj)
        {
            return SetObject(twinObj, null, null, CellBorderStyle.None);
        }

        // To do: test same type / data reload / etc
        public int SetObject(Twin<object> twinObj, Dictionary<string, IEnumerable<string>> valueHints, Dictionary<string, string> lableHints, CellBorderStyle borderStyle)
        {
            _twinObj = twinObj;
            Type tp = _twinObj.X.GetType();

            _overarchingPanel.SuspendLayout();
            _overarchingPanel.CellBorderStyle = borderStyle;

            int rowIndex = 0;
            //DrawLabel(ref rowIndex);
            //DrawSeparator(ref rowIndex);
            DrawData(tp, ref rowIndex);

            _overarchingPanel.ResumeLayout();
            return rowIndex;
        }

        #region Initialization
        private void InitializeComponent()
        {
            this._overarchingPanel = new System.Windows.Forms.TableLayoutPanel();
            this._propertyCtrlLeft = new Zen.UIControls.PropertyCtrl();
            this._propertyCtrlRight = new Zen.UIControls.PropertyCtrl();
            this._overarchingPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _overarchingPanel
            // 
            this._overarchingPanel.AutoSize = true;
            this._overarchingPanel.ColumnCount = 2;
            this._overarchingPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._overarchingPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._overarchingPanel.Controls.Add(this._propertyCtrlRight, 1, 0);
            this._overarchingPanel.Controls.Add(this._propertyCtrlLeft, 0, 0);
            this._overarchingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._overarchingPanel.Location = new System.Drawing.Point(0, 0);
            this._overarchingPanel.Margin = new System.Windows.Forms.Padding(0);
            this._overarchingPanel.Name = "_overarchingPanel";
            this._overarchingPanel.RowCount = 1;
            this._overarchingPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._overarchingPanel.Size = new System.Drawing.Size(89, 21);
            this._overarchingPanel.TabIndex = 0;
            // 
            // _propertyCtrlLeft
            // 
            this._propertyCtrlLeft.AutoSize = true;
            this._propertyCtrlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this._propertyCtrlLeft.Location = new System.Drawing.Point(0, 0);
            this._propertyCtrlLeft.Margin = new System.Windows.Forms.Padding(0);
            this._propertyCtrlLeft.Name = "_propertyCtrlLeft";
            this._propertyCtrlLeft.TabIndex = 0;
            // 
            // _propertyCtrlRight
            // 
            this._propertyCtrlRight.AutoSize = true;
            this._propertyCtrlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this._propertyCtrlRight.Location = new System.Drawing.Point(54, 0);
            this._propertyCtrlRight.Margin = new System.Windows.Forms.Padding(0);
            this._propertyCtrlRight.Name = "_propertyCtrlRight";
            this._propertyCtrlRight.TabIndex = 1;
            // 
            // TwinCtrl
            // 
            this.Controls.Add(this._overarchingPanel);
            this.Name = "TwinCtrl";
            this.Size = new System.Drawing.Size(108, 20);
            this._overarchingPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void OnLoad(object sender, EventArgs e)
        {
        }

        private void DrawLabel(ref int rowIndex)
        {
            _overarchingPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            Label lblKey = new Label();
            lblKey.AutoSize = true;
            lblKey.Text = "X";
            lblKey.Padding = new Padding(0, 0, 0, 0);
            lblKey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _overarchingPanel.Controls.Add(lblKey, 0, rowIndex);

            Label lblValue = new Label();
            lblValue.AutoSize = true;
            lblValue.Text = "Y";
            lblValue.Padding = new Padding(0, 0, 0, 0);
            lblValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _overarchingPanel.Controls.Add(lblValue, 1, rowIndex);

            rowIndex++;
        }

        private void DrawSeparator(ref int rowIndex)
        {
            _overarchingPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            HorizontalLineCtrl line = new HorizontalLineCtrl();
            line.Padding = new Padding(0, 0, 0, 0);
            line.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            _overarchingPanel.SetColumnSpan(line, 2);
            _overarchingPanel.Controls.Add(line, 0, rowIndex);

            rowIndex++;
        }

        private void DrawData(Type tp, ref int rowIndex)
        {
            //GenBuilder.BuildInstance(typeof(PropertyEnvelope<>), new Type[] { tp });
            _propertyCtrlLeft.SetObject(new PropertyEnvelope<object>(_twinObj.X));
            _propertyCtrlRight.SetObject(new PropertyEnvelope<object>(_twinObj.Y));
        }

        #endregion

        #region private data
        private TableLayoutPanel _overarchingPanel;
        private PropertyCtrl _propertyCtrlLeft;
        private PropertyCtrl _propertyCtrlRight;
        private Twin<object> _twinObj;
        #endregion

    }
}
