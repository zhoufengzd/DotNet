namespace Zen.DirectoryMonitor
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
            this._buttonStart = new System.Windows.Forms.Button();
            this._buttonStop = new System.Windows.Forms.Button();
            this._textBoxOut = new System.Windows.Forms.TextBox();
            this._tabControl = new System.Windows.Forms.TabControl();
            this._tabPageOptions = new System.Windows.Forms.TabPage();
            this._tabPageResult = new System.Windows.Forms.TabPage();
            this._buttonClearLog = new System.Windows.Forms.Button();
            this._optionsCtrl = new Zen.UIControls.PropertyCtrl();
            this._tabControl.SuspendLayout();
            this._tabPageOptions.SuspendLayout();
            this._tabPageResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // _buttonStart
            // 
            this._buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonStart.Location = new System.Drawing.Point(394, 33);
            this._buttonStart.Name = "_buttonStart";
            this._buttonStart.Size = new System.Drawing.Size(64, 23);
            this._buttonStart.TabIndex = 0;
            this._buttonStart.Text = "S&tart";
            this._buttonStart.Click += new System.EventHandler(this.OnStart);
            // 
            // _buttonStop
            // 
            this._buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonStop.Location = new System.Drawing.Point(394, 62);
            this._buttonStop.Name = "_buttonStop";
            this._buttonStop.Size = new System.Drawing.Size(64, 23);
            this._buttonStop.TabIndex = 1;
            this._buttonStop.Text = "Sto&p";
            this._buttonStop.Click += new System.EventHandler(this.OnStop);
            // 
            // _textBoxOut
            // 
            this._textBoxOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxOut.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._textBoxOut.Location = new System.Drawing.Point(0, 0);
            this._textBoxOut.Multiline = true;
            this._textBoxOut.Name = "_textBoxOut";
            this._textBoxOut.ReadOnly = true;
            this._textBoxOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._textBoxOut.Size = new System.Drawing.Size(356, 172);
            this._textBoxOut.TabIndex = 3;
            this._textBoxOut.WordWrap = false;
            // 
            // _tabControl
            // 
            this._tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tabControl.Controls.Add(this._tabPageOptions);
            this._tabControl.Controls.Add(this._tabPageResult);
            this._tabControl.Location = new System.Drawing.Point(12, 12);
            this._tabControl.Multiline = true;
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(364, 198);
            this._tabControl.TabIndex = 3;
            // 
            // _tabPageOptions
            // 
            this._tabPageOptions.Controls.Add(this._optionsCtrl);
            this._tabPageOptions.Location = new System.Drawing.Point(4, 22);
            this._tabPageOptions.Name = "_tabPageOptions";
            this._tabPageOptions.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageOptions.Size = new System.Drawing.Size(356, 172);
            this._tabPageOptions.TabIndex = 3;
            this._tabPageOptions.Text = "Options";
            this._tabPageOptions.UseVisualStyleBackColor = true;
            // 
            // _tabPageResult
            // 
            this._tabPageResult.Controls.Add(this._textBoxOut);
            this._tabPageResult.Location = new System.Drawing.Point(4, 22);
            this._tabPageResult.Name = "_tabPageResult";
            this._tabPageResult.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageResult.Size = new System.Drawing.Size(356, 172);
            this._tabPageResult.TabIndex = 3;
            this._tabPageResult.Text = "Results";
            this._tabPageResult.UseVisualStyleBackColor = true;
            // 
            // _buttonClearLog
            // 
            this._buttonClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonClearLog.Location = new System.Drawing.Point(394, 91);
            this._buttonClearLog.Name = "_buttonClearLog";
            this._buttonClearLog.Size = new System.Drawing.Size(64, 23);
            this._buttonClearLog.TabIndex = 2;
            this._buttonClearLog.Text = "&Clear Log";
            this._buttonClearLog.Click += new System.EventHandler(this.OnClearLog);
            // 
            // _optionsCtrl
            // 
            this._optionsCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._optionsCtrl.Location = new System.Drawing.Point(3, 3);
            this._optionsCtrl.Name = "_optionsCtrl";
            this._optionsCtrl.Size = new System.Drawing.Size(350, 166);
            this._optionsCtrl.TabIndex = 0;
            // 
            // MainDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(470, 222);
            this.Controls.Add(this._buttonClearLog);
            this.Controls.Add(this._tabControl);
            this.Controls.Add(this._buttonStop);
            this.Controls.Add(this._buttonStart);
            this.Name = "MainDlg";
            this.Text = "Directory Monitor";
            this.Load += new System.EventHandler(this.OnLoad);
            this._tabControl.ResumeLayout(false);
            this._tabPageOptions.ResumeLayout(false);
            this._tabPageResult.ResumeLayout(false);
            this._tabPageResult.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button _buttonStart;
        private System.Windows.Forms.Button _buttonStop;
        private System.Windows.Forms.TextBox _textBoxOut;
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage _tabPageOptions;
        private System.Windows.Forms.TabPage _tabPageResult;
        private System.Windows.Forms.Button _buttonClearLog;
        private Zen.UIControls.PropertyCtrl _optionsCtrl;
    }
}