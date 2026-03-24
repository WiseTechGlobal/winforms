using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable

namespace Demo;

partial class ToolBarForm
{
    private IContainer components = null;
    private TableLayoutPanel _rootLayout = null!;
    private Label _summaryLabel = null!;
    private Panel _toolbarHostPanel = null!;
    private FlowLayoutPanel _settingsPanel = null!;
    private Label _statusLabel = null!;
    private Label _testGuideLabel = null!;
    private TableLayoutPanel _logHeaderLayout = null!;
    private Label _logLabel = null!;
    private ListBox _eventLog = null!;
    private Button _clearLogButton = null!;
    private Button _toggleAppearanceButton = null!;
    private Button _toggleWrappableButton = null!;
    private Button _toggleDividerButton = null!;
    private Button _toggleToolTipsButton = null!;
    private Button _toggleAdvancedStateButton = null!;

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
        _rootLayout = new TableLayoutPanel();
        _summaryLabel = new Label();
        _toolbarHostPanel = new Panel();
        _settingsPanel = new FlowLayoutPanel();
        _statusLabel = new Label();
        _testGuideLabel = new Label();
        _logHeaderLayout = new TableLayoutPanel();
        _logLabel = new Label();
        _eventLog = new ListBox();
        _clearLogButton = new Button();
        _toggleAppearanceButton = new Button();
        _toggleWrappableButton = new Button();
        _toggleDividerButton = new Button();
        _toggleToolTipsButton = new Button();
        _toggleAdvancedStateButton = new Button();
        _rootLayout.SuspendLayout();
        _settingsPanel.SuspendLayout();
        _logHeaderLayout.SuspendLayout();
        SuspendLayout();
        // 
        // _summaryLabel
        // 
        _summaryLabel.AutoSize = true;
        _summaryLabel.Dock = DockStyle.Fill;
        _summaryLabel.Margin = new Padding(0, 0, 0, 12);
        _summaryLabel.Name = "_summaryLabel";
        _summaryLabel.Size = new Size(734, 30);
        _summaryLabel.TabIndex = 0;
        _summaryLabel.Text = "Click the core commands first, then test toolbar appearance, wrapping, divider, tooltips, and advanced button states. Use History to reset the surface quickly.";
        // 
        // _toolbarHostPanel
        // 
        _toolbarHostPanel.AutoSize = true;
        _toolbarHostPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _toolbarHostPanel.BorderStyle = BorderStyle.FixedSingle;
        _toolbarHostPanel.Dock = DockStyle.Fill;
        _toolbarHostPanel.Location = new Point(0, 42);
        _toolbarHostPanel.Margin = new Padding(0, 0, 0, 12);
        _toolbarHostPanel.Name = "_toolbarHostPanel";
        _toolbarHostPanel.Padding = new Padding(8, 6, 8, 6);
        _toolbarHostPanel.Size = new Size(734, 44);
        _toolbarHostPanel.TabIndex = 1;
        // 
        // _settingsPanel
        // 
        _settingsPanel.AutoSize = true;
        _settingsPanel.Dock = DockStyle.Fill;
        _settingsPanel.Location = new Point(0, 98);
        _settingsPanel.Margin = new Padding(0, 0, 0, 12);
        _settingsPanel.Name = "_settingsPanel";
        _settingsPanel.Size = new Size(734, 80);
        _settingsPanel.TabIndex = 2;
        _settingsPanel.WrapContents = true;
        // 
        // _toggleAppearanceButton
        // 
        _toggleAppearanceButton.Margin = new Padding(0, 0, 12, 12);
        _toggleAppearanceButton.MinimumSize = new Size(170, 32);
        _toggleAppearanceButton.Name = "_toggleAppearanceButton";
        _toggleAppearanceButton.Size = new Size(170, 32);
        _toggleAppearanceButton.TabIndex = 5;
        _toggleAppearanceButton.Text = "Appearance: Normal";
        _toggleAppearanceButton.Click += ToggleAppearanceButton_Click;
        // 
        // _toggleWrappableButton
        // 
        _toggleWrappableButton.Margin = new Padding(0, 0, 12, 12);
        _toggleWrappableButton.MinimumSize = new Size(170, 32);
        _toggleWrappableButton.Name = "_toggleWrappableButton";
        _toggleWrappableButton.Size = new Size(170, 32);
        _toggleWrappableButton.TabIndex = 6;
        _toggleWrappableButton.Text = "Wrappable: False";
        _toggleWrappableButton.Click += ToggleWrappableButton_Click;
        // 
        // _toggleDividerButton
        // 
        _toggleDividerButton.Margin = new Padding(0, 0, 12, 12);
        _toggleDividerButton.MinimumSize = new Size(170, 32);
        _toggleDividerButton.Name = "_toggleDividerButton";
        _toggleDividerButton.Size = new Size(170, 32);
        _toggleDividerButton.TabIndex = 7;
        _toggleDividerButton.Text = "Toolbar Divider: True";
        _toggleDividerButton.Click += ToggleDividerButton_Click;
        // 
        // _toggleToolTipsButton
        // 
        _toggleToolTipsButton.Margin = new Padding(0, 0, 12, 12);
        _toggleToolTipsButton.MinimumSize = new Size(170, 32);
        _toggleToolTipsButton.Name = "_toggleToolTipsButton";
        _toggleToolTipsButton.Size = new Size(170, 32);
        _toggleToolTipsButton.TabIndex = 8;
        _toggleToolTipsButton.Text = "ToolTips: True";
        _toggleToolTipsButton.Click += ToggleToolTipsButton_Click;
        // 
        // _toggleAdvancedStateButton
        // 
        _toggleAdvancedStateButton.Margin = new Padding(0, 0, 12, 12);
        _toggleAdvancedStateButton.MinimumSize = new Size(260, 32);
        _toggleAdvancedStateButton.Name = "_toggleAdvancedStateButton";
        _toggleAdvancedStateButton.Size = new Size(260, 32);
        _toggleAdvancedStateButton.TabIndex = 9;
        _toggleAdvancedStateButton.Text = "Advanced State Pack: Off";
        _toggleAdvancedStateButton.Click += ToggleAdvancedStateButton_Click;
        // 
        // _statusLabel
        // 
        _statusLabel.BorderStyle = BorderStyle.FixedSingle;
        _statusLabel.Dock = DockStyle.Fill;
        _statusLabel.Location = new Point(0, 190);
        _statusLabel.Margin = new Padding(0, 0, 0, 12);
        _statusLabel.MinimumSize = new Size(0, 54);
        _statusLabel.Name = "_statusLabel";
        _statusLabel.Size = new Size(734, 54);
        _statusLabel.TabIndex = 3;
        _statusLabel.Text = "Status messages appear here after each toolbar action so you can confirm the behavior being exercised and see the active state pack summary.";
        _statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _testGuideLabel
        // 
        _testGuideLabel.BorderStyle = BorderStyle.FixedSingle;
        _testGuideLabel.Dock = DockStyle.Fill;
        _testGuideLabel.Location = new Point(0, 256);
        _testGuideLabel.Margin = new Padding(0, 0, 0, 12);
        _testGuideLabel.Name = "_testGuideLabel";
        _testGuideLabel.Size = new Size(734, 47);
        _testGuideLabel.TabIndex = 4;
        _testGuideLabel.Text = "Suggested checks: click New, State, and Preview, toggle Pin, open Format and History, switch Appearance, Wrappable, and Toolbar Divider, then enable Advanced State Pack to disable State, partially push Partial, and hide Preview. Use History > Reset Demo to return to baseline.";
        _testGuideLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _logHeaderLayout
        // 
        _logHeaderLayout.ColumnCount = 2;
        _logHeaderLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _logHeaderLayout.ColumnStyles.Add(new ColumnStyle());
        _logHeaderLayout.Controls.Add(_logLabel, 0, 0);
        _logHeaderLayout.Controls.Add(_clearLogButton, 1, 0);
        _logHeaderLayout.Dock = DockStyle.Fill;
        _logHeaderLayout.Location = new Point(0, 315);
        _logHeaderLayout.Margin = new Padding(0, 0, 0, 8);
        _logHeaderLayout.Name = "_logHeaderLayout";
        _logHeaderLayout.RowCount = 1;
        _logHeaderLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _logHeaderLayout.Size = new Size(734, 38);
        _logHeaderLayout.TabIndex = 5;
        // 
        // _logLabel
        // 
        _logLabel.Anchor = AnchorStyles.Left;
        _logLabel.AutoSize = true;
        _logLabel.Location = new Point(0, 9);
        _logLabel.Margin = new Padding(0);
        _logLabel.Name = "_logLabel";
        _logLabel.Size = new Size(95, 15);
        _logLabel.TabIndex = 0;
        _logLabel.Text = "Interaction log";
        // 
        // _clearLogButton
        // 
        _clearLogButton.Anchor = AnchorStyles.Right;
        _clearLogButton.Margin = new Padding(12, 0, 0, 0);
        _clearLogButton.Name = "_clearLogButton";
        _clearLogButton.Size = new Size(124, 38);
        _clearLogButton.TabIndex = 1;
        _clearLogButton.Text = "Clear Log";
        _clearLogButton.Click += ClearLogButton_Click;
        // 
        // _eventLog
        // 
        _eventLog.Dock = DockStyle.Fill;
        _eventLog.FormattingEnabled = true;
        _eventLog.HorizontalScrollbar = true;
        _eventLog.IntegralHeight = false;
        _eventLog.Location = new Point(0, 361);
        _eventLog.Margin = new Padding(0);
        _eventLog.Name = "_eventLog";
        _eventLog.Size = new Size(734, 177);
        _eventLog.TabIndex = 6;
        // 
        // _rootLayout
        // 
        _rootLayout.ColumnCount = 1;
        _rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _rootLayout.Controls.Add(_summaryLabel, 0, 0);
        _rootLayout.Controls.Add(_toolbarHostPanel, 0, 1);
        _rootLayout.Controls.Add(_settingsPanel, 0, 2);
        _rootLayout.Controls.Add(_statusLabel, 0, 3);
        _rootLayout.Controls.Add(_testGuideLabel, 0, 4);
        _rootLayout.Controls.Add(_logHeaderLayout, 0, 5);
        _rootLayout.Controls.Add(_eventLog, 0, 6);
        _rootLayout.Dock = DockStyle.Fill;
        _rootLayout.Location = new Point(12, 12);
        _rootLayout.Name = "_rootLayout";
        _rootLayout.RowCount = 7;
        _rootLayout.RowStyles.Add(new RowStyle());
        _rootLayout.RowStyles.Add(new RowStyle());
        _rootLayout.RowStyles.Add(new RowStyle());
        _rootLayout.RowStyles.Add(new RowStyle());
        _rootLayout.RowStyles.Add(new RowStyle());
        _rootLayout.RowStyles.Add(new RowStyle());
        _rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _rootLayout.Size = new Size(734, 538);
        _rootLayout.TabIndex = 0;
        // 
        // _settingsPanel children
        // 
        _settingsPanel.Controls.Add(_toggleAppearanceButton);
        _settingsPanel.Controls.Add(_toggleWrappableButton);
        _settingsPanel.Controls.Add(_toggleDividerButton);
        _settingsPanel.Controls.Add(_toggleToolTipsButton);
        _settingsPanel.Controls.Add(_toggleAdvancedStateButton);
        _settingsPanel.ResumeLayout(false);
        // 
        // ToolBarForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(758, 562);
        Controls.Add(_rootLayout);
        MinimumSize = new Size(720, 520);
        Padding = new Padding(12);
        Name = "ToolBarForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "ToolBar Demo";
        _rootLayout.ResumeLayout(false);
        _rootLayout.PerformLayout();
        _logHeaderLayout.ResumeLayout(false);
        _logHeaderLayout.PerformLayout();
        ResumeLayout(false);
    }
}