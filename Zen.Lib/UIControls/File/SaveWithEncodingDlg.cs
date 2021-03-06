using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;

namespace Zen.UIControls.Encoding
{
    public enum EncodingType
    {
        UTF8 = 0,
        UTF8WithPreamble,
        Unicode,
        Ansi
    }

    #region private struct and constant
    delegate int OFNHookProcDelegate(int hdlg, int msg, int wParam, int lParam);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    struct OpenFileName
    {
        public int lStructSize;
        public IntPtr hwndOwner;
        public int hInstance;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpstrFilter;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpstrCustomFilter;
        public int nMaxCustFilter;
        public int nFilterIndex;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpstrFile;
        public int nMaxFile;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpstrFileTitle;
        public int nMaxFileTitle;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpstrInitialDir;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpstrTitle;
        public int Flags;
        public short nFileOffset;
        public short nFileExtension;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpstrDefExt;
        public int lCustData;
        public OFNHookProcDelegate lpfnHook;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpTemplateName;
        //only if on nt 5.0 or higher
        public int pvReserved;
        public int dwReserved;
        public int FlagsEx;
    }

    struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    struct Point
    {
        public int X;
        public int Y;
    }

    struct NMHDR
    {
        public int _HwndFrom;
        public int _IdFrom;
        public int _Code;

        public NMHDR(int HwndFrom, int IdFrom, int Code)
        {
            _HwndFrom = HwndFrom;
            _IdFrom   = IdFrom;
            _Code     = Code;
        }
    }

    #endregion


    class SaveWithEncodingDlg
    {
        private const int OFN_ENABLEHOOK = 0x00000020;
        private const int OFN_EXPLORER = 0x00080000;
        private const int OFN_FILEMUSTEXIST = 0x00001000;
        private const int OFN_HIDEREADONLY = 0x00000004;
        private const int OFN_CREATEPROMPT = 0x00002000;
        private const int OFN_NOTESTFILECREATE = 0x00010000;
        private const int OFN_OVERWRITEPROMPT = 0x00000002;
        private const int OFN_PATHMUSTEXIST = 0x00000800;

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;

        private const int WM_INITDIALOG = 0x110;
        private const int WM_DESTROY = 0x2;
        private const int WM_SETFONT = 0x0030;
        private const int WM_GETFONT = 0x0031;

        private const int CBS_DROPDOWNLIST = 0x0003;
        private const int CBS_HASSTRINGS = 0x0200;
        private const int CB_ADDSTRING = 0x0143;
        private const int CB_SETCURSEL = 0x014E;
        private const int CB_GETCURSEL = 0x0147;

        private const uint WS_VISIBLE = 0x10000000;
        private const uint WS_CHILD = 0x40000000;
        private const uint WS_TABSTOP = 0x00010000;

        private const int CDN_FILEOK = -606;
        private const int WM_NOTIFY = 0x004E;

        [DllImport("Comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetSaveFileName(ref OpenFileName lpofn);

        [DllImport("Comdlg32.dll")]
        private static extern int CommDlgExtendedError();

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(int hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(int hWnd, ref Rect lpRect);

        [DllImport("user32.dll")]
        private static extern int GetParent(int hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SetWindowText(int hWnd, string lpString);

        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(int hWnd, int Msg, int wParam, string lParam);

        [DllImport("user32.dll")]
        private static extern bool DestroyWindow(int hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetDlgItem(int hDlg, int nIDDlgItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int CreateWindowEx(int dwExStyle, string lpClassName, string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight, int hWndParent, int hMenu, int hInstance, int lpParam);

        [DllImport("user32.dll")]
        private static extern bool ScreenToClient(int hWnd, ref Point lpPoint);

        #region public functions
        public SaveWithEncodingDlg()
        {
        }

        ~SaveWithEncodingDlg()
        {
        }

        public string DefaultExt
        {
            get
            {
                return m_DefaultExt;
            }
            set
            {
                m_DefaultExt = value;
            }
        }

        public string Filter
        {
            get
            {
                return m_Filter;
            }
            set
            {
                m_Filter = value;
            }
        }

        public string FileName
        {
            get
            {
                return m_FileName;
            }
            set
            {
                m_FileName = value;
            }
        }

        public EncodingType EncodingType
        {
            get
            {
                return m_EncodingType;
            }
            set
            {
                m_EncodingType = value;
            }
        }

        public DialogResult ShowDialog()
        {
            //set up the struct and populate it

            OpenFileName ofn = new OpenFileName();

            ofn.lStructSize = Marshal.SizeOf(ofn);
            ofn.lpstrFilter = m_Filter.Replace('|', '\0') + '\0';

            ofn.lpstrFile = m_FileName + new string(' ', 512);
            ofn.nMaxFile = ofn.lpstrFile.Length;
            ofn.lpstrFileTitle = System.IO.Path.GetFileName(m_FileName) + new string(' ', 512);
            ofn.nMaxFileTitle = ofn.lpstrFileTitle.Length;
            ofn.lpstrTitle = "Save file as";
            ofn.lpstrDefExt = m_DefaultExt;

            //position the dialog above the active window
            ofn.hwndOwner = Form.ActiveForm.Handle;

            //we need to find out the active screen so the dialog box is
            //centred on the correct display

            m_ActiveScreen = Screen.FromControl(Form.ActiveForm);

            //set up some sensible flags
            ofn.Flags = OFN_EXPLORER | OFN_PATHMUSTEXIST | OFN_NOTESTFILECREATE | OFN_ENABLEHOOK | OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT;

            //this is where the hook is set. Note that we can use a C# delegate in place of a C function pointer
            ofn.lpfnHook = new OFNHookProcDelegate(HookProc);

            //if we're running on Windows 98/ME then the struct is smaller
            if (System.Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                ofn.lStructSize -= 12;
            }

            //show the dialog

            if (!GetSaveFileName(ref ofn))
            {
                int ret = CommDlgExtendedError();

                if (ret != 0)
                {
                    throw new ApplicationException("Couldn't show file open dialog - " + ret.ToString());
                }

                return DialogResult.Cancel;
            }

            m_FileName = ofn.lpstrFile;

            return DialogResult.OK;
        }

        #endregion

        #region protected functions
        #endregion

        #region private functions
        private int HookProc(int hdlg, int msg, int wParam, int lParam)
        {
            switch (msg)
            {
                case WM_INITDIALOG:

                    //we need to centre the dialog
                    Rectangle sr = m_ActiveScreen.Bounds;
                    Rect cr = new Rect();
                    int parent = GetParent(hdlg);
                    GetWindowRect(parent, ref cr);

                    int x = (sr.Right + sr.Left - (cr.Right - cr.Left)) / 2;
                    int y = (sr.Bottom + sr.Top - (cr.Bottom - cr.Top)) / 2;

                    SetWindowPos(parent, 0, x, y, cr.Right - cr.Left, cr.Bottom - cr.Top + 32, SWP_NOZORDER);


                    //we need to find the label to position our new label under

                    int fileTypeWindow = GetDlgItem(parent, 0x441);

                    Rect aboveRect = new Rect();
                    GetWindowRect(fileTypeWindow, ref aboveRect);

                    //now convert the label's screen co-ordinates to client co-ordinates
                    Point point = new Point();
                    point.X = aboveRect.Left;
                    point.Y = aboveRect.Bottom;

                    ScreenToClient(parent, ref point);

                    //create the label
                    int labelHandle = CreateWindowEx(0, "STATIC", "mylabel", WS_VISIBLE | WS_CHILD | WS_TABSTOP, point.X, point.Y + 12, 200, 100, parent, 0, 0, 0);
                    SetWindowText(labelHandle, "&Encoding:");

                    int fontHandle = SendMessage(fileTypeWindow, WM_GETFONT, 0, 0);
                    SendMessage(labelHandle, WM_SETFONT, fontHandle, 0);

                    //we now need to find the combo-box to position the new combo-box under

                    int fileComboWindow = GetDlgItem(parent, 0x470);
                    aboveRect = new Rect();
                    GetWindowRect(fileComboWindow, ref aboveRect);

                    point = new Point();
                    point.X = aboveRect.Left;
                    point.Y = aboveRect.Bottom;
                    ScreenToClient(parent, ref point);

                    Point rightPoint = new Point();
                    rightPoint.X = aboveRect.Right;
                    rightPoint.Y = aboveRect.Top;

                    ScreenToClient(parent, ref rightPoint);

                    //we create the new combobox

                    int comboHandle = CreateWindowEx(0, "ComboBox", "mycombobox", WS_VISIBLE | WS_CHILD | CBS_HASSTRINGS | CBS_DROPDOWNLIST | WS_TABSTOP, point.X, point.Y + 8, rightPoint.X - point.X, 100, parent, 0, 0, 0);
                    SendMessage(comboHandle, WM_SETFONT, fontHandle, 0);

                    //and add the encodings we want to offer
                    SendMessage(comboHandle, CB_ADDSTRING, 0, "UTF-8");
                    SendMessage(comboHandle, CB_ADDSTRING, 0, "UTF-8 with preamble");
                    SendMessage(comboHandle, CB_ADDSTRING, 0, "Unicode");
                    SendMessage(comboHandle, CB_ADDSTRING, 0, "ANSI");

                    SendMessage(comboHandle, CB_SETCURSEL, (int)m_EncodingType, 0);

                    //remember the handles of the controls we have created so we can destroy them after
                    m_LabelHandle = labelHandle;
                    m_ComboHandle = comboHandle;

                    break;
                case WM_DESTROY:
                    //destroy the handles we have created
                    if (m_ComboHandle != 0)
                    {
                        DestroyWindow(m_ComboHandle);
                    }

                    if (m_LabelHandle != 0)
                    {
                        DestroyWindow(m_LabelHandle);
                    }
                    break;
                case WM_NOTIFY:

                    //we need to intercept the CDN_FILEOK message
                    //which is sent when the user selects a filename
                    Object nmhdr = Marshal.PtrToStructure(new IntPtr(lParam), typeof(NMHDR));

                    if (((NMHDR)nmhdr)._Code == CDN_FILEOK)
                    {
                        //a file has been selected
                        //we need to get the encoding

                        m_EncodingType = (EncodingType)SendMessage(m_ComboHandle, CB_GETCURSEL, 0, 0);
                    }
                    break;

            }
            return 0;
        }

        #endregion

        #region private members
        // controller
        private Screen m_ActiveScreen;

        // dynamic status 
        private int m_LabelHandle = 0;
        private int m_ComboHandle = 0;

        private string m_Filter = "";
        private string m_DefaultExt = "";

        private string m_FileName = "";

        private EncodingType m_EncodingType;
        #endregion
    }
}


