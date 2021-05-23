namespace Zen.DBAToolbox
{
    partial class SchemaMgrDlg
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.sqlTextBox = new System.Windows.Forms.RichTextBox();
            this.btnGenerateSQL = new System.Windows.Forms.Button();
            this.schemaGroupBox = new System.Windows.Forms.GroupBox();
            this.case_ckBox = new System.Windows.Forms.CheckBox();
            this.ul_ckBox = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.schemaGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(380, 379);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 24);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.OnCancel);
            // 
            // sqlTextBox
            // 
            this.sqlTextBox.AcceptsTab = true;
            this.sqlTextBox.Location = new System.Drawing.Point(8, 70);
            this.sqlTextBox.Name = "sqlTextBox";
            this.sqlTextBox.ReadOnly = true;
            this.sqlTextBox.Size = new System.Drawing.Size(464, 303);
            this.sqlTextBox.TabIndex = 29;
            this.sqlTextBox.Text = "";
            // 
            // btnGenerateSQL
            // 
            this.btnGenerateSQL.Location = new System.Drawing.Point(286, 379);
            this.btnGenerateSQL.Name = "btnGenerateSQL";
            this.btnGenerateSQL.Size = new System.Drawing.Size(88, 24);
            this.btnGenerateSQL.TabIndex = 30;
            this.btnGenerateSQL.Text = "Generate SQL";
            this.btnGenerateSQL.Click += new System.EventHandler(this.OnGenerateSQL);
            // 
            // schemaGroupBox
            // 
            this.schemaGroupBox.Controls.Add(this.case_ckBox);
            this.schemaGroupBox.Controls.Add(this.ul_ckBox);
            this.schemaGroupBox.Controls.Add(this.label5);
            this.schemaGroupBox.Location = new System.Drawing.Point(8, 12);
            this.schemaGroupBox.Name = "schemaGroupBox";
            this.schemaGroupBox.Size = new System.Drawing.Size(464, 52);
            this.schemaGroupBox.TabIndex = 33;
            this.schemaGroupBox.TabStop = false;
            this.schemaGroupBox.Text = "Schema Options";
            // 
            // case_ckBox
            // 
            this.case_ckBox.Location = new System.Drawing.Point(280, 16);
            this.case_ckBox.Name = "case_ckBox";
            this.case_ckBox.Size = new System.Drawing.Size(104, 24);
            this.case_ckBox.TabIndex = 34;
            this.case_ckBox.Text = "Case Schema";
            // 
            // ul_ckBox
            // 
            this.ul_ckBox.Location = new System.Drawing.Point(152, 16);
            this.ul_ckBox.Name = "ul_ckBox";
            this.ul_ckBox.Size = new System.Drawing.Size(104, 24);
            this.ul_ckBox.TabIndex = 33;
            this.ul_ckBox.Text = "ULM Schema";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(24, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 24);
            this.label5.TabIndex = 32;
            this.label5.Text = "Schema Type:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SchemaMgrDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(480, 413);
            this.Controls.Add(this.schemaGroupBox);
            this.Controls.Add(this.btnGenerateSQL);
            this.Controls.Add(this.sqlTextBox);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SchemaMgrDlg";
            this.Text = "Schema Manager";
            this.Load += new System.EventHandler(this.OnLoad);
            this.schemaGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Windows Form Designer generated variables

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox ul_ckBox;
        private System.Windows.Forms.CheckBox case_ckBox;
        private System.Windows.Forms.RichTextBox sqlTextBox;
        private System.Windows.Forms.Button btnGenerateSQL;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox schemaGroupBox;
        #endregion
    }
}