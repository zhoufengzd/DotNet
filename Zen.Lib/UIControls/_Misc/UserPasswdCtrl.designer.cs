namespace Zen.UIControls
{
    partial class UserPasswdCtrl
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
            this._labelUserName = new System.Windows.Forms.Label();
            this._textBoxUsername = new System.Windows.Forms.TextBox();
            this._labelPassword = new System.Windows.Forms.Label();
            this._textBoxPassword = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _labelUserName
            // 
            this._labelUserName.AutoSize = true;
            this._labelUserName.Location = new System.Drawing.Point(3, 9);
            this._labelUserName.Name = "_labelUserName";
            this._labelUserName.Size = new System.Drawing.Size(58, 13);
            this._labelUserName.TabIndex = 1;
            this._labelUserName.Text = "User name";
            // 
            // _textBoxUsername
            // 
            this._textBoxUsername.Location = new System.Drawing.Point(67, 6);
            this._textBoxUsername.Name = "_textBoxUsername";
            this._textBoxUsername.Size = new System.Drawing.Size(200, 20);
            this._textBoxUsername.TabIndex = 0;
            // 
            // _labelPassword
            // 
            this._labelPassword.AutoSize = true;
            this._labelPassword.Location = new System.Drawing.Point(3, 37);
            this._labelPassword.Name = "_labelPassword";
            this._labelPassword.Size = new System.Drawing.Size(53, 13);
            this._labelPassword.TabIndex = 0;
            this._labelPassword.Text = "Password";
            // 
            // _textBoxPassword
            // 
            this._textBoxPassword.Location = new System.Drawing.Point(67, 34);
            this._textBoxPassword.Name = "_textBoxPassword";
            this._textBoxPassword.PasswordChar = '*';
            this._textBoxPassword.Size = new System.Drawing.Size(200, 20);
            this._textBoxPassword.TabIndex = 1;
            this._textBoxPassword.UseSystemPasswordChar = true;
            // 
            // UserPasswdCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._labelPassword);
            this.Controls.Add(this._labelUserName);
            this.Controls.Add(this._textBoxPassword);
            this.Controls.Add(this._textBoxUsername);
            this.Name = "UserPasswdCtrl";
            this.Size = new System.Drawing.Size(271, 58);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _labelUserName;
        private System.Windows.Forms.Label _labelPassword;
        private System.Windows.Forms.TextBox _textBoxUsername;
        private System.Windows.Forms.TextBox _textBoxPassword;
    }
}
