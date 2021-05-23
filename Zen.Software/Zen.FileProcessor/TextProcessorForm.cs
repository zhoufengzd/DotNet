using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Zen.Utilities;
using Zen.Utilities.FileUtil;
using Zen.Utilities.Proc;

namespace Zen.FileProcessor
{
    using OptionsDlg = Zen.UIControls.OptionsDlg;

    /// <summary>
    /// TextProcessorDlg
    /// </summary>
    public partial class TextProcessorForm : NotePadForm
    {
        static readonly string BatchConfigFile = "batch.config";

        public TextProcessorForm()
        {
        }

        #region private functions
        private void OnReplace(object sender, EventArgs e)
        {
            if (_procOpt == null)
            {
                _procOpt = new ReplaceOption();
                _optDlg = new OptionsDlg();
                _optDlg.AddOption(_procOpt);
            }

            if (_optDlg.ShowDialog() != DialogResult.OK)
                return;

            StrStreamProvider provider = new StrStreamProvider(_textBoxInput.Text);
            MemoryStream memstrm = new MemoryStream();
            TextReplacer processor = new TextReplacer((new StreamWriter(memstrm)), _procOpt);

            TextDispatcher dispatcher = new TextDispatcher(provider, processor);
            dispatcher.Run();
            UpdateOutputTextBox(Encoding.Default.GetString(memstrm.GetBuffer()));
        }

        private void OnBatchReplaceOperation(object sender, EventArgs e)
        {
            BatchOptionBase<ReplaceOption> bo = ObjSerializer.Load<BatchOptionBase<ReplaceOption>>(BatchConfigFile);
            _batchOptDlg = new OptionsDlg();
            _batchOptDlg.AddOption(bo);

            if (_batchOptDlg.ShowDialog() != DialogResult.OK)
                return;

            ObjSerializer.Save(BatchConfigFile, bo);
            BatchTextReplacer batchPro = new BatchTextReplacer(bo);
            batchPro.Start();
        }

        private void OnTextMining(object sender, EventArgs e)
        {
            Dictionary<string, int> result = null;
            Zen.Utilities.FileUtil.TextCounter.CountWord(_textBoxInput.Text, ref result);

            _toolStripStatusLabel.Text = "Total# of words: " + result.Count;
            UpdateOutputDataGrid(result);
        }

        private void OnBatchTextMining(object sender, EventArgs e)
        {
            OptionsDlg batchOptDlg = new OptionsDlg();
            BatchInOutOption fileOptions = new BatchInOutOption();
            batchOptDlg.AddOption(fileOptions);

            if (batchOptDlg.ShowDialog() != DialogResult.OK)
                return;

            BatchTextCounter batchPro = new BatchTextCounter(fileOptions);
            batchPro.Start();

            _toolStripStatusLabel.Text = "Total# of words: " + batchPro.Result.Count;
            UpdateOutputDataGrid(batchPro.Result);
        }

        private void OnToggleOutput(object sender, EventArgs e)
        {
            if (_textBoxOutput.Visible || _dataGridViewOutput.Visible)
            {
                _textBoxOutput.Visible = false;
                _dataGridViewOutput.Visible = false;
            }
            else
            {
                if (_showOutputGrid)
                    _dataGridViewOutput.Visible = true;
                else
                    _textBoxOutput.Visible = true;
            }
            ResizeControls();
        }

        private void UpdateOutputTextBox(string txt)
        {
            _showOutputGrid = false;
            _textBoxOutput.Text = txt;

            if (_textBoxOutput.Visible || _dataGridViewOutput.Visible)
            {
                _dataGridViewOutput.Visible = false;

                _textBoxOutput.Visible = true;
                _textBoxOutput.Update();
            }
        }
        private void UpdateOutputDataGrid(Dictionary<string, int> result)
        {
            _showOutputGrid = true;

            DataTable wordTbl = new DataTable("WordCount");
            wordTbl.Columns.Add("Word", typeof(string));
            wordTbl.Columns.Add("Count", typeof(int));
            foreach (KeyValuePair<string, int> kv in result)
            {
                object[] itemArray = new object[2];
                itemArray[0] = kv.Key;
                itemArray[1] = kv.Value;

                DataRow dtRowTmp = wordTbl.NewRow();
                dtRowTmp.ItemArray = itemArray;
                wordTbl.Rows.Add(dtRowTmp);
            }
            _dataGridViewOutput.DataSource = wordTbl;

            if (_textBoxOutput.Visible || _dataGridViewOutput.Visible)
            {
                _textBoxOutput.Visible = false;

                _dataGridViewOutput.Visible = true;
                _dataGridViewOutput.Update();
            }
        }
        #endregion

        #region private data
        private ConfigurationMgr _batchConfigMgr;
        private ReplaceOption _procOpt;
        private OptionsDlg _optDlg;
        private OptionsDlg _batchOptDlg;

        private bool _showOutputGrid;
        #endregion
    }

}
