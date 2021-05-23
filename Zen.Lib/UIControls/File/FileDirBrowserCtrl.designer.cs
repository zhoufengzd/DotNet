namespace Zen.UIControls.FileUI
{
    partial class FileDirBrowserCtrl
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
            this._textBoxPath = new System.Windows.Forms.TextBox();
            this._buttonBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _textBoxPath
            // 
            this._textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxPath.Location = new System.Drawing.Point(0, 0);
            this._textBoxPath.Margin = new System.Windows.Forms.Padding(0);
            this._textBoxPath.Name = "_textBoxPath";
            this._textBoxPath.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this._textBoxPath.Size = new System.Drawing.Size(48, 20);
            this._textBoxPath.TabIndex = 0;
            // 
            // _buttonBrowse
            // 
            this._buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonBrowse.Location = new System.Drawing.Point(55, 0);
            this._buttonBrowse.Name = "_buttonBrowse";
            this._buttonBrowse.Size = new System.Drawing.Size(25, 20);
            this._buttonBrowse.TabIndex = 1;
            this._buttonBrowse.Text = "...";
            this._buttonBrowse.UseVisualStyleBackColor = true;
            this._buttonBrowse.Click += new System.EventHandler(this.OnBrowse);
            // 
            // FileDirBrowserCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._textBoxPath);
            this.Controls.Add(this._buttonBrowse);
            this.Name = "FileDirBrowserCtrl";
            this.Size = new System.Drawing.Size(80, 20);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox _textBoxPath;
        private System.Windows.Forms.Button _buttonBrowse;
    }
}