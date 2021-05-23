namespace Zen.DBAToolbox
{
    partial class TableViewExplorer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._treeView = new System.Windows.Forms.TreeView();
            this._detailPanel = new System.Windows.Forms.TableLayoutPanel();
            this._tabDetailView = new System.Windows.Forms.TabControl();
            this._basePanel = new System.Windows.Forms.TableLayoutPanel();
            this._toolStripBtns = new System.Windows.Forms.ToolStrip();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this._detailPanel.SuspendLayout();
            this._basePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _splitContainer
            // 
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.Location = new System.Drawing.Point(3, 33);
            this._splitContainer.Name = "_splitContainer";
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this._treeView);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this._detailPanel);
            this._splitContainer.Size = new System.Drawing.Size(377, 301);
            this._splitContainer.SplitterDistance = 125;
            this._splitContainer.TabIndex = 0;
            // 
            // _treeView
            // 
            this._treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeView.Location = new System.Drawing.Point(0, 0);
            this._treeView.Name = "_treeView";
            this._treeView.Size = new System.Drawing.Size(125, 301);
            this._treeView.TabIndex = 0;
            this._treeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnContextMenu);
            // 
            // _detailPanel
            // 
            this._detailPanel.ColumnCount = 1;
            this._detailPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._detailPanel.Controls.Add(this._tabDetailView, 0, 1);
            this._detailPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._detailPanel.Location = new System.Drawing.Point(0, 0);
            this._detailPanel.Name = "_detailPanel";
            this._detailPanel.RowCount = 2;
            this._detailPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._detailPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._detailPanel.Size = new System.Drawing.Size(248, 301);
            this._detailPanel.TabIndex = 1;
            // 
            // _tabDetailView
            // 
            this._tabDetailView.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this._tabDetailView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabDetailView.Location = new System.Drawing.Point(3, 3);
            this._tabDetailView.Name = "_tabDetailView";
            this._tabDetailView.SelectedIndex = 0;
            this._tabDetailView.Size = new System.Drawing.Size(242, 295);
            this._tabDetailView.TabIndex = 0;
            // 
            // _basePanel
            // 
            this._basePanel.ColumnCount = 1;
            this._basePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._basePanel.Controls.Add(this._splitContainer, 0, 1);
            this._basePanel.Controls.Add(this._toolStripBtns, 0, 0);
            this._basePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._basePanel.Location = new System.Drawing.Point(0, 0);
            this._basePanel.Name = "_basePanel";
            this._basePanel.RowCount = 2;
            this._basePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this._basePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._basePanel.Size = new System.Drawing.Size(383, 337);
            this._basePanel.TabIndex = 1;
            // 
            // _toolStripBtns
            // 
            this._toolStripBtns.Location = new System.Drawing.Point(0, 0);
            this._toolStripBtns.Name = "_toolStripBtns";
            this._toolStripBtns.Size = new System.Drawing.Size(383, 25);
            this._toolStripBtns.TabIndex = 1;
            // 
            // TableViewExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 337);
            this.ControlBox = false;
            this.Controls.Add(this._basePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TableViewExplorer";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.OnLoad);
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            this._splitContainer.ResumeLayout(false);
            this._detailPanel.ResumeLayout(false);
            this._basePanel.ResumeLayout(false);
            this._basePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer _splitContainer;
        private System.Windows.Forms.TreeView _treeView;
        private System.Windows.Forms.TabControl _tabDetailView;
        private System.Windows.Forms.TableLayoutPanel _detailPanel;
        private System.Windows.Forms.TableLayoutPanel _basePanel;
        private System.Windows.Forms.ToolStrip _toolStripBtns;
    }
}