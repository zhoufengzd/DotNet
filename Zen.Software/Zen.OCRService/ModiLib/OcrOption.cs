using System;

namespace ModiLib
{
    /// <summary> MODI OCR options </summary>
    public sealed class OcrOption
    {
        public LanguageEnum Language
        {
            get { return _language; }
            set { _language = value; }
        }
        public bool WithAutoRotation
        {
            get { return _withAutoRotation; }
            set { _withAutoRotation = value; }
        }
        public bool WithStraightenImage
        {
            get { return _WithStraightenImage; }
            set { _WithStraightenImage = value; }
        }

        #region private data
        private LanguageEnum _language = LanguageEnum.Sysdefault;
        private bool _withAutoRotation = true;
        private bool _WithStraightenImage = true;
        #endregion
    }

}
