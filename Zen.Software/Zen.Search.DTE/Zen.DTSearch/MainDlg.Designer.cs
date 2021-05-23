namespace Zen.DTSearch
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
            this._propertyCtrl = new Zen.UIControls.PropertyCtrl();
            this.horizontalLineCtrl1 = new Zen.UIControls.HorizontalLineCtrl();
            this._buttonOk = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this._propertyCtrlIndexInfo = new Zen.UIControls.PropertyCtrl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _propertyCtrl
            // 
            this._propertyCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._propertyCtrl.Location = new System.Drawing.Point(3, 3);
            this._propertyCtrl.Name = "_propertyCtrl";
            this._propertyCtrl.Size = new System.Drawing.Size(276, 172);
            this._propertyCtrl.TabIndex = 0;
            // 
            // horizontalLineCtrl1
            // 
            this.horizontalLineCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLineCtrl1.BackColor = System.Drawing.Color.Transparent;
            this.horizontalLineCtrl1.Color = System.Drawing.Color.DarkGray;
            this.horizontalLineCtrl1.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.horizontalLineCtrl1.DoubleLine = false;
            this.horizontalLineCtrl1.LineWidth = 1;
            this.horizontalLineCtrl1.Location = new System.Drawing.Point(3, 206);
            this.horizontalLineCtrl1.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalLineCtrl1.Name = "horizontalLineCtrl1";
            this.horizontalLineCtrl1.Size = new System.Drawing.Size(342, 10);
            this.horizontalLineCtrl1.TabIndex = 1;
            // 
            // _buttonOk
            // 
            this._buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonOk.Location = new System.Drawing.Point(256, 219);
            this._buttonOk.Name = "_buttonOk";
            this._buttonOk.Size = new System.Drawing.Size(83, 23);
            this._buttonOk.TabIndex = 2;
            this._buttonOk.Text = "&OK";
            this._buttonOk.UseVisualStyleBackColor = true;
            this._buttonOk.Click += new System.EventHandler(this.OnOK);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(327, 191);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this._propertyCtrl);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(282, 178);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this._propertyCtrlIndexInfo);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(319, 165);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // _propertyCtrlIndexInfo
            // 
            this._propertyCtrlIndexInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this._propertyCtrlIndexInfo.Location = new System.Drawing.Point(3, 3);
            this._propertyCtrlIndexInfo.Name = "_propertyCtrlIndexInfo";
            this._propertyCtrlIndexInfo.Size = new System.Drawing.Size(313, 159);
            this._propertyCtrlIndexInfo.TabIndex = 0;
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 251);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this._buttonOk);
            this.Controls.Add(this.horizontalLineCtrl1);
            this.Name = "MainDlg";
            this.Text = "MainDlg";
            this.Load += new System.EventHandler(this.OnLoad);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Zen.UIControls.PropertyCtrl _propertyCtrl;
        private Zen.UIControls.HorizontalLineCtrl horizontalLineCtrl1;
        private System.Windows.Forms.Button _buttonOk;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Zen.UIControls.PropertyCtrl _propertyCtrlIndexInfo;
    }
}