namespace RenameFiles
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
            this._propertyCtrlOptions = new Zen.UIControls.PropertyCtrl();
            this._buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _buttonRun
            // 
            this._buttonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonRun.Location = new System.Drawing.Point(128, 112);
            this._buttonRun.Name = "_buttonRun";
            this._buttonRun.Size = new System.Drawing.Size(75, 23);
            this._buttonRun.TabIndex = 1;
            this._buttonRun.Text = "&Run";
            this._buttonRun.UseVisualStyleBackColor = true;
            this._buttonRun.Click += new System.EventHandler(this.OnRun);
            // 
            // _propertyCtrlOptions
            // 
            this._propertyCtrlOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._propertyCtrlOptions.Location = new System.Drawing.Point(12, 12);
            this._propertyCtrlOptions.Name = "_propertyCtrlOptions";
            this._propertyCtrlOptions.Size = new System.Drawing.Size(285, 94);
            this._propertyCtrlOptions.TabIndex = 2;
            // 
            // _buttonCancel
            // 
            this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonCancel.Location = new System.Drawing.Point(222, 112);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Size = new System.Drawing.Size(75, 23);
            this._buttonCancel.TabIndex = 3;
            this._buttonCancel.Text = "&Cancel";
            this._buttonCancel.UseVisualStyleBackColor = true;
            this._buttonCancel.Click += new System.EventHandler(this.OnShowNames);
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 146);
            this.Controls.Add(this._buttonCancel);
            this.Controls.Add(this._propertyCtrlOptions);
            this.Controls.Add(this._buttonRun);
            this.Name = "MainDlg";
            this.Text = "Rename files...";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _buttonRun;
        private Zen.UIControls.PropertyCtrl _propertyCtrlOptions;
        private System.Windows.Forms.Button _buttonCancel;
    }
}

