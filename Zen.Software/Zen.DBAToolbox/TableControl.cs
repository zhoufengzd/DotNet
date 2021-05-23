using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zen.Utilities.Text;
using Zen.DBMS;

namespace Zen.DBAToolbox
{
    public partial class TableControl : UserControl
    {
        public TableControl()
        {
            InitializeComponent();
            _dataGridView.DataError += new DataGridViewDataErrorEventHandler(OnGridError);
        }

        public DataTable DataTable
        {
            set { _dataGridView.DataSource = value;}
            get { return _dataGridView.DataSource as DataTable;}
        }

        public void Save()
        {
            DataTable tbl = this.DataTable;
            if (tbl == null)
                return;

            CsvWriter.Write(tbl, tbl.TableName + ".csv");
        }

        #region private functions
        private void OnGridError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // do nothing for now
        }
        #endregion

        #region private data
        #endregion

    }
}
