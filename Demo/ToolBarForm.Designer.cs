using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable

namespace Demo;

partial class ToolBarForm
{
    private IContainer components = null;
    private Label _summaryLabel = null!;
    private Label _statusLabel = null!;
    private Label _logLabel = null!;
    private ListBox _eventLog = null!;
    private Button _clearLogButton = null!;

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
        _statusLabel = new Label();
        _logLabel = new Label();
        _eventLog = new ListBox();
        _clearLogButton = new Button();
        SuspendLayout();
        // 
        // _summaryLabel
        // 
        _summaryLabel.Location = new Point(18, 16);
        _summaryLabel.Name = "_summaryLabel";
        _summaryLabel.Size = new Size(554, 40);
        _summaryLabel.TabIndex = 0;
        _summaryLabel.Text = "Phase 3 toolbar demo extracted from MenuStackForm so the legacy ToolBar family can be exercised on its own, including normal buttons, toggle buttons, and ContextMenu-based drop-down actions.";
        // 
        // _statusLabel
        // 
        _statusLabel.BorderStyle = BorderStyle.FixedSingle;
        _statusLabel.Location = new Point(18, 118);
        _statusLabel.Name = "_statusLabel";
        _statusLabel.Size = new Size(554, 32);
        _statusLabel.TabIndex = 1;
        _statusLabel.Text = "Use the ToolBar buttons above to trigger legacy ToolBar and ToolBarButton behavior.";
        _statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _logLabel
        // 
        _logLabel.Location = new Point(18, 164);
        _logLabel.Name = "_logLabel";
        _logLabel.Size = new Size(120, 18);
        _logLabel.TabIndex = 2;
        _logLabel.Text = "ToolBar event log";
        // 
        // _eventLog
        // 
        _eventLog.FormattingEnabled = true;
        _eventLog.HorizontalScrollbar = true;
        _eventLog.Location = new Point(18, 186);
        _eventLog.Name = "_eventLog";
        _eventLog.Size = new Size(554, 184);
        _eventLog.TabIndex = 3;
        // 
        // _clearLogButton
        // 
        _clearLogButton.Location = new Point(452, 154);
        _clearLogButton.Name = "_clearLogButton";
        _clearLogButton.Size = new Size(120, 26);
        _clearLogButton.TabIndex = 4;
        _clearLogButton.Text = "Clear Log";
        _clearLogButton.Click += ClearLogButton_Click;
        // 
        // ToolBarForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(592, 392);
        Controls.Add(_clearLogButton);
        Controls.Add(_eventLog);
        Controls.Add(_logLabel);
        Controls.Add(_statusLabel);
        Controls.Add(_summaryLabel);
        MinimumSize = new Size(608, 431);
        Name = "ToolBarForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Phase 3: ToolBar";
        ResumeLayout(false);
    }
}