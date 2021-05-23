namespace ScreenCapturer
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
            this._buttonSelectArea = new System.Windows.Forms.Button();
            this._buttonGO = new System.Windows.Forms.Button();
            this._buttonCancel = new System.Windows.Forms.Button();
            this._panelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this._horizontalLineCtrl = new Zen.UIControls.HorizontalLineCtrl();
            this._optionsCtrl = new Zen.UIControls.PropertyCtrl();
            this._panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // _buttonSelectArea
            // 
            this._buttonSelectArea.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonSelectArea.BackColor = System.Drawing.Color.Gainsboro;
            this._buttonSelectArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._buttonSelectArea.ForeColor = System.Drawing.SystemColors.ControlText;
            this._buttonSelectArea.Location = new System.Drawing.Point(238, 3);
            this._buttonSelectArea.Name = "_buttonSelectArea";
            this._buttonSelectArea.Size = new System.Drawing.Size(96, 32);
            this._buttonSelectArea.TabIndex = 1;
            this._buttonSelectArea.TabStop = false;
            this._buttonSelectArea.Text = "Select &Area...";
            this._buttonSelectArea.UseVisualStyleBackColor = false;
            this._buttonSelectArea.Click += new System.EventHandler(this.OnSelectArea);
            // 
            // _buttonGO
            // 
            this._buttonGO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonGO.BackColor = System.Drawing.Color.Gainsboro;
            this._buttonGO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._buttonGO.ForeColor = System.Drawing.SystemColors.ControlText;
            this._buttonGO.Location = new System.Drawing.Point(136, 3);
            this._buttonGO.Name = "_buttonGO";
            this._buttonGO.Size = new System.Drawing.Size(96, 32);
            this._buttonGO.TabIndex = 2;
            this._buttonGO.TabStop = false;
            this._buttonGO.Text = "&GO";
            this._buttonGO.UseVisualStyleBackColor = false;
            this._buttonGO.Click += new System.EventHandler(this.OnGO);
            // 
            // _buttonCancel
            // 
            this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonCancel.BackColor = System.Drawing.Color.Gainsboro;
            this._buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._buttonCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this._buttonCancel.Location = new System.Drawing.Point(34, 3);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Size = new System.Drawing.Size(96, 32);
            this._buttonCancel.TabIndex = 3;
            this._buttonCancel.TabStop = false;
            this._buttonCancel.Text = "&Cancel";
            this._buttonCancel.UseVisualStyleBackColor = false;
            this._buttonCancel.Click += new System.EventHandler(this.OnCancel);
            // 
            // _panelButtons
            // 
            this._panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._panelButtons.AutoSize = true;
            this._panelButtons.Controls.Add(this._buttonSelectArea);
            this._panelButtons.Controls.Add(this._buttonGO);
            this._panelButtons.Controls.Add(this._buttonCancel);
            this._panelButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this._panelButtons.Location = new System.Drawing.Point(15, 145);
            this._panelButtons.Name = "_panelButtons";
            this._panelButtons.Size = new System.Drawing.Size(337, 41);
            this._panelButtons.TabIndex = 7;
            // 
            // _horizontalLineCtrl
            // 
            this._horizontalLineCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._horizontalLineCtrl.BackColor = System.Drawing.Color.Transparent;
            this._horizontalLineCtrl.Color = System.Drawing.Color.DarkGray;
            this._horizontalLineCtrl.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this._horizontalLineCtrl.DoubleLine = false;
            this._horizontalLineCtrl.LineWidth = 1;
            this._horizontalLineCtrl.Location = new System.Drawing.Point(6, 134);
            this._horizontalLineCtrl.Margin = new System.Windows.Forms.Padding(0);
            this._horizontalLineCtrl.Name = "_horizontalLineCtrl";
            this._horizontalLineCtrl.Size = new System.Drawing.Size(356, 10);
            this._horizontalLineCtrl.TabIndex = 6;
            this._horizontalLineCtrl.TabStop = false;
            // 
            // _optionsCtrl
            // 
            this._optionsCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._optionsCtrl.AutoSize = true;
            this._optionsCtrl.Location = new System.Drawing.Point(14, 13);
            this._optionsCtrl.Name = "_optionsCtrl";
            this._optionsCtrl.Size = new System.Drawing.Size(340, 118);
            this._optionsCtrl.TabIndex = 0;
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(367, 190);
            this.Controls.Add(this._panelButtons);
            this.Controls.Add(this._horizontalLineCtrl);
            this.Controls.Add(this._optionsCtrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainDlg";
            this.Text = "Screen Capture";
            this.Load += new System.EventHandler(this.OnLoad);
            this._panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _buttonSelectArea;
        private System.Windows.Forms.Button _buttonGO;
        private System.Windows.Forms.Button _buttonCancel;
        private Zen.UIControls.PropertyCtrl _optionsCtrl;
        private Zen.UIControls.HorizontalLineCtrl _horizontalLineCtrl;
        private System.Windows.Forms.FlowLayoutPanel _panelButtons;
    }
}