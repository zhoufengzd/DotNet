using System;
using System.Drawing;
using System.Windows.Forms;


namespace ScreenCapturer
{
    using ConfigurationMgr = Zen.Utilities.Proc.ConfigMgr<CaptureOption>;

    public partial class MainDlg : Form
    {
        static readonly string ConfigFile = "ocrScreen.config";

        public MainDlg()
        {
            InitializeComponent();
        }

        #region private functions
        private void OnLoad(object sender, EventArgs e)
        {
            _configMgr = new ConfigurationMgr(ConfigFile);

            Rectangle boundsTmp = Screen.GetBounds(Point.Empty);
            _configMgr.Settings.StartPoint = Point.Empty;
            _configMgr.Settings.EndPoint = new Point(boundsTmp.Width, boundsTmp.Height);

            _optionsCtrl.SetObject(_configMgr.Settings);
        }

        private void OnSelectArea(object sender, EventArgs e)
        {
            this.Hide();

            AreaChooserDlg captureForm = new AreaChooserDlg();
            captureForm.ShowDialog();

            _configMgr.Settings.StartPoint = captureForm.TopLeft;
            _configMgr.Settings.EndPoint = captureForm.BottomRight;

            this.Show();
        }

        private void OnGO(object sender, EventArgs e)
        {
            _optionsCtrl.OnOk();
            _configMgr.Save();

            this.WindowState = FormWindowState.Minimized;
            System.Threading.Thread.Sleep(250); //Allow screen to repaint itself

            Capturer.DoWork(_configMgr.Settings);
        }

        private void OnCancel(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region private data
        private ConfigurationMgr _configMgr;
        #endregion

    }
}