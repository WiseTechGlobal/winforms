using System.Drawing;
using System.Windows.Forms;

namespace Demo;

public partial class ToolBarForm : Form
{
    private readonly ToolBar _demoToolBar;
    private ToolBarButton _newButton = null!;
    private ToolBarButton _pinButton = null!;
    private ToolBarButton _formatButton = null!;
    private ToolBarButton _disabledButton = null!;
    private ToolBarButton _partialStateButton = null!;
    private ToolBarButton _previewButton = null!;
    private ToolBarButton _timestampButton = null!;
    private ToolBarButton _historyButton = null!;
    private bool _advancedStatePackEnabled;

    public ToolBarForm()
    {
        InitializeComponent();

        _demoToolBar = CreateToolBar();
        _toolbarHostPanel.Controls.Add(_demoToolBar);

        ResetDemoSurface(clearLog: false);
        SetStatus(
            "Demo ready",
            "Start with New, State, Pin, Format, and History. Turn on Advanced State Pack to disable State, partially push Partial, and hide Preview.");
        AppendLog("ToolBar demo ready.");
    }

    private ToolBar CreateToolBar()
    {
        ToolBar toolBar = new()
        {
            Dock = DockStyle.Top,
            ButtonSize = new Size(92, 36),
            TextAlign = ToolBarTextAlign.Right,
            ShowToolTips = true,
            DropDownArrows = true,
            Wrappable = false
        };

        _newButton = new("New")
        {
            ToolTipText = "Basic push button command"
        };
        toolBar.Buttons.Add(_newButton);

        toolBar.Buttons.Add(new ToolBarButton("sep1") { Style = ToolBarButtonStyle.Separator });

        _pinButton = new("Pin")
        {
            Style = ToolBarButtonStyle.ToggleButton,
            ToolTipText = "Toggle button showing checked state"
        };
        toolBar.Buttons.Add(_pinButton);

        _formatButton = new("Format")
        {
            Style = ToolBarButtonStyle.DropDownButton,
            ToolTipText = "Drop-down button with command menu"
        };

        MenuItem waveMenuItem = new("Wave", WaveMenuItem_Click);
        MenuItem statusMenuItem = new("Write Status", WriteStatusMenuItem_Click);
        MenuItem resetDemoMenuItem = new("Reset Demo", ResetDemoMenuItem_Click);
        _formatButton.DropDownMenu = new ContextMenu([waveMenuItem, statusMenuItem, resetDemoMenuItem]);
        toolBar.Buttons.Add(_formatButton);

        toolBar.Buttons.Add(new ToolBarButton("sep2") { Style = ToolBarButtonStyle.Separator });

        _disabledButton = new("State")
        {
            ToolTipText = "Regular command that becomes disabled when Advanced State Pack is on."
        };
        toolBar.Buttons.Add(_disabledButton);

        _partialStateButton = new("Partial")
        {
            Style = ToolBarButtonStyle.ToggleButton,
            ToolTipText = "Toggle button that switches to a partial-push sample when Advanced State Pack is on."
        };
        toolBar.Buttons.Add(_partialStateButton);

        _previewButton = new("Preview")
        {
            ToolTipText = "Sample button that can be hidden by the advanced state pack"
        };
        toolBar.Buttons.Add(_previewButton);

        toolBar.Buttons.Add(new ToolBarButton("sep3") { Style = ToolBarButtonStyle.Separator });

        _timestampButton = new("Stamp")
        {
            ToolTipText = "Writes a timestamp to the status area"
        };
        toolBar.Buttons.Add(_timestampButton);

        _historyButton = new("History")
        {
            Style = ToolBarButtonStyle.DropDownButton,
            ToolTipText = "Secondary drop-down button"
        };
        _historyButton.DropDownMenu = new ContextMenu(
        [
            new MenuItem("Show Timestamp", WriteStatusMenuItem_Click),
            new MenuItem("Toggle Advanced State", ToggleAdvancedStateMenuItem_Click),
            new MenuItem("Reset Demo", ResetDemoMenuItem_Click)
        ]);
        toolBar.Buttons.Add(_historyButton);

        toolBar.ButtonClick += DemoToolBar_ButtonClick;
        toolBar.ButtonDropDown += DemoToolBar_ButtonDropDown;

        return toolBar;
    }

    private void DemoToolBar_ButtonClick(object? sender, ToolBarButtonClickEventArgs e)
    {
        if (ReferenceEquals(e.Button, _newButton))
        {
            SetStatus("Basic push command", "The New button behaves like a standard command-bar action.");
            AppendLog("Toolbar push button clicked: New");

            return;
        }

        if (ReferenceEquals(e.Button, _pinButton))
        {
            SetStatus("Toggle button", "Pin is now " + (_pinButton.Pushed ? "pressed" : "released") + '.');
            AppendLog("Toolbar toggle button changed: Pin = " + _pinButton.Pushed);

            return;
        }

        if (ReferenceEquals(e.Button, _disabledButton))
        {
            SetStatus(
                "State transition sample",
                "State is interactive now. Turn on Advanced State Pack to disable this slot and compare the visual change.");
            AppendLog("Toolbar state sample clicked while enabled.");

            return;
        }

        if (ReferenceEquals(e.Button, _partialStateButton))
        {
            SetStatus(
                "Partial push sample",
                "Partial = " + _partialStateButton.PartialPush + ", pushed = " + _partialStateButton.Pushed + '.');
            AppendLog(
                "Toolbar partial state clicked: Partial = "
                + _partialStateButton.PartialPush
                + ", Pushed = "
                + _partialStateButton.Pushed);

            return;
        }

        if (ReferenceEquals(e.Button, _previewButton))
        {
            SetStatus("Visibility sample", "Preview stays clickable until the advanced state pack hides it.");
            AppendLog("Toolbar sample button clicked: Preview");

            return;
        }

        if (ReferenceEquals(e.Button, _timestampButton))
        {
            WriteTimestampStatus("Toolbar push button clicked");

            return;
        }

        _statusLabel.Text = "Clicked: " + e.Button.Text;
        AppendLog("Toolbar button clicked: " + e.Button.Text);
    }

    private void DemoToolBar_ButtonDropDown(object? sender, ToolBarButtonClickEventArgs e)
    {
        SetStatus("Drop-down button", e.Button.Text + " opened its command menu.");
        AppendLog("Toolbar drop-down opened: " + e.Button.Text);
    }

    private void WaveMenuItem_Click(object? sender, EventArgs e)
    {
        SetStatus("Drop-down command", "Wave command selected from the toolbar menu.");
        AppendLog("Toolbar drop-down menu clicked: Wave");
        MessageBox.Show(this, "Wave back", Text);
    }

    private void WriteStatusMenuItem_Click(object? sender, EventArgs e)
    {
        WriteTimestampStatus("Toolbar drop-down menu clicked");
    }

    private void ResetDemoMenuItem_Click(object? sender, EventArgs e)
    {
        ResetDemoSurface(clearLog: true);
        SetStatus("Drop-down command", "The demo returned to its baseline state and the log was cleared.");
        AppendLog("Toolbar drop-down menu clicked: Reset Demo");
    }

    private void ToggleAdvancedStateMenuItem_Click(object? sender, EventArgs e)
    {
        ApplyAdvancedStatePack(!_advancedStatePackEnabled);
        SetStatus("Drop-down command", GetAdvancedStateSummary());
        AppendLog("Toolbar drop-down menu clicked: Toggle Advanced State = " + _advancedStatePackEnabled);
    }

    private void ToggleAppearanceButton_Click(object? sender, EventArgs e)
    {
        _demoToolBar.Appearance = _demoToolBar.Appearance == ToolBarAppearance.Normal
            ? ToolBarAppearance.Flat
            : ToolBarAppearance.Normal;

        UpdateToolbarSettingButtons();
        SetStatus("Appearance changed", "ToolBar.Appearance → " + _demoToolBar.Appearance);
        AppendLog("ToolBar.Appearance toggled to: " + _demoToolBar.Appearance);
    }

    private void ToggleWrappableButton_Click(object? sender, EventArgs e)
    {
        _demoToolBar.Wrappable = !_demoToolBar.Wrappable;

        UpdateToolbarSettingButtons();
        SetStatus("Layout changed", "ToolBar.Wrappable → " + _demoToolBar.Wrappable);
        AppendLog("ToolBar.Wrappable toggled to: " + _demoToolBar.Wrappable);
    }

    private void ToggleDividerButton_Click(object? sender, EventArgs e)
    {
        _demoToolBar.Divider = !_demoToolBar.Divider;

        UpdateToolbarSettingButtons();
        SetStatus("Divider changed", "ToolBar.Divider → " + _demoToolBar.Divider);
        AppendLog("ToolBar.Divider toggled to: " + _demoToolBar.Divider);
    }

    private void ToggleToolTipsButton_Click(object? sender, EventArgs e)
    {
        _demoToolBar.ShowToolTips = !_demoToolBar.ShowToolTips;

        UpdateToolbarSettingButtons();
        SetStatus("Tool tips changed", "ToolBar.ShowToolTips → " + _demoToolBar.ShowToolTips);
        AppendLog("ToolBar.ShowToolTips toggled to: " + _demoToolBar.ShowToolTips);
    }

    private void ToggleAdvancedStateButton_Click(object? sender, EventArgs e)
    {
        ApplyAdvancedStatePack(!_advancedStatePackEnabled);
        SetStatus("Advanced state pack", GetAdvancedStateSummary());
        AppendLog("Advanced toolbar state pack toggled to: " + _advancedStatePackEnabled);
    }

    private void ClearLogButton_Click(object? sender, EventArgs e)
    {
        _eventLog.Items.Clear();
        AppendLog("Log cleared.");
    }

    private void ApplyAdvancedStatePack(bool enabled)
    {
        _advancedStatePackEnabled = enabled;

        _disabledButton.Text = enabled ? "Locked" : "State";
        _disabledButton.ToolTipText = enabled
            ? "Disabled command sample. Turn Advanced State Pack off to make it interactive again."
            : "Regular command that becomes disabled when Advanced State Pack is on.";
        _disabledButton.Enabled = !enabled;

        _partialStateButton.PartialPush = enabled;
        _partialStateButton.Pushed = enabled;
        _partialStateButton.ToolTipText = enabled
            ? "Advanced state demo: partial push is active."
            : "Toggle button that switches to a partial-push sample when Advanced State Pack is on.";

        _previewButton.Visible = !enabled;
        _previewButton.ToolTipText = enabled
            ? "Preview is hidden while Advanced State Pack is on."
            : "Sample button that becomes hidden when Advanced State Pack is on.";

        UpdateToolbarSettingButtons();
    }

    private string GetAdvancedStateSummary()
    {
        return _advancedStatePackEnabled
            ? "State is disabled, Partial is partially pushed, and Preview is hidden."
            : "State is enabled, Partial is reset, and Preview is visible again.";
    }

    private void ResetDemoSurface(bool clearLog)
    {
        _demoToolBar.Appearance = ToolBarAppearance.Normal;
        _demoToolBar.Wrappable = false;
        _demoToolBar.TextAlign = ToolBarTextAlign.Right;
        _demoToolBar.Divider = true;
        _demoToolBar.ShowToolTips = true;
        _pinButton.Pushed = false;

        ApplyAdvancedStatePack(enabled: false);

        if (clearLog)
        {
            _eventLog.Items.Clear();
        }

        UpdateToolbarSettingButtons();
    }

    private void SetStatus(string title, string detail)
    {
        _statusLabel.Text = title + ": " + detail;
    }

    private void UpdateToolbarSettingButtons()
    {
        _toggleAppearanceButton.Text = "Appearance: " + _demoToolBar.Appearance;
        _toggleWrappableButton.Text = "Wrappable: " + _demoToolBar.Wrappable;
        _toggleDividerButton.Text = "Toolbar Divider: " + _demoToolBar.Divider;
        _toggleToolTipsButton.Text = "ToolTips: " + _demoToolBar.ShowToolTips;
        _toggleAdvancedStateButton.Text = "Advanced State Pack: " + (_advancedStatePackEnabled ? "On" : "Off");
    }

    private void WriteTimestampStatus(string source)
    {
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        SetStatus("Timestamp command", "Status updated at " + timestamp + '.');
        AppendLog(source + " at " + timestamp);
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
