namespace Zen.WinSvcController
{
    partial class SvcControllerDlg
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
            this._buttonStart = new System.Windows.Forms.Button();
            this._buttonStop = new System.Windows.Forms.Button();
            this._buttonPause = new System.Windows.Forms.Button();
            this._comboBoxServiceNames = new System.Windows.Forms.ComboBox();
            this._buttonEnable = new System.Windows.Forms.Button();
            this._buttonDisable = new System.Windows.Forms.Button();
            this._fileDirBrowserCtrl = new Zen.UIControls.FileUI.FileDirBrowserCtrl();
            this._buttonUninstall = new System.Windows.Forms.Button();
            this._buttonInstall = new System.Windows.Forms.Button();
            this._tabControl = new System.Windows.Forms.TabControl();
            this._tabPage1 = new System.Windows.Forms.TabPage();
            this._tabPage2 = new System.Windows.Forms.TabPage();
            this._tabControl.SuspendLayout();
            this._tabPage1.SuspendLayout();
            this._tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _buttonStart
            // 
            this._buttonStart.Location = new System.Drawing.Point(8, 43);
            this._buttonStart.Name = "_buttonStart";
            this._buttonStart.Size = new System.Drawing.Size(53, 23);
            this._buttonStart.TabIndex = 1;
            this._buttonStart.Text = "&Start";
            this._buttonStart.Click += new System.EventHandler(this.OnStart);
            // 
            // _buttonStop
            // 
            this._buttonStop.Location = new System.Drawing.Point(73, 43);
            this._buttonStop.Name = "_buttonStop";
            this._buttonStop.Size = new System.Drawing.Size(53, 23);
            this._buttonStop.TabIndex = 2;
            this._buttonStop.Text = "S&top";
            this._buttonStop.Click += new System.EventHandler(this.OnStop);
            // 
            // _buttonPause
            // 
            this._buttonPause.Location = new System.Drawing.Point(138, 43);
            this._buttonPause.Name = "_buttonPause";
            this._buttonPause.Size = new System.Drawing.Size(53, 23);
            this._buttonPause.TabIndex = 3;
            this._buttonPause.Text = "&Pause";
            this._buttonPause.Click += new System.EventHandler(this.OnPause);
            // 
            // _comboBoxServiceNames
            // 
            this._comboBoxServiceNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboBoxServiceNames.FormattingEnabled = true;
            this._comboBoxServiceNames.Location = new System.Drawing.Point(8, 6);
            this._comboBoxServiceNames.Name = "_comboBoxServiceNames";
            this._comboBoxServiceNames.Size = new System.Drawing.Size(314, 21);
            this._comboBoxServiceNames.TabIndex = 4;
            this._comboBoxServiceNames.SelectedIndexChanged += new System.EventHandler(this.OnSelectedSvcChanged);
            // 
            // _buttonEnable
            // 
            this._buttonEnable.Location = new System.Drawing.Point(203, 43);
            this._buttonEnable.Name = "_buttonEnable";
            this._buttonEnable.Size = new System.Drawing.Size(53, 23);
            this._buttonEnable.TabIndex = 7;
            this._buttonEnable.Text = "&Enable";
            this._buttonEnable.Click += new System.EventHandler(this.OnEnable);
            // 
            // _buttonDisable
            // 
            this._buttonDisable.Location = new System.Drawing.Point(268, 43);
            this._buttonDisable.Name = "_buttonDisable";
            this._buttonDisable.Size = new System.Drawing.Size(53, 23);
            this._buttonDisable.TabIndex = 8;
            this._buttonDisable.Text = "&Disable";
            this._buttonDisable.Click += new System.EventHandler(this.OnDisable);
            // 
            // _fileDirBrowserCtrl
            // 
            this._fileDirBrowserCtrl.Location = new System.Drawing.Point(8, 6);
            this._fileDirBrowserCtrl.Name = "_fileDirBrowserCtrl";
            this._fileDirBrowserCtrl.Text = "";
            this._fileDirBrowserCtrl.Size = new System.Drawing.Size(315, 26);
            this._fileDirBrowserCtrl.TabIndex = 10;
            // 
            // _buttonUninstall
            // 
            this._buttonUninstall.Location = new System.Drawing.Point(256, 43);
            this._buttonUninstall.Name = "_buttonUninstall";
            this._buttonUninstall.Size = new System.Drawing.Size(68, 22);
            this._buttonUninstall.TabIndex = 11;
            this._buttonUninstall.Text = "&UnInstall";
            this._buttonUninstall.Click += new System.EventHandler(this.OnUnInstall);
            // 
            // _buttonInstall
            // 
            this._buttonInstall.Location = new System.Drawing.Point(184, 43);
            this._buttonInstall.Name = "_buttonInstall";
            this._buttonInstall.Size = new System.Drawing.Size(68, 22);
            this._buttonInstall.TabIndex = 9;
            this._buttonInstall.Text = "&Install";
            this._buttonInstall.Click += new System.EventHandler(this.OnInstall);
            // 
            // _tabControl
            // 
            this._tabControl.Controls.Add(this._tabPage1);
            this._tabControl.Controls.Add(this._tabPage2);
            this._tabControl.Location = new System.Drawing.Point(12, 12);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(340, 108);
            this._tabControl.TabIndex = 12;
            // 
            // _tabPage1
            // 
            this._tabPage1.Controls.Add(this._comboBoxServiceNames);
            this._tabPage1.Controls.Add(this._buttonDisable);
            this._tabPage1.Controls.Add(this._buttonPause);
            this._tabPage1.Controls.Add(this._buttonStart);
            this._tabPage1.Controls.Add(this._buttonStop);
            this._tabPage1.Controls.Add(this._buttonEnable);
            this._tabPage1.Location = new System.Drawing.Point(4, 22);
            this._tabPage1.Name = "_tabPage1";
            this._tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this._tabPage1.Size = new System.Drawing.Size(332, 82);
            this._tabPage1.TabIndex = 0;
            this._tabPage1.Text = "Services";
            this._tabPage1.UseVisualStyleBackColor = true;
            // 
            // _tabPage2
            // 
            this._tabPage2.Controls.Add(this._buttonUninstall);
            this._tabPage2.Controls.Add(this._buttonInstall);
            this._tabPage2.Controls.Add(this._fileDirBrowserCtrl);
            this._tabPage2.Location = new System.Drawing.Point(4, 22);
            this._tabPage2.Name = "_tabPage2";
            this._tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this._tabPage2.Size = new System.Drawing.Size(332, 82);
            this._tabPage2.TabIndex = 1;
            this._tabPage2.Text = "Setup";
            this._tabPage2.UseVisualStyleBackColor = true;
            // 
            // MainDlg
            // 
            this.ClientSize = new System.Drawing.Size(363, 130);
            this.Controls.Add(this._tabControl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainDlg";
            this.Text = "Window Service Controller";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this._tabControl.ResumeLayout(false);
            this._tabPage1.ResumeLayout(false);
            this._tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button _buttonStart;
        private System.Windows.Forms.Button _buttonStop;
        private System.Windows.Forms.Button _buttonPause;
        private System.Windows.Forms.ComboBox _comboBoxServiceNames;
        private System.Windows.Forms.Button _buttonEnable;
        private System.Windows.Forms.Button _buttonDisable;
        private Zen.UIControls.FileUI.FileDirBrowserCtrl _fileDirBrowserCtrl;
        private System.Windows.Forms.Button _buttonUninstall;
        private System.Windows.Forms.Button _buttonInstall;
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage _tabPage1;
        private System.Windows.Forms.TabPage _tabPage2;
    }

}
