using System;
using System.IO;
using System.Windows.Forms;
using Zen.Utilities;
using Zen.Utilities.FileUtil;

namespace Zen.FileProcessor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && File.Exists(args[0]))
            {
                string batchOptFile = args[0];

                BatchOptionBase<ReplaceOption> bo = ObjSerializer.Load<BatchOptionBase<ReplaceOption>>(batchOptFile);
                BatchTextReplacer batchPro = new BatchTextReplacer(bo);
                batchPro.Start();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new TextProcessorForm());
            }
        }
    }
}