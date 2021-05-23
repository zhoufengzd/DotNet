using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RenameFiles
{
    public partial class MainDlg : Form
    {
        public MainDlg()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            _option = new RenameOptions();
            _propertyCtrlOptions.SetObject(_option);
        }

        private void OnRun(object sender, EventArgs e)
        {
            //string workingDir = null;
            //if (_fileDirBrowserCtrl.OnOk())
            //    workingDir = _fileDirBrowserCtrl.Path;

        }

        private void OnShowNames(object sender, EventArgs e)
        {

        }

        private RenameOptions _option;
    }
}
