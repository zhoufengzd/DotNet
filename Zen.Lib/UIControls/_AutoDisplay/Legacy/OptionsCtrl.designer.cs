namespace Zen.UIControls
{
    partial class OptionsCtrl
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
            this._panelOptions = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // _panelOptions
            // 
            this._panelOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._panelOptions.Location = new System.Drawing.Point(0, 0);
            this._panelOptions.Margin = new System.Windows.Forms.Padding(0);
            this._panelOptions.Name = "_panelOptions";
            this._panelOptions.Size = new System.Drawing.Size(128, 84);
            this._panelOptions.TabIndex = 0;
            this._panelOptions.Visible = false;
            // 
            // OptionsCtrl
            // 
            this.Controls.Add(this._panelOptions);
            this.Name = "OptionsCtrl";
            this.Size = new System.Drawing.Size(128, 84);
            this.ResumeLayout(false);

        }
        #endregion

        #region Windows Form Designer generated variables
        private System.Windows.Forms.Panel _panelOptions;
        #endregion
    }
}

