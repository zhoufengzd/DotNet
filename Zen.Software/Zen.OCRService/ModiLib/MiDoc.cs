using System;
using System.Collections.Generic;

namespace ModiLib
{
    using MODI;

    /// <summary>
    /// Modi Page Image. 
    /// </summary>
    public sealed class MiDoc : IDisposable
    {
        public MiDoc(Document doc)
        {
            _doc = doc;
        }

        ~MiDoc()
        {
            DoDispose();
        }

        public void Dispose()
        {
            DoDispose();
        }

        public List<MiPage> Pages
        {
            get
            {
                if (_doc == null)
                    return null;

                if (_miImages == null)
                {
                    Images rawImages = _doc.Images;
                    _miImages = new List<MiPage>(rawImages.Count);
                    foreach (Image img in rawImages)
                        _miImages.Add(new MiPage(img));
                }

                return _miImages;
            }
        }

        #region private functions
        /// <summary>
        /// MiDoc could only be constructed from an existing Document instance. 
        /// No default constructor allowed.
        /// </summary>
        private MiDoc()
        {
        }

        private void DoDispose()
        {
            if (_doc != null)
            {
                if (_miImages != null)
                {
                    foreach (MiPage mp in _miImages)
                        mp.Dispose();

                    _miImages.Clear();
                    _miImages = null;
                }

                _doc.Close(false);
                _doc = null;
            }
        }
        #endregion

        #region private data
        private Document _doc;
        private List<MiPage> _miImages;
        #endregion
    }
}
