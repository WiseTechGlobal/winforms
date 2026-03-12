using System.Drawing;
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
            Location = new Point(18, 72),
            Size = new Size(554, 28),
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