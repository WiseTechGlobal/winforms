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
    private Button _statusBarButton = null!;
    private Button _dataGridButton = null!;
    private Button _toolBarButton = null!;

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
        _statusBarButton = new Button();
        _dataGridButton = new Button();
        _toolBarButton = new Button();
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
        _descriptionLabel.Size = new Size(440, 36);
        _descriptionLabel.TabIndex = 1;
        _descriptionLabel.Text = "Select a demo to open. Each form independently exercises one legacy control family.";
        _descriptionLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // _menuStackButton
        // 
        _menuStackButton.Location = new Point(50, 108);
        _menuStackButton.Name = "_menuStackButton";
        _menuStackButton.Size = new Size(160, 60);
        _menuStackButton.TabIndex = 2;
        _menuStackButton.Text = "Menu Stack";
        _menuStackButton.Click += MenuStackButton_Click;
        // 
        // _statusBarButton
        // 
        _statusBarButton.Location = new Point(270, 108);
        _statusBarButton.Name = "_statusBarButton";
        _statusBarButton.Size = new Size(160, 60);
        _statusBarButton.TabIndex = 3;
        _statusBarButton.Text = "StatusBar";
        _statusBarButton.Click += StatusBarButton_Click;
        // 
        // _dataGridButton
        // 
        _dataGridButton.Location = new Point(50, 186);
        _dataGridButton.Name = "_dataGridButton";
        _dataGridButton.Size = new Size(160, 60);
        _dataGridButton.TabIndex = 4;
        _dataGridButton.Text = "DataGrid";
        _dataGridButton.Click += DataGridButton_Click;
        // 
        // _toolBarButton
        // 
        _toolBarButton.Location = new Point(270, 186);
        _toolBarButton.Name = "_toolBarButton";
        _toolBarButton.Size = new Size(160, 60);
        _toolBarButton.TabIndex = 5;
        _toolBarButton.Text = "ToolBar";
        _toolBarButton.Click += ToolBarButton_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(480, 274);
        Controls.Add(_toolBarButton);
        Controls.Add(_dataGridButton);
        Controls.Add(_statusBarButton);
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
