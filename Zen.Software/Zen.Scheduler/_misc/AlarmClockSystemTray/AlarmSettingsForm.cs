using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AlarmClockSystemTray
{
    //using OptionsDlg = Zen.UIControls.OptionsDlg;

	public partial class AlarmSettingsForm : Form
	{
        static readonly string SettingFile = "alarm.opt";

		public AlarmSettingsForm()
		{
            _alarmTimer = new Timer();
			InitializeComponent();
		}

		private void OnFormLoad(object sender, EventArgs e)
		{
            //load settings
            LoadFromFile();

            DateTime now = DateTime.Now;
            _settings.Time = new DateTime(now.Year, now.Month, now.Day, 
                _settings.Time.Hour, _settings.Time.Minute, _settings.Time.Second);
			_alarmDateTimePicker.Value = _settings.Time;

            _TextBoxMessage.Text = _settings.Message;
            _TextBoxSoundFilename.Text = _settings.SoundFile;
            _textBoxCmd.Text = _settings.Action;
            _alarmEnabledCheckBox.Checked = _settings.Enabled;

            _alarmTimer.Interval = 5000; // check every 5 seconds
            _alarmTimer.Tick += new System.EventHandler(this.OnTimer);
			_alarmNotifyIcon.Visible = true;

			// Set the sound dialog to My Music folder
			_soundBrowserDialog.InitialDirectory = Environment.SpecialFolder.MyMusic.ToString();
            _soundBrowserDialog.Filter = "Sound file (*.wav)|*.wav|All files (*.*)|*.*";
		}
		
		private void RolloverTime()
		{
			// If the user selects a time already passed, it must be for tomorrow
			if( DateTime.Now.TimeOfDay.CompareTo(_settings.Time.TimeOfDay) > 0 )
			{
				_settings.Time = new DateTime(DateTime.Now.Year, 
					DateTime.Now.Month, DateTime.Now.Day + 1, 
					_settings.Time.Hour, _settings.Time.Minute, _settings.Time.Second);
			}
			// Otherwise, set it for today
			else
			{
				_settings.Time = new DateTime(DateTime.Now.Year, 
					DateTime.Now.Month, DateTime.Now.Day, 
					_settings.Time.Hour, _settings.Time.Minute, _settings.Time.Second);
			}
		}

		private void SoundBrowseButton_Click(object sender, EventArgs e)
		{
			if( _soundBrowserDialog.ShowDialog() == DialogResult.OK )
			{
				_settings.SoundFile = _soundBrowserDialog.FileName;
				_TextBoxSoundFilename.Text = _settings.SoundFile;
			}
		}

		private void OnOK(object sender, EventArgs e)
		{
			this.Hide();

            _settings.Time = _alarmDateTimePicker.Value;
			_settings.Message = _TextBoxMessage.Text;
			_settings.SoundFile = _TextBoxSoundFilename.Text;
            _settings.Action = _textBoxCmd.Text;
			_settings.Enabled = _alarmEnabledCheckBox.Checked;
            SaveToFile();

			EnabledToolStripMenuItem.Checked = _settings.Enabled;
			_alarmTimer.Enabled = _settings.Enabled;

			if( _settings.Enabled )
				RolloverTime();

			if (_settings.SoundFile.Length > 0)
			{
				player.SoundLocation = _settings.SoundFile;
				player.Load();
			}
		}

		private void OnCancel(object sender, EventArgs e)
		{
			this.Hide();
		}

		private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
            _trayContextMenuStrip.Hide();
            this.Show();
		}

		private void EnabledToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Toggle the checkbox on the menu item
			_settings.Enabled = ! EnabledToolStripMenuItem.Checked;
			EnabledToolStripMenuItem.Checked = _settings.Enabled;
			_alarmEnabledCheckBox.Checked = _settings.Enabled;

			// Set the time for the current or next day accordingly
			RolloverTime();

			// Activate/deactive the timer
			_alarmTimer.Enabled = _settings.Enabled;

			_trayContextMenuStrip.Hide();
		}

		private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_alarmNotifyIcon.Visible = false;
			Application.Exit();
		}

		private void AlarmNotifyIcon_BalloonTipClicked(object sender, EventArgs e)
		{
			// When the user stops the alarm...
			player.Stop();
			RolloverTime();
		}

		private void AlarmNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.Show();
		}

		private void AlarmNotifyIcon_MouseClick(object sender, MouseEventArgs e)
		{
			if( e.Button == MouseButtons.Right )
			{
	            _trayContextMenuStrip.Show();
		    }
		}

		private void OnTimer(object sender, EventArgs e)
		{
			if( DateTime.Now.CompareTo(_settings.Time) >= 0 )
			{
				// Add a day to the alarm to reset it for the next day
				RolloverTime();

				// Show the notification for up to a minute.
				// After that, no one must be paying attention!
				_alarmNotifyIcon.BalloonTipText = 
					(_settings.Message.Length > 0?_settings.Message:"Attention!");
				_alarmNotifyIcon.ShowBalloonTip(60000);

				// Play the alarm sound
				if( _settings.SoundFile.Length > 0 )
				{
					player.PlayLooping();
				}

                RunCommand();
			}
		}

        private bool RunCommand()
        {
            if (_textBoxCmd.Text == null)
                return false;

            string rawCmd = _textBoxCmd.Text.Trim();
            if (rawCmd.Length < 1)
                return false;

            string[] splitted = _textBoxCmd.Text.Split(new char[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
            if (!File.Exists(splitted[0]))
            {
                MessageBox.Show("File does not exist. " + splitted[0]);
                return false;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = splitted[0];
            if (splitted.Length > 2)
                startInfo.Arguments = splitted[2];

            Process proc = Process.Start(startInfo);
            return true;
            //proc.WaitForExit();
            //return (proc.ExitCode > -1);
        }

        private void SaveToFile()
        {
            XmlSerializer serializer = new XmlSerializer(_settings.GetType());
            using (Stream fs = new FileStream(SettingFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                serializer.Serialize(fs, _settings);
                fs.Close();
            }
        }

        private void LoadFromFile()
        {
            if (!File.Exists(SettingFile))
            {
                _settings = new AlarmSettings();

                DateTime now = DateTime.Now;
                _settings.Time = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
                _settings.Message = "Attention!";
                _settings.Action = "\"C:\\WINDOWS\\system32\\shutdown.exe\" \"/s /t 180\"";
                _settings.Enabled = true;
                return;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(AlarmSettings));
            using (Stream fs = new FileStream(SettingFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                _settings = (AlarmSettings)serializer.Deserialize(fs);
                fs.Close();
            }
        }

        private Timer _alarmTimer;
        private AlarmSettings _settings;
        private System.Media.SoundPlayer player = new System.Media.SoundPlayer();
    }
}