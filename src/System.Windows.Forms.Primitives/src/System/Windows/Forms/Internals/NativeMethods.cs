// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;
using static Interop;

namespace System.Windows.Forms;

internal static class NativeMethods
{
    public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

    public const int WHEEL_DELTA = 120;

    public const int
        TPM_LEFTBUTTON = 0x0000,
        TPM_RIGHTBUTTON = 0x0002,
        TPM_LEFTALIGN = 0x0000,
        TPM_RIGHTALIGN = 0x0008,
        TPM_VERTICAL = 0x0040;

    public const int MEMBERID_NIL = (-1),
        ERROR_INSUFFICIENT_BUFFER = 122, //https://msdn.microsoft.com/en-us/library/windows/desktop/ms681382(v=vs.85).aspx
        MA_ACTIVATE = 0x0001,
        MA_ACTIVATEANDEAT = 0x0002,
        MA_NOACTIVATE = 0x0003,
        MA_NOACTIVATEANDEAT = 0x0004,
        MM_TEXT = 1,
        MM_ANISOTROPIC = 8,
        MNC_EXECUTE = 2,
        MNC_SELECT = 3,
        MIIM_STATE = 0x00000001,
        MIIM_ID = 0x00000002,
        MIIM_SUBMENU = 0x00000004,
        MIIM_TYPE = 0x00000010,
        MIIM_DATA = 0x00000020,
        MIIM_STRING = 0x00000040,
        MIIM_BITMAP = 0x00000080,
        MIIM_FTYPE = 0x00000100,
        MB_OK = 0x00000000,
        MFS_DISABLED = 0x00000003,
        MFT_MENUBREAK = 0x00000040,
        MFT_SEPARATOR = 0x00000800,
        MFT_RIGHTORDER = 0x00002000,
        MFT_RIGHTJUSTIFY = 0x00004000,
        MDIS_ALLCHILDSTYLES = 0x0001,
        MDITILE_VERTICAL = 0x0000,
        MDITILE_HORIZONTAL = 0x0001,
        MDITILE_SKIPDISABLED = 0x0002,
        MCM_SETMAXSELCOUNT = (0x1000 + 4),
        MCM_SETSELRANGE = (0x1000 + 6),
        MCM_GETMONTHRANGE = (0x1000 + 7),
        MCM_GETMINREQRECT = (0x1000 + 9),
        MCM_SETCOLOR = (0x1000 + 10),
        MCM_SETTODAY = (0x1000 + 12),
        MCM_GETTODAY = (0x1000 + 13),
        MCM_HITTEST = (0x1000 + 14),
        MCM_SETFIRSTDAYOFWEEK = (0x1000 + 15),
        MCM_SETRANGE = (0x1000 + 18),
        MCM_SETMONTHDELTA = (0x1000 + 20),
        MCM_GETMAXTODAYWIDTH = (0x1000 + 21),
        MCHT_TITLE = 0x00010000,
        MCHT_CALENDAR = 0x00020000,
        MCHT_TODAYLINK = 0x00030000,
        MCHT_TITLEBK = (0x00010000),
        MCHT_TITLEMONTH = (0x00010000 | 0x0001),
        MCHT_TITLEYEAR = (0x00010000 | 0x0002),
        MCHT_TITLEBTNNEXT = (0x00010000 | 0x01000000 | 0x0003),
        MCHT_TITLEBTNPREV = (0x00010000 | 0x02000000 | 0x0003),
        MCHT_CALENDARBK = (0x00020000),
        MCHT_CALENDARDATE = (0x00020000 | 0x0001),
        MCHT_CALENDARDATENEXT = ((0x00020000 | 0x0001) | 0x01000000),
        MCHT_CALENDARDATEPREV = ((0x00020000 | 0x0001) | 0x02000000),
        MCHT_CALENDARDAY = (0x00020000 | 0x0002),
        MCHT_CALENDARWEEKNUM = (0x00020000 | 0x0003),
        MCSC_TEXT = 1,
        MCSC_TITLEBK = 2,
        MCSC_TITLETEXT = 3,
        MCSC_MONTHBK = 4,
        MCSC_TRAILINGTEXT = 5,
        MCN_VIEWCHANGE = (0 - 750), // MCN_SELECT -4  - give state of calendar view
        MCN_SELCHANGE = ((0 - 750) + 1),
        MCN_GETDAYSTATE = ((0 - 750) + 3),
        MCN_SELECT = ((0 - 750) + 4),
        MCS_DAYSTATE = 0x0001,
        MCS_MULTISELECT = 0x0002,
        MCS_WEEKNUMBERS = 0x0004,
        MCS_NOTODAYCIRCLE = 0x0008,
        MCS_NOTODAY = 0x0010,
        MSAA_MENU_SIG = (unchecked((int)0xAA0DF00D));

    public delegate int ListViewCompareCallback(IntPtr lParam1, IntPtr lParam2, IntPtr lParamSort);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class PRINTDLGEX
    {
        public int lStructSize;

        public IntPtr hwndOwner;
        public IntPtr hDevMode;
        public IntPtr hDevNames;
        public IntPtr hDC;

        public Comdlg32.PD Flags;
        public int Flags2;

        public int ExclusionFlags;

        public int nPageRanges;
        public int nMaxPageRanges;

        public IntPtr pageRanges;

        public int nMinPage;
        public int nMaxPage;
        public int nCopies;

        public IntPtr hInstance;
        [MarshalAs(UnmanagedType.LPStr)]
        public string? lpPrintTemplateName;

        public WndProc? lpCallback;

        public int nPropertyPages;

        public IntPtr lphPropertyPages;

        public int nStartPage;
        public Comdlg32.PD_RESULT dwResultAction;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class TPMPARAMS
    {
        public int cbSize = Marshal.SizeOf<TPMPARAMS>();
        // rcExclude was a by-value RECT structure
        public int rcExclude_left;
        public int rcExclude_top;
        public int rcExclude_right;
        public int rcExclude_bottom;
    }

    public static class ActiveX
    {
        public const int ALIGN_MIN = 0x0;
        public const int ALIGN_NO_CHANGE = 0x0;
        public const int ALIGN_TOP = 0x1;
        public const int ALIGN_BOTTOM = 0x2;
        public const int ALIGN_LEFT = 0x3;
        public const int ALIGN_RIGHT = 0x4;
        public const int ALIGN_MAX = 0x4;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MSAAMENUINFO
    {
        public int dwMSAASignature;
        public int cchWText;
        public string pszWText;

        public MSAAMENUINFO(string text)
        {
            dwMSAASignature = unchecked((int)MSAA_MENU_SIG);
            cchWText = text.Length;
            pszWText = text;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MENUITEMINFO_T
    {
        public int cbSize = Marshal.SizeOf<MENUITEMINFO_T>();
        public int fMask;
        public int fType;
        public int fState;
        public int wID;
        public IntPtr hSubMenu;
        public IntPtr hbmpChecked;
        public IntPtr hbmpUnchecked;
        public IntPtr dwItemData;
        public string dwTypeData;
        public int cch;
    }

    public static class Util
    {
        public static int MAKELONG(int low, int high)
        {
            return (high << 16) | (low & 0xffff);
        }

        public static IntPtr MAKELPARAM(int low, int high)
        {
            return (IntPtr)((high << 16) | (low & 0xffff));
        }

        public static int HIWORD(int n)
        {
            return (n >> 16) & 0xffff;
        }

        public static int HIWORD(IntPtr n)
        {
            return HIWORD(unchecked((int)(long)n));
        }

        public static int LOWORD(int n)
        {
            return n & 0xffff;
        }

        public static int LOWORD(IntPtr n)
        {
            return LOWORD(unchecked((int)(long)n));
        }

        public static int SignedHIWORD(IntPtr n)
        {
            return SignedHIWORD(unchecked((int)(long)n));
        }

        public static int SignedLOWORD(IntPtr n)
        {
            return SignedLOWORD(unchecked((int)(long)n));
        }

        public static int SignedHIWORD(int n)
        {
            int i = (int)(short)((n >> 16) & 0xffff);

            return i;
        }

        public static int SignedLOWORD(int n)
        {
            int i = (int)(short)(n & 0xFFFF);

            return i;
        }

        private static int GetEmbeddedNullStringLengthAnsi(string s)
        {
            int n = s.IndexOf('\0');
            if (n > -1)
            {
                string left = s.Substring(0, n);
                string right = s.Substring(n + 1);
                return left.Length + GetEmbeddedNullStringLengthAnsi(right) + 1;
            }
            else
            {
                return s.Length;
            }
        }
    }
}
