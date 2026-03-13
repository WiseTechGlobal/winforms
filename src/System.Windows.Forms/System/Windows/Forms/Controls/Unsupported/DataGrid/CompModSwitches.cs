// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#pragma warning disable IDE1006, SA1518

namespace System.Windows.Forms;

internal static class CompModSwitches
{
    public static TraceSwitch DataGridCursor { get; } = new("DataGridCursor", "DataGrid cursor tracing");
    public static TraceSwitch DataGridEditing { get; } = new("DataGridEditing", "DataGrid editing tracing");
    public static TraceSwitch DataGridKeys { get; } = new("DataGridKeys", "DataGrid keyboard tracing");
    public static TraceSwitch DataGridLayout { get; } = new("DataGridLayout", "DataGrid layout tracing");
    public static TraceSwitch DataGridPainting { get; } = new("DataGridPainting", "DataGrid painting tracing");
    public static TraceSwitch DataGridParents { get; } = new("DataGridParents", "DataGrid parent rows tracing");
    public static TraceSwitch DataGridScrolling { get; } = new("DataGridScrolling", "DataGrid scrolling tracing");
    public static TraceSwitch DataGridSelection { get; } = new("DataGridSelection", "DataGrid selection tracing");
    public static TraceSwitch DGCaptionPaint { get; } = new("DGCaptionPaint", "DataGrid caption painting tracing");
    public static TraceSwitch DGEditColumnEditing { get; } = new("DGEditColumnEditing", "DataGrid edit column tracing");
    public static TraceSwitch DGRelationShpRowLayout { get; } = new("DGRelationShpRowLayout", "DataGrid relationship row layout tracing");
    public static TraceSwitch DGRelationShpRowPaint { get; } = new("DGRelationShpRowPaint", "DataGrid relationship row painting tracing");
    public static TraceSwitch DGRowPaint { get; } = new("DGRowPaint", "DataGrid row painting tracing");
}