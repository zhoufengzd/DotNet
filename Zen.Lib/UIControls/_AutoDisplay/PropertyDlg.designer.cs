
namespace Zen.UIControls
{
    partial class PropertyDlg
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
            this._buttonOK = new System.Windows.Forms.Button();
            this._buttonCancel = new System.Windows.Forms.Button();
            this._panelOptions = new System.Windows.Forms.Panel();
            this._panelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this._horizontalLineCtrl = new Zen.UIControls.HorizontalLineCtrl();
            this._panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // _buttonOK
            // 
            this._buttonOK.AccessibleDescription = "";
            this._buttonOK.Location = new System.Drawing.Point(77, 3);
            this._buttonOK.Name = "_buttonOK";
            this._buttonOK.Size = new System.Drawing.Size(75, 23);
            this._buttonOK.TabIndex = 1;
            this._buttonOK.Text = "&OK";
            this._buttonOK.UseVisualStyleBackColor = true;
            this._buttonOK.Click += new System.EventHandler(this.OnOK);
            // 
            // _buttonCancel
            // 
            this._buttonCancel.AccessibleDescription = "";
            this._buttonCancel.Location = new System.Drawing.Point(158, 3);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Size = new System.Drawing.Size(75, 23);
            this._buttonCancel.TabIndex = 2;
            this._buttonCancel.Text = "&Cancel";
            this._buttonCancel.UseVisualStyleBackColor = true;
            this._buttonCancel.Click += new System.EventHandler(this.OnCancel);
            // 
            // _panelOptions
            // 
            this._panelOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._panelOptions.AutoScroll = true;
            this._panelOptions.Location = new System.Drawing.Point(12, 12);
            this._panelOptions.Name = "_panelOptions";
            this._panelOptions.Size = new System.Drawing.Size(233, 24);
            this._panelOptions.TabIndex = 0;
            // 
            // _panelButtons
            // 
            this._panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._panelButtons.Controls.Add(this._buttonCancel);
            this._panelButtons.Controls.Add(this._buttonOK);
            this._panelButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this._panelButtons.Location = new System.Drawing.Point(12, 48);
            this._panelButtons.Margin = new System.Windows.Forms.Padding(0);
            this._panelButtons.Name = "_panelButtons";
            this._panelButtons.Size = new System.Drawing.Size(236, 30);
            this._panelButtons.TabIndex = 7;
            // 
            // _horizontalLineCtrl
            // 
            this._horizontalLineCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._horizontalLineCtrl.BackColor = System.Drawing.Color.Transparent;
            this._horizontalLineCtrl.Color = System.Drawing.SystemColors.Desktop;
            this._horizontalLineCtrl.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this._horizontalLineCtrl.LineWidth = 1;
            this._horizontalLineCtrl.Location = new System.Drawing.Point(6, 40);
            this._horizontalLineCtrl.Margin = new System.Windows.Forms.Padding(0);
            this._horizontalLineCtrl.Name = "_horizontalLineCtrl";
            this._horizontalLineCtrl.Size = new System.Drawing.Size(248, 10);
            this._horizontalLineCtrl.TabIndex = 8;
            this._horizontalLineCtrl.TabStop = false;
            // 
            // PropertyDlg
            // 
            this.ClientSize = new System.Drawing.Size(260, 85);
            this.Controls.Add(this._horizontalLineCtrl);
            this.Controls.Add(this._panelButtons);
            this.Controls.Add(this._panelOptions);
            this.MinimumSize = new System.Drawing.Size(200, 112);
            this.Name = "PropertyDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.OnFormLoad);
            this._panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Windows Form Designer generated variables
        private System.Windows.Forms.Button _buttonOK;
        private System.Windows.Forms.Button _buttonCancel;
        private System.Windows.Forms.Panel _panelOptions;
        private System.Windows.Forms.FlowLayoutPanel _panelButtons;
        #endregion
        private HorizontalLineCtrl _horizontalLineCtrl;

    }
}
