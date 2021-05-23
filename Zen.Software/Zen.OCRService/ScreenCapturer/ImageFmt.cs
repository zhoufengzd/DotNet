using System.Drawing.Imaging;
using Zen.Utilities.Generics;

namespace ScreenCapturer
{
    public enum ImageFmt
    {
        Unspecified = 0,
        Bmp,
        Emf,
        Exif,
        Gif,
        Icon,
        Jpeg,
        Png,
        Tiff,
        Wmf,
    }

    public sealed class ImageFmtMapper
    {
        public static ImageFormat ToImageFormat(ImageFmt fmt)
        {
            return _map.FindT2(fmt);
        }
        public static ImageFmt ToImageFmt(ImageFormat fmt)
        {
            return _map.FindT1(fmt);
        }

        #region static initializer
        static ImageFmtMapper()
        {
            _map = new TwowayMap<ImageFmt, ImageFormat>();
            _map.Add(ImageFmt.Bmp, ImageFormat.Bmp);
            _map.Add(ImageFmt.Emf, ImageFormat.Emf);
            _map.Add(ImageFmt.Exif, ImageFormat.Exif);
            _map.Add(ImageFmt.Gif, ImageFormat.Gif);
            _map.Add(ImageFmt.Icon, ImageFormat.Icon);
            _map.Add(ImageFmt.Jpeg, ImageFormat.Jpeg);
            _map.Add(ImageFmt.Png, ImageFormat.Png);
            _map.Add(ImageFmt.Tiff, ImageFormat.Tiff);
            _map.Add(ImageFmt.Wmf, ImageFormat.Wmf);
        }

        static TwowayMap<ImageFmt, ImageFormat> _map;
        #endregion
    }

}
