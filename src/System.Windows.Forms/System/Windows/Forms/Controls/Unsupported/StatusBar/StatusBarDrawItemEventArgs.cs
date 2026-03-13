// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing;

namespace System.Windows.Forms;

public class StatusBarDrawItemEventArgs : DrawItemEventArgs
{
    public StatusBarDrawItemEventArgs(Graphics g, Font font, Rectangle r, int itemId, DrawItemState itemState, StatusBarPanel panel)
        : base(g, font, r, itemId, itemState)
    {
        Panel = panel;
    }

    public StatusBarDrawItemEventArgs(
        Graphics g,
        Font font,
        Rectangle r,
        int itemId,
        DrawItemState itemState,
        StatusBarPanel panel,
        Color foreColor,
        Color backColor)
        : base(g, font, r, itemId, itemState, foreColor, backColor)
    {
        Panel = panel;
    }

    public StatusBarPanel Panel { get; }
}
