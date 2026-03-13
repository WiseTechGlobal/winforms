// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace System.Windows.Forms;

internal static class LegacyDataGridInteropCompat
{
    internal static partial class User32
    {
        [DllImport(Libraries.User32, ExactSpelling = true)]
        private static extern BOOL ScrollWindow(HWND hWnd, int nXAmount, int nYAmount, ref RECT rectScrollRegion, ref RECT rectClip);

        public static BOOL ScrollWindow(IHandle<HWND> hWnd, int nXAmount, int nYAmount, ref RECT rectScrollRegion, ref RECT rectClip)
        {
            BOOL result = ScrollWindow(hWnd.Handle, nXAmount, nYAmount, ref rectScrollRegion, ref rectClip);
            GC.KeepAlive(hWnd);

            return result;
        }
    }
}
