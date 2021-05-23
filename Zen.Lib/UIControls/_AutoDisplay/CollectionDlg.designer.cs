namespace Zen.UIControls
{
    partial class CollectionDlg
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
            this._collectionCtrl = new Zen.UIControls.Layout.CollectionCtrl();
            this.SuspendLayout();
            // 
            // _collectionCtrl
            // 
            this._collectionCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._collectionCtrl.Location = new System.Drawing.Point(8, 12);
            this._collectionCtrl.Name = "_collectionCtrl";
            this._collectionCtrl.Size = new System.Drawing.Size(340, 70);
            this._collectionCtrl.TabIndex = 0;
            // 
            // CollectionDlg
            // 
            this.ClientSize = new System.Drawing.Size(360, 94);
            this.Controls.Add(this._collectionCtrl);
            this.Name = "CollectionDlg";
            this.Text = "Options:";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);

        }
        #endregion

        #region Windows Form Designer generated variables
        private Zen.UIControls.Layout.CollectionCtrl _collectionCtrl;
        #endregion
    }
}

