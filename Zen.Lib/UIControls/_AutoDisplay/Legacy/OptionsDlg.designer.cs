namespace Zen.UIControls
{
    partial class OptionsDlg
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
            this._panelButtons = new System.Windows.Forms.Panel();
            this._optionsCtrl = new Zen.UIControls.OptionsCtrl();
            this._panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // _buttonOK
            // 
            this._buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonOK.Location = new System.Drawing.Point(204, 4);
            this._buttonOK.Name = "_buttonOK";
            this._buttonOK.Size = new System.Drawing.Size(75, 23);
            this._buttonOK.TabIndex = 1;
            this._buttonOK.Text = "&OK";
            this._buttonOK.UseVisualStyleBackColor = true;
            this._buttonOK.Click += new System.EventHandler(this.OnOK);
            // 
            // _buttonCancel
            // 
            this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._buttonCancel.Location = new System.Drawing.Point(286, 4);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Size = new System.Drawing.Size(75, 23);
            this._buttonCancel.TabIndex = 2;
            this._buttonCancel.Text = "&Cancel";
            this._buttonCancel.UseVisualStyleBackColor = true;
            this._buttonCancel.Click += new System.EventHandler(this.OnCancel);
            // 
            // _panelButtons
            // 
            this._panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._panelButtons.Controls.Add(this._buttonOK);
            this._panelButtons.Controls.Add(this._buttonCancel);
            this._panelButtons.Location = new System.Drawing.Point(0, 185);
            this._panelButtons.Name = "_panelButtons";
            this._panelButtons.Size = new System.Drawing.Size(372, 32);
            this._panelButtons.TabIndex = 3;
            // 
            // _optionsCtrl
            // 
            this._optionsCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._optionsCtrl.Location = new System.Drawing.Point(0, -1);
            this._optionsCtrl.Name = "_optionsCtrl";
            this._optionsCtrl.Size = new System.Drawing.Size(372, 184);
            this._optionsCtrl.TabIndex = 0;
            // 
            // OptionsDlg
            // 
            this.AcceptButton = this._buttonOK;
            this.CancelButton = this._buttonCancel;
            this.ClientSize = new System.Drawing.Size(372, 218);
            this.Controls.Add(this._optionsCtrl);
            this.Controls.Add(this._panelButtons);
            this.Name = "OptionsDlg";
            this.Text = "Options:";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this._panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Windows Form Designer generated variables
        private System.Windows.Forms.Button _buttonOK;
        private System.Windows.Forms.Button _buttonCancel;
        private System.Windows.Forms.Panel _panelButtons;
        #endregion
        private OptionsCtrl _optionsCtrl;
    }
}

