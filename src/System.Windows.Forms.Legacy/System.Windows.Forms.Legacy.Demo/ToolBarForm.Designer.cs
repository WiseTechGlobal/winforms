using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable

namespace Demo;

partial class ToolBarForm
{
    private IContainer components = null;
    private Panel _contentPanel = null!;
    private Label _summaryLabel = null!;
    private Label _statusLabel = null!;
    private Label _logLabel = null!;
    private ListBox _eventLog = null!;
    private Button _clearLogButton = null!;
    private Button _toggleAppearanceButton = null!;
    private Button _toggleWrappableButton = null!;

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
        _contentPanel = new Panel();
        _summaryLabel = new Label();
        _statusLabel = new Label();
        _logLabel = new Label();
        _eventLog = new ListBox();
        _clearLogButton = new Button();
        _toggleAppearanceButton = new Button();
        _toggleWrappableButton = new Button();
        _contentPanel.SuspendLayout();
        SuspendLayout();
        // 
        // _summaryLabel
        // 
        _summaryLabel.Location = new Point(18, 8);
        _summaryLabel.Name = "_summaryLabel";
        _summaryLabel.Size = new Size(554, 40);
        _summaryLabel.TabIndex = 0;
        _summaryLabel.Text = "ToolBar demo exercising button styles (push, toggle, separator, drop-down), appearance (Normal/Flat), and the Wrappable property.";
        // 
        // _toggleAppearanceButton
        // 
        _toggleAppearanceButton.Location = new Point(18, 62);
        _toggleAppearanceButton.Name = "_toggleAppearanceButton";
        _toggleAppearanceButton.Size = new Size(148, 26);
        _toggleAppearanceButton.TabIndex = 5;
        _toggleAppearanceButton.Text = "Appearance: Normal";
        _toggleAppearanceButton.Click += ToggleAppearanceButton_Click;
        // 
        // _toggleWrappableButton
        // 
        _toggleWrappableButton.Location = new Point(176, 62);
        _toggleWrappableButton.Name = "_toggleWrappableButton";
        _toggleWrappableButton.Size = new Size(148, 26);
        _toggleWrappableButton.TabIndex = 6;
        _toggleWrappableButton.Text = "Wrappable: False";
        _toggleWrappableButton.Click += ToggleWrappableButton_Click;
        // 
        // _statusLabel
        // 
        _statusLabel.BorderStyle = BorderStyle.FixedSingle;
        _statusLabel.Location = new Point(18, 102);
        _statusLabel.Name = "_statusLabel";
        _statusLabel.Size = new Size(554, 32);
        _statusLabel.TabIndex = 1;
        _statusLabel.Text = "Use the ToolBar buttons and the toggle controls above to exercise ToolBar behavior.";
        _statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _logLabel
        // 
        _logLabel.Location = new Point(18, 148);
        _logLabel.Name = "_logLabel";
        _logLabel.Size = new Size(120, 18);
        _logLabel.TabIndex = 2;
        _logLabel.Text = "ToolBar event log";
        // 
        // _clearLogButton
        // 
        _clearLogButton.Location = new Point(452, 138);
        _clearLogButton.Name = "_clearLogButton";
        _clearLogButton.Size = new Size(120, 26);
        _clearLogButton.TabIndex = 4;
        _clearLogButton.Text = "Clear Log";
        _clearLogButton.Click += ClearLogButton_Click;
        // 
        // _eventLog
        // 
        _eventLog.FormattingEnabled = true;
        _eventLog.HorizontalScrollbar = true;
        _eventLog.Location = new Point(18, 170);
        _eventLog.Name = "_eventLog";
        _eventLog.Size = new Size(554, 184);
        _eventLog.TabIndex = 3;
        // 
        // _contentPanel
        // 
        _contentPanel.Dock = DockStyle.Fill;
        _contentPanel.Controls.Add(_clearLogButton);
        _contentPanel.Controls.Add(_eventLog);
        _contentPanel.Controls.Add(_logLabel);
        _contentPanel.Controls.Add(_statusLabel);
        _contentPanel.Controls.Add(_toggleWrappableButton);
        _contentPanel.Controls.Add(_toggleAppearanceButton);
        _contentPanel.Controls.Add(_summaryLabel);
        _contentPanel.ResumeLayout(false);
        // 
        // ToolBarForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(592, 398);
        Controls.Add(_contentPanel);
        MinimumSize = new Size(608, 437);
        Name = "ToolBarForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "ToolBar Demo";
        ResumeLayout(false);
    }
}