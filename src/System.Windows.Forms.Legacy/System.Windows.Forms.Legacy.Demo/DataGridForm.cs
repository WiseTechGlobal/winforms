using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable

namespace Demo
{
    /// <summary>
    ///  Phase 4 DataGrid demo surface.
    /// </summary>
    public partial class DataGridForm : Form
    {
        private DataGrid myDataGrid;
        private DataSet myDataSet;
        private bool TablesAlreadyAdded;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private ListBox featureListBox;
        private Label selectionSummaryLabel;
        private CheckBox captionVisibleCheckBox;
        private CheckBox parentRowsVisibleCheckBox;
        private CheckBox rowHeadersVisibleCheckBox;
        private CheckBox readOnlyCheckBox;
        private CheckBox allowNavigationCheckBox;
        private Label classicOptionsHintLabel;
        private string lastHitTestSummary = "No grid hit test yet.";
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button10;
        private Button button11;
        private CheckBox columnHeadersVisibleCheckBox;
        private CheckBox allowSortingCheckBox;
        private CheckBox gridLinesCheckBox;
        private int _colorSchemeIndex = 0;
        private int _borderStyleIndex = 0;
        private int _rowLabelStyleIndex = 0;

        public DataGridForm()
        {
            InitializeComponent();

            Shown += DataGridForm_Shown;
        }

        private void DataGridForm_Shown(object sender, EventArgs e)
        {
            InitializeDataGrid();
            SetUp();
            PopulateFeatureList();
            UpdateFeatureToggleStates();
            UpdateSelectionSummary();
        }

        private void SetUp()
        {
            MakeDataSet();
            myDataGrid.SetDataBinding(myDataSet, "Customers");
            BindingContext[myDataSet, "Customers"].PositionChanged += CustomersBindingManager_PositionChanged;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (TablesAlreadyAdded)
            {
                return;
            }

            AddCustomDataTableStyle();
        }

        private void AddCustomDataTableStyle()
        {
            DataGridTableStyle ts1 = new()
            {
                MappingName = "Customers",
                AlternatingBackColor = Color.LightGray
            };

            DataGridColumnStyle boolCol = new DataGridBoolColumn
            {
                MappingName = "Current",
                HeaderText = "IsCurrent Customer",
                Width = 150
            };
            ts1.GridColumnStyles.Add(boolCol);

            DataGridColumnStyle textCol = new DataGridTextBoxColumn
            {
                MappingName = "custName",
                HeaderText = "Customer Name",
                Width = 250
            };
            ts1.GridColumnStyles.Add(textCol);

            DataGridTableStyle ts2 = new()
            {
                MappingName = "Orders",
                AlternatingBackColor = Color.LightBlue
            };

            DataGridTextBoxColumn cOrderDate = new();
            cOrderDate.MappingName = "OrderDate";
            cOrderDate.HeaderText = "Order Date";
            cOrderDate.Width = 100;
            ts2.GridColumnStyles.Add(cOrderDate);

            PropertyDescriptorCollection pcol = BindingContext[myDataSet, "Customers.custToOrders"].GetItemProperties();

            DataGridTextBoxColumn csOrderAmount = new(pcol["OrderAmount"], "c", true)
            {
                MappingName = "OrderAmount",
                HeaderText = "Total",
                Width = 100
            };
            ts2.GridColumnStyles.Add(csOrderAmount);

            myDataGrid.TableStyles.Add(ts1);
            myDataGrid.TableStyles.Add(ts2);

            TablesAlreadyAdded = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            BindingManagerBase bmGrid = BindingContext[myDataSet, "Customers"];
            MessageBox.Show("Current BindingManager Position: " + bmGrid.Position);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            myDataGrid.FlatMode = !myDataGrid.FlatMode;
            button3.Text = myDataGrid.FlatMode ? "Use 3D Borders" : "Use Flat Mode";
            UpdateSelectionSummary();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            CurrencyManager customersManager = (CurrencyManager)BindingContext[myDataSet, "Customers"];
            int nextPosition = customersManager.Position + 1;

            if (nextPosition >= customersManager.Count)
            {
                nextPosition = 0;
            }

            customersManager.Position = nextPosition;
            UpdateSelectionSummary();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            CurrencyManager customersManager = (CurrencyManager)BindingContext[myDataSet, "Customers"];
            customersManager.Position = 0;

            UpdateSelectionSummary();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            const int CompactColumnWidth = 75;
            const int WideColumnWidth = 140;

            myDataGrid.PreferredColumnWidth = myDataGrid.PreferredColumnWidth <= CompactColumnWidth
                ? WideColumnWidth
                : CompactColumnWidth;
            button6.Text = myDataGrid.PreferredColumnWidth <= CompactColumnWidth
                ? "Use Wide Columns"
                : "Use Compact Columns";

            UpdateSelectionSummary();
        }

        private void CaptionVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            myDataGrid.CaptionVisible = captionVisibleCheckBox.Checked;
            UpdateSelectionSummary();
        }

        private void ParentRowsVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            myDataGrid.ParentRowsVisible = parentRowsVisibleCheckBox.Checked;
            UpdateSelectionSummary();
        }

        private void RowHeadersVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            myDataGrid.RowHeadersVisible = rowHeadersVisibleCheckBox.Checked;
            UpdateSelectionSummary();
        }

        private void ReadOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            myDataGrid.ReadOnly = readOnlyCheckBox.Checked;
            UpdateSelectionSummary();
        }

        private void AllowNavigationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            myDataGrid.AllowNavigation = allowNavigationCheckBox.Checked;
            UpdateSelectionSummary();
        }

        private void CustomersBindingManager_PositionChanged(object sender, EventArgs e)
        {
            UpdateSelectionSummary();
        }

        private void Grid_MouseUp(object sender, MouseEventArgs e)
        {
            DataGrid myGrid = (DataGrid)sender;
            DataGrid.HitTestInfo myHitInfo = myGrid.HitTest(e.X, e.Y);
            Console.WriteLine(myHitInfo);
            Console.WriteLine(myHitInfo.Type);
            Console.WriteLine(myHitInfo.Row);
            Console.WriteLine(myHitInfo.Column);

            lastHitTestSummary = $"Last hit: {myHitInfo.Type} at row {myHitInfo.Row}, column {myHitInfo.Column}";

            UpdateSelectionSummary();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            _colorSchemeIndex = (_colorSchemeIndex + 1) % 3;
            ApplyColorScheme(_colorSchemeIndex);
            UpdateSelectionSummary();
        }

        private void ApplyColorScheme(int scheme)
        {
            switch (scheme)
            {
                case 1:
                    myDataGrid.AlternatingBackColor = Color.PeachPuff;
                    myDataGrid.HeaderBackColor = Color.Chocolate;
                    myDataGrid.HeaderForeColor = Color.White;
                    myDataGrid.GridLineColor = Color.Peru;
                    break;
                case 2:
                    myDataGrid.AlternatingBackColor = Color.LightCyan;
                    myDataGrid.HeaderBackColor = Color.SteelBlue;
                    myDataGrid.HeaderForeColor = Color.White;
                    myDataGrid.GridLineColor = Color.CadetBlue;
                    break;
                default:
                    myDataGrid.ResetAlternatingBackColor();
                    myDataGrid.ResetHeaderBackColor();
                    myDataGrid.ResetHeaderForeColor();
                    myDataGrid.ResetGridLineColor();
                    break;
            }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            myDataGrid.BorderStyle = _borderStyleIndex switch
            {
                0 => BorderStyle.FixedSingle,
                1 => BorderStyle.None,
                _ => BorderStyle.Fixed3D
            };
            _borderStyleIndex = (_borderStyleIndex + 1) % 3;
            UpdateSelectionSummary();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            myDataGrid.CurrentCell = new DataGridCell(0, 0);
            UpdateSelectionSummary();
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            if (myDataSet is null)
            {
                return;
            }

            CurrencyManager cm = (CurrencyManager)BindingContext[myDataSet, "Customers"];

            for (int i = 0; i < cm.Count; i++)
            {
                myDataGrid.Select(i);
            }

            UpdateSelectionSummary();
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            myDataGrid.ParentRowsLabelStyle = _rowLabelStyleIndex switch
            {
                0 => DataGridParentRowsLabelStyle.TableName,
                1 => DataGridParentRowsLabelStyle.ColumnName,
                2 => DataGridParentRowsLabelStyle.None,
                _ => DataGridParentRowsLabelStyle.Both
            };
            _rowLabelStyleIndex = (_rowLabelStyleIndex + 1) % 4;

            UpdateButtonLabels();
            UpdateSelectionSummary();
        }

        private void ColumnHeadersVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            myDataGrid.ColumnHeadersVisible = columnHeadersVisibleCheckBox.Checked;
            UpdateSelectionSummary();
        }

        private void AllowSortingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            myDataGrid.AllowSorting = allowSortingCheckBox.Checked;
            UpdateSelectionSummary();
        }

        private void GridLinesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            myDataGrid.GridLineStyle = gridLinesCheckBox.Checked
                ? DataGridLineStyle.Solid
                : DataGridLineStyle.None;
            UpdateSelectionSummary();
        }

        private void PopulateFeatureList()
        {
            featureListBox.Items.Clear();
            featureListBox.Items.Add("Parent/child navigation with Customers -> Orders relation");
            featureListBox.Items.Add("Automatic binding through DataSet and BindingContext");
            featureListBox.Items.Add("Custom table styles for Customers and Orders");
            featureListBox.Items.Add("Boolean column rendering with DataGridBoolColumn");
            featureListBox.Items.Add("Formatted currency and date columns");
            featureListBox.Items.Add("Hit testing for rows, columns, and captions");
            featureListBox.Items.Add("Row navigation using the binding manager");
            featureListBox.Items.Add("Classic flat or 3D border rendering modes");
            featureListBox.Items.Add("Preferred column width switching for compact and wide views");
            featureListBox.Items.Add("Reset-to-first-row navigation for basic browsing flows");
            featureListBox.Items.Add("Color scheme cycling: Default, Warm, and Cool palettes");
            featureListBox.Items.Add("BorderStyle cycling: Fixed3D, FixedSingle, and None");
            featureListBox.Items.Add("Programmatic CurrentCell navigation via DataGridCell");
            featureListBox.Items.Add("Programmatic row selection using the Select API");
            featureListBox.Items.Add("ParentRowsLabelStyle cycling for child table breadcrumbs");
            featureListBox.Items.Add("Column header visibility control");
            featureListBox.Items.Add("AllowSorting toggle for interactive column sorting");
            featureListBox.Items.Add("GridLineStyle toggle between Solid and None");
        }

        private void UpdateFeatureToggleStates()
        {
            captionVisibleCheckBox.Checked = myDataGrid.CaptionVisible;
            parentRowsVisibleCheckBox.Checked = myDataGrid.ParentRowsVisible;
            rowHeadersVisibleCheckBox.Checked = myDataGrid.RowHeadersVisible;
            readOnlyCheckBox.Checked = myDataGrid.ReadOnly;
            allowNavigationCheckBox.Checked = myDataGrid.AllowNavigation;
            columnHeadersVisibleCheckBox.Checked = myDataGrid.ColumnHeadersVisible;
            allowSortingCheckBox.Checked = myDataGrid.AllowSorting;
            gridLinesCheckBox.Checked = myDataGrid.GridLineStyle == DataGridLineStyle.Solid;

            UpdateButtonLabels();
        }

        private void UpdateButtonLabels()
        {
            const int CompactColumnWidth = 75;

            button6.Text = myDataGrid.PreferredColumnWidth <= CompactColumnWidth
                ? "Use Wide Columns"
                : "Use Compact Columns";
            button11.Text = myDataGrid.ParentRowsLabelStyle switch
            {
                DataGridParentRowsLabelStyle.TableName => "Labels: Table",
                DataGridParentRowsLabelStyle.ColumnName => "Labels: Column",
                DataGridParentRowsLabelStyle.None => "Labels: Hidden",
                _ => "Labels: Both"
            };
        }

        private void MakeDataSet()
        {
            myDataSet = new DataSet("myDataSet");

            DataTable tCust = new DataTable("Customers");
            DataTable tOrders = new DataTable("Orders");

            DataColumn cCustID = new DataColumn("CustID", typeof(int));
            DataColumn cCustName = new DataColumn("CustName");
            DataColumn cCurrent = new DataColumn("Current", typeof(bool));
            tCust.Columns.Add(cCustID);
            tCust.Columns.Add(cCustName);
            tCust.Columns.Add(cCurrent);

            DataColumn cID = new DataColumn("CustID", typeof(int));
            DataColumn cOrderDate = new DataColumn("orderDate", typeof(DateTime));
            DataColumn cOrderAmount = new DataColumn("OrderAmount", typeof(decimal));
            tOrders.Columns.Add(cOrderAmount);
            tOrders.Columns.Add(cID);
            tOrders.Columns.Add(cOrderDate);

            myDataSet.Tables.Add(tCust);
            myDataSet.Tables.Add(tOrders);

            DataRelation dr = new DataRelation("custToOrders", cCustID, cID);
            myDataSet.Relations.Add(dr);

            DataRow newRow1;
            DataRow newRow2;

            for (int i = 1; i < 4; i++)
            {
                newRow1 = tCust.NewRow();
                newRow1["custID"] = i;
                tCust.Rows.Add(newRow1);
            }

            tCust.Rows[0]["custName"] = "Customer1";
            tCust.Rows[1]["custName"] = "Customer2";
            tCust.Rows[2]["custName"] = "Customer3";

            tCust.Rows[0]["Current"] = true;
            tCust.Rows[1]["Current"] = true;
            tCust.Rows[2]["Current"] = false;

            for (int i = 1; i < 4; i++)
            {
                for (int j = 1; j < 6; j++)
                {
                    newRow2 = tOrders.NewRow();
                    newRow2["CustID"] = i;
                    newRow2["orderDate"] = new DateTime(2001, i, j * 2);
                    newRow2["OrderAmount"] = i * 10 + j * .1;
                    tOrders.Rows.Add(newRow2);
                }
            }
        }

        private void UpdateSelectionSummary()
        {
            if (myDataSet is null)
            {
                selectionSummaryLabel.Text = "Classic demo summary will appear after the grid is initialized.";

                return;
            }

            CurrencyManager customersManager = (CurrencyManager)BindingContext[myDataSet, "Customers"];

            if (customersManager.Current is not DataRowView customerView)
            {
                selectionSummaryLabel.Text = "No active customer row.";

                return;
            }

            DataRow[] relatedOrders = customerView.Row.GetChildRows("custToOrders");
            string customerName = customerView["CustName"].ToString();
            string currentFlag = customerView["Current"].ToString();
            string borderMode = myDataGrid is not null && myDataGrid.FlatMode ? "Flat" : "3D";
            string captionMode = myDataGrid.CaptionVisible ? "Caption on" : "Caption off";
            string relationMode = myDataGrid.AllowNavigation ? "Navigation on" : "Navigation off";
            string widthMode = myDataGrid.PreferredColumnWidth <= 75 ? "Compact columns" : "Wide columns";
            string parentRowLabelMode = myDataGrid.ParentRowsLabelStyle switch
            {
                DataGridParentRowsLabelStyle.TableName => "Table labels",
                DataGridParentRowsLabelStyle.ColumnName => "Column labels",
                DataGridParentRowsLabelStyle.None => "Labels hidden",
                _ => "Table + column labels"
            };

            string colorScheme = _colorSchemeIndex switch
            {
                1 => "Warm",
                2 => "Cool",
                _ => "Default"
            };
            string borderStyle = myDataGrid.BorderStyle switch
            {
                BorderStyle.FixedSingle => "Single",
                BorderStyle.None => "None",
                _ => "3D"
            };

            selectionSummaryLabel.Text = $"Current customer: {customerName} | Active: {currentFlag} | Orders: {relatedOrders.Length} | Border mode: {borderMode} | {captionMode} | {relationMode} | {widthMode} | {lastHitTestSummary}" +
                $"\nColors: {colorScheme} | Border: {borderStyle} | Sorting: {(myDataGrid.AllowSorting ? "on" : "off")} | Col headers: {(myDataGrid.ColumnHeadersVisible ? "on" : "off")} | Parent labels: {parentRowLabelMode} (visible after drilling into Orders)";
        }
    }
}
