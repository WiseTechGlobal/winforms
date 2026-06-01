// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Tests;

public class MenuSelectTests
{
    private const uint WM_MENUSELECT = 0x011F;
    private const int MF_POPUP = 0x0010;

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [StaFact]
    public void Control_WmMenuSelect_CommandItem_FiresSelectEvent()
    {
        using Control control = new() { Visible = true };

        MenuItem menuItem = new("Command Item");
        bool selectFired = false;
        menuItem.Select += (_, _) => selectFired = true;

        ContextMenu contextMenu = new(new[] { menuItem });
        control.ContextMenu = contextMenu;

        IntPtr controlHandle = control.Handle;
        IntPtr contextMenuHandle = contextMenu.Handle;
        int commandId = GetCommandId(menuItem);

        SendMessage(controlHandle, WM_MENUSELECT, unchecked((IntPtr)commandId), contextMenuHandle);

        Assert.True(selectFired);
    }

    [StaFact]
    public void Form_WmMenuSelect_PopupMenuItem_FiresSelectEvent()
    {
        using Form form = new();

        MainMenu mainMenu = new();
        MenuItem fileMenuItem = new("File");
        bool selectFired = false;
        fileMenuItem.Select += (_, _) => selectFired = true;

        mainMenu.MenuItems.Add(fileMenuItem);
        form.Menu = mainMenu;

        IntPtr formHandle = form.Handle;
        IntPtr mainMenuHandle = mainMenu.Handle;
        IntPtr wParam = unchecked((IntPtr)(MF_POPUP << 16));

        SendMessage(formHandle, WM_MENUSELECT, wParam, mainMenuHandle);

        Assert.True(selectFired);
    }

    private static int GetCommandId(MenuItem menuItem)
    {
        _ = menuItem.Handle;

        FieldInfo dataField = typeof(MenuItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic)!;
        object data = dataField.GetValue(menuItem)!;

        MethodInfo getMenuId = data.GetType().GetMethod("GetMenuID", BindingFlags.Instance | BindingFlags.NonPublic)!;
        return (int)getMenuId.Invoke(data, null)!;
    }
}
