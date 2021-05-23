using System;
using Zen.Common.Def;

namespace Zen.UIControls.FileUI
{
    using MessageBoxEx = Zen.UIControls.Misc.MessageBoxEx;

    public partial class FileDirBrowserCtrl : ValidationUserCtrlBase
    {
        public static readonly string DefaultInitialDirectory = ".\\";

        public FileDirBrowserCtrl()
        {
            InitializeComponent();
        }

        public BrowserOption Options
        {
            get { return _options; }
            set { _options = value; }
        }

        public override string Text
        {
            get { return _textBoxPath.Text; }
            set { _textBoxPath.Text = value; }
        }

        #region protected functions
        protected override bool ValidateValue()
        {
            if (string.IsNullOrEmpty(_textBoxPath.Text))
            {
                MessageBoxEx.ShowError("File/Directory can not be empty.", string.Empty);
                return false;
            }

            return true;
        }
        #endregion

        #region private functions
        private void OnBrowse(object sender, EventArgs e)
        {
            string path = null;
            if (_options is FileBrowserOpt)
                path = FileDirBrowser.BrowseFile(_options.Filter, _options.InitialDirectory);
            else
                path = FileDirBrowser.BrowseDir(_options.InitialDirectory);

            if (path == null)
                return;

            _textBoxPath.Text = path;
        }
        #endregion

        #region private data
        private BrowserOption _options;
        #endregion
    }


}