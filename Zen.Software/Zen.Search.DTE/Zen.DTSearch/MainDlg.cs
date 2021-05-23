using System;
using System.Windows.Forms;
using Zen.Search.Util;
using Zen.UIControls;
using System.IO;
using dtSearch.Engine;
using System.Collections.Generic;
using Zen.Utilities.Proc;

namespace Zen.DTSearch
{

    //public sealed class IndexInfo
    //{
    //    private int _docCount = 0;

    //    public int DocCount
    //    {
    //        get { return _docCount; }
    //        set { _docCount = value; }
    //    }
    //    private long _wordCount = 0;
    //    private DateTime _createDate = DateTime.MinValue;
    //    private long _indexSize = 0;
    //    private List<string> _fields;
    //}

    public partial class MainDlg : Form
    {
        const string DTFileIndexJob = "Index Job";
        public MainDlg(ConfigurationMgr configMgr)
        {
            _configMgr = configMgr;
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (!_configMgr.LoadSettings(DTFileIndexJob, ref _fileOptions))
                _fileOptions = new DTFileIndexOptions();

            _propertyCtrl.SetObject(_fileOptions);
        }

        private void OnOK(object sender, EventArgs e)
        {
            _propertyCtrl.OnOk();
            if (_fileOptions == null)
                return;

            _configMgr.SaveSettings(_fileOptions, DTFileIndexJob);

            FTFileIndexer indexer = new FTFileIndexer(_fileOptions);
            ControllerDlg dlg = new ControllerDlg(indexer);
            dlg.ShowDialog();
        }

        private void OnIndexInfo(object sender, EventArgs e)
        {
            string indexPath = _fileOptions.InOutOpt.OutputOptions.OutputDirectory;
            string indexName = Path.GetFileName(indexPath);

            //IndexInfo idxInfo = IndexJob.GetIndexInfo(indexPath);
            //_idxDocCount.Text = idxInfo.DocCount.ToString();
            //_idxWordCount.Text = idxInfo.WordCount.ToString();
            //_idxCreateDate.Text = idxInfo.CreatedDate.ToLocalTime().ToString();
            //_idxSize.Text = idxInfo.IndexSize.ToString();

            //ListIndexJob job = new ListIndexJob();
            //job.IndexPath = indexPath;
            //job.OutputToString = false;
            //job.OutputFile = Path.Combine(Path.GetDirectoryName(indexPath), indexName + ".fields.txt");
            //job.ListIndexFlags |= ListIndexFlags.dtsListIndexFields;

            //int fieldCount = 0;
            //if (job.Execute())
            //{
            //    using (StreamReader file = new StreamReader(job.OutputFile))
            //    {
            //        string line = null;
            //        while ((line = file.ReadLine()) != null)
            //        {
            //            if (line.Length < 1) // Skip empty line
            //                continue;

            //            if (fieldCount++ < 150)
            //                _listBoxFields.Items.Add(line);
            //        }
            //    }
            //    _idxFieldCount.Text = fieldCount.ToString();
            //}

        }


        private ConfigurationMgr _configMgr;
        private DTFileIndexOptions _fileOptions;
    }
}
