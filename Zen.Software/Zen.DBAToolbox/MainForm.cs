using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Windows.Forms;

using Zen.DBMS;
using Zen.DBMS.Schema;
using Zen.UIControls;
using Zen.UIControls.Misc;
using Zen.Utilities.Generics;
using Zen.Utilities.Proc;

namespace Zen.DBAToolbox
{

    /// <summary>
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm(AppInstance instance)
        {
            _instance = instance;
            _configMgr = instance.ConfigManager;
            InitializeComponent();
        }

        #region private functions
        #region Event handler
        private void OnLoad(object sender, EventArgs e)
        {
            _dbExplorer.MdiParent = this;
            _dbExplorer.Dock = DockStyle.Fill;
            _dbExplorer.Show();
        }

        #region embedded database
        private void OnConnectToAccess(object sender, EventArgs e)
        {
            ConnectEmbedded(DBMSPlatformEnum.Access);
        }
        private void OnConnectToSqlite(object sender, EventArgs e)
        {
            ConnectEmbedded(DBMSPlatformEnum.Sqlite);
        }
        private void OnConnectToSqlSvrCE(object sender, EventArgs e)
        {
            ConnectEmbedded(DBMSPlatformEnum.SqlCe);
        }
        #endregion

        private void OnConnectToDB2(object sender, EventArgs e)
        {
            IDataSource svr = TryConnectDatasource(DBMSPlatformEnum.DB2);
            if (svr == null)
                return;

            //List<System.Data.DataTable> schemas = svr.Schemas;
            //foreach (System.Data.DataTable tbl in schemas)
            //    _dbExplorer.ShowTable(tbl, tbl.TableName);
            DB2TblViewTree tree = new DB2TblViewTree(svr.GetTableLister());
            tree.DataSource = svr;
            tree.Root.Id = svr.DataSource;
            _dbExplorer.AttachSubTree(DBMSPlatformEnum.DB2, tree);
        }

        private void OnConnectToMySql(object sender, EventArgs e)
        {
            IDataSource svr = TryConnectDatasource(DBMSPlatformEnum.MySql);
            if (svr == null)
                return;

            MySqlTblViewTree tree = new MySqlTblViewTree(svr.GetTableLister());
            tree.DataSource = svr;
            tree.Root.Id = svr.DataSource;
            _dbExplorer.AttachSubTree(DBMSPlatformEnum.MySql, tree);
        }

        private void OnConnectToOracle(object sender, EventArgs e)
        {
            IDataSource svr = TryConnectDatasource(DBMSPlatformEnum.Oracle);
            if (svr == null)
                return;

            //List<System.Data.DataTable> schemas = svr.Schemas;
            //foreach (System.Data.DataTable tbl in schemas)
            //    _dbExplorer.ShowTable(tbl, tbl.TableName);
            OracleTblViewTree tree = new OracleTblViewTree(svr.GetTableLister());
            tree.DataSource = svr;
            tree.Root.Id = svr.DataSource;
            _dbExplorer.AttachSubTree(DBMSPlatformEnum.Oracle, tree);
        }
        private void OnConnectToSqlSvr(object sender, EventArgs e)
        {
            IDataSource svr = TryConnectDatasource(DBMSPlatformEnum.SqlServer);
            if (svr == null)
                return;

            SqlSvrTree sqlSvrTree = new SqlSvrTree(svr);
            _dbExplorer.AttachSubTree(DBMSPlatformEnum.SqlServer, sqlSvrTree.Root, sqlSvrTree.SubTrees);
        }

        private void OnSchemaMgr(object sender, EventArgs e)
        {
            SchemaMgrDlg dlg = new SchemaMgrDlg();
            dlg.ShowDialog();
        }

        private void OnDbProviderList(object sender, EventArgs e)
        {
            _dbExplorer.ShowTable(DbProviderFactories.GetFactoryClasses(), "Db Provider");
        }

        #region Default event handling
        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
        #endregion
        #endregion

        private void ConnectEmbedded(DBMSPlatformEnum dbmsEnum)
        {
            IDataSource svr = TryConnectDatasource(dbmsEnum);
            if (svr == null)
                return;

            IDBObjectTree tree = null;
            switch (dbmsEnum)
            {
                case DBMSPlatformEnum.Access:
                    tree = new AccessObjectTree(svr.GetTableLister()); break;
                case DBMSPlatformEnum.SqlCe:
                    tree = new SqlCeObjectTree(svr.GetTableLister()); break;
                case DBMSPlatformEnum.Sqlite:
                    tree = new SqliteObjectTree(svr.GetTableLister()); break;
            }
            tree.DataSource = svr;
            tree.Root.Id = Path.GetFileNameWithoutExtension(svr.DataSource);
            _dbExplorer.AttachSubTree(dbmsEnum, tree);
        }

        /// <summary>
        /// Show connection dialog and return a valid datasource if succeeded.
        /// </summary>
        private IDataSource TryConnectDatasource(DBMSPlatformEnum dbmsEnum)
        {
            DBMSContext context = _instance.GetDBMSContext(dbmsEnum);
            DBLoginInfo login = context.CreateDBLoginInfo();

            PropertyDlg dlg = new PropertyDlg("Connect to " + dbmsEnum.ToString());
            ObjValueLabelHint hint = null;
            List<string> dataSources = _instance.GetDataSources(dbmsEnum);
            if (dataSources != null && dataSources.Count > 0)
            {
                hint = new ObjValueLabelHint();
                hint.ValueHints = new Dictionary<string, IEnumerable<string>>();
                hint.ValueHints.Add("DataSource", dataSources);
            }
            dlg.AddOption(login, hint);
            if (dlg.ShowDialog() != DialogResult.OK)
                return null;

            IDataSource svr = context.CreateDatasource(login.ConnectionString);
            if (!svr.TestConnection())
            {
                MessageBoxEx.ShowError(svr.LastErrorMessage, "Connection Failed");
                return null;
            }

            // Remember the connection
            Dictionary<string, string> connectionList = _instance.GetServerConnectionList(dbmsEnum);
            Macros.SafeAdd(connectionList, login.DataSource, login.ConnectionString);

            return svr;
        }
        #endregion

        #region private data
        private AppInstance _instance;
        private ConfigurationMgr _configMgr;
        private TableViewExplorer _dbExplorer = new TableViewExplorer();
        private int _childFormCount = 0;
        #endregion

    }
}
