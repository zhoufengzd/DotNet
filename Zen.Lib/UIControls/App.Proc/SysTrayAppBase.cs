using System;
using System.Windows.Forms;

namespace Zen.UIControls.App
{
    public class WinAppBase
    {
        public WinAppBase()
        {
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            InitApplication();
            Application.Run(_mainForm);
            ExitApplication();
        }

        #region protected functions
        protected virtual void InitApplication()
        {
        }
        protected virtual void ExitApplication()
        {
        }
        #endregion

        #region protected data
        protected Form _mainForm;
        #endregion
    }


    /// <summary>
    /// Sys Tray application.
    /// </summary>
    public abstract class TrayAppBase : WinAppBase
    {
        public TrayAppBase()
        {
        }

        #region override base functions
        protected override void InitApplication()
        {
            if (_mainForm != null)
                return;

            SysTrayHostForm hostForm = new SysTrayHostForm();
            NotifyIcon icon = null;
            InitIcon(out icon);
            icon.Visible = true;
            hostForm.NotifyItem.NotifyIcon = icon;

            ContextMenuStrip menu = null;
            InitMenu(out menu);
            hostForm.NotifyItem.ContextMenu = menu;

            _mainForm = hostForm;
        }
        protected override void ExitApplication()
        {
        }
        #endregion

        #region virtual functions. Override required.
        protected virtual void InitIcon(out NotifyIcon icon) { icon = null; }
        protected virtual void InitMenu(out ContextMenuStrip menu) { menu = null; }
        #endregion
    }
}
