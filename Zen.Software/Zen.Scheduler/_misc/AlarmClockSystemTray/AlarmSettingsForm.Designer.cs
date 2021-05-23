namespace AlarmClockSystemTray
{
	partial class AlarmSettingsForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmSettingsForm));
            this._soundBrowserDialog = new System.Windows.Forms.OpenFileDialog();
            this._alarmNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this._trayContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EnabledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SoundBrowseButton = new System.Windows.Forms.Button();
            this._TextBoxSoundFilename = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this._TextBoxMessage = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.settingsCancelButton = new System.Windows.Forms.Button();
            this._alarmDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.settingsOKButton = new System.Windows.Forms.Button();
            this._alarmEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.Label2 = new System.Windows.Forms.Label();
            this._groupBox = new System.Windows.Forms.GroupBox();
            this._textBoxCmd = new System.Windows.Forms.TextBox();
            this._labelCmd = new System.Windows.Forms.Label();
            this._trayContextMenuStrip.SuspendLayout();
            this._groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _soundBrowserDialog
            // 
            this._soundBrowserDialog.AddExtension = false;
            this._soundBrowserDialog.Filter = "MP3 files|*.mp3|WAV files|*.wav";
            this._soundBrowserDialog.Title = "Please select a sound to play for alarm notification";
            // 
            // _alarmNotifyIcon
            // 
            this._alarmNotifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this._alarmNotifyIcon.BalloonTipText = "This is your requested alarm!";
            this._alarmNotifyIcon.BalloonTipTitle = "Alarm";
            this._alarmNotifyIcon.ContextMenuStrip = this._trayContextMenuStrip;
            this._alarmNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("_alarmNotifyIcon.Icon")));
            this._alarmNotifyIcon.Text = "Alarm Clock";
            this._alarmNotifyIcon.Visible = true;
            this._alarmNotifyIcon.BalloonTipClosed += new System.EventHandler(this.AlarmNotifyIcon_BalloonTipClicked);
            this._alarmNotifyIcon.BalloonTipClicked += new System.EventHandler(this.AlarmNotifyIcon_BalloonTipClicked);
            this._alarmNotifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AlarmNotifyIcon_MouseClick);
            this._alarmNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.AlarmNotifyIcon_MouseDoubleClick);
            // 
            // _trayContextMenuStrip
            // 
            this._trayContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SettingsToolStripMenuItem,
            this.EnabledToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this._trayContextMenuStrip.Name = "TrayContextMenuStrip";
            this._trayContextMenuStrip.Size = new System.Drawing.Size(137, 70);
            // 
            // SettingsToolStripMenuItem
            // 
            this.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem";
            this.SettingsToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.SettingsToolStripMenuItem.Text = "Settings...";
            this.SettingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsToolStripMenuItem_Click);
            // 
            // EnabledToolStripMenuItem
            // 
            this.EnabledToolStripMenuItem.Name = "EnabledToolStripMenuItem";
            this.EnabledToolStripMenuItem.ShowShortcutKeys = false;
            this.EnabledToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.EnabledToolStripMenuItem.Text = "Enabled";
            this.EnabledToolStripMenuItem.Click += new System.EventHandler(this.EnabledToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.ExitToolStripMenuItem.Text = "Exit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // SoundBrowseButton
            // 
            this.SoundBrowseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SoundBrowseButton.Location = new System.Drawing.Point(358, 98);
            this.SoundBrowseButton.Name = "SoundBrowseButton";
            this.SoundBrowseButton.Size = new System.Drawing.Size(37, 23);
            this.SoundBrowseButton.TabIndex = 16;
            this.SoundBrowseButton.Text = "&...";
            this.SoundBrowseButton.Click += new System.EventHandler(this.SoundBrowseButton_Click);
            // 
            // SoundFilenameTextBox
            // 
            this._TextBoxSoundFilename.Location = new System.Drawing.Point(94, 98);
            this._TextBoxSoundFilename.Name = "SoundFilenameTextBox";
            this._TextBoxSoundFilename.Size = new System.Drawing.Size(255, 23);
            this._TextBoxSoundFilename.TabIndex = 15;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(13, 98);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(75, 17);
            this.Label3.TabIndex = 14;
            this.Label3.Text = "&Sound File";
            // 
            // MessageTextBox
            // 
            this._TextBoxMessage.Location = new System.Drawing.Point(94, 59);
            this._TextBoxMessage.Name = "MessageTextBox";
            this._TextBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._TextBoxMessage.Size = new System.Drawing.Size(301, 23);
            this._TextBoxMessage.TabIndex = 13;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(12, 23);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(74, 17);
            this.Label1.TabIndex = 10;
            this.Label1.Text = "Alarm &time";
            // 
            // settingsCancelButton
            // 
            this.settingsCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.settingsCancelButton.Location = new System.Drawing.Point(323, 234);
            this.settingsCancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.settingsCancelButton.Name = "settingsCancelButton";
            this.settingsCancelButton.Size = new System.Drawing.Size(100, 28);
            this.settingsCancelButton.TabIndex = 19;
            this.settingsCancelButton.Text = "&Cancel";
            this.settingsCancelButton.Click += new System.EventHandler(this.OnCancel);
            // 
            // _alarmDateTimePicker
            // 
            this._alarmDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this._alarmDateTimePicker.Location = new System.Drawing.Point(94, 23);
            this._alarmDateTimePicker.Margin = new System.Windows.Forms.Padding(4);
            this._alarmDateTimePicker.Name = "_alarmDateTimePicker";
            this._alarmDateTimePicker.ShowUpDown = true;
            this._alarmDateTimePicker.Size = new System.Drawing.Size(106, 23);
            this._alarmDateTimePicker.TabIndex = 11;
            // 
            // settingsOKButton
            // 
            this.settingsOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.settingsOKButton.Location = new System.Drawing.Point(215, 233);
            this.settingsOKButton.Margin = new System.Windows.Forms.Padding(4);
            this.settingsOKButton.Name = "settingsOKButton";
            this.settingsOKButton.Size = new System.Drawing.Size(100, 28);
            this.settingsOKButton.TabIndex = 18;
            this.settingsOKButton.Text = "&OK";
            this.settingsOKButton.Click += new System.EventHandler(this.OnOK);
            // 
            // _alarmEnabledCheckBox
            // 
            this._alarmEnabledCheckBox.AutoSize = true;
            this._alarmEnabledCheckBox.Checked = true;
            this._alarmEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._alarmEnabledCheckBox.Location = new System.Drawing.Point(35, 163);
            this._alarmEnabledCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this._alarmEnabledCheckBox.Name = "_alarmEnabledCheckBox";
            this._alarmEnabledCheckBox.Size = new System.Drawing.Size(118, 21);
            this._alarmEnabledCheckBox.TabIndex = 17;
            this._alarmEnabledCheckBox.Text = "Alarm &enabled";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(12, 59);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(65, 17);
            this.Label2.TabIndex = 12;
            this.Label2.Text = "&Message";
            // 
            // _groupBox
            // 
            this._groupBox.Controls.Add(this._textBoxCmd);
            this._groupBox.Controls.Add(this._labelCmd);
            this._groupBox.Controls.Add(this._TextBoxMessage);
            this._groupBox.Controls.Add(this._alarmDateTimePicker);
            this._groupBox.Controls.Add(this.Label3);
            this._groupBox.Controls.Add(this.Label2);
            this._groupBox.Controls.Add(this._alarmEnabledCheckBox);
            this._groupBox.Controls.Add(this._TextBoxSoundFilename);
            this._groupBox.Controls.Add(this.Label1);
            this._groupBox.Controls.Add(this.SoundBrowseButton);
            this._groupBox.Location = new System.Drawing.Point(12, 12);
            this._groupBox.Name = "_groupBox";
            this._groupBox.Size = new System.Drawing.Size(411, 203);
            this._groupBox.TabIndex = 20;
            this._groupBox.TabStop = false;
            // 
            // _textBoxCmd
            // 
            this._textBoxCmd.Location = new System.Drawing.Point(94, 133);
            this._textBoxCmd.Name = "_textBoxCmd";
            this._textBoxCmd.Size = new System.Drawing.Size(301, 23);
            this._textBoxCmd.TabIndex = 19;
            // 
            // _labelCmd
            // 
            this._labelCmd.AutoSize = true;
            this._labelCmd.Location = new System.Drawing.Point(12, 133);
            this._labelCmd.Name = "_labelCmd";
            this._labelCmd.Size = new System.Drawing.Size(51, 17);
            this._labelCmd.TabIndex = 18;
            this._labelCmd.Text = "Actio&n:";
            // 
            // AlarmSettingsForm
            // 
            this.ClientSize = new System.Drawing.Size(435, 286);
            this.ControlBox = false;
            this.Controls.Add(this._groupBox);
            this.Controls.Add(this.settingsCancelButton);
            this.Controls.Add(this.settingsOKButton);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlarmSettingsForm";
            this.Text = "Shutdown settings";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this._trayContextMenuStrip.ResumeLayout(false);
            this._groupBox.ResumeLayout(false);
            this._groupBox.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.OpenFileDialog _soundBrowserDialog;
		private System.Windows.Forms.NotifyIcon _alarmNotifyIcon;
		private System.Windows.Forms.ContextMenuStrip _trayContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem SettingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem EnabledToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
		private System.Windows.Forms.Button SoundBrowseButton;
		private System.Windows.Forms.TextBox _TextBoxSoundFilename;
		private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.TextBox _TextBoxMessage;
		private System.Windows.Forms.Label Label1;
		private System.Windows.Forms.Button settingsCancelButton;
		private System.Windows.Forms.DateTimePicker _alarmDateTimePicker;
		private System.Windows.Forms.Button settingsOKButton;
		private System.Windows.Forms.CheckBox _alarmEnabledCheckBox;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.GroupBox _groupBox;
        private System.Windows.Forms.TextBox _textBoxCmd;
        private System.Windows.Forms.Label _labelCmd;
	}
}

