using System.Drawing;
using System.Windows.Forms;

namespace Demo;

public partial class StatusBarForm : Form
{
    private readonly StatusBar _statusBar;
    private StatusBarPanel _modePanel = null!;
    private StatusBarPanel _borderSamplePanel = null!;
    private StatusBarPanel _detailsPanel = null!;
    private StatusBarPanel _ownerDrawPanel = null!;
    private StatusBarPanel _clockPanel = null!;
    private int _dynamicPanelCount;

    public StatusBarForm()
    {
        InitializeComponent();

        _statusBar = CreateStatusBar();
        Controls.Add(_statusBar);

        _clockTimer.Start();
        AppendLog("StatusBar demo ready.");
    }

    private StatusBar CreateStatusBar()
    {
        StatusBar statusBar = new()
        {
            Dock = DockStyle.Bottom,
            ShowPanels = true,
            SizingGrip = true
        };

        _modePanel = new StatusBarPanel
        {
            Text = "Panels Mode",
            AutoSize = StatusBarPanelAutoSize.Contents,
            ToolTipText = "The current StatusBar presentation mode."
        };
        _borderSamplePanel = new StatusBarPanel
        {
            Text = "Border: Sunken",
            Width = 120,
            ToolTipText = "Dedicated panel for border-style changes."
        };
        _detailsPanel = new StatusBarPanel
        {
            Text = "Click a panel or use the controls above to mutate the legacy StatusBar surface.",
            AutoSize = StatusBarPanelAutoSize.Spring,
            ToolTipText = "Spring-sized panel text."
        };
        _ownerDrawPanel = new StatusBarPanel
        {
            Style = StatusBarPanelStyle.OwnerDraw,
            Width = 180,
            ToolTipText = "Owner-drawn panel that displays the current panel count."
        };
        _clockPanel = new StatusBarPanel
        {
            Text = DateTime.Now.ToString("HH:mm:ss"),
            AutoSize = StatusBarPanelAutoSize.Contents,
            Alignment = HorizontalAlignment.Right,
            ToolTipText = "Clock panel updated once per second."
        };

        statusBar.Panels.AddRange([_modePanel, _borderSamplePanel, _detailsPanel, _ownerDrawPanel, _clockPanel]);
        statusBar.PanelClick += StatusBar_PanelClick;
        statusBar.DrawItem += StatusBar_DrawItem;

        return statusBar;
    }

    private void StatusBar_DrawItem(object? sender, StatusBarDrawItemEventArgs e)
    {
        using SolidBrush backgroundBrush = new(Color.FromArgb(231, 238, 247));
        using Pen borderPen = new(Color.SteelBlue);

        e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
        Rectangle borderBounds = Rectangle.Inflate(e.Bounds, -1, -1);
        e.Graphics.DrawRectangle(borderPen, borderBounds);

        TextRenderer.DrawText(
            e.Graphics,
            $"Panels: {_statusBar.Panels.Count} | Grip: {(_statusBar.SizingGrip ? "On" : "Off")}",
            Font,
            e.Bounds,
            Color.MidnightBlue,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
    }

    private void StatusBar_PanelClick(object? sender, StatusBarPanelClickEventArgs e)
    {
        _detailsPanel.Text = "Clicked panel: " + e.StatusBarPanel.Text;
        AppendLog("Panel click: " + e.StatusBarPanel.Text);
    }

    private static StatusBarPanelBorderStyle GetNextBorderStyle(StatusBarPanelBorderStyle borderStyle) =>
        borderStyle switch
        {
            StatusBarPanelBorderStyle.None => StatusBarPanelBorderStyle.Raised,
            StatusBarPanelBorderStyle.Raised => StatusBarPanelBorderStyle.Sunken,
            _ => StatusBarPanelBorderStyle.None
        };

    private void ApplyBorderStyle(StatusBarPanelBorderStyle borderStyle)
    {
        _modePanel.BorderStyle = borderStyle;
        _borderSamplePanel.BorderStyle = borderStyle;
        _detailsPanel.BorderStyle = borderStyle;
        _clockPanel.BorderStyle = borderStyle;

        foreach (StatusBarPanel panel in _statusBar.Panels)
        {
            if (panel.Style == StatusBarPanelStyle.OwnerDraw)
            {
                continue;
            }

            panel.BorderStyle = borderStyle;
        }

        _borderSamplePanel.Text = "Border: " + borderStyle;
        _detailsPanel.Text = borderStyle switch
        {
            StatusBarPanelBorderStyle.None => "Flat separators removed across the standard panels.",
            StatusBarPanelBorderStyle.Raised => "Raised borders applied across the standard panels.",
            _ => "Sunken borders applied across the standard panels."
        };

        _statusBar.Refresh();
    }

    private void ShowPanelsButton_Click(object? sender, EventArgs e)
    {
        _statusBar.ShowPanels = true;
        _modePanel.Text = "Panels Mode";
        _detailsPanel.Text = "Panels restored with spring, owner-draw, and clock behavior.";
        _statusBar.Invalidate();
        AppendLog("Switched StatusBar to panel mode.");
    }

    private void ShowSimpleTextButton_Click(object? sender, EventArgs e)
    {
        _statusBar.ShowPanels = false;
        _statusBar.Text = "Simple text mode at " + DateTime.Now.ToString("HH:mm:ss");
        AppendLog("Switched StatusBar to simple text mode.");
    }

    private void AddPanelButton_Click(object? sender, EventArgs e)
    {
        _dynamicPanelCount++;
        StatusBarPanel panel = new()
        {
            Text = "Dynamic " + _dynamicPanelCount,
            AutoSize = StatusBarPanelAutoSize.Contents,
            BorderStyle = _borderSamplePanel.BorderStyle,
            ToolTipText = "Runtime-added panel #" + _dynamicPanelCount
        };

        _statusBar.Panels.Insert(Math.Max(0, _statusBar.Panels.Count - 1), panel);
        _statusBar.Invalidate();
        AppendLog("Added dynamic panel: " + panel.Text);
    }

    private void RemovePanelButton_Click(object? sender, EventArgs e)
    {
        if (_statusBar.Panels.Count <= 5)
        {
            AppendLog("No dynamic panel available to remove.");

            return;
        }

        StatusBarPanel panel = _statusBar.Panels[_statusBar.Panels.Count - 2];
        _statusBar.Panels.Remove(panel);
        _statusBar.Invalidate();
        AppendLog("Removed dynamic panel: " + panel.Text);
    }

    private void ToggleSizingGripButton_Click(object? sender, EventArgs e)
    {
        _statusBar.SizingGrip = !_statusBar.SizingGrip;
        _statusBar.Invalidate();
        AppendLog("Sizing grip " + (_statusBar.SizingGrip ? "enabled." : "disabled."));
    }

    private void CycleBorderStyleButton_Click(object? sender, EventArgs e)
    {
        StatusBarPanelBorderStyle borderStyle = GetNextBorderStyle(_borderSamplePanel.BorderStyle);

        ApplyBorderStyle(borderStyle);
        AppendLog("Standard panel border style set to " + borderStyle + '.');
    }

    private void ClearLogButton_Click(object? sender, EventArgs e)
    {
        _eventLog.Items.Clear();
        AppendLog("Log cleared.");
    }

    private void ClockTimer_Tick(object? sender, EventArgs e)
    {
        string currentTime = DateTime.Now.ToString("HH:mm:ss");

        if (_statusBar.ShowPanels)
        {
            _clockPanel.Text = currentTime;

            return;
        }

        _statusBar.Text = "Simple text mode at " + currentTime;
    }

    private void AppendLog(string message)
    {
        _eventLog.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {message}");

        if (_eventLog.Items.Count > 200)
        {
            _eventLog.Items.RemoveAt(_eventLog.Items.Count - 1);
        }
    }
}
