﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using System.Collections;

namespace Zen.UIControls
{
    /// <summary>
    /// Generic options dialog: support built in system types & user types
    ///   To Do: upgrade with home grown property grid, to handle
    ///     1. File / directory path; 2. Radio buttons; 3.etc.
    /// </summary>
    public partial class CollectionDlg : Form
    {
        public CollectionDlg()
        {
            InitializeComponent();
        }

        public void AddOption(IEnumerable collections)
        {
            _collectionCtrl.SetObject(collections);
        }
        //public void AddOption(object options, string category)
        //{
        //    _optionsCtrl.AddOption(options, category);
        //}

        #region private functions
        private void OnFormLoad(object sender, EventArgs e)
        {
        }

        private void OnOK(object sender, EventArgs e)
        {
            Close();
            this.DialogResult = DialogResult.OK;
        }

        private void OnCancel(object sender, EventArgs e)
        {
            Close();
        }
        #endregion
    }

}