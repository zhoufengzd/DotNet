namespace Zen.UIControls
{
    partial class ControllerCtrl
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
            this._buttonStart = new System.Windows.Forms.Button();
            this._buttonStop = new System.Windows.Forms.Button();
            this._buttonPause = new System.Windows.Forms.Button();
            this._btnPanel = new System.Windows.Forms.FlowLayoutPanel();
            this._btnPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _buttonStart
            // 
            this._buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonStart.Location = new System.Drawing.Point(3, 3);
            this._buttonStart.Name = "_buttonStart";
            this._buttonStart.Size = new System.Drawing.Size(53, 23);
            this._buttonStart.TabIndex = 1;
            this._buttonStart.Text = "&Start";
            this._buttonStart.Click += new System.EventHandler(this.OnStart);
            // 
            // _buttonStop
            // 
            this._buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonStop.Location = new System.Drawing.Point(62, 3);
            this._buttonStop.Name = "_buttonStop";
            this._buttonStop.Size = new System.Drawing.Size(53, 23);
            this._buttonStop.TabIndex = 2;
            this._buttonStop.Text = "S&top";
            this._buttonStop.Click += new System.EventHandler(this.OnStop);
            // 
            // _buttonPause
            // 
            this._buttonPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonPause.Location = new System.Drawing.Point(121, 3);
            this._buttonPause.Name = "_buttonPause";
            this._buttonPause.Size = new System.Drawing.Size(53, 23);
            this._buttonPause.TabIndex = 3;
            this._buttonPause.Text = "&Pause";
            this._buttonPause.Click += new System.EventHandler(this.OnPause);
            // 
            // _btnPanel
            // 
            this._btnPanel.Controls.Add(this._buttonStart);
            this._btnPanel.Controls.Add(this._buttonStop);
            this._btnPanel.Controls.Add(this._buttonPause);
            this._btnPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._btnPanel.Location = new System.Drawing.Point(0, 0);
            this._btnPanel.Name = "_btnPanel";
            this._btnPanel.Size = new System.Drawing.Size(177, 29);
            this._btnPanel.TabIndex = 4;
            // 
            // ControllerCtrl
            // 
            this.Controls.Add(this._btnPanel);
            this.Name = "ControllerCtrl";
            this.Size = new System.Drawing.Size(177, 29);
            this.Load += new System.EventHandler(this.OnLoad);
            this._btnPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button _buttonStart;
        private System.Windows.Forms.Button _buttonStop;
        private System.Windows.Forms.Button _buttonPause;
        private System.Windows.Forms.FlowLayoutPanel _btnPanel;

        #region Windows Form Designer generated variables

        #endregion
    }
}

