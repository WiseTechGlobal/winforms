// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace System.Windows.Forms;

internal static class LegacyMenuNativeMethods
{
    public const int MNC_EXECUTE = 2;
    public const int MNC_SELECT = 3;
    public const int MIIM_STATE = 0x00000001;
    public const int MIIM_ID = 0x00000002;
    public const int MIIM_SUBMENU = 0x00000004;
    public const int MIIM_TYPE = 0x00000010;
    public const int MIIM_DATA = 0x00000020;
    public const int MF_BYPOSITION = 0x00000400;
    public const int MFT_MENUBREAK = 0x00000040;
    public const int MFT_SEPARATOR = 0x00000800;
    public const int MFT_RIGHTORDER = 0x00002000;
    public const int MFT_RIGHTJUSTIFY = 0x00004000;
    public const int TPM_RIGHTBUTTON = 0x0002;
    public const int TPM_LEFTALIGN = 0x0000;
    public const int TPM_RIGHTALIGN = 0x0008;
    public const int TPM_VERTICAL = 0x0040;

    [StructLayout(LayoutKind.Sequential)]
    public class DRAWITEMSTRUCT
    {
        public int CtlType;
        public int CtlID;
        public int itemID;
        public int itemAction;
        public int itemState;
        public IntPtr hwndItem;
        public IntPtr hDC;
        public RECT rcItem;
        public IntPtr itemData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class MEASUREITEMSTRUCT
    {
        public int CtlType;
        public int CtlID;
        public int itemID;
        public int itemWidth;
        public int itemHeight;
        public IntPtr itemData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class TPMPARAMS
    {
        public int cbSize = Marshal.SizeOf<TPMPARAMS>();
        public int rcExclude_left;
        public int rcExclude_top;
        public int rcExclude_right;
        public int rcExclude_bottom;
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
        public static int MAKELONG(int low, int high) => (high << 16) | (low & 0xffff);

        public static int LOWORD(nint n) => PARAM.LOWORD(n);
    }
}

internal static class LegacyMenuUnsafeNativeMethods
{
    [DllImport(Libraries.User32, ExactSpelling = true)]
    public static extern IntPtr CreatePopupMenu();

    [DllImport(Libraries.User32, ExactSpelling = true)]
    public static extern IntPtr CreateMenu();

    [DllImport(Libraries.User32, ExactSpelling = true)]
    public static extern bool DestroyMenu(HandleRef hMenu);

    [DllImport(Libraries.User32, ExactSpelling = true)]
    public static extern int GetMenuItemCount(HandleRef hMenu);

    [DllImport(Libraries.User32, ExactSpelling = true)]
    public static extern bool RemoveMenu(HandleRef hMenu, int uPosition, int uFlags);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    private static extern bool GetMenuItemInfo(IntPtr hMenu, int uItem, bool fByPosition, [In, Out] LegacyMenuNativeMethods.MENUITEMINFO_T lpmii);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern bool InsertMenuItem(HandleRef hMenu, int uItem, bool fByPosition, LegacyMenuNativeMethods.MENUITEMINFO_T lpmii);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern bool SetMenuItemInfo(HandleRef hMenu, int uItem, bool fByPosition, LegacyMenuNativeMethods.MENUITEMINFO_T lpmii);

    [DllImport(Libraries.User32, ExactSpelling = true)]
    public static extern bool SetMenuDefaultItem(HandleRef hMenu, int uItem, bool fByPos);

    public static bool GetMenuItemInfo(HandleRef hMenu, int uItem, bool fByPosition, LegacyMenuNativeMethods.MENUITEMINFO_T lpmii)
    {
        bool result = GetMenuItemInfo(hMenu.Handle, uItem, fByPosition, lpmii);
        GC.KeepAlive(hMenu.Wrapper);
        return result;
    }
}

internal static class LegacyMenuSafeNativeMethods
{
    [DllImport(Libraries.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
    public static extern bool TrackPopupMenuEx(HandleRef hmenu, int fuFlags, int x, int y, HandleRef hwnd, LegacyMenuNativeMethods.TPMPARAMS? tpm);

    [DllImport(Libraries.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
    public static extern bool DrawMenuBar(HandleRef hWnd);

    [DllImport(Libraries.Gdi32, ExactSpelling = true)]
    public static extern IntPtr SelectPalette(HandleRef hdc, HandleRef hpal, int bForceBkgd);
}