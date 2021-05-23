using System;
using System.Windows.Forms;
using Zen.UIControls;
using System.Collections.Generic;

namespace Zen.DirectoryMonitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainDlg());
        }
    }
}
