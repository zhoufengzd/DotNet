namespace Zen.DBAToolbox
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._menuItemConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseServersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dB2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mySqlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oracleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sqlServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.embeddedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sqliteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sqlServerCEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProvidersServerStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schemaManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverToolStripMenuItem,
            this.viewMenu,
            this.toolsMenu,
            this.windowsMenu,
            this.helpMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.MdiWindowListItem = this.windowsMenu;
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(463, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // serverToolStripMenuItem
            // 
            this.serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuItemConnect,
            this.ProvidersServerStripMenuItem});
            this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            this.serverToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.serverToolStripMenuItem.Text = "&Server";
            // 
            // _menuItemConnect
            // 
            this._menuItemConnect.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.databaseServersToolStripMenuItem,
            this.embeddedToolStripMenuItem});
            this._menuItemConnect.Name = "_menuItemConnect";
            this._menuItemConnect.Size = new System.Drawing.Size(148, 22);
            this._menuItemConnect.Text = "&Connect...";
            // 
            // databaseServersToolStripMenuItem
            // 
            this.databaseServersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dB2ToolStripMenuItem,
            this.mySqlToolStripMenuItem,
            this.oracleToolStripMenuItem,
            this.sqlServerToolStripMenuItem});
            this.databaseServersToolStripMenuItem.Name = "databaseServersToolStripMenuItem";
            this.databaseServersToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.databaseServersToolStripMenuItem.Text = "Database Servers";
            // 
            // dB2ToolStripMenuItem
            // 
            this.dB2ToolStripMenuItem.Name = "dB2ToolStripMenuItem";
            this.dB2ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.dB2ToolStripMenuItem.Text = "&DB2";
            this.dB2ToolStripMenuItem.Click += new System.EventHandler(this.OnConnectToDB2);
            // 
            // mySqlToolStripMenuItem
            // 
            this.mySqlToolStripMenuItem.Name = "mySqlToolStripMenuItem";
            this.mySqlToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.mySqlToolStripMenuItem.Text = "&MySql";
            this.mySqlToolStripMenuItem.Click += new System.EventHandler(this.OnConnectToMySql);
            // 
            // oracleToolStripMenuItem
            // 
            this.oracleToolStripMenuItem.Name = "oracleToolStripMenuItem";
            this.oracleToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.oracleToolStripMenuItem.Text = "&Oracle";
            this.oracleToolStripMenuItem.Click += new System.EventHandler(this.OnConnectToOracle);
            // 
            // sqlServerToolStripMenuItem
            // 
            this.sqlServerToolStripMenuItem.Name = "sqlServerToolStripMenuItem";
            this.sqlServerToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.sqlServerToolStripMenuItem.Text = "&Sql Server";
            this.sqlServerToolStripMenuItem.Click += new System.EventHandler(this.OnConnectToSqlSvr);
            // 
            // embeddedToolStripMenuItem
            // 
            this.embeddedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accessToolStripMenuItem,
            this.sqliteToolStripMenuItem,
            this.sqlServerCEToolStripMenuItem});
            this.embeddedToolStripMenuItem.Name = "embeddedToolStripMenuItem";
            this.embeddedToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.embeddedToolStripMenuItem.Text = "&Embedded";
            // 
            // accessToolStripMenuItem
            // 
            this.accessToolStripMenuItem.Name = "accessToolStripMenuItem";
            this.accessToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.accessToolStripMenuItem.Text = "A&ccess";
            this.accessToolStripMenuItem.Click += new System.EventHandler(this.OnConnectToAccess);
            // 
            // sqliteToolStripMenuItem
            // 
            this.sqliteToolStripMenuItem.Name = "sqliteToolStripMenuItem";
            this.sqliteToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.sqliteToolStripMenuItem.Text = "Sqli&te";
            this.sqliteToolStripMenuItem.Click += new System.EventHandler(this.OnConnectToSqlite);
            // 
            // sqlServerCEToolStripMenuItem
            // 
            this.sqlServerCEToolStripMenuItem.Name = "sqlServerCEToolStripMenuItem";
            this.sqlServerCEToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.sqlServerCEToolStripMenuItem.Text = "Sql Server C&E";
            this.sqlServerCEToolStripMenuItem.Click += new System.EventHandler(this.OnConnectToSqlSvrCE);
            // 
            // ProvidersServerStripMenuItem
            // 
            this.ProvidersServerStripMenuItem.Name = "ProvidersServerStripMenuItem";
            this.ProvidersServerStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.ProvidersServerStripMenuItem.Text = "Provider In&fo";
            this.ProvidersServerStripMenuItem.Click += new System.EventHandler(this.OnDbProviderList);
            // 
            // viewMenu
            // 
            this.viewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBarToolStripMenuItem,
            this.statusBarToolStripMenuItem});
            this.viewMenu.Name = "viewMenu";
            this.viewMenu.Size = new System.Drawing.Size(41, 20);
            this.viewMenu.Text = "&View";
            // 
            // toolBarToolStripMenuItem
            // 
            this.toolBarToolStripMenuItem.Checked = true;
            this.toolBarToolStripMenuItem.CheckOnClick = true;
            this.toolBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolBarToolStripMenuItem.Name = "toolBarToolStripMenuItem";
            this.toolBarToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.toolBarToolStripMenuItem.Text = "&Toolbar";
            // 
            // statusBarToolStripMenuItem
            // 
            this.statusBarToolStripMenuItem.Checked = true;
            this.statusBarToolStripMenuItem.CheckOnClick = true;
            this.statusBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statusBarToolStripMenuItem.Name = "statusBarToolStripMenuItem";
            this.statusBarToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.statusBarToolStripMenuItem.Text = "&Status Bar";
            this.statusBarToolStripMenuItem.Click += new System.EventHandler(this.StatusBarToolStripMenuItem_Click);
            // 
            // toolsMenu
            // 
            this.toolsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.schemaManagerToolStripMenuItem});
            this.toolsMenu.Name = "toolsMenu";
            this.toolsMenu.Size = new System.Drawing.Size(44, 20);
            this.toolsMenu.Text = "&Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // schemaManagerToolStripMenuItem
            // 
            this.schemaManagerToolStripMenuItem.Name = "schemaManagerToolStripMenuItem";
            this.schemaManagerToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.schemaManagerToolStripMenuItem.Text = "&Schema Manager";
            this.schemaManagerToolStripMenuItem.Click += new System.EventHandler(this.OnSchemaMgr);
            // 
            // windowsMenu
            // 
            this.windowsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeAllToolStripMenuItem});
            this.windowsMenu.Name = "windowsMenu";
            this.windowsMenu.Size = new System.Drawing.Size(62, 20);
            this.windowsMenu.Text = "&Windows";
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.closeAllToolStripMenuItem.Text = "C&lose All";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.CloseAllToolStripMenuItem_Click);
            // 
            // helpMenu
            // 
            this.helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator8,
            this.aboutToolStripMenuItem});
            this.helpMenu.Name = "helpMenu";
            this.helpMenu.Size = new System.Drawing.Size(40, 20);
            this.helpMenu.Text = "&Help";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.contentsToolStripMenuItem.Text = "&Contents";
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("indexToolStripMenuItem.Image")));
            this.indexToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.indexToolStripMenuItem.Text = "&Index";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("searchToolStripMenuItem.Image")));
            this.searchToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.searchToolStripMenuItem.Text = "&Search";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(170, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.aboutToolStripMenuItem.Text = "&About ... ...";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 240);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(463, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(38, 17);
            this.toolStripStatusLabel.Text = "Status";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 262);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "DBA Toolbox";
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewMenu;
        private System.Windows.Forms.ToolStripMenuItem toolBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsMenu;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsMenu;
        private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenu;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _menuItemConnect;
        private System.Windows.Forms.ToolStripMenuItem schemaManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ProvidersServerStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dB2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oracleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sqlServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem embeddedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sqliteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sqlServerCEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mySqlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseServersToolStripMenuItem;
    }
}



