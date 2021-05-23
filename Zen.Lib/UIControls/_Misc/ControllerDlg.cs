using System;
using System.Windows.Forms;
using Zen.Utilities.Threading;

namespace Zen.UIControls
{
    public partial class ControllerDlg : Form
    {
        public ControllerDlg(IControllable controller)
        {
            _controller = controller;
            InitializeComponent();
        }

        public string Title
        {
            set { Text = value; }
        }

        #region private functions
        private void OnLoad(object sender, EventArgs e)
        {
            if (_timer == null)
            {
                _controllerCtrl.Controller = _controller;
                _timer = new Timer();
                _timer.Interval = 250;
                _timer.Tick += new EventHandler(OnUpdateStatus);
            }

            _timer.Start();
            _controller.Start();
        }

        private void OnUpdateStatus(object sender, EventArgs eArgs)
        {
            if (_controller.State == State.Stopped)
            {
                _timer.Stop();
                _progressBar.Value = 100;

                Close();
                DialogResult = DialogResult.OK;
            }
            else if (_controller.State == State.Running)
            {
                if (_progressBar.Value == 100)
                    _progressBar.Value = 10;
                else
                    _progressBar.Value++;
            }
        }

        #endregion

        #region private data
        private IControllable _controller;
        private Timer _timer;
        #endregion

    }
}
