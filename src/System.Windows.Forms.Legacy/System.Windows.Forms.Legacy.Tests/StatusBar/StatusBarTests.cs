using System.Drawing;

namespace System.Windows.Forms.Legacy.Tests;

public class StatusBarTests
{
    [StaFact]
    public void StatusBar_DefaultProperties_MatchExpectedValues()
    {
        using StatusBar statusBar = new();

        Assert.Equal(DockStyle.Bottom, statusBar.Dock);
        Assert.False(statusBar.ShowPanels);
        Assert.True(statusBar.SizingGrip);
        Assert.False(statusBar.TabStop);
        Assert.Equal(string.Empty, statusBar.Text);
        Assert.Empty(statusBar.Panels);
    }

    [StaFact]
    public void StatusBar_Panels_AddRange_SetsPanelCountAndParent()
    {
        using StatusBar statusBar = new();
        StatusBarPanel panel1 = new() { Text = "Panel 1" };
        StatusBarPanel panel2 = new() { Text = "Panel 2" };
        StatusBarPanel panel3 = new() { Text = "Panel 3" };

        statusBar.Panels.AddRange([panel1, panel2, panel3]);

        Assert.Equal(3, statusBar.Panels.Count);
        Assert.Same(panel1, statusBar.Panels[0]);
        Assert.Same(panel2, statusBar.Panels[1]);
        Assert.Same(panel3, statusBar.Panels[2]);
        Assert.Same(statusBar, panel1.Parent);
        Assert.Same(statusBar, panel2.Parent);
        Assert.Same(statusBar, panel3.Parent);
    }

    [StaFact]
    public void StatusBar_Panels_AddByText_CreatesAndReturnsPanel()
    {
        using StatusBar statusBar = new();

        StatusBarPanel panel = statusBar.Panels.Add("Ready");

        Assert.Single(statusBar.Panels);
        Assert.Equal("Ready", panel.Text);
        Assert.Same(statusBar, panel.Parent);
    }

    [StaFact]
    public void StatusBar_Panels_Insert_PlacesAtCorrectIndex()
    {
        using StatusBar statusBar = new();
        StatusBarPanel first = new() { Name = "first", Text = "First" };
        StatusBarPanel last = new() { Name = "last", Text = "Last" };
        StatusBarPanel inserted = new() { Name = "inserted", Text = "Inserted" };

        statusBar.Panels.AddRange([first, last]);

        statusBar.Panels.Insert(1, inserted);

        Assert.Equal(3, statusBar.Panels.Count);
        Assert.Same(first, statusBar.Panels[0]);
        Assert.Same(inserted, statusBar.Panels[1]);
        Assert.Same(last, statusBar.Panels[2]);
        Assert.Same(statusBar, inserted.Parent);
    }

    [StaFact]
    public void StatusBar_Panels_Remove_ClearsParentReference()
    {
        using StatusBar statusBar = new();
        StatusBarPanel panel = new() { Text = "Removable" };
        statusBar.Panels.Add(panel);

        statusBar.Panels.Remove(panel);

        Assert.Empty(statusBar.Panels);
        Assert.Null(panel.Parent);
    }

    [StaFact]
    public void StatusBar_Panels_RemoveAt_RemovesByIndexAndClearsParent()
    {
        using StatusBar statusBar = new();
        StatusBarPanel keep = new() { Text = "Keep" };
        StatusBarPanel remove = new() { Text = "Remove" };
        statusBar.Panels.AddRange([keep, remove]);

        statusBar.Panels.RemoveAt(1);

        Assert.Single(statusBar.Panels);
        Assert.Same(keep, statusBar.Panels[0]);
        Assert.Null(remove.Parent);
    }

    [StaFact]
    public void StatusBar_Panels_Clear_RemovesAllPanelsAndClearsParents()
    {
        using StatusBar statusBar = new();
        StatusBarPanel panel1 = new() { Text = "A" };
        StatusBarPanel panel2 = new() { Text = "B" };
        statusBar.Panels.AddRange([panel1, panel2]);

        statusBar.Panels.Clear();

        Assert.Empty(statusBar.Panels);
        Assert.Null(panel1.Parent);
        Assert.Null(panel2.Parent);
    }

    [StaFact]
    public void StatusBar_Panels_Contains_ReturnsTrueForAddedPanel_FalseAfterRemoval()
    {
        using StatusBar statusBar = new();
        StatusBarPanel panel = new() { Text = "Test" };

        statusBar.Panels.Add(panel);

        Assert.True(statusBar.Panels.Contains(panel));

        statusBar.Panels.Remove(panel);

        Assert.False(statusBar.Panels.Contains(panel));
    }

    [StaFact]
    public void StatusBar_Panels_ContainsKey_FindsByName()
    {
        using StatusBar statusBar = new();
        statusBar.Panels.Add(new StatusBarPanel { Name = "clock", Text = "12:00" });

        Assert.True(statusBar.Panels.ContainsKey("clock"));
        Assert.False(statusBar.Panels.ContainsKey("missing"));
    }

    [StaFact]
    public void StatusBar_Panels_IndexOf_ReturnsCorrectPosition()
    {
        using StatusBar statusBar = new();
        StatusBarPanel panel1 = new() { Name = "mode" };
        StatusBarPanel panel2 = new() { Name = "detail" };
        statusBar.Panels.AddRange([panel1, panel2]);

        Assert.Equal(0, statusBar.Panels.IndexOf(panel1));
        Assert.Equal(1, statusBar.Panels.IndexOf(panel2));
        Assert.Equal(0, statusBar.Panels.IndexOfKey("mode"));
        Assert.Equal(1, statusBar.Panels.IndexOfKey("detail"));
        Assert.Equal(-1, statusBar.Panels.IndexOfKey("missing"));
    }

    [StaFact]
    public void StatusBar_ShowPanels_ToggleToFalse_SetsSimpleTextWithoutLosingPanels()
    {
        using Form form = new() { ClientSize = new Size(600, 300) };
        using StatusBar statusBar = new() { ShowPanels = true };
        StatusBarPanel panel = new() { Text = "Status" };
        statusBar.Panels.Add(panel);
        form.Controls.Add(statusBar);
        _ = form.Handle;

        statusBar.ShowPanels = false;
        statusBar.Text = "Ready";

        Assert.False(statusBar.ShowPanels);
        Assert.Equal("Ready", statusBar.Text);
        Assert.Single(statusBar.Panels);
        Assert.Same(statusBar, panel.Parent);
    }

    [StaFact]
    public void StatusBar_ShowPanels_RestoreToTrue_ReexposesExistingPanels()
    {
        using Form form = new() { ClientSize = new Size(600, 300) };
        using StatusBar statusBar = new() { ShowPanels = true };
        StatusBarPanel panel = new() { Name = "mode", Text = "Panels Mode" };
        statusBar.Panels.Add(panel);
        form.Controls.Add(statusBar);
        _ = form.Handle;

        statusBar.ShowPanels = false;
        statusBar.ShowPanels = true;
        panel.Text = "Panels Mode";

        Assert.True(statusBar.ShowPanels);
        Assert.Single(statusBar.Panels);
        Assert.Equal("Panels Mode", statusBar.Panels[0].Text);
    }

    [StaFact]
    public void StatusBar_ToString_IncludesPanelCountAndFirstPanelText()
    {
        using StatusBar statusBar = new();
        statusBar.Panels.AddRange([
            new StatusBarPanel { Text = "Ready" },
            new StatusBarPanel { Text = "12:00" }
        ]);

        string result = statusBar.ToString();

        Assert.Contains("Panels.Count: 2", result);
        Assert.Contains("StatusBarPanel: {Ready}", result);
    }

    [StaFact]
    public void StatusBarPanel_Alignment_DefaultIsLeft_CanBeChangedToRightAndCenter()
    {
        StatusBarPanel panel = new();

        Assert.Equal(HorizontalAlignment.Left, panel.Alignment);

        panel.Alignment = HorizontalAlignment.Right;
        Assert.Equal(HorizontalAlignment.Right, panel.Alignment);

        panel.Alignment = HorizontalAlignment.Center;
        Assert.Equal(HorizontalAlignment.Center, panel.Alignment);
    }

    [StaFact]
    public void StatusBarPanel_AutoSize_DefaultIsNone_AllValuesSupported()
    {
        StatusBarPanel panel = new();

        Assert.Equal(StatusBarPanelAutoSize.None, panel.AutoSize);

        panel.AutoSize = StatusBarPanelAutoSize.Spring;
        Assert.Equal(StatusBarPanelAutoSize.Spring, panel.AutoSize);

        panel.AutoSize = StatusBarPanelAutoSize.Contents;
        Assert.Equal(StatusBarPanelAutoSize.Contents, panel.AutoSize);
    }

    [StaFact]
    public void StatusBarPanel_BorderStyle_DefaultIsSunken_AllValuesSupported()
    {
        StatusBarPanel panel = new();

        Assert.Equal(StatusBarPanelBorderStyle.Sunken, panel.BorderStyle);

        panel.BorderStyle = StatusBarPanelBorderStyle.Raised;
        Assert.Equal(StatusBarPanelBorderStyle.Raised, panel.BorderStyle);

        panel.BorderStyle = StatusBarPanelBorderStyle.None;
        Assert.Equal(StatusBarPanelBorderStyle.None, panel.BorderStyle);
    }

    [StaFact]
    public void StatusBarPanel_Style_DefaultIsText_OwnerDrawSupported()
    {
        StatusBarPanel panel = new();

        Assert.Equal(StatusBarPanelStyle.Text, panel.Style);

        panel.Style = StatusBarPanelStyle.OwnerDraw;

        Assert.Equal(StatusBarPanelStyle.OwnerDraw, panel.Style);
    }

    [StaFact]
    public void StatusBarPanel_Width_ThrowsWhenSetBelowMinWidth()
    {
        StatusBarPanel panel = new() { MinWidth = 50 };

        Assert.Throws<ArgumentOutOfRangeException>(() => panel.Width = 30);
    }

    [StaFact]
    public void StatusBarPanel_MinWidth_WhenRaisedAboveCurrentWidth_ExpandsWidth()
    {
        StatusBarPanel panel = new() { Width = 60, MinWidth = 10 };

        panel.MinWidth = 80;

        Assert.Equal(80, panel.MinWidth);
        Assert.Equal(80, panel.Width);
    }

    [StaFact]
    public void StatusBarPanel_ToolTipText_DefaultIsEmpty_CanBeSet()
    {
        StatusBarPanel panel = new();

        Assert.Equal(string.Empty, panel.ToolTipText);

        panel.ToolTipText = "Shows the current connection status.";

        Assert.Equal("Shows the current connection status.", panel.ToolTipText);
    }

    [StaFact]
    public void StatusBarPanel_ToString_IncludesText()
    {
        StatusBarPanel panel = new() { Text = "Ready" };

        Assert.Equal("StatusBarPanel: {Ready}", panel.ToString());
    }

    [StaFact]
    public void StatusBar_PanelClick_Event_FiredThroughOnPanelClick()
    {
        using SubStatusBar statusBar = new();
        StatusBarPanel panel = new() { Text = "Clickable" };
        statusBar.Panels.Add(panel);

        StatusBarPanelClickEventArgs? received = null;
        statusBar.PanelClick += (_, e) => received = e;

        StatusBarPanelClickEventArgs args = new(panel, MouseButtons.Left, 1, 10, 5);
        statusBar.RaisePanelClick(args);

        Assert.NotNull(received);
        Assert.Same(panel, received.StatusBarPanel);
        Assert.Equal(MouseButtons.Left, received.Button);
        Assert.Equal(1, received.Clicks);
    }

    [StaFact]
    public void StatusBar_DrawItem_Event_FiredThroughOnDrawItem()
    {
        using SubStatusBar statusBar = new();
        StatusBarPanel panel = new() { Style = StatusBarPanelStyle.OwnerDraw, Width = 100 };
        statusBar.Panels.Add(panel);

        StatusBarDrawItemEventArgs? received = null;
        statusBar.DrawItem += (_, e) => received = e;

        using Bitmap bitmap = new(10, 10);
        using Graphics g = Graphics.FromImage(bitmap);
        StatusBarDrawItemEventArgs args = new(g, SystemFonts.DefaultFont!, Rectangle.Empty, 0, DrawItemState.None, panel);
        statusBar.RaiseDrawItem(args);

        Assert.NotNull(received);
        Assert.Same(panel, received.Panel);
        Assert.Equal(DrawItemState.None, received.State);
    }

    private sealed class SubStatusBar : StatusBar
    {
        public void RaisePanelClick(StatusBarPanelClickEventArgs e) => OnPanelClick(e);

        public void RaiseDrawItem(StatusBarDrawItemEventArgs e) => OnDrawItem(e);
    }
}
