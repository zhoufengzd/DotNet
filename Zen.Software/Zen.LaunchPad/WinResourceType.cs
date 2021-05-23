
namespace Zen.Utilities.Win32
{
    public sealed class WinResourceType
    {
        public const int RT_CURSOR = 1;
        public const int RT_BITMAP = 2;
        public const int RT_ICON = 3;
        public const int RT_MENU = 4;
        public const int RT_DIALOG = 5;
        public const int RT_STRING = 6;
        public const int RT_FONTDIR = 7;
        public const int RT_FONT = 8;
        public const int RT_ACCELERATOR = 9;
        public const int RT_RCDATA = 10;
        public const int RT_MESSAGETABLE = 11;

        //#define DIFFERENCE 11
        public const int RT_GROUP_CURSOR = 11 + RT_CURSOR;
        public const int RT_GROUP_ICON = 11 + RT_ICON;
    }

    public sealed class WinResourceSize
    {
        public const int IcondirSize = 6;            // sizeof(ICONDIR) 
        public const int IcondirEntrySize = 16;      // sizeof(ICONDIRENTRY)
        public const int GrpIcondirEntrySize = 14;   // sizeof(GRPICONDIRENTRY)
    }
}
