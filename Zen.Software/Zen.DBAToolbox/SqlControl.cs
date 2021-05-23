using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Zen.Common.Def;
using Zen.DBMS;
using Zen.UIControls.FileUI;
using Zen.Utilities.Text;

namespace Zen.DBAToolbox
{
    public partial class SqlControl : UserControl
    {
        public SqlControl()
        {
            InitializeComponent();
            LoadControllers();
        }

        public IDataSource DataSource
        {
            set { _svr = value; }
        }
        public string SqlText
        {
            set
            {
                string formatted = RTFInvoker.HighLight(value, _syntaxColorMap);
                _richTextBoxSql.Rtf = formatted;
            }
            get
            {
                return _richTextBoxSql.Text;
            }
        }

        public void Save()
        {
            string filePath = FileDirBrowser.SaveFile(FileTypeFilter.SqlFile, null, null);
            if (filePath != null)
                _richTextBoxSql.SaveFile(filePath, RichTextBoxStreamType.PlainText);
        }

        public void RunSql()
        {
            string sql = (_richTextBoxSql.SelectionLength > 0) ? _richTextBoxSql.SelectedText : _richTextBoxSql.Text;
            if (string.IsNullOrEmpty(sql))
                return;

            DataTable result = null;
            if (_svr.Execute(sql, out result))
                _gridTblResult.DataSource = result;
            else
                _richTextBoxMsg.Text = _svr.LastErrorMessage;
        }

        #region private functions
        private void LoadControllers()
        {
            if (_syntaxColorMap != null)
                return;

            string[] sqlDDLWords = new string[] { "CREATE", "TABLE", };
            string[] sqlValueWords = new string[] { "NOT", "NULL" };

            _syntaxColorMap = new Dictionary<Color, IEnumerable<string>>();
            _syntaxColorMap.Add(Color.Blue, sqlDDLWords);
            _syntaxColorMap.Add(Color.Green, sqlValueWords);
        }

        #endregion

        #region private data
        private IDataSource _svr;
        private Dictionary<Color, IEnumerable<string>> _syntaxColorMap;
        #endregion
    }
}
