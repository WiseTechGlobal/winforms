// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Windows.Forms;

/// <summary>
///  Specifies how a panel on a status bar changes when the status bar resizes.
/// </summary>
public enum StatusBarPanelAutoSize
{
    /// <summary>
    ///  The panel does not change its size when the status bar resizes.
    /// </summary>
    None = 1,

    /// <summary>
    ///  The panel shares the available status bar space with other spring panels.
    /// </summary>
    Spring = 2,

    /// <summary>
    ///  The width of the panel is determined by its contents.
    /// </summary>
    Contents = 3,
}
