// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#pragma warning disable RS0036

namespace System.Windows.Forms;

public class StatusBarPanelClickEventArgs : MouseEventArgs
{
    public StatusBarPanelClickEventArgs(StatusBarPanel statusBarPanel, MouseButtons button, int clicks, int x, int y)
        : base(button, clicks, x, y, 0)
    {
        StatusBarPanel = statusBarPanel;
    }

    public StatusBarPanel StatusBarPanel { get; }
}
