// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Globalization;
using System.Drawing;
using System.Runtime.InteropServices;
using static Interop;

namespace System.Windows.Forms;

// DataGrid-specific enum validation helper — ClientUtils from System.Windows.Forms.Primitives
// does not include IsEnumValid, and we intentionally avoid redefining ClientUtils to prevent
// CS0436 conflicts with the imported type.
internal static class DataGridClientUtils
{
    public static bool IsEnumValid(Enum value, int valueAsInt, int minValue, int maxValue) =>
        valueAsInt >= minValue
        && valueAsInt <= maxValue
        && Enum.IsDefined(value.GetType(), value);
}

internal static class WindowMessages
{
    public const int WM_CHAR = (int)PInvokeCore.WM_CHAR;
    public const int WM_CLEAR = (int)PInvokeCore.WM_CLEAR;
    public const int WM_CUT = (int)PInvokeCore.WM_CUT;
    public const int WM_KEYDOWN = (int)PInvokeCore.WM_KEYDOWN;
    public const int WM_KEYUP = (int)PInvokeCore.WM_KEYUP;
    public const int WM_PASTE = (int)PInvokeCore.WM_PASTE;
    public const int TTM_ADDTOOLW = (int)PInvoke.TTM_ADDTOOLW;
    public const int TTM_DELTOOLW = (int)PInvoke.TTM_DELTOOLW;
    public const int TTM_SETDELAYTIME = (int)PInvoke.TTM_SETDELAYTIME;
    public const int TTM_SETMAXTIPWIDTH = (int)PInvoke.TTM_SETMAXTIPWIDTH;
}

internal static class DpiHelper
{
    public static Bitmap GetBitmapFromIcon(Type type, string bitmapName) =>
        ScaleHelper.GetIconResourceAsDefaultSizeBitmap(type, bitmapName);
}

// DataGrid-specific native constants/structs — NativeMethods from System.Windows.Forms.Primitives
// only exposes ListViewCompareCallback; everything below is DataGrid-specific. We use a
// distinct name to avoid CS0436 conflicts with the imported NativeMethods type.
internal static class DataGridNativeMethods
{
    public const INITCOMMONCONTROLSEX_ICC ICC_TAB_CLASSES = INITCOMMONCONTROLSEX_ICC.ICC_TAB_CLASSES;
    public const string TOOLTIPS_CLASS = PInvoke.TOOLTIPS_CLASS;
    public const int TTS_ALWAYSTIP = (int)PInvoke.TTS_ALWAYSTIP;
    public const int WHEEL_DELTA = 120;

    [StructLayout(LayoutKind.Sequential)]
    public struct INITCOMMONCONTROLSEX
    {
        public uint dwSize;
        public INITCOMMONCONTROLSEX_ICC dwICC;
    }
}

internal static class DataGridSafeNativeMethods
{
    public static unsafe bool InitCommonControlsEx(DataGridNativeMethods.INITCOMMONCONTROLSEX icc)
    {
        INITCOMMONCONTROLSEX native = new()
        {
            dwSize = icc.dwSize,
            dwICC = icc.dwICC
        };

        return PInvoke.InitCommonControlsEx(native);
    }

    public static unsafe bool ScrollWindow(HandleRef hWnd, int xAmount, int yAmount, ref RECT rect, ref RECT clipRect)
    {
        fixed (RECT* rectPtr = &rect)
        fixed (RECT* clipPtr = &clipRect)
        {
            return PInvoke.ScrollWindow((HWND)hWnd.Handle, xAmount, yAmount, rectPtr, clipPtr);
        }
    }
}

internal static class DataGridUnsafeNativeMethods
{
    public static RECT[]? GetRectsFromRegion(IntPtr hrgn)
    {
        HRGN region = (HRGN)hrgn;
        return region.IsNull ? null : region.GetRegionRects();
    }
}

internal static class CompModSwitches
{
    private static TraceSwitch? s_dataGridCursor;
    private static TraceSwitch? s_dataGridEditing;
    private static TraceSwitch? s_dataGridKeys;
    private static TraceSwitch? s_dataGridLayout;
    private static TraceSwitch? s_dataGridPainting;
    private static TraceSwitch? s_dataGridParents;
    private static TraceSwitch? s_dataGridScrolling;
    private static TraceSwitch? s_dataGridSelection;
    private static TraceSwitch? s_dgCaptionPaint;
    private static TraceSwitch? s_dgRelationShpRowLayout;
    private static TraceSwitch? s_dgRelationShpRowPaint;
    private static TraceSwitch? s_dgRowPaint;

    public static TraceSwitch DataGridCursor =>
        s_dataGridCursor ??= new TraceSwitch(nameof(DataGridCursor), "DataGrid cursor tracing");

    public static TraceSwitch DataGridEditing =>
        s_dataGridEditing ??= new TraceSwitch(nameof(DataGridEditing), "DataGrid editing tracing");

    public static TraceSwitch DataGridKeys =>
        s_dataGridKeys ??= new TraceSwitch(nameof(DataGridKeys), "DataGrid keyboard tracing");

    public static TraceSwitch DataGridLayout =>
        s_dataGridLayout ??= new TraceSwitch(nameof(DataGridLayout), "DataGrid layout tracing");

    public static TraceSwitch DataGridPainting =>
        s_dataGridPainting ??= new TraceSwitch(nameof(DataGridPainting), "DataGrid painting tracing");

    public static TraceSwitch DataGridParents =>
        s_dataGridParents ??= new TraceSwitch(nameof(DataGridParents), "DataGrid parent rows tracing");

    public static TraceSwitch DataGridScrolling =>
        s_dataGridScrolling ??= new TraceSwitch(nameof(DataGridScrolling), "DataGrid scrolling tracing");

    public static TraceSwitch DataGridSelection =>
        s_dataGridSelection ??= new TraceSwitch(nameof(DataGridSelection), "DataGrid selection tracing");

    public static TraceSwitch DGCaptionPaint =>
        s_dgCaptionPaint ??= new TraceSwitch(nameof(DGCaptionPaint), "DataGrid caption paint tracing");

    public static TraceSwitch DGRelationShpRowLayout =>
        s_dgRelationShpRowLayout ??= new TraceSwitch(nameof(DGRelationShpRowLayout), "DataGrid relationship row layout tracing");

    public static TraceSwitch DGRelationShpRowPaint =>
        s_dgRelationShpRowPaint ??= new TraceSwitch(nameof(DGRelationShpRowPaint), "DataGrid relationship row paint tracing");

    public static TraceSwitch DGRowPaint =>
        s_dgRowPaint ??= new TraceSwitch(nameof(DGRowPaint), "DataGrid row paint tracing");
}

internal enum TriangleDirection
{
    Up,
    Down,
}

internal static class Triangle
{
    public static void Paint(
        Graphics graphics,
        Rectangle bounds,
        TriangleDirection direction,
        Brush backBrush,
        Pen pen1,
        Pen pen2,
        Pen pen3,
        bool fill)
    {
        Point[] points = direction switch
        {
            TriangleDirection.Down =>
            [
                new Point(bounds.Left, bounds.Top),
                new Point(bounds.Right, bounds.Top),
                new Point(bounds.Left + (bounds.Width / 2), bounds.Bottom),
            ],
            _ =>
            [
                new Point(bounds.Left + (bounds.Width / 2), bounds.Top),
                new Point(bounds.Left, bounds.Bottom),
                new Point(bounds.Right, bounds.Bottom),
            ],
        };

        if (fill)
        {
            graphics.FillPolygon(backBrush, points);
        }

        graphics.DrawPolygon(pen1, points);
    }
}