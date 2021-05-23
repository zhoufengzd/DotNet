namespace Zen.DBAToolbox
{
    partial class SqlControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._overarchingPanel = new System.Windows.Forms.TableLayoutPanel();
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._richTextBoxSql = new System.Windows.Forms.RichTextBox();
            this._tabControlResult = new System.Windows.Forms.TabControl();
            this._tbResultGrid = new System.Windows.Forms.TabPage();
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._gridTblResult = new System.Windows.Forms.DataGridView();
            this._tbResultMsg = new System.Windows.Forms.TabPage();
            this._richTextBoxMsg = new System.Windows.Forms.RichTextBox();
            this._overarchingPanel.SuspendLayout();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this._tabControlResult.SuspendLayout();
            this._tbResultGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridTblResult)).BeginInit();
            this._tbResultMsg.SuspendLayout();
            this.SuspendLayout();
            // 
            // _overarchingPanel
            // 
            this._overarchingPanel.AutoSize = true;
            this._overarchingPanel.ColumnCount = 1;
            this._overarchingPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._overarchingPanel.Controls.Add(this._splitContainer, 0, 1);
            this._overarchingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._overarchingPanel.Location = new System.Drawing.Point(0, 0);
            this._overarchingPanel.Name = "_overarchingPanel";
            this._overarchingPanel.RowCount = 2;
            this._overarchingPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._overarchingPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._overarchingPanel.Size = new System.Drawing.Size(160, 191);
            this._overarchingPanel.TabIndex = 3;
            // 
            // _splitContainer
            // 
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.Location = new System.Drawing.Point(3, 3);
            this._splitContainer.Name = "_splitContainer";
            this._splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this._richTextBoxSql);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this._tabControlResult);
            this._splitContainer.Size = new System.Drawing.Size(154, 185);
            this._splitContainer.SplitterDistance = 92;
            this._splitContainer.TabIndex = 1;
            // 
            // _richTextBoxSql
            // 
            this._richTextBoxSql.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._richTextBoxSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this._richTextBoxSql.Location = new System.Drawing.Point(0, 0);
            this._richTextBoxSql.Name = "_richTextBoxSql";
            this._richTextBoxSql.Size = new System.Drawing.Size(154, 92);
            this._richTextBoxSql.TabIndex = 0;
            this._richTextBoxSql.Text = "";
            // 
            // _tabControlResult
            // 
            this._tabControlResult.Controls.Add(this._tbResultGrid);
            this._tabControlResult.Controls.Add(this._tbResultMsg);
            this._tabControlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControlResult.Location = new System.Drawing.Point(0, 0);
            this._tabControlResult.Name = "_tabControlResult";
            this._tabControlResult.SelectedIndex = 0;
            this._tabControlResult.Size = new System.Drawing.Size(154, 89);
            this._tabControlResult.TabIndex = 0;
            // 
            // _tbResultGrid
            // 
            this._tbResultGrid.Controls.Add(this._statusStrip);
            this._tbResultGrid.Controls.Add(this._gridTblResult);
            this._tbResultGrid.Location = new System.Drawing.Point(4, 22);
            this._tbResultGrid.Name = "_tbResultGrid";
            this._tbResultGrid.Padding = new System.Windows.Forms.Padding(3);
            this._tbResultGrid.Size = new System.Drawing.Size(146, 63);
            this._tbResultGrid.TabIndex = 0;
            this._tbResultGrid.Text = "Result";
            this._tbResultGrid.UseVisualStyleBackColor = true;
            // 
            // _statusStrip
            // 
            this._statusStrip.Location = new System.Drawing.Point(3, 38);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(140, 22);
            this._statusStrip.TabIndex = 1;
            this._statusStrip.Text = "statusStrip1";
            // 
            // _gridTblResult
            // 
            this._gridTblResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridTblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridTblResult.GridColor = System.Drawing.SystemColors.Window;
            this._gridTblResult.Location = new System.Drawing.Point(3, 3);
            this._gridTblResult.Name = "_gridTblResult";
            this._gridTblResult.Size = new System.Drawing.Size(140, 57);
            this._gridTblResult.TabIndex = 0;
            // 
            // _tbResultMsg
            // 
            this._tbResultMsg.Controls.Add(this._richTextBoxMsg);
            this._tbResultMsg.Location = new System.Drawing.Point(4, 22);
            this._tbResultMsg.Name = "_tbResultMsg";
            this._tbResultMsg.Padding = new System.Windows.Forms.Padding(3);
            this._tbResultMsg.Size = new System.Drawing.Size(146, 63);
            this._tbResultMsg.TabIndex = 1;
            this._tbResultMsg.Text = "Messages";
            this._tbResultMsg.UseVisualStyleBackColor = true;
            // 
            // _richTextBoxMsg
            // 
            this._richTextBoxMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._richTextBoxMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this._richTextBoxMsg.Location = new System.Drawing.Point(3, 3);
            this._richTextBoxMsg.Name = "_richTextBoxMsg";
            this._richTextBoxMsg.Size = new System.Drawing.Size(140, 57);
            this._richTextBoxMsg.TabIndex = 0;
            this._richTextBoxMsg.Text = "";
            // 
            // SqlControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._overarchingPanel);
            this.Name = "SqlControl";
            this.Size = new System.Drawing.Size(160, 191);
            this._overarchingPanel.ResumeLayout(false);
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            this._splitContainer.ResumeLayout(false);
            this._tabControlResult.ResumeLayout(false);
            this._tbResultGrid.ResumeLayout(false);
            this._tbResultGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridTblResult)).EndInit();
            this._tbResultMsg.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _overarchingPanel;
        private System.Windows.Forms.SplitContainer _splitContainer;
        private System.Windows.Forms.RichTextBox _richTextBoxSql;
        private System.Windows.Forms.TabControl _tabControlResult;
        private System.Windows.Forms.TabPage _tbResultGrid;
        private System.Windows.Forms.DataGridView _gridTblResult;
        private System.Windows.Forms.TabPage _tbResultMsg;
        private System.Windows.Forms.RichTextBox _richTextBoxMsg;
        private System.Windows.Forms.StatusStrip _statusStrip;
    }
}
