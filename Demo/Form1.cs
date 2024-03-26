using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Demo
{
    public partial class Form1 : Form
    {
        private Button button1;
        private Button button2;
        private System.Windows.Forms.DataGrid dataGrid;
        private DataSet myDataSet;
        private bool TablesAlreadyAdded;

        public Form1()
        {
            InitializeComponent();
        }
    }
}
