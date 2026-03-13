using System.Drawing;
using System.Windows.Forms;

namespace Demo;

public partial class StatusBarForm : Form
{
    private readonly StatusBar _statusBar;
    private StatusBarPanel _modePanel;
    private StatusBarPanel _detailsPanel;
    private StatusBarPanel _ownerDrawPanel;
    private StatusBarPanel _clockPanel;
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
        _detailsPanel = new StatusBarPanel
        {
            Text = "Click a panel or use the controls above to mutate the legacy StatusBar surface.",
            AutoSize = StatusBarPanelAutoSize.Spring,
            ToolTipText = "Spring-sized panel text."
        };
        _ownerDrawPanel = new StatusBarPanel
        {
            Style = StatusBarPanelStyle.OwnerDraw,
            Width = 150,
            ToolTipText = "Owner-drawn panel that displays the current panel count."
        };
        _clockPanel = new StatusBarPanel
        {
            Text = DateTime.Now.ToString("HH:mm:ss"),
            AutoSize = StatusBarPanelAutoSize.Contents,
            Alignment = HorizontalAlignment.Right,
            ToolTipText = "Clock panel updated once per second."
        };

        statusBar.Panels.AddRange([_modePanel, _detailsPanel, _ownerDrawPanel, _clockPanel]);
        statusBar.PanelClick += StatusBar_PanelClick;
        statusBar.DrawItem += StatusBar_DrawItem;

        return statusBar;
    }

    private void StatusBar_DrawItem(object sender, StatusBarDrawItemEventArgs e)
    {
        using SolidBrush backgroundBrush = new(Color.FromArgb(231, 238, 247));
        using Pen borderPen = new(Color.SteelBlue);

        e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
        Rectangle borderBounds = Rectangle.Inflate(e.Bounds, -1, -1);
        e.Graphics.DrawRectangle(borderPen, borderBounds);

        TextRenderer.DrawText(
            e.Graphics,
            $"Panels: {_statusBar.Panels.Count}",
            Font,
            e.Bounds,
            Color.MidnightBlue,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
    }

    private void StatusBar_PanelClick(object sender, StatusBarPanelClickEventArgs e)
    {
        _detailsPanel.Text = "Clicked panel: " + e.StatusBarPanel.Text;
        AppendLog("Panel click: " + e.StatusBarPanel.Text);
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
            ToolTipText = "Runtime-added panel #" + _dynamicPanelCount
        };

        _statusBar.Panels.Insert(Math.Max(0, _statusBar.Panels.Count - 1), panel);
        _statusBar.Invalidate();
        AppendLog("Added dynamic panel: " + panel.Text);
    }

    private void RemovePanelButton_Click(object? sender, EventArgs e)
    {
        if (_statusBar.Panels.Count <= 4)
        {
            AppendLog("No dynamic panel available to remove.");

            return;
        }

        StatusBarPanel panel = _statusBar.Panels[_statusBar.Panels.Count - 2];
        _statusBar.Panels.Remove(panel);
        _statusBar.Invalidate();
        AppendLog("Removed dynamic panel: " + panel.Text);
    }

    private void ClearLogButton_Click(object? sender, EventArgs e)
    {
        _eventLog.Items.Clear();
        AppendLog("Log cleared.");
    }

    private void ClockTimer_Tick(object? sender, EventArgs e)
    {
        _clockPanel.Text = DateTime.Now.ToString("HH:mm:ss");
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
