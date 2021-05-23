using System;
using System.Windows.Forms;
using Zen.Utilities.Proc;

namespace OcrExecutor
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
            Application.Run(new MainDlg(Instance.ConfigManager));
        }

        #region private data
        private static AppInstanceBase Instance = new AppInstanceBase();
        #endregion
    }
}
