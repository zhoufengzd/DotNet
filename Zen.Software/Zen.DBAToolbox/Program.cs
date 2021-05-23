using System;
using System.Windows.Forms;

namespace Zen.DBAToolbox
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(_instance));
        }

        #region private data
        private static AppInstance _instance = new AppInstance();
        #endregion
    }
}