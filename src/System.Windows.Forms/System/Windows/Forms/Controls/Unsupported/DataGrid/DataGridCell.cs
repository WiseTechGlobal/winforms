// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#pragma warning disable CA1066, CA1815, RS0016, IDE0251

using System.ComponentModel;

namespace System.Windows.Forms;

#nullable disable

/// <summary>
///  This type is provided for binary compatibility with .NET Framework and is not intended to be used directly from your code.
/// </summary>
[Obsolete(
    Obsoletions.DataGridMessage,
    error: false,
    DiagnosticId = Obsoletions.UnsupportedControlsDiagnosticId,
    UrlFormat = Obsoletions.SharedUrlFormat)]
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
public struct DataGridCell
{
    public DataGridCell(int r, int c)
    {
        RowNumber = r;
        ColumnNumber = c;
    }

    public int ColumnNumber
    {
        readonly get;
        set;
    }

    public int RowNumber
    {
        readonly get;
        set;
    }

    public override bool Equals(object obj)
    {
        if (obj is not DataGridCell rhs)
        {
            return false;
        }

        return rhs.RowNumber == RowNumber && rhs.ColumnNumber == ColumnNumber;
    }

    public override int GetHashCode() => HashCode.Combine(RowNumber, ColumnNumber);

    public override string ToString()
    {
        return $"DataGridCell {{RowNumber = {RowNumber}, ColumnNumber = {ColumnNumber}}}";
    }
}
