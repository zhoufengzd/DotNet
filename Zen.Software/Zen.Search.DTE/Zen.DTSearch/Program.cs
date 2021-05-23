using System;
using System.Windows.Forms;
using Zen.Utilities.Proc;

namespace Zen.DTSearch
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
            Application.Run(new MainDlg(Instance.ConfigurationManager));
        }

        #region private data
        private static AppInstance Instance = new AppInstance();
        #endregion
    }
}
