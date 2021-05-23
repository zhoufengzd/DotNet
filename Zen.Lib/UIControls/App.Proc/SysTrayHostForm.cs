using System;
using System.Windows.Forms;

namespace Zen.UIControls
{
    using Macros = Zen.Utilities.Generics.Macros;

    public class InvisibleForm: Form
    {
        public InvisibleForm()
        {
            InitializeComponent();
        }

        #region protected functions
        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary> Called during 'OnFormLoad' </summary>
        protected virtual void LoadCustomComponent()
        {
        }
        #endregion

        #region private functions
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ControlBox = false;
            this.Enabled = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InvisibleForm";
            this.Opacity = 0;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            this.SuspendLayout();
            LoadCustomComponent();
            this.ResumeLayout(false);

            this.Hide();
        }
        #endregion

        #region private data
        private System.ComponentModel.IContainer _components = null;
        #endregion
    }

    public sealed class SysTrayHostForm : InvisibleForm
    {
        public SysTrayHostForm()
        {
        }

        public NotificationItem NotifyItem
        {
            get { return Macros.SafeGet(ref _notifyItem); }
            set { _notifyItem = value; }
        }

        #region protected functions
        /// <summary> Called during 'OnFormLoad' </summary>
        protected override void LoadCustomComponent()
        {
            NotifyItem.ContextMenu.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Exit");
            exitMenuItem.Click += new System.EventHandler(this.OnExit);
            NotifyItem.ContextMenu.Items.Add(exitMenuItem);
        }
        #endregion

        #region private functions
        private void OnExit(object sender, EventArgs e)
        {
            _notifyItem.NotifyIcon.Visible = false;
            this.Close();
        }
        #endregion

        #region private data
        private NotificationItem _notifyItem;
        #endregion
    }

    public sealed class NotificationItem
    {
        public NotificationItem()
        {
        }

        public NotifyIcon NotifyIcon
        {
            get { return _notifyIcon; }
            set { _notifyIcon = value; }
        }

        public ContextMenuStrip ContextMenu
        {
            get { return _notifyIcon.ContextMenuStrip; }
            set { _notifyIcon.ContextMenuStrip = value; }
        }

        #region private data
        private NotifyIcon _notifyIcon;
        #endregion

    }

}
