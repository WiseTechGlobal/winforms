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

    public const string WC_DATETIMEPICK = "SysDateTimePick32",
        WC_LISTVIEW = "SysListView32",
        WC_MONTHCAL = "SysMonthCal32",
        WC_PROGRESS = "msctls_progress32",
        WC_STATUSBAR = "msctls_statusbar32",
        WC_TOOLBAR = "ToolbarWindow32",
        WC_TRACKBAR = "msctls_trackbar32",
        WC_TREEVIEW = "SysTreeView32",
        WC_TABCONTROL = "SysTabControl32",
        MSH_MOUSEWHEEL = "MSWHEEL_ROLLMSG",
        MSH_SCROLL_LINES = "MSH_SCROLL_LINES_MSG";

    public const int CF_TEXT = 1,
        CF_BITMAP = 2,
        CF_METAFILEPICT = 3,
        CF_SYLK = 4,
        CF_DIF = 5,
        CF_TIFF = 6,
        CF_OEMTEXT = 7,
        CF_DIB = 8,
        CF_PALETTE = 9,
        CF_PENDATA = 10,
        CF_RIFF = 11,
        CF_WAVE = 12,
        CF_UNICODETEXT = 13,
        CF_ENHMETAFILE = 14,
        CF_HDROP = 15,
        CF_LOCALE = 16,
        CLSCTX_INPROC_SERVER = 0x1,
        CLSCTX_LOCAL_SERVER = 0x4,
        CW_USEDEFAULT = (unchecked((int)0x80000000)),
        CWP_SKIPINVISIBLE = 0x0001,
        COLOR_WINDOW = 5,
        CB_ERR = (-1),
        CBN_SELCHANGE = 1,
        CBN_DBLCLK = 2,
        CBN_EDITCHANGE = 5,
        CBN_EDITUPDATE = 6,
        CBN_DROPDOWN = 7,
        CBN_CLOSEUP = 8,
        CBN_SELENDOK = 9,
        CBS_SIMPLE = 0x0001,
        CBS_DROPDOWN = 0x0002,
        CBS_DROPDOWNLIST = 0x0003,
        CBS_OWNERDRAWFIXED = 0x0010,
        CBS_OWNERDRAWVARIABLE = 0x0020,
        CBS_AUTOHSCROLL = 0x0040,
        CBS_HASSTRINGS = 0x0200,
        CBS_NOINTEGRALHEIGHT = 0x0400,
        CB_GETEDITSEL = 0x0140,
        CB_LIMITTEXT = 0x0141,
        CB_SETEDITSEL = 0x0142,
        CB_ADDSTRING = 0x0143,
        CB_DELETESTRING = 0x0144,
        CB_GETCURSEL = 0x0147,
        CB_GETLBTEXT = 0x0148,
        CB_GETLBTEXTLEN = 0x0149,
        CB_INSERTSTRING = 0x014A,
        CB_RESETCONTENT = 0x014B,
        CB_FINDSTRING = 0x014C,
        CB_SETCURSEL = 0x014E,
        CB_SHOWDROPDOWN = 0x014F,
        CB_GETITEMDATA = 0x0150,
        CB_SETITEMHEIGHT = 0x0153,
        CB_GETITEMHEIGHT = 0x0154,
        CB_GETDROPPEDSTATE = 0x0157,
        CB_GETTOPINDEX = 0x015b,
        CB_SETTOPINDEX = 0x015c,
        CB_FINDSTRINGEXACT = 0x0158,
        CB_GETDROPPEDWIDTH = 0x015F,
        CB_SETDROPPEDWIDTH = 0x0160,
        CDRF_DODEFAULT = 0x00000000,
        CDRF_NEWFONT = 0x00000002,
        CDRF_SKIPDEFAULT = 0x00000004,
        CDRF_NOTIFYPOSTPAINT = 0x00000010,
        CDRF_NOTIFYITEMDRAW = 0x00000020,
        CDRF_NOTIFYSUBITEMDRAW = CDRF_NOTIFYITEMDRAW,
        CDDS_PREPAINT = 0x00000001,
        CDDS_POSTPAINT = 0x00000002,
        CDDS_ITEM = 0x00010000,
        CDDS_SUBITEM = 0x00020000,
        CDDS_ITEMPREPAINT = (0x00010000 | 0x00000001),
        CDDS_ITEMPOSTPAINT = (0x00010000 | 0x00000002),
        CDIS_SELECTED = 0x0001,
        CDIS_GRAYED = 0x0002,
        CDIS_DISABLED = 0x0004,
        CDIS_CHECKED = 0x0008,
        CDIS_FOCUS = 0x0010,
        CDIS_DEFAULT = 0x0020,
        CDIS_HOT = 0x0040,
        CDIS_MARKED = 0x0080,
        CDIS_INDETERMINATE = 0x0100,
        CDIS_SHOWKEYBOARDCUES = 0x0200,
        CLR_NONE = unchecked((int)0xFFFFFFFF),
        CLR_DEFAULT = unchecked((int)0xFF000000),
        CCM_SETVERSION = (0x2000 + 0x7),
        CCM_GETVERSION = (0x2000 + 0x8),
        CCS_NORESIZE = 0x00000004,
        CCS_NOPARENTALIGN = 0x00000008,
        CCS_NODIVIDER = 0x00000040,
        CBEM_INSERTITEM = (0x0400 + 11),
        CBEM_SETITEM = (0x0400 + 12),
        CBEM_GETITEM = (0x0400 + 13),
        CBEN_ENDEDIT = ((0 - 800) - 6),
        CONNECT_E_NOCONNECTION = unchecked((int)0x80040200),
        CONNECT_E_CANNOTCONNECT = unchecked((int)0x80040202),
        CTRLINFO_EATS_RETURN = 1,
        CTRLINFO_EATS_ESCAPE = 2;

    public const int SW_SCROLLCHILDREN = 0x0001,
        SW_INVALIDATE = 0x0002,
        SW_ERASE = 0x0004,
        SW_SMOOTHSCROLL = 0x0010,
        SC_SIZE = 0xF000,
        SC_MINIMIZE = 0xF020,
        SC_MAXIMIZE = 0xF030,
        SC_CLOSE = 0xF060,
        SC_KEYMENU = 0xF100,
        SC_RESTORE = 0xF120,
        SC_MOVE = 0xF010,
        SC_CONTEXTHELP = 0xF180,
        SS_LEFT = 0x00000000,
        SS_CENTER = 0x00000001,
        SS_RIGHT = 0x00000002,
        SS_OWNERDRAW = 0x0000000D,
        SS_NOPREFIX = 0x00000080,
        SS_SUNKEN = 0x00001000,
        SBS_HORZ = 0x0000,
        SBS_VERT = 0x0001,
        SIF_RANGE = 0x0001,
        SIF_PAGE = 0x0002,
        SIF_POS = 0x0004,
        SIF_TRACKPOS = 0x0010,
        SIF_ALL = (0x0001 | 0x0002 | 0x0004 | 0x0010),
        SPI_GETFONTSMOOTHING = 0x004A,
        SPI_GETDROPSHADOW = 0x1024,
        SPI_GETFLATMENU = 0x1022,
        SPI_GETFONTSMOOTHINGTYPE = 0x200A,
        SPI_GETFONTSMOOTHINGCONTRAST = 0x200C,
        SPI_ICONHORIZONTALSPACING = 0x000D,
        SPI_ICONVERTICALSPACING = 0x0018,
        // SPI_GETICONMETRICS =        0x002D,
        SPI_GETICONTITLEWRAP = 0x0019,
        SPI_GETKEYBOARDCUES = 0x100A,
        SPI_GETKEYBOARDDELAY = 0x0016,
        SPI_GETKEYBOARDPREF = 0x0044,
        SPI_GETKEYBOARDSPEED = 0x000A,
        SPI_GETMOUSEHOVERWIDTH = 0x0062,
        SPI_GETMOUSEHOVERHEIGHT = 0x0064,
        SPI_GETMOUSEHOVERTIME = 0x0066,
        SPI_GETMOUSESPEED = 0x0070,
        SPI_GETMENUDROPALIGNMENT = 0x001B,
        SPI_GETMENUFADE = 0x1012,
        SPI_GETMENUSHOWDELAY = 0x006A,
        SPI_GETCOMBOBOXANIMATION = 0x1004,
        SPI_GETGRADIENTCAPTIONS = 0x1008,
        SPI_GETHOTTRACKING = 0x100E,
        SPI_GETLISTBOXSMOOTHSCROLLING = 0x1006,
        SPI_GETMENUANIMATION = 0x1002,
        SPI_GETSELECTIONFADE = 0x1014,
        SPI_GETTOOLTIPANIMATION = 0x1016,
        SPI_GETUIEFFECTS = 0x103E,
        SPI_GETACTIVEWINDOWTRACKING = 0x1000,
        SPI_GETACTIVEWNDTRKTIMEOUT = 0x2002,
        SPI_GETANIMATION = 0x0048,
        SPI_GETBORDER = 0x0005,
        SPI_GETCARETWIDTH = 0x2006,
        SPI_GETDRAGFULLWINDOWS = 38,
        SPI_GETNONCLIENTMETRICS = 41,
        SPI_GETWORKAREA = 48,
        SPI_GETHIGHCONTRAST = 66,
        SPI_GETDEFAULTINPUTLANG = 89,
        SPI_GETSNAPTODEFBUTTON = 95,
        SPI_GETWHEELSCROLLLINES = 104,
        SBARS_SIZEGRIP = 0x0100,
        SB_SETTEXT = (0x0400 + 11),
        SB_GETTEXT = (0x0400 + 13),
        SB_GETTEXTLENGTH = (0x0400 + 12),
        SB_SETPARTS = (0x0400 + 4),
        SB_SIMPLE = (0x0400 + 9),
        SB_GETRECT = (0x0400 + 10),
        SB_SETICON = (0x0400 + 15),
        SB_SETTIPTEXT = (0x0400 + 17),
        SB_GETTIPTEXT = (0x0400 + 19),
        SBT_OWNERDRAW = 0x1000,
        SBT_NOBORDERS = 0x0100,
        SBT_POPOUT = 0x0200,
        SBT_RTLREADING = 0x0400,
        SRCCOPY = 0x00CC0020;

    public static HandleRef HWND_TOPMOST = new HandleRef(null, new IntPtr(-1));

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

    public const int IME_CMODE_NATIVE = 0x0001,
        IME_CMODE_KATAKANA = 0x0002,
        IME_CMODE_FULLSHAPE = 0x0008,
        INPLACE_E_NOTOOLSPACE = unchecked((int)0x800401A1),
        ICON_SMALL = 0,
        ICON_BIG = 1,
        IMAGE_ICON = 1,
        IMAGE_CURSOR = 2,
        ICC_LISTVIEW_CLASSES = 0x00000001,
        ICC_TREEVIEW_CLASSES = 0x00000002,
        ICC_BAR_CLASSES = 0x00000004,
        ICC_TAB_CLASSES = 0x00000008,
        ICC_PROGRESS_CLASS = 0x00000020,
        ICC_DATE_CLASSES = 0x00000100,
        ILC_MASK = 0x0001,
        ILC_COLOR = 0x0000,
        ILC_COLOR4 = 0x0004,
        ILC_COLOR8 = 0x0008,
        ILC_COLOR16 = 0x0010,
        ILC_COLOR24 = 0x0018,
        ILC_COLOR32 = 0x0020,
        ILC_MIRROR = 0x00002000,
        ILD_NORMAL = 0x0000,
        ILD_TRANSPARENT = 0x0001,
        ILD_MASK = 0x0010,
        ILD_ROP = 0x0040,

// ImageList
//
        ILP_NORMAL = 0,
        ILP_DOWNLEVEL = 1,
        ILS_NORMAL = 0x0,
        ILS_GLOW = 0x1,
        ILS_SHADOW = 0x2,
        ILS_SATURATE = 0x4,
        ILS_ALPHA = 0x8;

    [StructLayout(LayoutKind.Sequential)]
    public class DRAWITEMSTRUCT
    {
        public int CtlType = 0;
        public int CtlID = 0;
        public int itemID = 0;
        public int itemAction = 0;
        public int itemState = 0;
        public IntPtr hwndItem = IntPtr.Zero;
        public IntPtr hDC = IntPtr.Zero;
        public RECT rcItem;
        public IntPtr itemData = IntPtr.Zero;
    }

    public const int NIM_ADD = 0x00000000,
        NIM_MODIFY = 0x00000001,
        NIM_DELETE = 0x00000002,
        NIF_MESSAGE = 0x00000001,
        NIM_SETVERSION = 0x00000004,
        NIF_ICON = 0x00000002,
        NIF_INFO = 0x00000010,
        NIF_TIP = 0x00000004,
        NIIF_NONE = 0x00000000,
        NIIF_INFO = 0x00000001,
        NIIF_WARNING = 0x00000002,
        NIIF_ERROR = 0x00000003,
        NIN_BALLOONSHOW = (Interop.WindowMessages.WM_USER + 2),
        NIN_BALLOONHIDE = (Interop.WindowMessages.WM_USER + 3),
        NIN_BALLOONTIMEOUT = (Interop.WindowMessages.WM_USER + 4),
        NIN_BALLOONUSERCLICK = (Interop.WindowMessages.WM_USER + 5),
        NFR_ANSI = 1,
        NFR_UNICODE = 2,
        NM_CLICK = ((0 - 0) - 2),
        NM_DBLCLK = ((0 - 0) - 3),
        NM_RCLICK = ((0 - 0) - 5),
        NM_RDBLCLK = ((0 - 0) - 6),
        NM_CUSTOMDRAW = ((0 - 0) - 12),
        NM_RELEASEDCAPTURE = ((0 - 0) - 16);

    public const int
        HC_ACTION = 0,
        HC_GETNEXT = 1,
        HC_SKIP = 2,
        HTTRANSPARENT = (-1),
        HTNOWHERE = 0,
        HTCLIENT = 1,
        HTLEFT = 10,
        HTBOTTOM = 15,
        HTBOTTOMLEFT = 16,
        HTBOTTOMRIGHT = 17,
        HTBORDER = 18,
        HELPINFO_WINDOW = 0x0001,
        HCF_HIGHCONTRASTON = 0x00000001,
        HDI_ORDER = 0x0080,
        HDI_WIDTH = 0x0001,
        HDM_GETITEMCOUNT = (0x1200 + 0),
        HDM_INSERTITEMW = (0x1200 + 10),
        HDM_GETITEMW = (0x1200 + 11),
        HDM_LAYOUT = (0x1200 + 5),
        HDM_SETITEMW = (0x1200 + 12),
        HDN_ITEMCHANGING = ((0 - 300) - 20),
        HDN_ITEMCHANGED = ((0 - 300) - 21),
        HDN_ITEMCLICK = ((0 - 300) - 22),
        HDN_ITEMDBLCLICK = ((0 - 300) - 23),
        HDN_DIVIDERDBLCLICK = ((0 - 300) - 25),
        HDN_BEGINTDRAG = ((0 - 300) - 10),
        HDN_BEGINTRACK = ((0 - 300) - 26),
        HDN_ENDDRAG = ((0 - 300) - 11),
        HDN_ENDTRACK = ((0 - 300) - 27),
        HDN_TRACK = ((0 - 300) - 28),
        HDN_GETDISPINFO = ((0 - 300) - 29);
    // HOVER_DEFAULT = Do not use this value ever! It crashes entire servers.

    public const string TOOLTIPS_CLASS = "tooltips_class32";

    public const int stc4 = 0x0443,
        STARTF_USESHOWWINDOW = 0x00000001,
        SB_HORZ = 0,
        SB_VERT = 1,
        SB_CTL = 2,
        SB_LINEUP = 0,
        SB_LINELEFT = 0,
        SB_LINEDOWN = 1,
        SB_LINERIGHT = 1,
        SB_PAGEUP = 2,
        SB_PAGELEFT = 2,
        SB_PAGEDOWN = 3,
        SB_PAGERIGHT = 3,
        SB_THUMBPOSITION = 4,
        SB_THUMBTRACK = 5,
        SB_LEFT = 6,
        SB_RIGHT = 7,
        SB_ENDSCROLL = 8,
        SB_TOP = 6,
        SB_BOTTOM = 7,
        SIZE_RESTORED = 0,
        SIZE_MAXIMIZED = 2,
        ESB_ENABLE_BOTH = 0x0000,
        ESB_DISABLE_BOTH = 0x0003,
        SORT_DEFAULT = 0x0,
        SUBLANG_DEFAULT = 0x01,
        SW_HIDE = 0,
        SW_NORMAL = 1,
        SW_SHOWMINIMIZED = 2,
        SW_SHOWMAXIMIZED = 3,
        SW_MAXIMIZE = 3,
        SW_SHOWNOACTIVATE = 4,
        SW_SHOW = 5,
        SW_MINIMIZE = 6,
        SW_SHOWMINNOACTIVE = 7,
        SW_SHOWNA = 8,
        SW_RESTORE = 9,
        SW_MAX = 10,
        SWP_NOSIZE = 0x0001,
        SWP_NOMOVE = 0x0002,
        SWP_NOZORDER = 0x0004,
        SWP_NOACTIVATE = 0x0010,
        SWP_SHOWWINDOW = 0x0040,
        SWP_HIDEWINDOW = 0x0080,
        SWP_DRAWFRAME = 0x0020,
        SWP_NOOWNERZORDER = 0x0200;


    public const int TRANSPARENT = 1,
        OPAQUE = 2,
        TME_HOVER = 0x00000001,
        TME_LEAVE = 0x00000002,
        TPM_LEFTBUTTON = 0x0000,
        TPM_RIGHTBUTTON = 0x0002,
        TPM_LEFTALIGN = 0x0000,
        TPM_RIGHTALIGN = 0x0008,
        TPM_VERTICAL = 0x0040,
        TV_FIRST = 0x1100,
        TBSTATE_CHECKED = 0x01,
        TBSTATE_ENABLED = 0x04,
        TBSTATE_HIDDEN = 0x08,
        TBSTATE_INDETERMINATE = 0x10,
        TBSTYLE_BUTTON = 0x00,
        TBSTYLE_SEP = 0x01,
        TBSTYLE_CHECK = 0x02,
        TBSTYLE_DROPDOWN = 0x08,
        TBSTYLE_TOOLTIPS = 0x0100,
        TBSTYLE_FLAT = 0x0800,
        TBSTYLE_LIST = 0x1000,
        TBSTYLE_EX_DRAWDDARROWS = 0x00000001,
        TB_ENABLEBUTTON = (0x0400 + 1),
        TB_ISBUTTONCHECKED = (0x0400 + 10),
        TB_ISBUTTONINDETERMINATE = (0x0400 + 13),
        TB_ADDBUTTONS = (0x0400 + 68),
        TB_INSERTBUTTON = (0x0400 + 67),
        TB_DELETEBUTTON = (0x0400 + 22),
        TB_GETBUTTON = (0x0400 + 23),
        TB_SAVERESTORE = (0x0400 + 76),
        TB_ADDSTRING = (0x0400 + 77),
        TB_BUTTONSTRUCTSIZE = (0x0400 + 30),
        TB_SETBUTTONSIZE = (0x0400 + 31),
        TB_AUTOSIZE = (0x0400 + 33),
        TB_GETROWS = (0x0400 + 40),
        TB_GETBUTTONTEXT = (0x0400 + 75),
        TB_SETIMAGELIST = (0x0400 + 48),
        TB_GETRECT = (0x0400 + 51),
        TB_GETBUTTONSIZE = (0x0400 + 58),
        TB_GETBUTTONINFO = (0x0400 + 63),
        TB_SETBUTTONINFO = (0x0400 + 64),
        TB_SETEXTENDEDSTYLE = (0x0400 + 84),
        TB_MAPACCELERATOR = (0x0400 + 90),
        TB_GETTOOLTIPS = (0x0400 + 35),
        TB_SETTOOLTIPS = (0x0400 + 36),
        TBIF_IMAGE = 0x00000001,
        TBIF_TEXT = 0x00000002,
        TBIF_STATE = 0x00000004,
        TBIF_STYLE = 0x00000008,
        TBIF_COMMAND = 0x00000020,
        TBIF_SIZE = 0x00000040,
        TBN_GETBUTTONINFO = ((0 - 700) - 20),
        TBN_QUERYINSERT = ((0 - 700) - 6),
        TBN_DROPDOWN = ((0 - 700) - 10),
        TBN_HOTITEMCHANGE = ((0 - 700) - 13),
        TBN_GETDISPINFO = ((0 - 700) - 17),
        TBN_GETINFOTIP = ((0 - 700) - 19),
        TTS_ALWAYSTIP = 0x01,
        TTS_NOPREFIX = 0x02,
        TTS_NOANIMATE = 0x10,
        TTS_NOFADE = 0x20,
        TTS_BALLOON = 0x40,
        //TTI_NONE                =0,
        //TTI_INFO                =1,
        TTI_WARNING = 2,
        //TTI_ERROR               =3,
        TTN_GETDISPINFO = ((0 - 520) - 10),
        TTN_SHOW = ((0 - 520) - 1),
        TTN_POP = ((0 - 520) - 2),
        TTN_NEEDTEXT = ((0 - 520) - 10),
        TBS_AUTOTICKS = 0x0001,
        TBS_VERT = 0x0002,
        TBS_TOP = 0x0004,
        TBS_BOTTOM = 0x0000,
        TBS_BOTH = 0x0008,
        TBS_NOTICKS = 0x0010,
        TBM_GETPOS = (0x0400),
        TBM_SETTIC = (0x0400 + 4),
        TBM_SETPOS = (0x0400 + 5),
        TBM_SETRANGE = (0x0400 + 6),
        TBM_SETRANGEMIN = (0x0400 + 7),
        TBM_SETRANGEMAX = (0x0400 + 8),
        TBM_SETTICFREQ = (0x0400 + 20),
        TBM_SETPAGESIZE = (0x0400 + 21),
        TBM_SETLINESIZE = (0x0400 + 23),
        TB_LINEUP = 0,
        TB_LINEDOWN = 1,
        TB_PAGEUP = 2,
        TB_PAGEDOWN = 3,
        TB_THUMBPOSITION = 4,
        TB_THUMBTRACK = 5,
        TB_TOP = 6,
        TB_BOTTOM = 7,
        TB_ENDTRACK = 8,
        TVS_HASBUTTONS = 0x0001,
        TVS_HASLINES = 0x0002,
        TVS_LINESATROOT = 0x0004,
        TVS_EDITLABELS = 0x0008,
        TVS_SHOWSELALWAYS = 0x0020,
        TVS_RTLREADING = 0x0040,
        TVS_CHECKBOXES = 0x0100,
        TVS_TRACKSELECT = 0x0200,
        TVS_FULLROWSELECT = 0x1000,
        TVS_NONEVENHEIGHT = 0x4000,
        TVS_INFOTIP = 0x0800,
        TVS_NOTOOLTIPS = 0x0080,
        TVIF_TEXT = 0x0001,
        TVIF_IMAGE = 0x0002,
        TVIF_PARAM = 0x0004,
        TVIF_STATE = 0x0008,
        TVIF_HANDLE = 0x0010,
        TVIF_SELECTEDIMAGE = 0x0020,
        TVIS_SELECTED = 0x0002,
        TVIS_EXPANDED = 0x0020,
        TVIS_EXPANDEDONCE = 0x0040,
        TVIS_STATEIMAGEMASK = 0xF000,
        TVI_ROOT = (unchecked((int)0xFFFF0000)),
        TVI_FIRST = (unchecked((int)0xFFFF0001)),
        TVM_INSERTITEM = (0x1100 + 50),
        TVM_DELETEITEM = (0x1100 + 1),
        TVM_EXPAND = (0x1100 + 2),
        TVE_COLLAPSE = 0x0001,
        TVE_EXPAND = 0x0002,
        TVM_GETITEMRECT = (0x1100 + 4),
        TVM_GETINDENT = (0x1100 + 6),
        TVM_SETINDENT = (0x1100 + 7),
        TVM_GETIMAGELIST = (0x1100 + 8),
        TVM_SETIMAGELIST = (0x1100 + 9),
        TVM_GETNEXTITEM = (0x1100 + 10),
        TVGN_NEXT = 0x0001,
        TVGN_PREVIOUS = 0x0002,
        TVGN_FIRSTVISIBLE = 0x0005,
        TVGN_NEXTVISIBLE = 0x0006,
        TVGN_PREVIOUSVISIBLE = 0x0007,
        TVGN_DROPHILITE = 0x0008,
        TVGN_CARET = 0x0009,
        TVM_SELECTITEM = (0x1100 + 11),
        TVM_GETITEM = (0x1100 + 62),
        TVM_SETITEM = (0x1100 + 63),
        TVM_EDITLABEL = (0x1100 + 65),
        TVM_GETEDITCONTROL = (0x1100 + 15),
        TVM_GETVISIBLECOUNT = (0x1100 + 16),
        TVM_HITTEST = (0x1100 + 17),
        TVM_ENSUREVISIBLE = (0x1100 + 20),
        TVM_ENDEDITLABELNOW = (0x1100 + 22),
        TVM_GETISEARCHSTRING = (0x1100 + 64),
        TVM_SETITEMHEIGHT = (0x1100 + 27),
        TVM_GETITEMHEIGHT = (0x1100 + 28),
        TVN_SELCHANGING = ((0 - 400) - 50),
        TVN_GETINFOTIP = ((0 - 400) - 14),
        TVN_SELCHANGED = ((0 - 400) - 51),
        TVC_UNKNOWN = 0x0000,
        TVC_BYMOUSE = 0x0001,
        TVC_BYKEYBOARD = 0x0002,
        TVN_GETDISPINFO = ((0 - 400) - 52),
        TVN_SETDISPINFO = ((0 - 400) - 53),
        TVN_ITEMEXPANDING = ((0 - 400) - 54),
        TVN_ITEMEXPANDED = ((0 - 400) - 55),
        TVN_BEGINDRAG = ((0 - 400) - 56),
        TVN_BEGINRDRAG = ((0 - 400) - 57),
        TVN_BEGINLABELEDIT = ((0 - 400) - 59),
        TVN_ENDLABELEDIT = ((0 - 400) - 60),
        TCS_BOTTOM = 0x0002,
        TCS_RIGHT = 0x0002,
        TCS_FLATBUTTONS = 0x0008,
        TCS_HOTTRACK = 0x0040,
        TCS_VERTICAL = 0x0080,
        TCS_TABS = 0x0000,
        TCS_BUTTONS = 0x0100,
        TCS_MULTILINE = 0x0200,
        TCS_RIGHTJUSTIFY = 0x0000,
        TCS_FIXEDWIDTH = 0x0400,
        TCS_RAGGEDRIGHT = 0x0800,
        TCS_OWNERDRAWFIXED = 0x2000,
        TCS_TOOLTIPS = 0x4000,
        TCM_SETIMAGELIST = (0x1300 + 3),
        TCIF_TEXT = 0x0001,
        TCIF_IMAGE = 0x0002,
        TCM_GETITEM = (0x1300 + 60),
        TCM_SETITEM = (0x1300 + 61),
        TCM_INSERTITEM = (0x1300 + 62),
        TCM_DELETEITEM = (0x1300 + 8),
        TCM_DELETEALLITEMS = (0x1300 + 9),
        TCM_GETITEMRECT = (0x1300 + 10),
        TCM_GETCURSEL = (0x1300 + 11),
        TCM_SETCURSEL = (0x1300 + 12),
        TCM_ADJUSTRECT = (0x1300 + 40),
        TCM_SETITEMSIZE = (0x1300 + 41),
        TCM_SETPADDING = (0x1300 + 43),
        TCM_GETROWCOUNT = (0x1300 + 44),
        TCM_GETTOOLTIPS = (0x1300 + 45),
        TCM_SETTOOLTIPS = (0x1300 + 46),
        TCN_SELCHANGE = ((0 - 550) - 1),
        TCN_SELCHANGING = ((0 - 550) - 2),
        TBSTYLE_WRAPPABLE = 0x0200,
        TVM_SETBKCOLOR = (TV_FIRST + 29),
        TVM_SETTEXTCOLOR = (TV_FIRST + 30),
        TYMED_NULL = 0,
        TVM_GETLINECOLOR = (TV_FIRST + 41),
        TVM_SETLINECOLOR = (TV_FIRST + 40),
        TVM_SETTOOLTIPS = (TV_FIRST + 24),
        TVSIL_STATE = 2,
        TVM_SORTCHILDRENCB = (TV_FIRST + 21),
        TMPF_FIXED_PITCH = 0x01;

    [StructLayout(LayoutKind.Sequential)]
    public struct NMHDR
    {
        public IntPtr hwndFrom;
        public IntPtr idFrom; //This is declared as UINT_PTR in winuser.h
        public int code;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class INITCOMMONCONTROLSEX
    {
        public int dwSize = 8; //ndirect.DllLib.sizeOf(this);
        public int dwICC;
    }

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

        public const int OCM__BASE = 0x2000;
        public const int DISPID_VALUE = unchecked((int)0x0);
        public const int DISPID_UNKNOWN = unchecked((int)0xFFFFFFFF);
        public const int DISPID_AUTOSIZE = unchecked((int)0xFFFFFE0C);
        public const int DISPID_BACKCOLOR = unchecked((int)0xFFFFFE0B);
        public const int DISPID_BACKSTYLE = unchecked((int)0xFFFFFE0A);
        public const int DISPID_BORDERCOLOR = unchecked((int)0xFFFFFE09);
        public const int DISPID_BORDERSTYLE = unchecked((int)0xFFFFFE08);
        public const int DISPID_BORDERWIDTH = unchecked((int)0xFFFFFE07);
        public const int DISPID_DRAWMODE = unchecked((int)0xFFFFFE05);
        public const int DISPID_DRAWSTYLE = unchecked((int)0xFFFFFE04);
        public const int DISPID_DRAWWIDTH = unchecked((int)0xFFFFFE03);
        public const int DISPID_FILLCOLOR = unchecked((int)0xFFFFFE02);
        public const int DISPID_FILLSTYLE = unchecked((int)0xFFFFFE01);
        public const int DISPID_FONT = unchecked((int)0xFFFFFE00);
        public const int DISPID_FORECOLOR = unchecked((int)0xFFFFFDFF);
        public const int DISPID_ENABLED = unchecked((int)0xFFFFFDFE);
        public const int DISPID_HWND = unchecked((int)0xFFFFFDFD);
        public const int DISPID_TABSTOP = unchecked((int)0xFFFFFDFC);
        public const int DISPID_TEXT = unchecked((int)0xFFFFFDFB);
        public const int DISPID_CAPTION = unchecked((int)0xFFFFFDFA);
        public const int DISPID_BORDERVISIBLE = unchecked((int)0xFFFFFDF9);
        public const int DISPID_APPEARANCE = unchecked((int)0xFFFFFDF8);
        public const int DISPID_MOUSEPOINTER = unchecked((int)0xFFFFFDF7);
        public const int DISPID_MOUSEICON = unchecked((int)0xFFFFFDF6);
        public const int DISPID_PICTURE = unchecked((int)0xFFFFFDF5);
        public const int DISPID_VALID = unchecked((int)0xFFFFFDF4);
        public const int DISPID_READYSTATE = unchecked((int)0xFFFFFDF3);
        public const int DISPID_REFRESH = unchecked((int)0xFFFFFDDA);
        public const int DISPID_DOCLICK = unchecked((int)0xFFFFFDD9);
        public const int DISPID_ABOUTBOX = unchecked((int)0xFFFFFDD8);
        public const int DISPID_CLICK = unchecked((int)0xFFFFFDA8);
        public const int DISPID_DBLCLICK = unchecked((int)0xFFFFFDA7);
        public const int DISPID_KEYDOWN = unchecked((int)0xFFFFFDA6);
        public const int DISPID_KEYPRESS = unchecked((int)0xFFFFFDA5);
        public const int DISPID_KEYUP = unchecked((int)0xFFFFFDA4);
        public const int DISPID_MOUSEDOWN = unchecked((int)0xFFFFFDA3);
        public const int DISPID_MOUSEMOVE = unchecked((int)0xFFFFFDA2);
        public const int DISPID_MOUSEUP = unchecked((int)0xFFFFFDA1);
        public const int DISPID_ERROREVENT = unchecked((int)0xFFFFFDA0);
        public const int DISPID_RIGHTTOLEFT = unchecked((int)0xFFFFFD9D);
        public const int DISPID_READYSTATECHANGE = unchecked((int)0xFFFFFD9F);
        public const int DISPID_AMBIENT_BACKCOLOR = unchecked((int)0xFFFFFD43);
        public const int DISPID_AMBIENT_DISPLAYNAME = unchecked((int)0xFFFFFD42);
        public const int DISPID_AMBIENT_FONT = unchecked((int)0xFFFFFD41);
        public const int DISPID_AMBIENT_FORECOLOR = unchecked((int)0xFFFFFD40);
        public const int DISPID_AMBIENT_LOCALEID = unchecked((int)0xFFFFFD3F);
        public const int DISPID_AMBIENT_MESSAGEREFLECT = unchecked((int)0xFFFFFD3E);
        public const int DISPID_AMBIENT_SCALEUNITS = unchecked((int)0xFFFFFD3D);
        public const int DISPID_AMBIENT_TEXTALIGN = unchecked((int)0xFFFFFD3C);
        public const int DISPID_AMBIENT_USERMODE = unchecked((int)0xFFFFFD3B);
        public const int DISPID_AMBIENT_UIDEAD = unchecked((int)0xFFFFFD3A);
        public const int DISPID_AMBIENT_SHOWGRABHANDLES = unchecked((int)0xFFFFFD39);
        public const int DISPID_AMBIENT_SHOWHATCHING = unchecked((int)0xFFFFFD38);
        public const int DISPID_AMBIENT_DISPLAYASDEFAULT = unchecked((int)0xFFFFFD37);
        public const int DISPID_AMBIENT_SUPPORTSMNEMONICS = unchecked((int)0xFFFFFD36);
        public const int DISPID_AMBIENT_AUTOCLIP = unchecked((int)0xFFFFFD35);
        public const int DISPID_AMBIENT_APPEARANCE = unchecked((int)0xFFFFFD34);
        public const int DISPID_AMBIENT_PALETTE = unchecked((int)0xFFFFFD2A);
        public const int DISPID_AMBIENT_TRANSFERPRIORITY = unchecked((int)0xFFFFFD28);
        public const int DISPID_AMBIENT_RIGHTTOLEFT = unchecked((int)0xFFFFFD24);
        public const int DISPID_Name = unchecked((int)0xFFFFFCE0);
        public const int DISPID_Delete = unchecked((int)0xFFFFFCDF);
        public const int DISPID_Object = unchecked((int)0xFFFFFCDE);
        public const int DISPID_Parent = unchecked((int)0xFFFFFCDD);
        public const int DVASPECT_CONTENT = 0x1;
        public const int DVASPECT_THUMBNAIL = 0x2;
        public const int DVASPECT_ICON = 0x4;
        public const int DVASPECT_DOCPRINT = 0x8;
        public const int OLEMISC_RECOMPOSEONRESIZE = 0x1;
        public const int OLEMISC_ONLYICONIC = 0x2;
        public const int OLEMISC_INSERTNOTREPLACE = 0x4;
        public const int OLEMISC_STATIC = 0x8;
        public const int OLEMISC_CANTLINKINSIDE = 0x10;
        public const int OLEMISC_CANLINKBYOLE1 = 0x20;
        public const int OLEMISC_ISLINKOBJECT = 0x40;
        public const int OLEMISC_INSIDEOUT = 0x80;
        public const int OLEMISC_ACTIVATEWHENVISIBLE = 0x100;
        public const int OLEMISC_RENDERINGISDEVICEINDEPENDENT = 0x200;
        public const int OLEMISC_INVISIBLEATRUNTIME = 0x400;
        public const int OLEMISC_ALWAYSRUN = 0x800;
        public const int OLEMISC_ACTSLIKEBUTTON = 0x1000;
        public const int OLEMISC_ACTSLIKELABEL = 0x2000;
        public const int OLEMISC_NOUIACTIVATE = 0x4000;
        public const int OLEMISC_ALIGNABLE = 0x8000;
        public const int OLEMISC_SIMPLEFRAME = 0x10000;
        public const int OLEMISC_SETCLIENTSITEFIRST = 0x20000;
        public const int OLEMISC_IMEMODE = 0x40000;
        public const int OLEMISC_IGNOREACTIVATEWHENVISIBLE = 0x80000;
        public const int OLEMISC_WANTSTOMENUMERGE = 0x100000;
        public const int OLEMISC_SUPPORTSMULTILEVELUNDO = 0x200000;
        public const int QACONTAINER_SHOWHATCHING = 0x1;
        public const int QACONTAINER_SHOWGRABHANDLES = 0x2;
        public const int QACONTAINER_USERMODE = 0x4;
        public const int QACONTAINER_DISPLAYASDEFAULT = 0x8;
        public const int QACONTAINER_UIDEAD = 0x10;
        public const int QACONTAINER_AUTOCLIP = 0x20;
        public const int QACONTAINER_MESSAGEREFLECT = 0x40;
        public const int QACONTAINER_SUPPORTSMNEMONICS = 0x80;
        public const int XFORMCOORDS_POSITION = 0x1;
        public const int XFORMCOORDS_SIZE = 0x2;
        public const int XFORMCOORDS_HIMETRICTOCONTAINER = 0x4;
        public const int XFORMCOORDS_CONTAINERTOHIMETRIC = 0x8;
        public const int PROPCAT_Nil = unchecked((int)0xFFFFFFFF);
        public const int PROPCAT_Misc = unchecked((int)0xFFFFFFFE);
        public const int PROPCAT_Font = unchecked((int)0xFFFFFFFD);
        public const int PROPCAT_Position = unchecked((int)0xFFFFFFFC);
        public const int PROPCAT_Appearance = unchecked((int)0xFFFFFFFB);
        public const int PROPCAT_Behavior = unchecked((int)0xFFFFFFFA);
        public const int PROPCAT_Data = unchecked((int)0xFFFFFFF9);
        public const int PROPCAT_List = unchecked((int)0xFFFFFFF8);
        public const int PROPCAT_Text = unchecked((int)0xFFFFFFF7);
        public const int PROPCAT_Scale = unchecked((int)0xFFFFFFF6);
        public const int PROPCAT_DDE = unchecked((int)0xFFFFFFF5);
        public const int GC_WCH_SIBLING = 0x1;
        public const int GC_WCH_CONTAINER = 0x2;
        public const int GC_WCH_CONTAINED = 0x3;
        public const int GC_WCH_ALL = 0x4;
        public const int GC_WCH_FREVERSEDIR = 0x8000000;
        public const int GC_WCH_FONLYNEXT = 0x10000000;
        public const int GC_WCH_FONLYPREV = 0x20000000;
        public const int GC_WCH_FSELECTED = 0x40000000;
        public const int OLECONTF_EMBEDDINGS = 0x1;
        public const int OLECONTF_LINKS = 0x2;
        public const int OLECONTF_OTHERS = 0x4;
        public const int OLECONTF_ONLYUSER = 0x8;
        public const int OLECONTF_ONLYIFRUNNING = 0x10;

        public const int OLEVERBATTRIB_NEVERDIRTIES = 0x1;
        public const int OLEVERBATTRIB_ONCONTAINERMENU = 0x2;

        public static Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");
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
        public string? dwTypeData;
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
