using System;
using System.Collections.Generic;

namespace ModiLib
{
    using MODI;

    public sealed class PixelInfo
    {
        public PixelInfo(int bitCount, int height, int width, int xDPI, int yDPI)
        {
            _bitsPerPixel = bitCount;
            _pixelHeight = height;
            _pixelWidth = width;
            _xDPI = xDPI;
            _yDPI = yDPI;
        }

        public int BitsPerPixel
        {
            get { return _bitsPerPixel; }
            set { _bitsPerPixel = value; }
        }
        public int PixelHeight
        {
            get { return _pixelHeight; }
            set { _pixelHeight = value; }
        }
        public int PixelWidth
        {
            get { return _pixelWidth; }
            set { _pixelWidth = value; }
        }
        public int XDPI
        {
            get { return _xDPI; }
            set { _xDPI = value; }
        }
        public int YDPI
        {
            get { return _yDPI; }
            set { _yDPI = value; }
        }

        private int _bitsPerPixel;
        private int _pixelHeight;
        private int _pixelWidth;
        private int _xDPI;
        private int _yDPI;
    }

    /// <summary>
    /// Modi Page Image. 
    /// </summary>
    public sealed class MiPage : IDisposable
    {
        public MiPage(Image image)
        {
            _image = image;
            _pixelInfo = new PixelInfo(_image.BitsPerPixel, _image.PixelHeight,
                _image.PixelWidth, _image.XDPI, _image.YDPI);
        }
        ~MiPage()
        {
            DoDispose();
        }
        public void Dispose()
        {
            DoDispose();
        }

        public string Text 
        {
            get { return _image.Layout.Text; }
        }

        public IEnumerator<string> Words
        {
            get { return (IEnumerator<string>)_image.Layout.Words; }
        }

        public PixelInfo PixelInfo
        {
            get { return _pixelInfo; }
        }

        public void OCR(OcrOption opt)
        {
            _image.OCR(MiLanguageMapper.ToMiLanguage(opt.Language), opt.WithAutoRotation, opt.WithStraightenImage);
        }

        public void Rotate(int angle)
        {
            _image.Rotate(angle);
        }

        #region private functions
        private MiPage()
        {
        }
        private void DoDispose()
        {
            _pixelInfo = null;
            _image = null;
        }
        #endregion

        #region private data
        private Image _image;
        private PixelInfo _pixelInfo;
        #endregion

    }
}
