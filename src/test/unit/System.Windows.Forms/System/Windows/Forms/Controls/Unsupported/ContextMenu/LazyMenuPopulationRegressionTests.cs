// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;

namespace System.Windows.Forms.Tests;

/// <summary>
///  Regression tests proving that the top-level <em>Universal Copy</em> and <em>Grid Colors</em>
///  context-menu entries are actually populated when the user hovers them — not just that the
///  internal event wiring exists.
/// </summary>
/// <remarks>
///  <para>
///   The screenshot that drove this test shows the Universal Copy submenu open with four
///   entries: "New Copy Template", "Edit Copy Template", "New Copy Template From", and
///   "Copy Schedules".  Without the WM_MENUSELECT fix in <see cref="Control.WndProc"/> the
///   Grid Colors menu's <see cref="MenuItem.Select"/> handler never fires and the submenu
///   stays on stale placeholder items; without the WM_INITMENUPOPUP fix the Universal Copy
///   <see cref="MenuItem.Popup"/> handler never fires and the placeholder is never replaced.
///  </para>
///  <para>
///   These tests simulate the real message sequence Windows delivers when the user hovers a
///   top-level context-menu entry and assert the resulting menu item count and text, mirroring
///   the production patterns in <c>GridColourSchemeManager</c> (Select-based) and
///   <c>ZFilterGridModule.AddUniversalCopyMenuItems</c> (Popup-based).
///  </para>
/// </remarks>
public class LazyMenuPopulationRegressionTests
{
    private const uint WM_MENUSELECT = 0x011F;
    private const uint WM_INITMENUPOPUP = 0x0117;
    private const int MF_POPUP = 0x0010;

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    ///  Grid Colors pattern (<c>GridColourSchemeManager.ToggleAndPopulateGridColourMenuItems</c>):
    ///  the top-level "Grid Colors" menu item subscribes to <see cref="MenuItem.Select"/> and
    ///  repopulates its children from the colour store each time the user hovers it.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   Before the WM_MENUSELECT fix the message was dropped in <see cref="Control.WndProc"/>,
    ///   so <see cref="MenuItem.Select"/> never fired and the submenu stayed on the original
    ///   placeholder items ("Select Color Scheme", "Create New Scheme", "Manage Color Schemes").
    ///   This test verifies the submenu is replaced with live entries after the message lands.
    ///  </para>
    /// </remarks>
    [StaFact]
    public void GridColors_SelectHandler_PopulatesSubMenuItems_WhenWmMenuSelectReceived()
    {
        // Arrange: replicate the GridColourSchemeManager setup — parent item with three static
        // placeholders, replaced on Select by a dynamically sourced set of colour schemes.
        using Control control = new() { Visible = true };

        MenuItem gridColorsItem = new("Grid Colors");
        gridColorsItem.MenuItems.Add(new MenuItem("Select Color Scheme"));
        gridColorsItem.MenuItems.Add(new MenuItem("Create New Scheme"));
        gridColorsItem.MenuItems.Add(new MenuItem("Manage Color Schemes"));

        gridColorsItem.Select += (_, _) =>
        {
            // Mirror ToggleAndPopulateGridColourMenuItems(): clear placeholders, add live entries.
            gridColorsItem.MenuItems.Clear();
            gridColorsItem.MenuItems.Add(new MenuItem("Standard*"));
            gridColorsItem.MenuItems.Add(new MenuItem("Dark Theme"));
            gridColorsItem.MenuItems.Add(new MenuItem("High Contrast"));
        };

        ContextMenu contextMenu = new(new[] { gridColorsItem });
        control.ContextMenu = contextMenu;

        // Force native handles so ProcessMenuSelect can match by HMENU.
        IntPtr controlHandle = control.Handle;
        IntPtr contextMenuHandle = contextMenu.Handle;

        Assert.NotEqual(IntPtr.Zero, controlHandle);
        Assert.NotEqual(IntPtr.Zero, contextMenuHandle);

        // "Grid Colors" is at native index 0 in the context menu; it has children so Windows sets MF_POPUP.
        IntPtr wParam = unchecked((IntPtr)(0 | (MF_POPUP << 16)));

        // Act: simulate Windows delivering WM_MENUSELECT when the user hovers "Grid Colors".
        SendMessage(controlHandle, WM_MENUSELECT, wParam, contextMenuHandle);

        // Assert: the submenu must contain the live entries, not the original placeholders.
        Assert.Equal(3, gridColorsItem.MenuItems.Count);
        Assert.Equal("Standard*",     gridColorsItem.MenuItems[0].Text);
        Assert.Equal("Dark Theme",    gridColorsItem.MenuItems[1].Text);
        Assert.Equal("High Contrast", gridColorsItem.MenuItems[2].Text);
    }

    /// <summary>
    ///  Regression test for the hidden-items index mismatch: when context menu items have
    ///  <c>Visible = false</c>, the native menu skips them, so the native index differs from
    ///  the managed <see cref="Menu.MenuItems"/> index.  The original ProcessMenuSelect used
    ///  <c>MenuItems[nativeIndex]</c> which returned the wrong item.  The fix uses native
    ///  Win32 APIs (GetMenuItemID, GetSubMenu) to navigate the actual menu structure.
    /// </summary>
    [StaFact]
    public void GridColors_SelectHandler_WorksWithHiddenItems_WhenNativeIndexDiffersFromManaged()
    {
        using Control control = new() { Visible = true };

        // Build a context menu with hidden items before Grid Colors to create index mismatch.
        MenuItem viewItem = new("View");
        MenuItem deactivateItem = new("Deactivate") { Visible = false };
        MenuItem activateItem = new("Activate") { Visible = false };
        MenuItem gridColorsItem = new("Grid Colors");
        gridColorsItem.MenuItems.Add(new MenuItem("Placeholder"));

        bool selectFired = false;
        gridColorsItem.Select += (_, _) =>
        {
            selectFired = true;
            gridColorsItem.MenuItems.Clear();
            gridColorsItem.MenuItems.Add(new MenuItem("Scheme A"));
            gridColorsItem.MenuItems.Add(new MenuItem("Scheme B"));
        };

        // Managed indices: View=0, Deactivate=1(hidden), Activate=2(hidden), GridColors=3
        // Native indices:  View=0, GridColors=1 (hidden items skipped)
        ContextMenu contextMenu = new(new[] { viewItem, deactivateItem, activateItem, gridColorsItem });
        control.ContextMenu = contextMenu;

        IntPtr controlHandle = control.Handle;
        IntPtr contextMenuHandle = contextMenu.Handle;

        // Grid Colors is at NATIVE index 1 (after View), not managed index 3.
        IntPtr wParam = unchecked((IntPtr)(1 | (MF_POPUP << 16)));

        SendMessage(controlHandle, WM_MENUSELECT, wParam, contextMenuHandle);

        Assert.True(selectFired, "Select event should fire on Grid Colors despite hidden items shifting the native index.");
        Assert.Equal(2, gridColorsItem.MenuItems.Count);
        Assert.Equal("Scheme A", gridColorsItem.MenuItems[0].Text);
        Assert.Equal("Scheme B", gridColorsItem.MenuItems[1].Text);
    }

    /// <summary>
    ///  Universal Copy pattern (<c>ZFilterGridModule.AddUniversalCopyMenuItems</c> via
    ///  <see cref="MenuItem.Popup"/>): the "Universal Copy" submenu starts with a single
    ///  placeholder and switches to four real entries the first time it is opened.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   The screenshot captures exactly these four entries:
    ///   "New Copy Template", "Edit Copy Template", "New Copy Template From", "Copy Schedules".
    ///   This test asserts all four are present after WM_INITMENUPOPUP is delivered to the owning
    ///   control — proving the user-visible submenu content, not merely that Popup fired.
    ///  </para>
    /// </remarks>
    [StaFact]
    public void UniversalCopy_PopupHandler_PopulatesExactSubMenuItems_WhenWmInitMenuPopupReceived()
    {
        // Arrange: replicate the Universal Copy entry — starts with a loading placeholder,
        // real items added on first Popup (AddUniversalCopyMenuItems pattern from PR #53445).
        using Control control = new() { Visible = true };

        MenuItem universalCopyItem = new("Universal Copy");
        universalCopyItem.MenuItems.Add(new MenuItem("<Loading...>"));   // placeholder

        bool alreadyPopulated = false;
        universalCopyItem.Popup += (_, _) =>
        {
            if (alreadyPopulated) return;
            alreadyPopulated = true;

            // Mirror AddUniversalCopyMenuItems(): replace placeholder with real entries.
            universalCopyItem.MenuItems.Clear();
            universalCopyItem.MenuItems.Add(new MenuItem("New Copy Template"));
            universalCopyItem.MenuItems.Add(new MenuItem("Edit Copy Template"));
            universalCopyItem.MenuItems.Add(new MenuItem("New Copy Template From"));
            universalCopyItem.MenuItems.Add(new MenuItem("Copy Schedules"));
        };

        ContextMenu contextMenu = new(new[] { universalCopyItem });
        control.ContextMenu = contextMenu;

        IntPtr controlHandle = control.Handle;
        IntPtr universalCopyHandle = universalCopyItem.Handle;

        Assert.NotEqual(IntPtr.Zero, controlHandle);
        Assert.NotEqual(IntPtr.Zero, universalCopyHandle);

        // Act: simulate Windows delivering WM_INITMENUPOPUP when Universal Copy is hovered.
        // Before the WM_INITMENUPOPUP fix in Control.WndProc, ProcessInitMenuPopup was never
        // reached, Popup never fired, and the "<Loading...>" placeholder was never replaced.
        SendMessage(controlHandle, WM_INITMENUPOPUP, universalCopyHandle, IntPtr.Zero);

        // Assert: exact submenu items visible in the screenshot.
        Assert.Equal(4, universalCopyItem.MenuItems.Count);
        Assert.Equal("New Copy Template",      universalCopyItem.MenuItems[0].Text);
        Assert.Equal("Edit Copy Template",     universalCopyItem.MenuItems[1].Text);
        Assert.Equal("New Copy Template From", universalCopyItem.MenuItems[2].Text);
        Assert.Equal("Copy Schedules",         universalCopyItem.MenuItems[3].Text);
    }

    /// <summary>
    ///  Regression guard: sending WM_MENUSELECT a second time must not duplicate entries.
    ///  The Grid Colors handler clears and rebuilds on every Select; verifies idempotent
    ///  population when the user repeatedly moves the mouse away and back.
    /// </summary>
    [StaFact]
    public void GridColors_SelectHandler_ReplacesItemsOnRepeatHover_NoDuplicates()
    {
        using Control control = new() { Visible = true };

        MenuItem gridColorsItem = new("Grid Colors");
        gridColorsItem.MenuItems.Add(new MenuItem("Select Color Scheme"));

        int populateCount = 0;
        gridColorsItem.Select += (_, _) =>
        {
            populateCount++;
            gridColorsItem.MenuItems.Clear();
            gridColorsItem.MenuItems.Add(new MenuItem($"Scheme A (call {populateCount})"));
            gridColorsItem.MenuItems.Add(new MenuItem($"Scheme B (call {populateCount})"));
        };

        ContextMenu contextMenu = new(new[] { gridColorsItem });
        control.ContextMenu = contextMenu;

        IntPtr controlHandle = control.Handle;
        IntPtr contextMenuHandle = contextMenu.Handle;
        IntPtr wParam = unchecked((IntPtr)(0 | (MF_POPUP << 16)));

        // First hover
        SendMessage(controlHandle, WM_MENUSELECT, wParam, contextMenuHandle);
        Assert.Equal(2, gridColorsItem.MenuItems.Count);
        Assert.Equal("Scheme A (call 1)", gridColorsItem.MenuItems[0].Text);

        // Second hover — must replace, not append
        SendMessage(controlHandle, WM_MENUSELECT, wParam, contextMenuHandle);
        Assert.Equal(2, gridColorsItem.MenuItems.Count);
        Assert.Equal("Scheme A (call 2)", gridColorsItem.MenuItems[0].Text);
    }
}
