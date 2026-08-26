// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing;
using WinFormsControlsTest;

// Set STAThread
Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
ApplicationConfiguration.Initialize();

Application.SetColorMode(SystemColorMode.Classic);
Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

// Headless repro mode for the TreeView AddRange sort-order experiment.
// When asked, print the resulting node order (optionally with the AppContext switch
// flipped) and exit without showing the UI. Used by the "Compare switch ON / OFF"
// button, which launches this same build as child processes.
if (TreeViewSortAddRangeTest.TryRunConsoleReport(args, out int reportExitCode))
{
    Environment.Exit(reportExitCode);
}

try
{
    MainForm form = new()
    {
        Icon = SystemIcons.GetStockIcon(StockIconId.Shield, StockIconOptions.SmallIcon)
    };

    Application.Run(form);
}
catch (Exception)
{
    Environment.Exit(-1);
}

Environment.Exit(0);
