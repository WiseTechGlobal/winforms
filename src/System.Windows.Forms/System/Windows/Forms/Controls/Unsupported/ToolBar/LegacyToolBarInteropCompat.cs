// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace System.Windows.Forms;

internal static class LegacyToolBarSR
{
    public const string ToolBarBadToolBarButton = "Value must be a ToolBarButton.";
    public const string ToolBarButtonInvalidDropDownMenuType = "DropDownMenu must be a ContextMenu.";
}

internal static class ToolBarNativeMethods
{
    public const string WC_TOOLBAR = "ToolbarWindow32";
    public const int TPM_LEFTALIGN = 0x0000;
    public const int TPM_LEFTBUTTON = 0x0000;
    public const int TPM_VERTICAL = 0x0040;
    public const int CCS_NORESIZE = 0x0004;
    public const int CCS_NOPARENTALIGN = 0x0008;
    public const int CCS_NODIVIDER = 0x0040;
    public const int TBSTYLE_TOOLTIPS = 0x0100;
    public const int TBSTYLE_WRAPABLE = 0x0200;
    public const int TBSTYLE_FLAT = 0x0800;
    public const int TBSTYLE_LIST = 0x1000;
    public const int TBSTYLE_BUTTON = 0x0000;
    public const int TBSTYLE_SEP = 0x0001;
    public const int TBSTYLE_CHECK = 0x0002;
    public const int TBSTYLE_DROPDOWN = 0x0008;
    public const int TBSTYLE_EX_DRAWDDARROWS = 0x00000001;
    public const int TBSTATE_CHECKED = 0x01;
    public const int TBSTATE_PRESSED = 0x02;
    public const int TBSTATE_ENABLED = 0x04;
    public const int TBSTATE_HIDDEN = 0x08;
    public const int TBSTATE_INDETERMINATE = 0x10;
    public const uint TBIF_IMAGE = 0x00000001;
    public const uint TBIF_TEXT = 0x00000002;
    public const uint TBIF_STATE = 0x00000004;
    public const uint TBIF_STYLE = 0x00000008;
    public const uint TBIF_COMMAND = 0x00000020;
    public const uint TBIF_SIZE = 0x00000040;
    public const uint TBN_QUERYINSERT = unchecked((uint)-706);
    public const uint TBN_DROPDOWN = unchecked((uint)-710);
    public const uint TBN_HOTITEMCHANGE = unchecked((uint)-713);

    [Flags]
    public enum HICF
    {
        HICF_OTHER = 0x00000000,
        HICF_MOUSE = 0x00000001,
        HICF_ARROWKEYS = 0x00000002,
        HICF_ACCELERATOR = 0x00000004,
        HICF_DUPACCEL = 0x00000008,
        HICF_ENTERING = 0x00000010,
        HICF_LEAVING = 0x00000020,
        HICF_RESELECT = 0x00000040,
        HICF_LMOUSE = 0x00000080,
        HICF_TOGGLEDROPDOWN = 0x00000100,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NMTOOLBAR
    {
        public NMHDR hdr;
        public int iItem;
        public TBBUTTON tbButton;
        public int cchText;
        public IntPtr pszText;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NMTBHOTITEM
    {
        public NMHDR hdr;
        public int idOld;
        public int idNew;
        public HICF dwFlags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TBBUTTON
    {
        public int iBitmap;
        public int idCommand;
        public byte fsState;
        public byte fsStyle;
        public byte bReserved0;
        public byte bReserved1;
        public IntPtr dwData;
        public IntPtr iString;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TOOLTIPTEXT
    {
        public NMHDR hdr;
        public string? lpszText;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string? szText = null;

        public IntPtr hinst;
        public int uFlags;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct TBBUTTONINFO
    {
        public int cbSize;
        public int dwMask;
        public int idCommand;
        public int iImage;
        public byte fsState;
        public byte fsStyle;
        public short cx;
        public IntPtr lParam;
        public IntPtr pszText;
        public int cchText;
    }

    public static class Util
    {
        public static int MAKELONG(int low, int high) => (high << 16) | (low & 0xffff);

        public static IntPtr MAKELPARAM(int low, int high) => (IntPtr)MAKELONG(low, high);

        public static int HIWORD(int n) => (n >> 16) & 0xffff;

        public static int LOWORD(int n) => n & 0xffff;

        public static int LOWORD(nint n) => PARAM.LOWORD(n);
    }
}
