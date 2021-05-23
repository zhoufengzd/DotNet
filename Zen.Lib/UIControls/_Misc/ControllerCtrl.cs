using System.Windows.Forms;
using Zen.Utilities.Threading;
using System;

namespace Zen.UIControls
{
    /// <summary>
    /// </summary>
    public partial class ControllerCtrl : UserControl
    {
        public ControllerCtrl()
        {
            InitializeComponent();
        }

        public IControllable Controller
        {
            set { _controller = value; }
        }

        #region private functions
        private void OnLoad(object sender, System.EventArgs e)
        {
            _statusTimer = new Timer();
            _statusTimer.Interval = 250;
            _statusTimer.Tick += new System.EventHandler(SetButtonStatus);
        }

        private void OnStart(object sender, System.EventArgs e)
        {
            if (!_controller.Start())
            {
                //MessageBoxEx.ShowError(_controller.LastError, this.Text);
                return;
            }

            if (!_statusTimer.Enabled)
                _statusTimer.Start();

        }

        private void OnStop(object sender, System.EventArgs e)
        {
            if (!_controller.Stop())
            {
                //MessageBoxEx.ShowError(_controller.LastError, this.Text);
                return;
            }
        }

        private void OnPause(object sender, System.EventArgs e)
        {
            if (!_controller.Pause())
            {
                //MessageBoxEx.ShowError(_controller.LastError, this.Text);
                return;
            }
        }

        private void SetButtonStatus(object sender, System.EventArgs e)
        {
            State st = _controller.State;
            if (st == State.Stopped)
            {
                _buttonStart.Enabled = true;
                _buttonPause.Enabled = false;
                _buttonStop.Enabled = false;

                if (_statusTimer.Enabled)
                    _statusTimer.Stop();
            }
            else if (_controller.State == State.Running)
            {
                _buttonPause.Enabled = true;
                _buttonStop.Enabled = true;
                _buttonStart.Enabled = false;
            }
            else if (st == State.Paused)
            {
                _buttonStart.Enabled = true;
                _buttonPause.Enabled = true;
                _buttonStop.Enabled = false;
            }
            else
            {
                _buttonStart.Enabled = false;
                _buttonPause.Enabled = false;
                _buttonStop.Enabled = false;
            }
        }
        #endregion

        #region private data
        private IControllable _controller;
        private Timer _statusTimer;
        #endregion
    }

}