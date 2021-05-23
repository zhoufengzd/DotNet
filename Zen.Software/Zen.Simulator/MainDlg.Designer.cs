namespace Zen.Simulator
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
            this._buttonOk = new System.Windows.Forms.Button();
            this._textBoxOutput = new System.Windows.Forms.TextBox();
            this._propertyCtrlOptions = new Zen.UIControls.PropertyCtrl();
            this.SuspendLayout();
            // 
            // _buttonOk
            // 
            this._buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonOk.Location = new System.Drawing.Point(319, 12);
            this._buttonOk.Name = "_buttonOk";
            this._buttonOk.Size = new System.Drawing.Size(75, 23);
            this._buttonOk.TabIndex = 1;
            this._buttonOk.Text = "&Go";
            this._buttonOk.UseVisualStyleBackColor = true;
            this._buttonOk.Click += new System.EventHandler(this.OnGo);
            // 
            // _textBoxOutput
            // 
            this._textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxOutput.Location = new System.Drawing.Point(12, 221);
            this._textBoxOutput.Name = "_textBoxOutput";
            this._textBoxOutput.Size = new System.Drawing.Size(281, 20);
            this._textBoxOutput.TabIndex = 2;
            // 
            // _propertyCtrlOptions
            // 
            this._propertyCtrlOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._propertyCtrlOptions.Location = new System.Drawing.Point(12, 12);
            this._propertyCtrlOptions.Name = "_propertyCtrlOptions";
            this._propertyCtrlOptions.Size = new System.Drawing.Size(281, 193);
            this._propertyCtrlOptions.TabIndex = 0;
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 258);
            this.Controls.Add(this._textBoxOutput);
            this.Controls.Add(this._buttonOk);
            this.Controls.Add(this._propertyCtrlOptions);
            this.Name = "MainDlg";
            this.Text = "Simulator";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Zen.UIControls.PropertyCtrl _propertyCtrlOptions;
        private System.Windows.Forms.Button _buttonOk;
        private System.Windows.Forms.TextBox _textBoxOutput;
    }
}

