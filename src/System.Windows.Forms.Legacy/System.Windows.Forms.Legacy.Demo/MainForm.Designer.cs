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
    private Label _menuStackLabel = null!;
    private Button _dataGridButton = null!;
    private Label _dataGridLabel = null!;
    private Button _toolBarButton = null!;
    private Label _toolBarLabel = null!;

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
        _menuStackLabel = new Label();
        _dataGridButton = new Button();
        _dataGridLabel = new Label();
        _toolBarButton = new Button();
        _toolBarLabel = new Label();
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
        _descriptionLabel.Size = new Size(440, 54);
        _descriptionLabel.TabIndex = 1;
        _descriptionLabel.Text = "Start with ToolBar for the clearest command-bar scenario: push, toggle, drop-down, wrapping, text layout, and advanced state transitions. Menu Stack and DataGrid remain available for broader validation.";
        _descriptionLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // _toolBarButton
        // 
        _toolBarButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        _toolBarButton.Location = new Point(140, 126);
        _toolBarButton.Name = "_toolBarButton";
        _toolBarButton.Size = new Size(200, 60);
        _toolBarButton.TabIndex = 2;
        _toolBarButton.Text = "ToolBar  (Recommended)";
        _toolBarButton.Click += ToolBarButton_Click;
        // 
        // _toolBarLabel
        // 
        _toolBarLabel.Location = new Point(76, 190);
        _toolBarLabel.Name = "_toolBarLabel";
        _toolBarLabel.Size = new Size(328, 34);
        _toolBarLabel.TabIndex = 3;
        _toolBarLabel.Text = "Best quick validation path for legacy command bars: click buttons, open menus, switch layout settings, then toggle advanced state behavior.";
        _toolBarLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // _menuStackButton
        // 
        _menuStackButton.Location = new Point(140, 234);
        _menuStackButton.Name = "_menuStackButton";
        _menuStackButton.Size = new Size(200, 60);
        _menuStackButton.TabIndex = 4;
        _menuStackButton.Text = "Menu Stack";
        _menuStackButton.Click += MenuStackButton_Click;
        // 
        // _menuStackLabel
        // 
        _menuStackLabel.Location = new Point(76, 298);
        _menuStackLabel.Name = "_menuStackLabel";
        _menuStackLabel.Size = new Size(328, 34);
        _menuStackLabel.TabIndex = 5;
        _menuStackLabel.Text = "Use for menu composition and nested-command coverage when you need more than the ToolBar surface.";
        _menuStackLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // _dataGridButton
        // 
        _dataGridButton.Location = new Point(140, 342);
        _dataGridButton.Name = "_dataGridButton";
        _dataGridButton.Size = new Size(200, 60);
        _dataGridButton.TabIndex = 6;
        _dataGridButton.Text = "DataGrid";
        _dataGridButton.Click += DataGridButton_Click;
        // 
        // _dataGridLabel
        // 
        _dataGridLabel.Location = new Point(76, 406);
        _dataGridLabel.Name = "_dataGridLabel";
        _dataGridLabel.Size = new Size(328, 34);
        _dataGridLabel.TabIndex = 7;
        _dataGridLabel.Text = "Heavier table-surface validation for editing, navigation, and legacy grid behavior.";
        _dataGridLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(480, 458);
        Controls.Add(_dataGridLabel);
        Controls.Add(_menuStackLabel);
        Controls.Add(_toolBarLabel);
        Controls.Add(_toolBarButton);
        Controls.Add(_dataGridButton);
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
