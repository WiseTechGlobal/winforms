using System.Windows.Forms;

namespace Demo;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private void MenuStackButton_Click(object? sender, EventArgs e)
    {
        MenuStackForm form = new();
        form.Show(this);
    }

    private void DataGridButton_Click(object? sender, EventArgs e)
    {
        DataGridForm form = new();
        form.Show(this);
    }
}
