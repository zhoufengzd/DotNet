namespace Zen.UIControls
{
    partial class SvcRegisterDlg
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
            this._buttonInstall = new System.Windows.Forms.Button();
            this._svcLogonCtrl = new Zen.UIControls.UserPasswdCtrl();
            this._groupBox = new System.Windows.Forms.GroupBox();
            this._radioButtonWinUser = new System.Windows.Forms.RadioButton();
            this._radioButtonLocalSys = new System.Windows.Forms.RadioButton();
            this._buttonUnInstall = new System.Windows.Forms.Button();
            this._groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _buttonInstall
            // 
            this._buttonInstall.Location = new System.Drawing.Point(338, 12);
            this._buttonInstall.Name = "_buttonInstall";
            this._buttonInstall.Size = new System.Drawing.Size(75, 23);
            this._buttonInstall.TabIndex = 1;
            this._buttonInstall.Text = "&Install";
            this._buttonInstall.UseVisualStyleBackColor = true;
            this._buttonInstall.Click += new System.EventHandler(this.OnInstall);
            // 
            // _svcLogonCtrl
            // 
            this._svcLogonCtrl.Location = new System.Drawing.Point(34, 33);
            this._svcLogonCtrl.Name = "_svcLogonCtrl";
            this._svcLogonCtrl.PassWord = null;
            this._svcLogonCtrl.Size = new System.Drawing.Size(273, 57);
            this._svcLogonCtrl.TabIndex = 4;
            this._svcLogonCtrl.UserName = null;
            // 
            // _groupBox
            // 
            this._groupBox.Controls.Add(this._radioButtonWinUser);
            this._groupBox.Controls.Add(this._radioButtonLocalSys);
            this._groupBox.Controls.Add(this._svcLogonCtrl);
            this._groupBox.Location = new System.Drawing.Point(12, 12);
            this._groupBox.Name = "_groupBox";
            this._groupBox.Size = new System.Drawing.Size(313, 131);
            this._groupBox.TabIndex = 5;
            this._groupBox.TabStop = false;
            this._groupBox.Text = "Services Log On As:";
            // 
            // _radioButtonWinUser
            // 
            this._radioButtonWinUser.AutoSize = true;
            this._radioButtonWinUser.Location = new System.Drawing.Point(21, 19);
            this._radioButtonWinUser.Name = "_radioButtonWinUser";
            this._radioButtonWinUser.Size = new System.Drawing.Size(112, 17);
            this._radioButtonWinUser.TabIndex = 6;
            this._radioButtonWinUser.TabStop = true;
            this._radioButtonWinUser.Text = "&Windows Account";
            this._radioButtonWinUser.UseVisualStyleBackColor = true;
            this._radioButtonWinUser.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // _radioButtonLocalSys
            // 
            this._radioButtonLocalSys.AutoSize = true;
            this._radioButtonLocalSys.Location = new System.Drawing.Point(21, 96);
            this._radioButtonLocalSys.Name = "_radioButtonLocalSys";
            this._radioButtonLocalSys.Size = new System.Drawing.Size(131, 17);
            this._radioButtonLocalSys.TabIndex = 5;
            this._radioButtonLocalSys.TabStop = true;
            this._radioButtonLocalSys.Text = "&Local System Account";
            this._radioButtonLocalSys.UseVisualStyleBackColor = true;
            this._radioButtonLocalSys.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // _buttonUnInstall
            // 
            this._buttonUnInstall.Location = new System.Drawing.Point(338, 41);
            this._buttonUnInstall.Name = "_buttonUnInstall";
            this._buttonUnInstall.Size = new System.Drawing.Size(75, 23);
            this._buttonUnInstall.TabIndex = 2;
            this._buttonUnInstall.Text = "&UnInstall";
            this._buttonUnInstall.UseVisualStyleBackColor = true;
            this._buttonUnInstall.Click += new System.EventHandler(this.OnUnInstall);
            // 
            // SvcRegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 158);
            this.Controls.Add(this._buttonUnInstall);
            this.Controls.Add(this._groupBox);
            this.Controls.Add(this._buttonInstall);
            this.Name = "SvcRegisterForm";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this._groupBox.ResumeLayout(false);
            this._groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UserPasswdCtrl _svcLogonCtrl;
        private System.Windows.Forms.GroupBox _groupBox;
        private System.Windows.Forms.RadioButton _radioButtonWinUser;
        private System.Windows.Forms.RadioButton _radioButtonLocalSys;
        private System.Windows.Forms.Button _buttonInstall;
        private System.Windows.Forms.Button _buttonUnInstall;
    }
}