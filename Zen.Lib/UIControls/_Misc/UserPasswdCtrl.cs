using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Zen.UIControls
{
    public partial class UserPasswdCtrl : UserControl
    {
        public UserPasswdCtrl()
        {
            InitializeComponent();
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        public string PassWord
        {
            get { return _passWord; }
            set { _passWord = value; }
        }

        public string ErrorMsg
        {
            set { _errorMsg = value; }
        }
        public string SuccessMsg
        {
            set { _successMsg = value; }
        }

        public bool OnOk()
        {
            _userName = _textBoxUsername.Text.Trim();
            _passWord = _textBoxPassword.Text.Trim();

            if (string.IsNullOrEmpty(_userName) || string.IsNullOrEmpty(_passWord))
            {
                if (!string.IsNullOrEmpty(_errorMsg))
                    MessageBox.Show(_errorMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                return false;
            }

            return true;
        }

        public bool OnCancel()
        {
            return true;
        }

        #region private functions
        private void OnFormLoad(object sender, EventArgs e)
        {
            _textBoxUsername.Text = _userName;
            _textBoxPassword.Text = _passWord;
        }
        #endregion

        private string _userName;
        private string _passWord;
        private string _errorMsg;
        private string _successMsg;
    }
}