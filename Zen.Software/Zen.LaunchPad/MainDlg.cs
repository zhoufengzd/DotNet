using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Zen.Utilities;
using Zen.Utilities.FileUtil;
using Zen.Utilities.Proc;
using Zen.UIControls;

namespace Zen.LaunchPad
{
    public partial class MainDlg : Form
    {
        static readonly string CmdConfigFile = "Command.config";
        static readonly string ExplorerCmd = "explorer.exe";
        static readonly string ExplorerParam = "/e,\"c:\\";

        public MainDlg()
        {
            InitializeComponent();
        }

        #region private functions
        private void OnFormLoad(object sender, EventArgs e)
        {
            _executor = new Executor();
            _env = new ProcEnvironInfo();
            _defaultCmdPaths = new List<string>();
            _defaultCmdPaths.Add(_env.SysDir.System32);
            _defaultCmdPaths.Add(_env.SysDir.WinDir);

            _configFile = Path.Combine(_env.WorkingDir, CmdConfigFile);
            if (FileValidator.IsValid(_configFile))
            {
                _cmdTable = ObjSerializer.Load<DataTable>(Path.Combine(_env.WorkingDir, CmdConfigFile));
            }
            else
            {
                _cmdTable = new DataTable("CommandTable");
                _cmdTable.Columns.Add("Command");
                _cmdTable.Columns.Add("Parameters");

                object[] itemArray = new object[_cmdTable.Columns.Count];
                itemArray[0] = ExplorerCmd;
                itemArray[1] = ExplorerParam;
                DataRow dtRowTmp = _cmdTable.NewRow();
                dtRowTmp.ItemArray = itemArray;
                _cmdTable.Rows.Add(dtRowTmp);
            }

            FillSettingTable();

        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            ObjSerializer.Save(_configFile, _cmdTable);
        }

        private void OnBrowse(object sender, EventArgs e)
        {
            //IconPickerDlg isf = new IconPickerDlg();
            //isf.ShowDialog();
        }

        private void OnRun(object sender, EventArgs e)
        {
            RunProcess();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnCmdGridDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            RunProcess();
        }

        private void FillSettingTable()
        {
            Dictionary<string, string> shellSettings = new Dictionary<string, string>();
            shellSettings.Add("cdl", @"c:\xyz\cdl");

            _processListCtrl.SetObject(new PropertyEnvelope<Dictionary<string, string>>(shellSettings));
        }

        private void RunProcess()
        {
            //if (_cmdGrid.SelectedCells.Count > 0)
            //{
            //    int rowIndex = _cmdGrid.SelectedCells[0].RowIndex;
            //    DataGridViewCellCollection cells = _cmdGrid.Rows[rowIndex].Cells;
            //    string cmd = cells[0].Value.ToString();
            //    string param = cells[1].Value.ToString();

            //    if (string.IsNullOrEmpty(cmd))
            //        return;

            //    if (PathBuilder.BuildFullPath(cmd, _defaultCmdPaths) == null)
            //    {
            //        _outputTextBox.Text = "Invalid Command!";
            //        return;
            //    }

            //    _executor.RunProcess(cmd, param, true, true);
            //    _outputTextBox.Text = _executor.Output;

            //}
        }
        #endregion

        #region private data
        private ProcEnvironInfo _env;
        private List<string> _defaultCmdPaths;

        private string _configFile;
        private DataTable _cmdTable;

        private Executor _executor;
        #endregion
    }
}