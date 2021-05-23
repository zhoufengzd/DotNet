using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MP3AlarmClock
{
    public partial class SettingsDlg : Form
    {       
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        #region Private Variables
        // Private variables ////////////////////////////////////////////////////////
        string fileName;
        uint alarmTimeout;
        char theLetter;
        bool alarmInProgress;
        uint holdTime;
        #endregion

        #region Constructors
        // Constructors /////////////////////////////////////////////////////////////
        public SettingsDlg()
        {
            InitializeComponent();
            fileName = "";
            alarmInProgress = false;
        }
        #endregion

        #region Private Methods
        // Private Methods //////////////////////////////////////////////////////////
        private void playFile()
        {
            // Opens and play an MP3 file on repeat using MCI
            string Pcommand;
            Pcommand = "open \"" + fileName + "\" type mpegvideo alias MediaFile";
            mciSendString(Pcommand, null, 0, IntPtr.Zero);
            Pcommand = "play MediaFile REPEAT";
            mciSendString(Pcommand, null, 0, IntPtr.Zero);
        } // end playFile

        private void stopFile()
        {
            // Stops and closes a media file being played using MCI
            string Pcommand = "stop MediaFile";
            mciSendString(Pcommand, null, 0, IntPtr.Zero);
            Pcommand = "close MediaFile";
            mciSendString(Pcommand, null, 0, IntPtr.Zero);
        } // end stopFile
        #endregion

        #region Event Handlers
        // Event Handlers //////////////////////////////////////////////////////////
        private void btnPlay_Click(object sender, EventArgs e)
        {
            // If the user clicks play, play some music
            playFile();
        } // end btnPlay_Click

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // Set up our file open dialog box
            OpenFileDialog myFD = new OpenFileDialog();
            myFD.DefaultExt = "mp3";
            myFD.Multiselect = false;
            myFD.CheckPathExists = true;
            myFD.CheckFileExists = true;
            myFD.AddExtension = true;
            myFD.ValidateNames = true;
            myFD.Title = "Find the file you want to wake up to";

            // Show the user the dialog.  If the user clicks on "Cancel"
            // we'll do nothing and keep the previous file selection, if any.
            if (myFD.ShowDialog() == DialogResult.OK)
            {
                fileName = myFD.FileName;
                txtFileName.Text = fileName;
            } // end if
        } // end btnBrowse_Click

        private void btnStop_Click(object sender, EventArgs e)
        {
            // If the user clicks "Stop," we stop the music.
            stopFile();
        } // end btnStop_Click

        private void btnSetAlarm_Click(object sender, EventArgs e)
        {
            // If the user hasn't specified a file name, we can't play anything
            // can we?  So we'll just point out that they need to tell us what to
            // play and abort setting the alarm.
            if (txtFileName.Text == "")
            {
                MessageBox.Show("Please choose a file to play!");
                return;
            } // end if
                        
            // First we'll set up our alarm settings
            alarmTimeout = (uint)numericUpDown1.Value;  // get user's timeout value
            Random r = new Random();
            theLetter = Convert.ToChar(r.Next(0x41, 0x5B)); // Choose a new random letter
                                                            // that will stop the alarm.

            // Don't let the user change any settings once the alarm is set.
            btnSetAlarm.Enabled = false;
            btnCancel.Enabled = true;
            txtFileName.Enabled = false;
            btnBrowse.Enabled = false;
            dateTimePicker1.Enabled = false;
            numericUpDown1.Enabled = false;
            
            // Finally, start the timer.
            timer1.Start();
        } // end btnSetAlarm_Click

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Every second, we'll check and see if it's time for the alarm
            if ((dateTimePicker1.Value.Hour == DateTime.Now.Hour) &&
                (dateTimePicker1.Value.Minute == DateTime.Now.Minute) &&
                (dateTimePicker1.Value.Second == DateTime.Now.Second))
            {
                // If it is and the program is minimized, restore it.
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                    this.ShowInTaskbar = true;
                } // end if

                // Make our tray icon show a little balloon message, just for fun.
                notifyIcon1.BalloonTipText = "WAKE UP!";
                notifyIcon1.ShowBalloonTip(5000);

                // Set flag and show labels to tell the user how to stop the alarm.
                alarmInProgress = true;
                label5.Visible = true;
                lblButton.Visible = true;
                lblButton.Text = theLetter.ToString();
                lblTimeout.Visible = true;
                lblTimeout.Text = "for " + alarmTimeout.ToString() + " seconds to" +
                                    " stop alarm.";

                // Stop the alarm timer.  We don't care what time it is now that
                // our alarm time has passed.
                timer1.Stop();

                playFile(); // Start the music!

                holdTime = alarmTimeout;    // Set how many seconds left for our keypress

                // Disable anything that would let the user cheat and end the
                // alarm by clicking cancel, stop, or the red "X"
                btnCancel.Enabled = false;
                btnPlay.Enabled = false;
                btnStop.Enabled = false;
                this.ControlBox = false;
                btnBrowse.Enabled = false;

                // Set focus to our program so the user doesn't have to mess with
                // the mouse.
                this.Focus();
            } // end if
        } // end timer1_Tick

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // If cancel alarm is clicked, we open up the settings
            // and stop our timer.
            dateTimePicker1.Enabled = true;
            numericUpDown1.Enabled = true;
            btnSetAlarm.Enabled = true;
            btnCancel.Enabled = false;
            txtFileName.Enabled = true;
            btnBrowse.Enabled = true;
            timer1.Stop();
        } // end btnCancel_Click

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // If the user presses a key and our alarm is in progress,
            if (alarmInProgress)
            {
                // See if it's the magic key
                if (e.KeyValue == theLetter)
                {
                    timerTimeout.Start();   // if so, start the timer for alarm stoppage
                } // end if
            } // end if
        } // end Form1_KeyDown

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            // If the alarm is in progress and the user lets up a key
            if (alarmInProgress)
            {
                // see if it's the magic key
                if (e.KeyValue == theLetter)
                {
                    // If so, stop the timer and reset the timeout.
                    timerTimeout.Stop();
                    holdTime = alarmTimeout;
                    lblTimeout.Text = "for " + holdTime.ToString() + " seconds to" +
                                " stop alarm.";
                } // end if
            } // end if
        } // end Form1_KeyUp

        private void timerTimeout_Tick(object sender, EventArgs e)
        {
            // This function is for our timeout timer.  It decrements holdTime
            // which keeps track of how much longer the user has to hold down the
            // magic key to stop the alarm.
            holdTime--;
            lblTimeout.Text = "for " + holdTime.ToString() + " seconds to" +
                                " stop alarm.";

            // If they've held it down long enough, stop the music, stop the timer,
            // and re-enable all controls.
            if (holdTime == 0)
            {
                stopFile();
                alarmInProgress = false;
                timerTimeout.Stop();
                dateTimePicker1.Enabled = true;
                numericUpDown1.Enabled = true;
                btnSetAlarm.Enabled = true;
                btnCancel.Enabled = false;
                lblTimeout.Visible = false;
                lblButton.Visible = false;
                label5.Visible = false;
                btnCancel.Enabled = true;
                btnPlay.Enabled = true;
                btnStop.Enabled = true;
                txtFileName.Enabled = true;
                this.ControlBox = true;
                btnBrowse.Enabled = true;
            } // end if
        } // end timerTimeout_Tick

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopFile();
        } // end Form1_FormClosing

        private void Form1_Resize(object sender, EventArgs e)
        {
            // If the user minimizes the window, pop up a bubble from our tray icon
            // and let them know what is going on.
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.BalloonTipText = "Your alarm clock is now hidden, but your alarm\n" +
                                            "will remain set.  Double-click the alarm clock icon\n" +
                                            "to make adjustments.  Mouse over the icon to see\n" +
                                            "current alarm status.";
                notifyIcon1.ShowBalloonTip(3000);
                this.ShowInTaskbar = false;
            } // end if
        } // end Form1_Resize

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            // If the user double clicks the tray icon, be sure our window is visible
            // and shown in the task bar.
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        } // end notifyIcon1_DoubleClick

        private void notifyIcon1_MouseMove(object sender, MouseEventArgs e)
        {
            // If the user moves the mouse over the tray icon, show a balloon
            // detailing current status.
            if (btnSetAlarm.Enabled)
            {
                notifyIcon1.BalloonTipText = "No alarm is set.";
                notifyIcon1.ShowBalloonTip(3000);
            } // end if
            else
            {
                notifyIcon1.BalloonTipText = "Alarm is set for:\n" +
                                    dateTimePicker1.Value.TimeOfDay.ToString();
                notifyIcon1.ShowBalloonTip(3000);
            }// end else
        } // end notifyIcon1_MouseMove
        #endregion

    } // end class
} // end namespace