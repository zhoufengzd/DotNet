using System;
using System.Windows.Forms;

namespace Zen.UIControls.FileUI
{
    public sealed class FileDirBrowser
    {
        public static string BrowseFile(string fileFilter, string initialDir)
        {
            OpenFileDialog openDlg = new OpenFileDialog();

            openDlg.InitialDirectory = initialDir;
            openDlg.Filter = fileFilter;
            openDlg.FilterIndex = 0;
            openDlg.RestoreDirectory = true;

            if (openDlg.ShowDialog() != DialogResult.OK)
                return null;

            return openDlg.FileName;
        }
        public static string SaveFile(string fileFilter, string optionalFileName, string initialDir)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();

            saveDlg.FileName = optionalFileName;
            saveDlg.InitialDirectory = initialDir;
            saveDlg.Filter = fileFilter;
            saveDlg.FilterIndex = 0;
            saveDlg.RestoreDirectory = true;

            if (saveDlg.ShowDialog() != DialogResult.OK)
                return null;

            return saveDlg.FileName;
        }

        public static string BrowseDir(string initialDir)
        {
            return BrowseDir(initialDir, false);
        }
        public static string BrowseDir(string initialDir, bool showNewFolderBtn)
        {
            FolderBrowserDialog openDlg = new FolderBrowserDialog();
            openDlg.RootFolder = Environment.SpecialFolder.DesktopDirectory;
            openDlg.SelectedPath = initialDir;
            openDlg.ShowNewFolderButton = showNewFolderBtn;

            if (openDlg.ShowDialog() != DialogResult.OK)
                return null;

            return openDlg.SelectedPath;
        }
    }
}
