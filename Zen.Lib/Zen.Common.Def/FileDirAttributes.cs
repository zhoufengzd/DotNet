
namespace Zen.Common.Def
{
    public static class FileTypeFilter
    {
        public static readonly string AllFile = "All files (*.*)|*.*";
        public static readonly string TextFile = "Text Files (*.txt)|*.txt" + "|" + AllFile;
        public static readonly string ExeFile = "Executable Files|*.dll;*.exe;*.ocx" + "|" + AllFile;
        public static readonly string CompressedFile = "Compressed Files|*.gz;*.asdf;*.zip" + "|" + AllFile;
        public static readonly string VideoFile = "Video Files|*.avi;*.mpg;*.wmv;*.mkv" + "|" + AllFile;
        public static readonly string ImageFile = "Image File|*.bmp;*.gif;*.tif;*.jpg" + "|" + AllFile;
        public static readonly string SqlFile = "SQL files (*.sql)|*.sql" + "|" + AllFile;
    }

    public abstract class BrowserOption
    {
        public string InitialDirectory
        {
            get { return _initialDir; }
            set { if (!string.IsNullOrEmpty(value)) _initialDir = value; }
        }
        public string Filter
        {
            get { return _filter; }
            set { if (!string.IsNullOrEmpty(value)) _filter = value; }
        }

        #region private data
        protected string _filter = FileTypeFilter.AllFile;
        protected string _initialDir = @".\";
        #endregion
    }

    public sealed class FileBrowserOpt: BrowserOption
    {
        public FileBrowserOpt(string initDir, string filter)
        {
            InitialDirectory = initDir;
            Filter = filter;
        }
    }

    public sealed class DirectoryBrowserOpt: BrowserOption
    {
        public DirectoryBrowserOpt(string initDir, string filter)
        {
            InitialDirectory = initDir;
            Filter = filter;
        }
    }
}
