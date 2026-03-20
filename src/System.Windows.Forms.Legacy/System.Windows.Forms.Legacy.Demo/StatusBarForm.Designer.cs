using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

#nullable disable

namespace Demo;

partial class StatusBarForm
{
    private IContainer components = null;
    private Label _summaryLabel = null!;
    private Button _showPanelsButton = null!;
    private Button _showSimpleTextButton = null!;
    private Button _addPanelButton = null!;
    private Button _removePanelButton = null!;
    private Button _toggleSizingGripButton = null!;
    private Button _cycleBorderStyleButton = null!;
    private Button _clearLogButton = null!;
    private Label _logLabel = null!;
    private ListBox _eventLog = null!;
    private Timer _clockTimer = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new Container();
        _summaryLabel = new Label();
        _showPanelsButton = new Button();
        _showSimpleTextButton = new Button();
        _addPanelButton = new Button();
        _removePanelButton = new Button();
        _toggleSizingGripButton = new Button();
        _cycleBorderStyleButton = new Button();
        _clearLogButton = new Button();
        _logLabel = new Label();
        _eventLog = new ListBox();
        _clockTimer = new Timer(components);
        SuspendLayout();
        // 
        // _summaryLabel
        // 
        _summaryLabel.Location = new Point(18, 16);
        _summaryLabel.Name = "_summaryLabel";
        _summaryLabel.Size = new Size(654, 40);
        _summaryLabel.TabIndex = 0;
        _summaryLabel.Text = "status bar demo covering legacy StatusBar panel layout, simple text mode, owner-draw rendering, dynamic panel mutation, panel border styles, sizing grip changes, tooltips, and click events.";
        // 
        // _showPanelsButton
        // 
        _showPanelsButton.Location = new Point(18, 72);
        _showPanelsButton.Name = "_showPanelsButton";
        _showPanelsButton.Size = new Size(120, 30);
        _showPanelsButton.TabIndex = 1;
        _showPanelsButton.Text = "Show Panels";
        _showPanelsButton.Click += ShowPanelsButton_Click;
        // 
        // _showSimpleTextButton
        // 
        _showSimpleTextButton.Location = new Point(150, 72);
        _showSimpleTextButton.Name = "_showSimpleTextButton";
        _showSimpleTextButton.Size = new Size(142, 30);
        _showSimpleTextButton.TabIndex = 2;
        _showSimpleTextButton.Text = "Show Simple Text";
        _showSimpleTextButton.Click += ShowSimpleTextButton_Click;
        // 
        // _addPanelButton
        // 
        _addPanelButton.Location = new Point(304, 72);
        _addPanelButton.Name = "_addPanelButton";
        _addPanelButton.Size = new Size(112, 30);
        _addPanelButton.TabIndex = 3;
        _addPanelButton.Text = "Add Panel";
        _addPanelButton.Click += AddPanelButton_Click;
        // 
        // _removePanelButton
        // 
        _removePanelButton.Location = new Point(428, 72);
        _removePanelButton.Name = "_removePanelButton";
        _removePanelButton.Size = new Size(120, 30);
        _removePanelButton.TabIndex = 4;
        _removePanelButton.Text = "Remove Panel";
        _removePanelButton.Click += RemovePanelButton_Click;
        // 
        // _toggleSizingGripButton
        // 
        _toggleSizingGripButton.Location = new Point(18, 110);
        _toggleSizingGripButton.Name = "_toggleSizingGripButton";
        _toggleSizingGripButton.Size = new Size(132, 30);
        _toggleSizingGripButton.TabIndex = 5;
        _toggleSizingGripButton.Text = "Toggle Sizing Grip";
        _toggleSizingGripButton.Click += ToggleSizingGripButton_Click;
        // 
        // _cycleBorderStyleButton
        // 
        _cycleBorderStyleButton.Location = new Point(162, 110);
        _cycleBorderStyleButton.Name = "_cycleBorderStyleButton";
        _cycleBorderStyleButton.Size = new Size(138, 30);
        _cycleBorderStyleButton.TabIndex = 6;
        _cycleBorderStyleButton.Text = "Cycle Border Style";
        _cycleBorderStyleButton.Click += CycleBorderStyleButton_Click;
        // 
        // _clearLogButton
        // 
        _clearLogButton.Location = new Point(560, 110);
        _clearLogButton.Name = "_clearLogButton";
        _clearLogButton.Size = new Size(112, 30);
        _clearLogButton.TabIndex = 7;
        _clearLogButton.Text = "Clear Log";
        _clearLogButton.Click += ClearLogButton_Click;
        // 
        // _logLabel
        // 
        _logLabel.Location = new Point(18, 156);
        _logLabel.Name = "_logLabel";
        _logLabel.Size = new Size(180, 18);
        _logLabel.TabIndex = 8;
        _logLabel.Text = "StatusBar event log";
        // 
        // _eventLog
        // 
        _eventLog.FormattingEnabled = true;
        _eventLog.HorizontalScrollbar = true;
        _eventLog.Location = new Point(18, 178);
        _eventLog.Name = "_eventLog";
        _eventLog.Size = new Size(654, 229);
        _eventLog.TabIndex = 9;
        // 
        // _clockTimer
        // 
        _clockTimer.Interval = 1000;
        _clockTimer.Tick += ClockTimer_Tick;
        // 
        // StatusBarForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(692, 434);
        Controls.Add(_eventLog);
        Controls.Add(_logLabel);
        Controls.Add(_clearLogButton);
        Controls.Add(_cycleBorderStyleButton);
        Controls.Add(_toggleSizingGripButton);
        Controls.Add(_removePanelButton);
        Controls.Add(_addPanelButton);
        Controls.Add(_showSimpleTextButton);
        Controls.Add(_showPanelsButton);
        Controls.Add(_summaryLabel);
        MinimumSize = new Size(708, 473);
        Name = "StatusBarForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "StatusBar";
        ResumeLayout(false);
    }
}
