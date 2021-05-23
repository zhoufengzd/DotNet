using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Zen.UIControls.Layout;

namespace Zen.FileProcessor
{
    public partial class TextProcessorForm
    {
        #region protected functions
        /// <summary> Called during 'OnFormLoad' </summary>
        protected override void LoadCustomComponent()
        {
            this.Title = "Text Processor";

            AddMenuItems();
            AddOutputTextBox();
            AddOutputGrid();
        }

        #endregion

        #region private functions
        /// <summary>
        /// Add customized menu items
        /// </summary>
        private void AddMenuItems()
        {
            _separatorEditAdvanced = new ToolStripSeparator();
            _menuItemAdvanced = new ToolStripMenuItem("Ad&vanced");
            _menuItemReplace = new ToolStripMenuItem("&Replace...", null, OnReplace);
            _menuItemTextMining = new ToolStripMenuItem("Text &Mining", null, OnTextMining);

            _menuItemAdvanced.DropDownItems.Add(_menuItemReplace);
            _menuItemAdvanced.DropDownItems.Add(_menuItemTextMining);

            // Attached to the base standard menu
            editToolStripMenuItem.DropDownItems.Add(_separatorEditAdvanced);
            editToolStripMenuItem.DropDownItems.Add(_menuItemAdvanced);

            _menuItemBatchReplace = new ToolStripMenuItem("&Batch Replace...", null, OnBatchReplaceOperation);
            toolsToolStripMenuItem.DropDownItems.Add(_menuItemBatchReplace);

            _menuItemBatchTxtMing = new ToolStripMenuItem("&Batch Text Mining...", null, OnBatchTextMining);
            toolsToolStripMenuItem.DropDownItems.Add(_menuItemBatchTxtMing);

            _menuItemToggleOutput = new ToolStripMenuItem("&Output", null, OnToggleOutput);
            viewToolStripMenuItem.DropDownItems.Add(_menuItemToggleOutput);
        }

        private void AddOutputTextBox()
        {
            _textBoxOutput = new TextBox();
            _textBoxOutput.Visible = false;
            _textBoxOutput.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            _textBoxOutput.Location = new Point(0, 0);
            _textBoxOutput.Size = new Size(0, 0);

            _textBoxOutput.Multiline = true;
            _textBoxOutput.ScrollBars = ScrollBars.Both;
            _textBoxOutput.TabIndex = _textBoxInput.TabIndex + 1;
            _textBoxOutput.WordWrap = false;
            _panelClient.Controls.Add(_textBoxOutput);
        }

        private void AddOutputGrid()
        {
            _dataGridViewOutput = new DataGridView();
            _dataGridViewOutput.Visible = false;
            _dataGridViewOutput.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            _dataGridViewOutput.Location = new Point(0, 0);
            _dataGridViewOutput.Size = new Size(0, 0);

            _dataGridViewOutput.AllowUserToAddRows = false;
            _dataGridViewOutput.AllowUserToDeleteRows = false;
            _dataGridViewOutput.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            _dataGridViewOutput.TabIndex = _textBoxInput.TabIndex + 1;
            _panelClient.Controls.Add(_dataGridViewOutput);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_textBoxOutput != null) // To skip the call during InitializeComponent()
                ResizeControls();
        }

        private void ResizeControls()
        {
            bool ouputVisible = (_textBoxOutput.Visible || _dataGridViewOutput.Visible);
            _panelClient.SuspendLayout();
            if (ouputVisible)
            {
                int width = _panelClient.Size.Width / 2 - UIDefault.Margin;
                _textBoxInput.Size = new Size(width, _panelClient.Size.Height);

                if (_textBoxOutput.Visible)
                {
                    _textBoxOutput.Location = new Point(width + UIDefault.Margin, _textBoxInput.Location.Y);
                    _textBoxOutput.Size = new Size(width, _panelClient.Size.Height);
                }
                else
                {
                    _dataGridViewOutput.Location = new Point(width + UIDefault.Margin, _textBoxInput.Location.Y);
                    _dataGridViewOutput.Size = new Size(width, _panelClient.Size.Height);
                }
            }
            else
            {
                _textBoxInput.Size = new Size(_panelClient.Size.Width, _panelClient.Size.Height);
            }
            _panelClient.ResumeLayout(false);
        }
        #endregion

        #region private UI element
        private ToolStripSeparator _separatorEditAdvanced;
        private ToolStripMenuItem _menuItemAdvanced;
        private ToolStripMenuItem _menuItemReplace;
        private ToolStripMenuItem _menuItemTextMining;
        private ToolStripMenuItem _menuItemBatchReplace;
        private ToolStripMenuItem _menuItemBatchTxtMing;
        private ToolStripMenuItem _menuItemToggleOutput;

        private TextBox _textBoxOutput;
        private DataGridView _dataGridViewOutput;
        #endregion
    }
}
