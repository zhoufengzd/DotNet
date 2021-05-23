using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Zen.Simulator
{
    public partial class MainDlg : Form
    {
        public MainDlg()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            //_strOpt.Values = new string[] { "private", "void", "OnLoad(object", "sender", "EventArgs", "e" };
            _propertyCtrlOptions.SetObject(_strOpt);
        }

        private void OnGo(object sender, EventArgs e)
        {
            _propertyCtrlOptions.OnOk();
            RandomString rs = new RandomString(_strOpt);
            _textBoxOutput.Text = rs.Next();
        }

        private StringOption _strOpt = new StringOption();
    }
}
