using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Cassini.WebServer
{
    static class Program
    {
        //
        // Simple WinForms application to start a Cassini server
        //
        // Command line:  <physical-path> <port> <virtual-path>
        // Example:       c:\foo 80 /
        //
        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AdminForm(args));
        }
    }
}