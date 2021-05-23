using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using Zen.Utilities.Win32;

namespace Zen.LaunchPad
{
    public class IconExtractor : IDisposable
    {
        #region Win32 interop.

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Auto)]
        private delegate bool EnumResNameProc(IntPtr hModule, int lpszType, IntPtr lpszName, IntPtr lParam);
        private const int LOAD_LIBRARY_AS_DATAFILE = 0x00000002;

        #region API Functions

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, int dwFlags);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool EnumResourceNames(
            IntPtr hModule, int lpszType, EnumResNameProc lpEnumFunc, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, int lpType);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr LockResource(IntPtr hResData);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern int SizeofResource(IntPtr hModule, IntPtr hResInfo);

        #endregion
        #endregion

        #region Managed Types
        private class ResourceName
        {
            public ResourceName(IntPtr lpName)
            {
                if (((uint)lpName >> 16) == 0) // #define IS_INTRESOURCE(_r) ((((ULONG_PTR)(_r)) >> 16) == 0)
                {
                    _id = lpName;
                    _name = null;
                }
                else
                {
                    _id = IntPtr.Zero;
                    _name = Marshal.PtrToStringAuto(lpName);
                }
            }

            public IntPtr Id
            {
                get { return _id; }
                set { _id = value; }
            }

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            public IntPtr GetValue()
            {
                if (_name == null)
                {
                    return _id;
                }
                else
                {
                    _bufPtr = Marshal.StringToHGlobalAuto(_name);
                    return _bufPtr;
                }
            }

            public void Free()
            {
                if (_bufPtr != IntPtr.Zero)
                {
                    try { Marshal.FreeHGlobal(_bufPtr); }
                    catch { }

                    _bufPtr = IntPtr.Zero;
                }
            }

            private IntPtr _id;
            private string _name;
            private IntPtr _bufPtr = IntPtr.Zero;
        }

        #endregion

        public static int GetIconBitDepth(Icon icon)
        {
            byte[] data = null;
            using (MemoryStream stream = new MemoryStream())
            {
                icon.Save(stream);
                data = stream.ToArray();
            }

            return BitConverter.ToInt16(data, 12);
        }   

        /// <summary>
        /// Split an Icon consists of multiple icons into an array of Icon each consist of single icons.
        /// </summary>
        /// <param name="icon">The System.Drawing.Icon to be split.</param>
        /// <returns>An array of System.Drawing.Icon each consist of single icons.</returns>
        public static Icon[] SplitIcon(Icon icon)
        {
            if (icon == null)
            {
                throw new ArgumentNullException("icon");
            }

            // Get multiple .ico file image.
            byte[] srcBuf = null;
            using (MemoryStream stream = new MemoryStream())
            {
                icon.Save(stream);
                srcBuf = stream.ToArray();
            }

            int count = BitConverter.ToInt16(srcBuf, 4); // ICONDIR.idCount
            Icon[] splitIcons = new Icon[count];
            for (int i = 0; i < count; i++)
            {
                using (MemoryStream destStream = new MemoryStream())
                using (BinaryWriter writer = new BinaryWriter(destStream))
                {
                    // Copy ICONDIR and ICONDIRENTRY.
                    writer.Write(srcBuf, 0, WinResourceSize.IcondirSize - 2);
                    writer.Write((short)1);    // ICONDIR.idCount == 1;

                    writer.Write(srcBuf, WinResourceSize.IcondirSize + WinResourceSize.IcondirEntrySize * i, WinResourceSize.IcondirEntrySize - 4);
                    writer.Write(WinResourceSize.IcondirSize + WinResourceSize.IcondirEntrySize);    // ICONDIRENTRY.dwImageOffset = sizeof(ICONDIR) + sizeof(ICONDIRENTRY)

                    // Copy picture and mask data.
                    int imgSize = BitConverter.ToInt32(srcBuf, WinResourceSize.IcondirSize + WinResourceSize.IcondirEntrySize * i + 8);       // ICONDIRENTRY.dwBytesInRes
                    int imgOffset = BitConverter.ToInt32(srcBuf, WinResourceSize.IcondirSize + WinResourceSize.IcondirEntrySize * i + 12);    // ICONDIRENTRY.dwImageOffset
                    writer.Write(srcBuf, imgOffset, imgSize);

                    // Create new icon.
                    destStream.Seek(0, SeekOrigin.Begin);
                    splitIcons[i] = (new Icon(destStream));
                }
            }

            return splitIcons;
        }

        /// <summary>
        /// Load the specified executable file or DLL, and get ready to extract the icons.
        /// </summary>
        /// <param name="filename">The name of a file from which icons will be extracted.</param>
        public IconExtractor(string filename)
        {
            _filename = filename;
        }

        ~IconExtractor()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_hModule != IntPtr.Zero)
            {
                try { FreeLibrary(_hModule); }
                catch { }

                _hModule = IntPtr.Zero;
            }

            if (_iconCache != null)
            {
                foreach (Icon i in _iconCache)
                {
                    if (i != null)
                    {
                        try { i.Dispose(); }
                        catch { }
                    }
                }

                _iconCache = null;
            }
        }

        /// <summary>
        /// Extract an icon from the loaded executable file or DLL. 
        /// </summary>
        /// <param name="iconIndex">The zero-based index of the icon to be extracted.</param>
        /// <returns>A System.Drawing.Icon object which may consists of multiple icons.</returns>
        /// <remarks>Always returns new copy of the Icon. It should be disposed by the user.</remarks>
        public Icon GetIcon(int iconIndex)
        {
            LoadResources();

            if (_iconCache[iconIndex] == null)
            {
                _iconCache[iconIndex] = CreateIcon(iconIndex);
            }

            return (Icon)_iconCache[iconIndex].Clone();
        }

        #region Private Methods
        private void LoadResources()
        {
            if (_hModule != IntPtr.Zero)
                return;

            _hModule = LoadLibraryEx(_filename, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);

            EnumResourceNames(_hModule, WinResourceType.RT_GROUP_ICON, EnumResNameCallBack, IntPtr.Zero);

            // cache all!
            _iconCache = new Icon[_iconIdList.Count];
            string dir = Path.GetFileNameWithoutExtension(_filename);
            Directory.CreateDirectory(dir);
            string iconFilePath = null;
            for (int ind = 0; ind < _iconIdList.Count; ind++)
            {
                Icon icon = CreateIcon(ind);
                _iconCache[ind] = icon;

                iconFilePath = string.Format(@"{0}\{1}.ico", dir, ind);
                icon.Save(new FileStream(iconFilePath, FileMode.Create, FileAccess.Write, FileShare.None));
            }
        }

        private bool EnumResNameCallBack(IntPtr hModule, int lpszType, IntPtr lpszName, IntPtr lParam)
        {
            if (lpszType == WinResourceType.RT_GROUP_ICON)
            {
                _iconIdList.Add(new ResourceName(lpszName));
            }
            return true;
        }

        private Icon CreateIcon(int iconIndex)
        {
            // Get group icon resource.
            byte[] srcBuf = GetResourceData(_hModule, _iconIdList[iconIndex], WinResourceType.RT_GROUP_ICON);

            // Convert the resouce into an .ico file image.
            using (MemoryStream destStream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(destStream))
            {
                int count = BitConverter.ToUInt16(srcBuf, 4); // ICONDIR.idCount
                int imgOffset = WinResourceSize.IcondirSize + WinResourceSize.IcondirEntrySize * count;

                // Copy ICONDIR.
                writer.Write(srcBuf, 0, WinResourceSize.IcondirSize);

                for (int i = 0; i < count; i++)
                {
                    // Copy GRPICONDIRENTRY converting into ICONDIRENTRY.
                    writer.BaseStream.Seek(WinResourceSize.IcondirSize + WinResourceSize.IcondirEntrySize * i, SeekOrigin.Begin);
                    writer.Write(srcBuf, WinResourceSize.IcondirSize + WinResourceSize.GrpIcondirEntrySize * i, WinResourceSize.IcondirEntrySize - 4);   // Common fields of structures
                    writer.Write(imgOffset);                                                    // ICONDIRENTRY.dwImageOffset

                    // Get picture and mask data, then copy them.
                    IntPtr nID = (IntPtr)BitConverter.ToUInt16(srcBuf, WinResourceSize.IcondirSize + WinResourceSize.GrpIcondirEntrySize * i + 12); // GRPICONDIRENTRY.nID
                    byte[] imgBuf = GetResourceData(_hModule, nID, WinResourceType.RT_ICON);

                    writer.BaseStream.Seek(imgOffset, SeekOrigin.Begin);
                    writer.Write(imgBuf, 0, imgBuf.Length);

                    imgOffset += imgBuf.Length;
                }

                destStream.Seek(0, SeekOrigin.Begin);
                return new Icon(destStream);
            }
        }

        private byte[] GetResourceData(IntPtr hModule, IntPtr lpName, int lpType)
        {
            // Get binary image of the specified resource.
            IntPtr hResInfo = FindResource(hModule, lpName, lpType);
            if (hResInfo == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            IntPtr hResData = LoadResource(hModule, hResInfo);
            if (hResData == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            IntPtr hGlobal = LockResource(hResData);
            if (hGlobal == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            int resSize = SizeofResource(hModule, hResInfo);
            if (resSize == 0)
            {
                throw new Win32Exception();
            }

            byte[] buf = new byte[resSize];
            Marshal.Copy(hGlobal, buf, 0, buf.Length);

            return buf;
        }

        private byte[] GetResourceData(IntPtr hModule, ResourceName name, int lpType)
        {
            try
            {
                IntPtr lpName = name.GetValue();
                return GetResourceData(hModule, lpName, lpType);
            }
            finally
            {
                name.Free();
            }
        }

        #endregion

        #region Private Fields
        private IntPtr _hModule = IntPtr.Zero;
        private List<ResourceName> _iconIdList = new List<ResourceName>();

        private Icon[] _iconCache = null;
        private string _filename = null;
        #endregion
    }
}
