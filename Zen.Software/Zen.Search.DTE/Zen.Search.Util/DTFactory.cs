using System.IO;
using dtSearch.Engine;
using System.Collections.Specialized;

namespace Zen.Search.Util.DTS
{
    public sealed class DTFactory
    {
        public DTFactory(string optionDir)
        {
            _dtOptionDir = optionDir;
            SetDefaultOptions();
        }

        public IndexJob GetCreateIndexJob(string indexPath, dtSearch.Engine.DataSource dtSource)
        {
            IndexJob indexJob = new IndexJob();
            indexJob.DataSourceToIndex = dtSource;
            indexJob.IndexPath = indexPath;
            indexJob.ActionCreate = true;
            indexJob.ActionAdd = true;
            indexJob.ActionCompress = false;

            indexJob.CreateRelativePaths = false;
            indexJob.ExcludeFilters = null;
            indexJob.IncludeFilters = null;
            indexJob.IndexingFlags = 0;

            // Optional properties
            //StringCollection stored = new StringCollection();
            //stored.Add("E-table_Docid");
            //indexJob.StoredFields = stored;
            indexJob.TempFileDir = Path.GetTempPath();
            indexJob.MaxMemToUseMB = 512;
            indexJob.AutoCommitIntervalMB = 512;

            return indexJob;
        }

        #region private functions
        /// <summary>
        /// Options must be set up at least once when your program starts. 
        /// </summary>
        private void SetDefaultOptions()
        {
            using (Options options = BuildOptions())
            {
                options.Save();
            }
        }

        /// <summary>
        /// Option settings are maintained separately for each thread, so changes to 
        /// options in one thread will not affect a job in progress on another thread. 
        /// When a new thread is started, it will inherit the most recent option settings
        /// from other threads in the process.
        /// </summary>
        private Options BuildOptions()
        {
            Options options = new Options();
            options.AlphabetFile = Path.Combine(_dtOptionDir, "English.abc");
            options.BinaryFiles = BinaryFilesSettings.dtsoIndexSkipBinary;
            options.FieldFlags = FieldFlags.dtsoFfSkipFilenameField;
            options.FileTypeTableFile = Path.Combine(_dtOptionDir, "FileType.xml");
            options.Hyphens = HyphenSettings.dtsoHyphenAll;
            options.IndexNumbers = true;
            options.MaxWordLength = 128;
            options.MaxWordsToRetrieve = 8000;
            options.NoiseWordFile = Path.Combine(_dtOptionDir, "Noise.dat");
            options.TitleSize = 0;
            options.TextFlags = TextFlags.dtsoTfAutoBreakCJK;

            return options;
        }
        #endregion

        #region private data
        private string _dtOptionDir;
        #endregion
    }

    public class DTStatusHandler : ISearchStatusHandler
    {
        public DTStatusHandler()
        {
        }

        public void OnFound(dtSearch.Engine.SearchResultsItem item)
        {
        }
        public void OnSearchingIndex(string index)
        {
        }
        public void OnSearchingFile(string file)
        {
        }
        public AbortValue CheckForAbort()
        {
            //return AbortValue.Cancel;
            return AbortValue.Continue;
        }

    }
}
