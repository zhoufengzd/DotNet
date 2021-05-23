namespace OcrExecutor
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
            this.btnStart = new System.Windows.Forms.Button();
            this._textBoxResult = new System.Windows.Forms.TextBox();
            this._groupBoxResults = new System.Windows.Forms.GroupBox();
            this._groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this._optionsCtrl = new Zen.UIControls.PropertyCtrl();
            this._groupBoxResults.SuspendLayout();
            this._groupBoxOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(355, 28);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "&Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.OnStart);
            // 
            // _textBoxResult
            // 
            this._textBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._textBoxResult.Location = new System.Drawing.Point(3, 19);
            this._textBoxResult.Multiline = true;
            this._textBoxResult.Name = "_textBoxResult";
            this._textBoxResult.Size = new System.Drawing.Size(320, 43);
            this._textBoxResult.TabIndex = 1;
            // 
            // _groupBoxResults
            // 
            this._groupBoxResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._groupBoxResults.Controls.Add(this._textBoxResult);
            this._groupBoxResults.Location = new System.Drawing.Point(12, 201);
            this._groupBoxResults.Name = "_groupBoxResults";
            this._groupBoxResults.Size = new System.Drawing.Size(328, 68);
            this._groupBoxResults.TabIndex = 7;
            this._groupBoxResults.TabStop = false;
            this._groupBoxResults.Text = "Results";
            // 
            // _groupBoxOptions
            // 
            this._groupBoxOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._groupBoxOptions.Controls.Add(this._optionsCtrl);
            this._groupBoxOptions.Location = new System.Drawing.Point(12, 12);
            this._groupBoxOptions.Name = "_groupBoxOptions";
            this._groupBoxOptions.Size = new System.Drawing.Size(328, 183);
            this._groupBoxOptions.TabIndex = 8;
            this._groupBoxOptions.TabStop = false;
            this._groupBoxOptions.Text = "Options";
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Location = new System.Drawing.Point(355, 57);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "S&top";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.OnStop);
            // 
            // btnPause
            // 
            this.btnPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPause.Location = new System.Drawing.Point(355, 86);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 23);
            this.btnPause.TabIndex = 10;
            this.btnPause.Text = "&Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.OnPause);
            // 
            // _optionsCtrl
            // 
            this._optionsCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._optionsCtrl.Location = new System.Drawing.Point(3, 16);
            this._optionsCtrl.Name = "_optionsCtrl";
            this._optionsCtrl.Size = new System.Drawing.Size(322, 164);
            this._optionsCtrl.TabIndex = 0;
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 280);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this._groupBoxOptions);
            this.Controls.Add(this._groupBoxResults);
            this.Controls.Add(this.btnStart);
            this.Name = "MainDlg";
            this.Text = "Ocr Processor";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this._groupBoxResults.ResumeLayout(false);
            this._groupBoxResults.PerformLayout();
            this._groupBoxOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox _textBoxResult;
        private System.Windows.Forms.GroupBox _groupBoxResults;
        private System.Windows.Forms.GroupBox _groupBoxOptions;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPause;
        private Zen.UIControls.PropertyCtrl _optionsCtrl;
    }
}

