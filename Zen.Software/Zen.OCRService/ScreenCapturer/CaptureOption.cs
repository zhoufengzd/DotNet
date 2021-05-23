using System.Drawing;
using Zen.UIControls.Layout;
using Zen.UIControls.FileUI;

namespace ScreenCapturer
{
    /// <summary> Screen Capture Options </summary>
    public sealed class CaptureOption
    {
        public Point StartPoint
        {
            get { return _topLeft; }
            set { _topLeft = value; }
        }
        public Point EndPoint
        {
            get { return _bottomRight; }
            set { _bottomRight = value; }
        }

        public bool CopyToClipboard
        {
            get { return _copyToClipboard; }
            set { _copyToClipboard = value; }
        }
        public bool OCRCapturedImage
        {
            get { return _ocrCaptured; }
            set { _ocrCaptured = value; }
        }
        [TextBoxHint(BrowserMode = BrowserMode.File)]
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }
        public ImageFmt ImageFormat
        {
            get { return _imageFormat; }
            set { _imageFormat = value; }
        }

        #region private data
        private Point _topLeft;
        private Point _bottomRight;

        private bool _copyToClipboard = true;
        private bool _ocrCaptured = false;
        private string _filePath;
        private ImageFmt _imageFormat = ImageFmt.Tiff;
        #endregion
    }

}
