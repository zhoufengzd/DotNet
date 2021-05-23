namespace Zen.LaunchPad
{
    partial class MainDlg
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
            this._buttonRun = new System.Windows.Forms.Button();
            this._buttonSaveCmds = new System.Windows.Forms.Button();
            this._buttonSelectProgram = new System.Windows.Forms.Button();
            this._tabControl = new System.Windows.Forms.TabControl();
            this._tabPageCmd = new System.Windows.Forms.TabPage();
            this._tabPageSettings = new System.Windows.Forms.TabPage();
            this._propertyCtrlSettings = new Zen.UIControls.PropertyCtrl();
            this._processListCtrl = new Zen.UIControls.PropertyCtrl();
            this._tabControl.SuspendLayout();
            this._tabPageCmd.SuspendLayout();
            this._tabPageSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // _buttonRun
            // 
            this._buttonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonRun.BackColor = System.Drawing.SystemColors.Info;
            this._buttonRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._buttonRun.Location = new System.Drawing.Point(413, 72);
            this._buttonRun.Name = "_buttonRun";
            this._buttonRun.Size = new System.Drawing.Size(88, 23);
            this._buttonRun.TabIndex = 2;
            this._buttonRun.Text = "&Run";
            this._buttonRun.UseVisualStyleBackColor = false;
            this._buttonRun.Click += new System.EventHandler(this.OnRun);
            // 
            // _buttonSaveCmds
            // 
            this._buttonSaveCmds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonSaveCmds.BackColor = System.Drawing.SystemColors.Info;
            this._buttonSaveCmds.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._buttonSaveCmds.Location = new System.Drawing.Point(413, 110);
            this._buttonSaveCmds.Name = "_buttonSaveCmds";
            this._buttonSaveCmds.Size = new System.Drawing.Size(88, 23);
            this._buttonSaveCmds.TabIndex = 3;
            this._buttonSaveCmds.Text = "&Cancel";
            this._buttonSaveCmds.UseVisualStyleBackColor = false;
            this._buttonSaveCmds.Click += new System.EventHandler(this.OnCancel);
            // 
            // _buttonSelectProgram
            // 
            this._buttonSelectProgram.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonSelectProgram.BackColor = System.Drawing.SystemColors.Info;
            this._buttonSelectProgram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._buttonSelectProgram.Location = new System.Drawing.Point(413, 34);
            this._buttonSelectProgram.Name = "_buttonSelectProgram";
            this._buttonSelectProgram.Size = new System.Drawing.Size(88, 23);
            this._buttonSelectProgram.TabIndex = 4;
            this._buttonSelectProgram.Text = "&Browse...";
            this._buttonSelectProgram.UseVisualStyleBackColor = false;
            this._buttonSelectProgram.Click += new System.EventHandler(this.OnBrowse);
            // 
            // _tabControl
            // 
            this._tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tabControl.Controls.Add(this._tabPageCmd);
            this._tabControl.Controls.Add(this._tabPageSettings);
            this._tabControl.Location = new System.Drawing.Point(12, 12);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(385, 210);
            this._tabControl.TabIndex = 5;
            // 
            // _tabPageCmd
            // 
            this._tabPageCmd.Controls.Add(this._processListCtrl);
            this._tabPageCmd.Location = new System.Drawing.Point(4, 22);
            this._tabPageCmd.Name = "_tabPageCmd";
            this._tabPageCmd.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageCmd.Size = new System.Drawing.Size(377, 184);
            this._tabPageCmd.TabIndex = 0;
            this._tabPageCmd.Text = "Process";
            this._tabPageCmd.UseVisualStyleBackColor = true;
            // 
            // _tabPageSettings
            // 
            this._tabPageSettings.Controls.Add(this._propertyCtrlSettings);
            this._tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this._tabPageSettings.Name = "_tabPageSettings";
            this._tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageSettings.Size = new System.Drawing.Size(377, 184);
            this._tabPageSettings.TabIndex = 2;
            this._tabPageSettings.Text = "Settings";
            this._tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // _propertyCtrlSettings
            // 
            this._propertyCtrlSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this._propertyCtrlSettings.Location = new System.Drawing.Point(3, 3);
            this._propertyCtrlSettings.Name = "_propertyCtrlSettings";
            this._propertyCtrlSettings.Size = new System.Drawing.Size(371, 178);
            this._propertyCtrlSettings.TabIndex = 0;
            // 
            // _processListCtrl
            // 
            this._processListCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._processListCtrl.Location = new System.Drawing.Point(3, 3);
            this._processListCtrl.Name = "_processListCtrl";
            this._processListCtrl.Size = new System.Drawing.Size(371, 178);
            this._processListCtrl.TabIndex = 0;
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 234);
            this.Controls.Add(this._tabControl);
            this.Controls.Add(this._buttonSelectProgram);
            this.Controls.Add(this._buttonSaveCmds);
            this.Controls.Add(this._buttonRun);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainDlg";
            this.Text = "Windows Launch Pad";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this._tabControl.ResumeLayout(false);
            this._tabPageCmd.ResumeLayout(false);
            this._tabPageSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _buttonRun;
        private System.Windows.Forms.Button _buttonSaveCmds;
        private System.Windows.Forms.Button _buttonSelectProgram;
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage _tabPageCmd;
        private System.Windows.Forms.TabPage _tabPageSettings;
        private Zen.UIControls.PropertyCtrl _propertyCtrlSettings;
        private Zen.UIControls.PropertyCtrl _processListCtrl;
    }
}

