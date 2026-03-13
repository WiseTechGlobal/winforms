using System.Windows.Forms;

namespace Demo;

public partial class ToolBarForm : Form
{
    private readonly ToolBar _demoToolBar;

    public ToolBarForm()
    {
        InitializeComponent();

        _demoToolBar = CreateToolBar();
        Controls.Add(_demoToolBar);

        AppendLog("ToolBar demo ready.");
    }

    private ToolBar CreateToolBar()
    {
        ToolBar toolBar = new()
        {
            TextAlign = ToolBarTextAlign.Right,
            ShowToolTips = true,
            DropDownArrows = true,
            Wrappable = false
        };

        toolBar.Buttons.Add("1st button");
        ToolBarButton firstButton = toolBar.Buttons[0];
        firstButton.ToolTipText = "This is the first button";

        ToolBarButton separatorButton = new("sep1")
        {
            Style = ToolBarButtonStyle.Separator
        };
        toolBar.Buttons.Add(separatorButton);

        ToolBarButton toggleButton = new("btn2 toggle")
        {
            Style = ToolBarButtonStyle.ToggleButton,
            ToolTipText = "This is the second button"
        };
        toolBar.Buttons.Add(toggleButton);

        ToolBarButton dropDownButton = new("btn3 drop-down")
        {
            Style = ToolBarButtonStyle.DropDownButton,
            ToolTipText = "This is the third button"
        };

        MenuItem waveMenuItem = new("Wave", WaveMenuItem_Click);
        MenuItem statusMenuItem = new("Write Status", WriteStatusMenuItem_Click);
        dropDownButton.DropDownMenu = new ContextMenu([waveMenuItem, statusMenuItem]);
        toolBar.Buttons.Add(dropDownButton);

        toolBar.Buttons.Add(new ToolBarButton("sep2") { Style = ToolBarButtonStyle.Separator });

        toolBar.Buttons.Add(new ToolBarButton("btn4 push") { ToolTipText = "Push button 4" });
        toolBar.Buttons.Add(new ToolBarButton("btn5 toggle") { Style = ToolBarButtonStyle.ToggleButton, ToolTipText = "Toggle button 5" });
        toolBar.Buttons.Add(new ToolBarButton("sep3") { Style = ToolBarButtonStyle.Separator });

        ToolBarButton dropDownButton2 = new("btn6 drop-down")
        {
            Style = ToolBarButtonStyle.DropDownButton,
            ToolTipText = "Drop-down button 6"
        };
        dropDownButton2.DropDownMenu = new ContextMenu([new MenuItem("Action A", WaveMenuItem_Click), new MenuItem("Action B", WriteStatusMenuItem_Click)]);
        toolBar.Buttons.Add(dropDownButton2);

        toolBar.Buttons.Add(new ToolBarButton("btn7 push") { ToolTipText = "Push button 7" });
        toolBar.Buttons.Add(new ToolBarButton("btn8 push") { ToolTipText = "Push button 8" });

        toolBar.ButtonClick += DemoToolBar_ButtonClick;

        return toolBar;
    }

    private void DemoToolBar_ButtonClick(object? sender, ToolBarButtonClickEventArgs e)
    {
        _statusLabel.Text = "Clicked: " + e.Button.Text;
        AppendLog("Toolbar button clicked: " + e.Button.Text);
        MessageBox.Show(this, "Button clicked. text = " + e.Button.Text, Text);
    }

    private void WaveMenuItem_Click(object? sender, EventArgs e)
    {
        _statusLabel.Text = "Wave back";
        AppendLog("Toolbar drop-down menu clicked: Wave");
        MessageBox.Show(this, "Wave back", Text);
    }

    private void WriteStatusMenuItem_Click(object? sender, EventArgs e)
    {
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        _statusLabel.Text = "Status updated at " + timestamp;
        AppendLog("Toolbar drop-down menu clicked: Write Status at " + timestamp);
    }

    private void ToggleAppearanceButton_Click(object? sender, EventArgs e)
    {
        _demoToolBar.Appearance = _demoToolBar.Appearance == ToolBarAppearance.Normal
            ? ToolBarAppearance.Flat
            : ToolBarAppearance.Normal;

        _toggleAppearanceButton.Text = "Appearance: " + _demoToolBar.Appearance;
        _statusLabel.Text = "ToolBar.Appearance → " + _demoToolBar.Appearance;
        AppendLog("ToolBar.Appearance toggled to: " + _demoToolBar.Appearance);
    }

    private void ToggleWrappableButton_Click(object? sender, EventArgs e)
    {
        _demoToolBar.Wrappable = !_demoToolBar.Wrappable;
        _toggleWrappableButton.Text = "Wrappable: " + _demoToolBar.Wrappable;
        _statusLabel.Text = "ToolBar.Wrappable → " + _demoToolBar.Wrappable;
        AppendLog("ToolBar.Wrappable toggled to: " + _demoToolBar.Wrappable);
    }

    private void ClearLogButton_Click(object? sender, EventArgs e)
    {
        _eventLog.Items.Clear();
        AppendLog("Log cleared.");
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