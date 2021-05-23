using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using ModiLib;

namespace ScreenCapturer
{
    public sealed class Capturer
    {
        public static void DoWork(CaptureOption option)
        {
            //    Point StartPoint = new Point(_topLeft.X, _topLeft.Y);
            //    Rectangle bounds = new Rectangle(_topLeft.X, _topLeft.Y, _bottomRight.X - _topLeft.X, _bottomRight.Y - _topLeft.Y);
            //    ScreenShot.CaptureImage(StartPoint, Point.Empty, bounds, _screenShotFilePath);
            Size area = new Size(option.EndPoint.X - option.StartPoint.X, option.EndPoint.Y - option.StartPoint.Y);
            using (Bitmap screenBmpTmp = new Bitmap(area.Width, area.Height))
            {
                using (Graphics g = Graphics.FromImage(screenBmpTmp))
                {
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                    g.CopyFromScreen(option.StartPoint, Point.Empty, area);
                }

                if (option.CopyToClipboard)
                    System.Windows.Forms.Clipboard.SetDataObject(screenBmpTmp, true);

                ImageFormat fmt = ImageFmtMapper.ToImageFormat(option.ImageFormat);
                string scrFilePath = option.FilePath;
                if (!string.IsNullOrEmpty(scrFilePath))
                {
                    if (string.Compare(fmt.ToString(), Path.GetExtension(scrFilePath), true) != 0)
                        scrFilePath = scrFilePath + "." + fmt.ToString();

                    screenBmpTmp.Save(scrFilePath, fmt);

                    if (option.OCRCapturedImage)
                    {
                        if (fmt != ImageFormat.Tiff)
                        {
                            scrFilePath = Path.GetTempFileName() + ".tif";
                            screenBmpTmp.Save(scrFilePath, ImageFormat.Tiff);
                        }

                        OcrEngine ocrEngine = new OcrEngine();
                        ocrEngine.Process(scrFilePath, Path.ChangeExtension(scrFilePath, ".txt"));
                    }
                }
            }

        }
    }
}