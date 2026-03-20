using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable

namespace Demo;

partial class MainForm
{
    private IContainer components = null;
    private Label _titleLabel = null!;
    private Label _descriptionLabel = null!;
    private Button _menuStackButton = null!;
    private Label _menuStackDescriptionLabel = null!;
    private Button _dataGridButton = null!;
    private Label _dataGridDescriptionLabel = null!;
    private Button _statusBarButton = null!;
    private Label _statusBarDescriptionLabel = null!;

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
        _titleLabel = new Label();
        _descriptionLabel = new Label();
        _menuStackButton = new Button();
        _menuStackDescriptionLabel = new Label();
        _dataGridButton = new Button();
        _dataGridDescriptionLabel = new Label();
        _statusBarButton = new Button();
        _statusBarDescriptionLabel = new Label();
        SuspendLayout();
        // 
        // _titleLabel
        // 
        _titleLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
        _titleLabel.Location = new Point(20, 20);
        _titleLabel.Name = "_titleLabel";
        _titleLabel.Size = new Size(440, 30);
        _titleLabel.TabIndex = 0;
        _titleLabel.Text = "WinForms Legacy Controls Demo";
        _titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // _descriptionLabel
        // 
        _descriptionLabel.Location = new Point(20, 58);
        _descriptionLabel.Name = "_descriptionLabel";
        _descriptionLabel.Size = new Size(680, 42);
        _descriptionLabel.TabIndex = 1;
        _descriptionLabel.Text = "Use this launcher to validate legacy control surfaces under active migration. Menu Stack remains the main branch focus, while DataGrid and StatusBar cover separate recovery and regression paths.";
        _descriptionLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // _menuStackButton
        // 
        _menuStackButton.Location = new Point(20, 118);
        _menuStackButton.Name = "_menuStackButton";
        _menuStackButton.Size = new Size(200, 60);
        _menuStackButton.TabIndex = 2;
        _menuStackButton.Text = "Menu Stack";
        _menuStackButton.Click += MenuStackButton_Click;
        // 
        // _menuStackDescriptionLabel
        // 
        _menuStackDescriptionLabel.Location = new Point(20, 188);
        _menuStackDescriptionLabel.Name = "_menuStackDescriptionLabel";
        _menuStackDescriptionLabel.Size = new Size(200, 64);
        _menuStackDescriptionLabel.TabIndex = 3;
        _menuStackDescriptionLabel.Text = "Nested menus, popup routing, shortcut processing, and stack behavior across the legacy menu surface.";
        _menuStackDescriptionLabel.TextAlign = ContentAlignment.TopCenter;
        // 
        // _dataGridButton
        // 
        _dataGridButton.Location = new Point(250, 118);
        _dataGridButton.Name = "_dataGridButton";
        _dataGridButton.Size = new Size(200, 60);
        _dataGridButton.TabIndex = 4;
        _dataGridButton.Text = "DataGrid";
        _dataGridButton.Click += DataGridButton_Click;
        // 
        // _dataGridDescriptionLabel
        // 
        _dataGridDescriptionLabel.Location = new Point(250, 188);
        _dataGridDescriptionLabel.Name = "_dataGridDescriptionLabel";
        _dataGridDescriptionLabel.Size = new Size(200, 64);
        _dataGridDescriptionLabel.TabIndex = 5;
        _dataGridDescriptionLabel.Text = "Legacy editing, navigation, and bound or unbound grid scenarios that still need targeted recovery coverage.";
        _dataGridDescriptionLabel.TextAlign = ContentAlignment.TopCenter;
        // 
        // _statusBarButton
        // 
        _statusBarButton.Location = new Point(480, 118);
        _statusBarButton.Name = "_statusBarButton";
        _statusBarButton.Size = new Size(200, 60);
        _statusBarButton.TabIndex = 6;
        _statusBarButton.Text = "StatusBar";
        _statusBarButton.Click += StatusBarButton_Click;
        // 
        // _statusBarDescriptionLabel
        // 
        _statusBarDescriptionLabel.Location = new Point(480, 188);
        _statusBarDescriptionLabel.Name = "_statusBarDescriptionLabel";
        _statusBarDescriptionLabel.Size = new Size(200, 64);
        _statusBarDescriptionLabel.TabIndex = 7;
        _statusBarDescriptionLabel.Text = "Simple text mode, panel layout, owner-draw rendering, border styles, and sizing grip behavior.";
        _statusBarDescriptionLabel.TextAlign = ContentAlignment.TopCenter;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(720, 270);
        Controls.Add(_statusBarDescriptionLabel);
        Controls.Add(_statusBarButton);
        Controls.Add(_dataGridDescriptionLabel);
        Controls.Add(_dataGridButton);
        Controls.Add(_menuStackDescriptionLabel);
        Controls.Add(_menuStackButton);
        Controls.Add(_descriptionLabel);
        Controls.Add(_titleLabel);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Demo Launcher";
        ResumeLayout(false);
    }
}
