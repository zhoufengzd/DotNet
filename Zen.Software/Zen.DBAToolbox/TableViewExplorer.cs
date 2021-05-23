using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Zen.Common.Def;
using Zen.DBAToolbox.Resources;
using Zen.DBMS.Schema;
using Zen.Utilities.Generics;

namespace Zen.DBAToolbox
{
    using Cell = Pair<int, int>;

    /// <summary>
    /// <TreeLayout>
    /// Manage DB Table & Views
    /// DBMS --Platform
    ///           |___ Datasource
    ///                     |___ Database 1 (sql server)
    ///                              |___ Table / View
    ///                     |___ Database 2 (sql server)
    ///                              |___ Table / View
    ///           |___ Datasource
    ///                     |___ Table / View
    /// </TreeLayout>
    /// <DetailView>
    /// 1. Sql Control: Run sql / Show Result / Save Sql
    /// 2. Table Editor
    /// </DetailView>
    /// Act as both view and controller (table manipulation)
    /// </summary>
    public partial class TableViewExplorer : Form
    {
        public TableViewExplorer()
        {
            InitializeComponent();
        }

        public void ClearTree()
        {
            _rootNode.Nodes.Clear();
        }

        public void AttachSubTree(DBMSPlatformEnum dbmsEnum, DataNode<string, Cell> root, IEnumerable<IDataTableTree> subTrees)
        {
            TreeNode dbmsRoot = AddDBMSRoot(dbmsEnum);
            TreeNode dataSourceRoot = dbmsRoot.Nodes.Add(root.Id);
            foreach (IDataTableTree objTree in subTrees)
            {
                DataNode<string, Cell> dataRoot = objTree.Root;
                if (dataRoot.Children.Count > 0)
                {
                    TreeNode subTreeRoot = dataSourceRoot.Nodes.Add(dataRoot.Id);
                    AddChildNodes(subTreeRoot, dataRoot.Children.Values, objTree);
                }
            }
        }

        public void AttachSubTree(DBMSPlatformEnum dbmsEnum, IDataTableTree dataSourceTree)
        {
            DataNode<string, Cell> dataRoot = dataSourceTree.Root;
            if (dataRoot.Children.Count < 1)
                return;

            TreeNode dbmsRoot = AddDBMSRoot(dbmsEnum);
            TreeNode subTreeRoot = dbmsRoot.Nodes.Add(dataRoot.Id);
            AddChildNodes(subTreeRoot, dataRoot.Children.Values, dataSourceTree);
        }

        public void ShowTable(DataTable tbl, string tabTitle)
        {
            TabPage tp = Zen.UIControls.CtrlBuilder.BuildTabPage(tabTitle, _tabDetailView.Size, _tabDetailView.Controls.Count);

            TableControl ctrl = new TableControl();
            ctrl.Dock = DockStyle.Fill;
            ctrl.DataTable = tbl;

            tp.Controls.Add(ctrl);
            _tabDetailView.Controls.Add(tp);
        }

        #region private functions
        #region load event
        private void OnLoad(object sender, EventArgs e)
        {
            _treeView.ImageList = LoadTreeIcons();
            _rootNode = _treeView.Nodes.Add("Datasource");
            _rootNode.ImageIndex = 0;
            _rootNode.SelectedImageIndex = 1;

            LoadToolbar();
        }

        private void OnClose(object sender, EventArgs e)
        {
            if (_tabDetailView.SelectedIndex > -1)
                _tabDetailView.Controls.RemoveAt(_tabDetailView.SelectedIndex);
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (_tabDetailView.SelectedIndex < 0)
                return;

            TabPage tp = _tabDetailView.Controls[_tabDetailView.SelectedIndex] as TabPage;
            if (tp == null)
                return;

            SqlControl sqlCtrl = tp.Controls[0] as SqlControl;
            TableControl tblCtrl = tp.Controls[0] as TableControl;
            if (sqlCtrl != null)
                sqlCtrl.Save();
            else if (tblCtrl != null)
                tblCtrl.Save();
        }

        private void OnExecute(object sender, EventArgs e)
        {
            if (_tabDetailView.SelectedIndex < 0)
                return;

            TabPage tp = _tabDetailView.Controls[_tabDetailView.SelectedIndex] as TabPage;
            if (tp == null)
                return;

            SqlControl sqlCtrl = tp.Controls[0] as SqlControl;
            if (sqlCtrl != null)
                sqlCtrl.RunSql();
        }
        #endregion

        #region context menu
        private void OnContextMenu(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            Point pt = new Point(e.X, e.Y);
            TreeNode node = _treeView.GetNodeAt(pt);
            if (node == null)
                return;

            TableLink link = (node.Tag == null) ? null : node.Tag as TableLink;
            if (link != null)
            {
                _treeView.SelectedNode = node;
                _ctxtNode = node;
                ContextMenuStrip ctxtMenu = LoadContextMenu();
                ctxtMenu.Show(_treeView, pt);
            }
            _treeView.SelectedNode = null;
        }

        private void OnSqlQuery(object sender, EventArgs e)
        {
            //try
            {
                TableLink link = (_ctxtNode.Tag == null) ? null : _ctxtNode.Tag as TableLink;
                if (link == null)
                    return;

                IDBObjectTree dbTree = link.Tree as IDBObjectTree;
                if (dbTree == null)
                    return;

                DBObjectIdentifier id = dbTree.GetIdentifier(link.Cell);
                string selectSql = string.Format("SELECT * FROM {0}", id.FullName);

                TabPage tp = Zen.UIControls.CtrlBuilder.BuildTabPage(string.Format("Query{0}", _sqlTabCount++), _tabDetailView.Size, _tabDetailView.Controls.Count);

                SqlControl ctrl = new SqlControl();
                ctrl.Dock = DockStyle.Fill;
                ctrl.SqlText = selectSql;
                ctrl.DataSource = dbTree.DataSource;

                tp.Controls.Add(ctrl);
                _tabDetailView.Controls.Add(tp);
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void OnImport(object sender, EventArgs e)
        {
            //DBObjectIdentifier idf = _dbExplorer.Selected;
            //if (idf != null && idf.Type == DBObjectEnum.Table)
            //{
            //    //try
            //    {
            //        SqlConnection conn = new SqlConnection(_dbExplorer.LogIn.ToString());
            //        conn.Open();
            //        conn.ChangeDatabase(idf.Database);

            //        TableSchema ts = new TableSchema(conn, idf.FullName);
            //        //TableBulkCopier bc = new TableBulkCopier(ts, conn);
            //        //DataTable tbl = bc.BuildDataTable();

            //        //TableBulkUpdater bu = new TableBulkUpdater(idf.FullName, null, conn, null);
            //        //tbl = bu.BuildDataTable();
            //        //bu.DoBatch(tbl);

            //        PropertyDlg dlg = new PropertyDlg("Table Info");
            //        //OptionsDlg dlg = new OptionsDlg();
            //        dlg.AddOption(ts);
            //        dlg.ShowDialog();
            //    }
            //    //catch (Exception ex)
            //    //{
            //    //    MessageBox.Show(ex.Message);
            //    //}
            //}
        }
        private void OnExport(object sender, EventArgs e)
        {
            //try
            {
                TableLink link = (_ctxtNode.Tag == null) ? null : _ctxtNode.Tag as TableLink;
                if (link == null)
                    return;

                IDBObjectTree dbTree = link.Tree as IDBObjectTree;
                if (dbTree == null)
                    return;

                DataTable tbl = dbTree.Export(link.Cell);
                ShowTable(tbl, tbl.TableName);
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void OnObjToTable(object sender, EventArgs e)
        {
            //ORMGenerator gen = new ORMGenerator();
            //SchemaContext context = new SchemaContext();
            //context.DBMSContext = _instance.GetDBMSContext(DBMSPlatformEnum.SqlServer);
            //string ddl = gen.GenerateDDL(typeof(Zen.UIControls.Layout.TextBoxHint), context);

            //_dbExplorer.ShowSql(ddl, "DDL");
        }

        private void OnTableToObj(object sender, EventArgs e)
        {
            TableLink link = (_ctxtNode.Tag == null) ? null : _ctxtNode.Tag as TableLink;
            if (link == null)
                return;

            IDBObjectTree dbTree = link.Tree as IDBObjectTree;
            if (dbTree == null)
                return;

            DBObjectIdentifier id = dbTree.GetIdentifier(link.Cell);
            TableSchema schema = new TableSchema(dbTree.DataSource, id.FullName);

            ShowTable(schema.ColumnDefs, id.Name);
        }

        /// <summary>
        /// Load table / view context menu
        /// </summary>
        private ContextMenuStrip LoadContextMenu()
        {
            ContextMenuStrip ctxtMenu = new ContextMenuStrip();
            ctxtMenu.SuspendLayout();
            ToolStripMenuItem itemSql = new ToolStripMenuItem("&Sql Query...", null, OnSqlQuery);
            ToolStripMenuItem itemImport = new ToolStripMenuItem("&Import...", null, OnImport);
            ToolStripMenuItem itemExport = new ToolStripMenuItem("&Export", null, OnExport);
            ToolStripMenuItem itemTblToObj = new ToolStripMenuItem("&Dynamic Form", null, OnTableToObj);

            ctxtMenu.Items.Add(itemSql);
            ctxtMenu.Items.Add(itemImport);
            ctxtMenu.Items.Add(itemExport);
            ctxtMenu.Items.Add(itemTblToObj);

            ctxtMenu.ResumeLayout(false);
            return ctxtMenu;
        }
        #endregion

        #region Load tree
        private ImageList LoadTreeIcons()
        {
            ImageList imgList = new ImageList();

            ImageList.ImageCollection images = imgList.Images;
            images.Add(Icons.Datasource);//0
            images.Add(Icons.DatasourceSelected);
            images.Add(Icons.Server);//2
            images.Add(Icons.ServerSelected);
            images.Add(Icons.DB);//4
            images.Add(Icons.DBSelected);
            images.Add(Icons.Group);//6
            images.Add(Icons.GroupSelected);
            images.Add(Icons.Table);//8
            images.Add(Icons.TableSelected);

            return imgList;
        }

        private TreeNode AddDBMSRoot(DBMSPlatformEnum dbmsEnum)
        {
            TreeNode dbmsRoot = null;

            string dbmsName = dbmsEnum.ToString();
            if (_rootNode.Nodes.ContainsKey(dbmsName))
            {
                dbmsRoot = _rootNode.Nodes[dbmsName];
            }
            else
            {
                dbmsRoot = _rootNode.Nodes.Add(dbmsName, dbmsName);
                dbmsRoot.ImageIndex = 4;
                dbmsRoot.SelectedImageIndex = dbmsRoot.ImageIndex;
            }

            return dbmsRoot;
        }

        private void AddChildNodes(TreeNode vwParentNode, ICollection<DataNode<string, Cell>> childrenData, IDataTableTree objTree)
        {
            foreach (DataNode<string, Cell> nodeData in childrenData)
            {
                TreeNode child = new TreeNode();

                bool isGroup = (nodeData.Children.Count > 0);
                child.Text = objTree.ToString(nodeData.Data);
                child.ImageIndex = (isGroup) ? 6 : 8;
                child.SelectedImageIndex = child.ImageIndex;
                child.Tag = new TableLink(objTree, nodeData.Data);
                vwParentNode.Nodes.Add(child);

                if (isGroup)
                    AddChildNodes(child, nodeData.Children.Values, objTree);
            }
        }
        #endregion

        private void LoadToolbar()
        {
            //ImageList imgList = new ImageList();

            //ImageList.ImageCollection images = imgList.Images;
            //images.Add(Icons.Save);
            //images.Add(Icons.Execute);
            //images.Add(Icons.Close);
            //_toolStripButtons.ImageList = imgList;

            ToolStripButton btnSave = new ToolStripButton(Icons.Save.ToBitmap());
            btnSave.ToolTipText = "Save";
            btnSave.Click += new EventHandler(this.OnSave);

            ToolStripButton btnExecute = new ToolStripButton(Icons.Execute.ToBitmap());
            btnExecute.Click += new EventHandler(this.OnExecute);
            btnExecute.ToolTipText = "Execute";

            ToolStripButton btnClose = new ToolStripButton(Icons.Close.ToBitmap());
            btnClose.Click += new EventHandler(this.OnClose);
            btnClose.ToolTipText = "Close";

            _toolStripBtns.Items.Add(btnSave);
            _toolStripBtns.Items.Add(btnExecute);
            _toolStripBtns.Items.Add(btnClose);
        }
        #endregion

        #region private data
        private TreeNode _rootNode;
        private TreeNode _ctxtNode;
        private int _sqlTabCount = 0;
        #endregion


    }
}
