using System;
using System.Text;
using System.Windows.Forms;
using ModiLib;
using Zen.UIControls;
using Zen.Utilities.FileUtil;
using Zen.Utilities.Proc;
using Zen.Utilities.Threading;

namespace OcrExecutor
{
    public partial class MainDlg : Form
    {
        static readonly string OCRJob = "OCR Job";

        public MainDlg(ConfigurationMgr configMgr)
        {
            _configMgr = configMgr;
            InitializeComponent();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            _configMgr.LoadProfile(OCRJob, out _settings);
            _optionsCtrl.SetObject(_settings);
        }

        private void OnStart(object sender, EventArgs e)
        {
            _optionsCtrl.OnOk();
            if (_settings == null)
                return;

            _configMgr.SaveProfile(_settings, OCRJob);

            if (_resultTimer == null)
            {
                _resultTimer = new Timer();
                _resultTimer.Interval = 250;
                _resultTimer.Tick += new System.EventHandler(OnUpdateResult);
            }

            _resultTimer.Start();
            if (_batchPro == null)
                _batchPro = new BatchOcrProcessor(_settings);

            ControllerDlg dlg = new ControllerDlg(_batchPro);
            dlg.ShowDialog();
        }

        private void OnStop(object sender, EventArgs e)
        {
            _resultTimer.Stop();
            _batchPro.Stop();
        }

        private void OnPause(object sender, EventArgs e)
        {
            _resultTimer.Stop();
            _batchPro.Pause();
        }

        private void OnUpdateResult(object sender, EventArgs e)
        {
            if (_batchPro.State == State.Stopped)
                _resultTimer.Stop();

            StringBuilder result = new StringBuilder();
            //result.Append(string.Format("Document Count = {0} \r\nPage Count = {1} \r\nDuration in seconds: {2:F2}",
            result.Append(string.Format("Document Count = {0} \r\nPage Count = {1} \r\n",
                _batchPro.DocCount, _batchPro.PageCount));
            foreach (string errorFile in _batchPro.ErrorFiles)
                result.AppendFormat("{0}\r\n", errorFile);

            _textBoxResult.Text = result.ToString();
        }

        #region private data
        private ConfigurationMgr _configMgr;
        private BatchOptionBase<OcrOption> _settings;
        private BatchOcrProcessor _batchPro;

        private Timer _resultTimer;
        #endregion
    }
}
