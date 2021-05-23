using System.Windows.Forms;

namespace Zen.UIControls.Layout
{
    /// <summary>
    /// TableLayoutPanel with builtin: Header + Body + Footer
    /// </summary>
    public class HeadBodyFootCtrl: UserControl
    {
        public HeadBodyFootCtrl()
        {
            InitializeComponent();
        }

        #region Layout functions
        private void InitializeComponent()
        {
            _overarchingPanel = new TableLayoutPanel();
            SuspendLayout();
            // 
            // _overarchingPanel
            // 
            _overarchingPanel.Anchor = UIConst.AutoSize;
            _overarchingPanel.AutoScroll = true;
            _overarchingPanel.ColumnCount = 1;
            _overarchingPanel.ColumnStyles.Add(new ColumnStyle());
            _overarchingPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            _overarchingPanel.Location = new System.Drawing.Point(0, 0);
            _overarchingPanel.Margin = new Padding(0);
            _overarchingPanel.Name = "_overarchingPanel";
            _overarchingPanel.RowCount = 3;
            _overarchingPanel.RowStyles.Add(new RowStyle());
            _overarchingPanel.RowStyles.Add(new RowStyle());
            _overarchingPanel.RowStyles.Add(new RowStyle());
            _overarchingPanel.Size = new System.Drawing.Size(164, 148);
            _overarchingPanel.TabIndex = 0;
            // 
            // HeadBodyFootCtrl
            // 
            Controls.Add(_overarchingPanel);
            Name = "HeadBodyFootCtrl";
            Size = new System.Drawing.Size(164, 148);
            ResumeLayout(false);
        }

        protected PanelT BuildPanel<PanelT>(TableLayoutPanel container, int rowIndex) where PanelT : Panel, new()
        {
            PanelT panel = new PanelT();
            panel.Anchor = UIConst.AutoSize;
            panel.AutoSize = true;
            container.Controls.Add(panel, 0, rowIndex);

            return panel;
        }
        #endregion

        #region Windows Form Designer generated variables
        protected TableLayoutPanel _overarchingPanel;
        #endregion
    }
}
