namespace Zen.UIControls
{
    partial class ControllerDlg
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
            this._controllerCtrl = new Zen.UIControls.ControllerCtrl();
            this._horizontalLine = new Zen.UIControls.HorizontalLineCtrl();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // _controllerCtrl
            // 
            this._controllerCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._controllerCtrl.Location = new System.Drawing.Point(117, 57);
            this._controllerCtrl.Margin = new System.Windows.Forms.Padding(0);
            this._controllerCtrl.Name = "_controllerCtrl";
            this._controllerCtrl.Size = new System.Drawing.Size(188, 30);
            this._controllerCtrl.TabIndex = 0;
            // 
            // _horizontalLine
            // 
            this._horizontalLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._horizontalLine.BackColor = System.Drawing.Color.Transparent;
            this._horizontalLine.Color = System.Drawing.Color.DarkGray;
            this._horizontalLine.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this._horizontalLine.DoubleLine = false;
            this._horizontalLine.LineWidth = 1;
            this._horizontalLine.Location = new System.Drawing.Point(0, 50);
            this._horizontalLine.Margin = new System.Windows.Forms.Padding(0);
            this._horizontalLine.Name = "_horizontalLine";
            this._horizontalLine.Size = new System.Drawing.Size(317, 10);
            this._horizontalLine.TabIndex = 1;
            // 
            // _progressBar
            // 
            this._progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._progressBar.Location = new System.Drawing.Point(12, 13);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(292, 23);
            this._progressBar.TabIndex = 2;
            // 
            // ControllerDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 94);
            this.ControlBox = false;
            this.Controls.Add(this._progressBar);
            this.Controls.Add(this._horizontalLine);
            this.Controls.Add(this._controllerCtrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ControllerDlg";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "In Progress...";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private ControllerCtrl _controllerCtrl;
        private HorizontalLineCtrl _horizontalLine;
        private System.Windows.Forms.ProgressBar _progressBar;

    }
}