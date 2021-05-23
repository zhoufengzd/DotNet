using System.Collections.Generic;
using System.IO;
using MODI;

namespace ModiLib
{
    /// <summary>
    /// Wrapper for Modi.Document. Office 2007 installation is required.
    /// Note  The following TIFF file types are not supported:
    ///   YCbCr color space, except when the image is JPEG 
    ///   CIE Lab color space 
    ///   Images with more than five samples per pixel, or images with sample sizes larger than 32 bits 
    /// </summary>
    public sealed class OcrEngine
    {
        public delegate void OnProgressEventHandler(int Progress, ref bool Cancel);
        public event OnProgressEventHandler OnOCRProgressEvent;

        public OcrEngine()
        {
        }
        public OcrOption OcrOption
        {
            set { _opt = value; }
        }
        public List<string> ErrorFiles
        {
            get { return _errorFiles; }
        }

        /// <summary> 
        /// Process OCR and return page count 
        /// </summary>
        public int Process(string inputFile, string outputFile)
        {
            MiDoc doc = Process(inputFile);
            if (doc == null)
                return 0;

            int pageCount = 0;

            StreamWriter writer = new StreamWriter(outputFile, false);
            foreach (MiPage mp in doc.Pages)
            {
                if (pageCount != 0)
                    writer.WriteLine('\f');
                writer.Write(mp.Text);

                pageCount++;
            }
            writer.Flush();
            writer.Close();
            doc.Dispose();

            return pageCount;
        }

        /// <summary> 
        /// Advanced. It's user's responsibility to displose MiDoc properly.
        /// </summary>
        public MiDoc Process(string inputFile)
        {
            Document doc = new Document();
            doc.OnOCRProgress += new _IDocumentEvents_OnOCRProgressEventHandler(this.OnProgress);

            try
            {
                doc.Create(inputFile);
                doc.OCR(MiLanguageMapper.ToMiLanguage(_opt.Language), _opt.WithAutoRotation, _opt.WithStraightenImage);
            }
            catch(System.Exception)
            {
                _errorFiles.Add(inputFile);

                doc.Close(false);
                doc = null;
                return null;
            }

            return (new MiDoc(doc));
        }

        #region private functions
        private void OnProgress(int Progress, ref bool Cancel)
        {
            if (OnOCRProgressEvent != null)
                OnOCRProgressEvent(Progress, ref Cancel);
        }
        #endregion

        #region private data
        private OcrOption _opt = new OcrOption();
        private List<string> _errorFiles = new List<string>();
        #endregion
    }
}
