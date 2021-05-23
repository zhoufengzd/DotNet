
namespace Zen.UIControls
{
    partial class PropertyCtrl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer _components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this._overarchingPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // _overarchingPanel
            // 
            this._overarchingPanel.AutoScroll = true;
            this._overarchingPanel.AutoSize = true;
            this._overarchingPanel.ColumnCount = 1;
            this._overarchingPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._overarchingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._overarchingPanel.Location = new System.Drawing.Point(0, 0);
            this._overarchingPanel.Margin = new System.Windows.Forms.Padding(0);
            this._overarchingPanel.Name = "_overarchingPanel";
            this._overarchingPanel.RowCount = 1;
            this._overarchingPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._overarchingPanel.Size = new System.Drawing.Size(60, 20);
            this._overarchingPanel.TabIndex = 0;
            // 
            // PropertyCtrl
            // 
            this.Controls.Add(this._overarchingPanel);
            this.Name = "PropertyCtrl";
            this.Size = new System.Drawing.Size(60, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Windows Form Designer generated variables
        private System.Windows.Forms.TableLayoutPanel _overarchingPanel;
        #endregion
    }
}
