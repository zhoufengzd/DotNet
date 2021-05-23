using Zen.Common.Def;
using Zen.Common.Def.Layout;

namespace RenameFiles
{
    public sealed class RenameOptions
    {
        [DirBrowserHint]
        public string SourceDirectory
        {
            get { return _sourceDir; }
            set { _sourceDir = value; }
        }
        public string SourceFormat
        {
            get { return _sourceFormat; }
            set { _sourceFormat = value; }
        }
        public string TargetFormat
        {
            get { return _targetFormat; }
            set { _targetFormat = value; }
        }

        #region private data
        private string _sourceDir;
        private string _sourceFormat;
        private string _targetFormat;
        #endregion
    }
}
