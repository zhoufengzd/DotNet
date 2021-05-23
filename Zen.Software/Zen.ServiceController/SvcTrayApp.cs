using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zen.UIControls.App;
using System.Windows.Forms;

namespace Zen.WinSvcController
{
    class SvcTrayApp : TrayAppBase
    {
        public SvcTrayApp()
        {
        }

        #region Override TrayAppBase functions.
        protected override void InitIcon(out NotifyIcon icon)
        {
            icon = new NotifyIcon();
            icon.Icon = Properties.Resource.SvcIcon;
            icon.Text = "Window Service Controller";
            icon.MouseDoubleClick += new MouseEventHandler(OnIconDoubleClick);
        }
        protected override void InitMenu(out ContextMenuStrip menu)
        {
            menu = new ContextMenuStrip();
            ToolStripMenuItem showControllerMenuItem = new ToolStripMenuItem("&Show Controller Form...");
            showControllerMenuItem.Click += new System.EventHandler(this.OnShowControllerForm);
            menu.Items.Add(showControllerMenuItem);
        }
        #endregion

        #region private functions
        private void OnIconDoubleClick(object sender, MouseEventArgs e)
        {
            ShowControllerForm();
        }

        private void OnShowControllerForm(object sender, EventArgs e)
        {
            ShowControllerForm();
        }

        private void ShowControllerForm()
        {
            if (_controllerForm == null || _controllerForm.IsDisposed)
            {
                _controllerForm = new SvcControllerDlg();
                _controllerForm.Show();
            }
            else
            {
                _controllerForm.BringToFront();
            }
        }
        #endregion

        #region private data
        private SvcControllerDlg _controllerForm;
        #endregion
    }
}
