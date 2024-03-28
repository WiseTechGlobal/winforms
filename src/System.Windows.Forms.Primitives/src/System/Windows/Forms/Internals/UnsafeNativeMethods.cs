// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;
using static Interop;

namespace System.Windows.Forms;

internal static class UnsafeNativeMethods
{
    [DllImport(Libraries.Comdlg32, SetLastError = true, CharSet = CharSet.Auto)]
    public static extern HRESULT PrintDlgEx([In, Out] NativeMethods.PRINTDLGEX lppdex);

    [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, string lParam);

    [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(HandleRef hWnd, int msg, HandleRef wParam, int lParam);

    [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern BOOL GetMenuItemInfo(IntPtr hMenu, int uItem, bool fByPosition, [In, Out] NativeMethods.MENUITEMINFO_T lpmii);

    [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
    public extern static bool InsertMenuItem(HandleRef hMenu, int uItem, bool fByPosition, NativeMethods.MENUITEMINFO_T lpmii);

    [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
    public extern static bool SetMenuItemInfo(HandleRef hMenu, int uItem, bool fByPosition, NativeMethods.MENUITEMINFO_T lpmii);

    public static BOOL GetMenuItemInfo(HandleRef hMenu, int uItem, bool fByPosition, NativeMethods.MENUITEMINFO_T lpmii)
    {
        BOOL result = GetMenuItemInfo(hMenu.Handle, uItem, fByPosition, lpmii);
        GC.KeepAlive(hMenu.Wrapper);
        return result;
    }
}
