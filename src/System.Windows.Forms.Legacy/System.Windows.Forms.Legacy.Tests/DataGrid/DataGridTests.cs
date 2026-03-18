// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms.Legacy.Tests;

public class DataGridTests
{
    [StaFact]
    public void DataGrid_Ctor_Default()
    {
        using SubDataGrid dataGrid = new();

        Assert.True(dataGrid.AllowNavigation);
        Assert.True(dataGrid.AllowSorting);
        Assert.Equal(SystemColors.Window, dataGrid.AlternatingBackColor);
        Assert.Equal(SystemColors.Window, dataGrid.BackColor);
        Assert.Equal(SystemColors.AppWorkspace, dataGrid.BackgroundColor);
        Assert.Equal(BorderStyle.Fixed3D, dataGrid.BorderStyle);
        Assert.Equal(SystemColors.ActiveCaption, dataGrid.CaptionBackColor);
        Assert.Equal(SystemColors.ActiveCaptionText, dataGrid.CaptionForeColor);
        Assert.Empty(dataGrid.CaptionText);
        Assert.True(dataGrid.CaptionVisible);
        Assert.True(dataGrid.ColumnHeadersVisible);
        Assert.Equal(0, dataGrid.CurrentCell.RowNumber);
        Assert.Equal(0, dataGrid.CurrentCell.ColumnNumber);
        Assert.Equal(-1, dataGrid.CurrentRowIndex);
        Assert.Empty(dataGrid.DataMember);
        Assert.Null(dataGrid.DataSource);
        Assert.Equal(0, dataGrid.FirstVisibleColumn);
        Assert.False(dataGrid.FlatMode);
        Assert.Equal(SystemColors.WindowText, dataGrid.ForeColor);
        Assert.Equal(SystemColors.Control, dataGrid.GridLineColor);
        Assert.Equal(DataGridLineStyle.Solid, dataGrid.GridLineStyle);
        Assert.Equal(SystemColors.Control, dataGrid.HeaderBackColor);
        Assert.Equal(SystemColors.ControlText, dataGrid.HeaderForeColor);
        Assert.NotNull(dataGrid.HorizScrollBar);
        Assert.Same(dataGrid.HorizScrollBar, dataGrid.HorizScrollBar);
        Assert.Equal(SystemColors.HotTrack, dataGrid.LinkColor);
        Assert.Equal(SystemColors.HotTrack, dataGrid.LinkHoverColor);
        Assert.Null(dataGrid.ListManager);
        Assert.Equal(SystemColors.Control, dataGrid.ParentRowsBackColor);
        Assert.Equal(SystemColors.WindowText, dataGrid.ParentRowsForeColor);
        Assert.Equal(DataGridParentRowsLabelStyle.Both, dataGrid.ParentRowsLabelStyle);
        Assert.True(dataGrid.ParentRowsVisible);
        Assert.Equal(75, dataGrid.PreferredColumnWidth);
        Assert.Equal(Control.DefaultFont.Height + 3, dataGrid.PreferredRowHeight);
        Assert.False(dataGrid.ReadOnly);
        Assert.True(dataGrid.RowHeadersVisible);
        Assert.Equal(35, dataGrid.RowHeaderWidth);
        Assert.Equal(SystemColors.ActiveCaption, dataGrid.SelectionBackColor);
        Assert.Equal(SystemColors.ActiveCaptionText, dataGrid.SelectionForeColor);
        Assert.Equal(new Size(130, 80), dataGrid.Size);
        Assert.Empty(dataGrid.TableStyles);
        Assert.Same(dataGrid.TableStyles, dataGrid.TableStyles);
        Assert.NotNull(dataGrid.VertScrollBar);
        Assert.Same(dataGrid.VertScrollBar, dataGrid.VertScrollBar);
        Assert.Equal(0, dataGrid.VisibleColumnCount);
        Assert.Equal(0, dataGrid.VisibleRowCount);
    }

    [StaFact]
    public void DataGrid_AllowNavigation_Set_GetReturnsExpected()
    {
        using DataGrid dataGrid = new();

        dataGrid.AllowNavigation = false;

        Assert.False(dataGrid.AllowNavigation);

        // Set same.
        dataGrid.AllowNavigation = false;

        Assert.False(dataGrid.AllowNavigation);

        // Set different.
        dataGrid.AllowNavigation = true;

        Assert.True(dataGrid.AllowNavigation);
    }

    [StaFact]
    public void DataGrid_AllowNavigation_SetWithHandler_CallsAllowNavigationChanged()
    {
        using DataGrid dataGrid = new();
        int callCount = 0;
        EventHandler handler = (sender, e) =>
        {
            Assert.Same(dataGrid, sender);
            Assert.Same(EventArgs.Empty, e);
            callCount++;
        };
        dataGrid.AllowNavigationChanged += handler;

        // Set different.
        dataGrid.AllowNavigation = false;

        Assert.False(dataGrid.AllowNavigation);
        Assert.Equal(1, callCount);

        // Set same.
        dataGrid.AllowNavigation = false;

        Assert.False(dataGrid.AllowNavigation);
        Assert.Equal(1, callCount);

        // Set different.
        dataGrid.AllowNavigation = true;

        Assert.True(dataGrid.AllowNavigation);
        Assert.Equal(2, callCount);

        // Remove handler.
        dataGrid.AllowNavigationChanged -= handler;
        dataGrid.AllowNavigation = false;

        Assert.Equal(2, callCount);
    }

    [StaFact]
    public void DataGrid_AllowSorting_Set_GetReturnsExpected()
    {
        using DataGrid dataGrid = new();

        dataGrid.AllowSorting = false;

        Assert.False(dataGrid.AllowSorting);

        // Set same.
        dataGrid.AllowSorting = false;

        Assert.False(dataGrid.AllowSorting);

        // Set different.
        dataGrid.AllowSorting = true;

        Assert.True(dataGrid.AllowSorting);
    }

    [StaFact]
    public void DataGrid_AlternatingBackColor_Set_GetReturnsExpected()
    {
        using DataGrid dataGrid = new();

        dataGrid.AlternatingBackColor = Color.Red;

        Assert.Equal(Color.Red, dataGrid.AlternatingBackColor);

        // Set same.
        dataGrid.AlternatingBackColor = Color.Red;

        Assert.Equal(Color.Red, dataGrid.AlternatingBackColor);

        // Set different.
        dataGrid.AlternatingBackColor = Color.Blue;

        Assert.Equal(Color.Blue, dataGrid.AlternatingBackColor);
    }

    [StaFact]
    public void DataGrid_AlternatingBackColor_SetEmpty_ThrowsArgumentException()
    {
        using DataGrid dataGrid = new();

        Assert.Throws<ArgumentException>(() => dataGrid.AlternatingBackColor = Color.Empty);
    }

    [StaFact]
    public void DataGrid_BorderStyle_Set_GetReturnsExpected()
    {
        using DataGrid dataGrid = new();

        dataGrid.BorderStyle = BorderStyle.FixedSingle;

        Assert.Equal(BorderStyle.FixedSingle, dataGrid.BorderStyle);

        // Set same.
        dataGrid.BorderStyle = BorderStyle.FixedSingle;

        Assert.Equal(BorderStyle.FixedSingle, dataGrid.BorderStyle);

        // Set different.
        dataGrid.BorderStyle = BorderStyle.None;

        Assert.Equal(BorderStyle.None, dataGrid.BorderStyle);
    }

    [StaFact]
    public void DataGrid_BorderStyle_SetWithHandler_CallsBorderStyleChanged()
    {
        using DataGrid dataGrid = new();
        int callCount = 0;
        EventHandler handler = (sender, e) =>
        {
            Assert.Same(dataGrid, sender);
            Assert.Same(EventArgs.Empty, e);
            callCount++;
        };
        dataGrid.BorderStyleChanged += handler;

        // Set different.
        dataGrid.BorderStyle = BorderStyle.FixedSingle;

        Assert.Equal(BorderStyle.FixedSingle, dataGrid.BorderStyle);
        Assert.Equal(1, callCount);

        // Set same.
        dataGrid.BorderStyle = BorderStyle.FixedSingle;

        Assert.Equal(BorderStyle.FixedSingle, dataGrid.BorderStyle);
        Assert.Equal(1, callCount);

        // Set different.
        dataGrid.BorderStyle = BorderStyle.None;

        Assert.Equal(BorderStyle.None, dataGrid.BorderStyle);
        Assert.Equal(2, callCount);

        // Remove handler.
        dataGrid.BorderStyleChanged -= handler;
        dataGrid.BorderStyle = BorderStyle.Fixed3D;

        Assert.Equal(2, callCount);
    }

    [StaFact]
    public void DataGrid_CaptionText_Set_GetReturnsExpected()
    {
        using DataGrid dataGrid = new();

        dataGrid.CaptionText = "My Grid";

        Assert.Equal("My Grid", dataGrid.CaptionText);

        // Set same.
        dataGrid.CaptionText = "My Grid";

        Assert.Equal("My Grid", dataGrid.CaptionText);

        // Set different.
        dataGrid.CaptionText = string.Empty;

        Assert.Empty(dataGrid.CaptionText);
    }

    [StaFact]
    public void DataGrid_CaptionVisible_SetWithHandler_CallsCaptionVisibleChanged()
    {
        using DataGrid dataGrid = new();
        int callCount = 0;
        EventHandler handler = (sender, e) =>
        {
            Assert.Same(dataGrid, sender);
            Assert.Same(EventArgs.Empty, e);
            callCount++;
        };
        dataGrid.CaptionVisibleChanged += handler;

        // Set different.
        dataGrid.CaptionVisible = false;

        Assert.False(dataGrid.CaptionVisible);
        Assert.Equal(1, callCount);

        // Set same.
        dataGrid.CaptionVisible = false;

        Assert.False(dataGrid.CaptionVisible);
        Assert.Equal(1, callCount);

        // Set different.
        dataGrid.CaptionVisible = true;

        Assert.True(dataGrid.CaptionVisible);
        Assert.Equal(2, callCount);

        // Remove handler.
        dataGrid.CaptionVisibleChanged -= handler;
        dataGrid.CaptionVisible = false;

        Assert.Equal(2, callCount);
    }

    [StaFact]
    public void DataGrid_FlatMode_Set_GetReturnsExpected()
    {
        using DataGrid dataGrid = new();

        dataGrid.FlatMode = true;

        Assert.True(dataGrid.FlatMode);

        // Set same.
        dataGrid.FlatMode = true;

        Assert.True(dataGrid.FlatMode);

        // Set different.
        dataGrid.FlatMode = false;

        Assert.False(dataGrid.FlatMode);
    }

    [StaFact]
    public void DataGrid_FlatMode_SetWithHandler_CallsFlatModeChanged()
    {
        using DataGrid dataGrid = new();
        int callCount = 0;
        EventHandler handler = (sender, e) =>
        {
            Assert.Same(dataGrid, sender);
            Assert.Same(EventArgs.Empty, e);
            callCount++;
        };
        dataGrid.FlatModeChanged += handler;

        // Set different.
        dataGrid.FlatMode = true;

        Assert.True(dataGrid.FlatMode);
        Assert.Equal(1, callCount);

        // Set same.
        dataGrid.FlatMode = true;

        Assert.True(dataGrid.FlatMode);
        Assert.Equal(1, callCount);

        // Set different.
        dataGrid.FlatMode = false;

        Assert.False(dataGrid.FlatMode);
        Assert.Equal(2, callCount);

        // Remove handler.
        dataGrid.FlatModeChanged -= handler;
        dataGrid.FlatMode = true;

        Assert.Equal(2, callCount);
    }

    [StaFact]
    public void DataGrid_GridLineStyle_Set_GetReturnsExpected()
    {
        using DataGrid dataGrid = new();

        dataGrid.GridLineStyle = DataGridLineStyle.None;

        Assert.Equal(DataGridLineStyle.None, dataGrid.GridLineStyle);

        // Set same.
        dataGrid.GridLineStyle = DataGridLineStyle.None;

        Assert.Equal(DataGridLineStyle.None, dataGrid.GridLineStyle);

        // Set different.
        dataGrid.GridLineStyle = DataGridLineStyle.Solid;

        Assert.Equal(DataGridLineStyle.Solid, dataGrid.GridLineStyle);
    }

    [StaFact]
    public void DataGrid_ParentRowsLabelStyle_Set_GetReturnsExpected()
    {
        using DataGrid dataGrid = new();

        dataGrid.ParentRowsLabelStyle = DataGridParentRowsLabelStyle.None;

        Assert.Equal(DataGridParentRowsLabelStyle.None, dataGrid.ParentRowsLabelStyle);

        dataGrid.ParentRowsLabelStyle = DataGridParentRowsLabelStyle.TableName;

        Assert.Equal(DataGridParentRowsLabelStyle.TableName, dataGrid.ParentRowsLabelStyle);

        dataGrid.ParentRowsLabelStyle = DataGridParentRowsLabelStyle.ColumnName;

        Assert.Equal(DataGridParentRowsLabelStyle.ColumnName, dataGrid.ParentRowsLabelStyle);

        // Set same.
        dataGrid.ParentRowsLabelStyle = DataGridParentRowsLabelStyle.ColumnName;

        Assert.Equal(DataGridParentRowsLabelStyle.ColumnName, dataGrid.ParentRowsLabelStyle);

        // Restore default.
        dataGrid.ParentRowsLabelStyle = DataGridParentRowsLabelStyle.Both;

        Assert.Equal(DataGridParentRowsLabelStyle.Both, dataGrid.ParentRowsLabelStyle);
    }

    [StaFact]
    public void DataGrid_ParentRowsVisible_SetWithHandler_CallsParentRowsVisibleChanged()
    {
        using DataGrid dataGrid = new();
        int callCount = 0;
        EventHandler handler = (sender, e) =>
        {
            Assert.Same(dataGrid, sender);
            Assert.Same(EventArgs.Empty, e);
            callCount++;
        };
        dataGrid.ParentRowsVisibleChanged += handler;

        // Set different.
        dataGrid.ParentRowsVisible = false;

        Assert.False(dataGrid.ParentRowsVisible);
        Assert.Equal(1, callCount);

        // Set same.
        dataGrid.ParentRowsVisible = false;

        Assert.False(dataGrid.ParentRowsVisible);
        Assert.Equal(1, callCount);

        // Set different.
        dataGrid.ParentRowsVisible = true;

        Assert.True(dataGrid.ParentRowsVisible);
        Assert.Equal(2, callCount);

        // Remove handler.
        dataGrid.ParentRowsVisibleChanged -= handler;
        dataGrid.ParentRowsVisible = false;

        Assert.Equal(2, callCount);
    }

    [StaFact]
    public void DataGrid_ReadOnly_Set_GetReturnsExpected()
    {
        using DataGrid dataGrid = new();

        dataGrid.ReadOnly = true;

        Assert.True(dataGrid.ReadOnly);

        // Set same.
        dataGrid.ReadOnly = true;

        Assert.True(dataGrid.ReadOnly);

        // Set different.
        dataGrid.ReadOnly = false;

        Assert.False(dataGrid.ReadOnly);
    }

    [StaFact]
    public void DataGrid_ReadOnly_SetWithHandler_CallsReadOnlyChanged()
    {
        using DataGrid dataGrid = new();
        int callCount = 0;
        EventHandler handler = (sender, e) =>
        {
            Assert.Same(dataGrid, sender);
            Assert.Same(EventArgs.Empty, e);
            callCount++;
        };
        dataGrid.ReadOnlyChanged += handler;

        // Set different.
        dataGrid.ReadOnly = true;

        Assert.True(dataGrid.ReadOnly);
        Assert.Equal(1, callCount);

        // Set same.
        dataGrid.ReadOnly = true;

        Assert.True(dataGrid.ReadOnly);
        Assert.Equal(1, callCount);

        // Set different.
        dataGrid.ReadOnly = false;

        Assert.False(dataGrid.ReadOnly);
        Assert.Equal(2, callCount);

        // Remove handler.
        dataGrid.ReadOnlyChanged -= handler;
        dataGrid.ReadOnly = true;

        Assert.Equal(2, callCount);
    }

    [StaFact]
    public void DataGrid_SelectionBackColor_Set_GetReturnsExpected()
    {
        using DataGrid dataGrid = new();

        dataGrid.SelectionBackColor = Color.Navy;

        Assert.Equal(Color.Navy, dataGrid.SelectionBackColor);

        // Set same.
        dataGrid.SelectionBackColor = Color.Navy;

        Assert.Equal(Color.Navy, dataGrid.SelectionBackColor);

        // Set different.
        dataGrid.SelectionBackColor = Color.Red;

        Assert.Equal(Color.Red, dataGrid.SelectionBackColor);
    }

    [StaFact]
    public void DataGrid_SelectionBackColor_SetEmpty_ThrowsArgumentException()
    {
        using DataGrid dataGrid = new();

        Assert.Throws<ArgumentException>(() => dataGrid.SelectionBackColor = Color.Empty);
    }

    [StaFact]
    public void DataGrid_SelectionForeColor_Set_GetReturnsExpected()
    {
        using DataGrid dataGrid = new();

        dataGrid.SelectionForeColor = Color.White;

        Assert.Equal(Color.White, dataGrid.SelectionForeColor);

        // Set same.
        dataGrid.SelectionForeColor = Color.White;

        Assert.Equal(Color.White, dataGrid.SelectionForeColor);

        // Set different.
        dataGrid.SelectionForeColor = Color.Black;

        Assert.Equal(Color.Black, dataGrid.SelectionForeColor);
    }

    [StaFact]
    public void DataGrid_SelectionForeColor_SetEmpty_ThrowsArgumentException()
    {
        using DataGrid dataGrid = new();

        Assert.Throws<ArgumentException>(() => dataGrid.SelectionForeColor = Color.Empty);
    }

    [StaFact]
    public void DataGrid_SetDataBinding_SetsDataSourceAndDataMember()
    {
        using Form form = new();
        using DataGrid dataGrid = new();
        using DataSet dataSet = CreateDemoDataSet();

        form.Controls.Add(dataGrid);
        dataGrid.SetDataBinding(dataSet, "Customers");

        Assert.Same(dataSet, dataGrid.DataSource);
        Assert.Equal("Customers", dataGrid.DataMember);
    }

    [StaFact]
    public void DataGrid_SetDataBinding_CurrentRowIndexStaysInSyncWithBindingManager()
    {
        using Form form = new();
        using DataGrid dataGrid = new();
        using DataSet dataSet = CreateDemoDataSet();

        form.Controls.Add(dataGrid);
        dataGrid.SetDataBinding(dataSet, "Customers");

        CurrencyManager customersManager = Assert.IsAssignableFrom<CurrencyManager>(form.BindingContext[dataSet, "Customers"]);

        Assert.Equal(0, dataGrid.CurrentRowIndex);

        customersManager.Position = 2;

        Assert.Equal(2, dataGrid.CurrentRowIndex);

        DataRowView customerAtPositionTwo = Assert.IsType<DataRowView>(customersManager.Current);
        Assert.Equal("Customer3", customerAtPositionTwo["CustName"]);

        dataGrid.CurrentRowIndex = 1;

        Assert.Equal(1, customersManager.Position);

        DataRowView currentCustomer = Assert.IsType<DataRowView>(customersManager.Current);
        Assert.Equal("Customer2", currentCustomer["CustName"]);
        Assert.Equal(5, currentCustomer.Row.GetChildRows("custToOrders").Length);
    }

    [StaFact]
    public void DataGrid_SetDataBinding_EmptyTable_CurrentRowIndexIsMinusOne()
    {
        using Form form = new();
        using DataGrid dataGrid = new();
        using DataSet dataSet = new("EmptySet");

        DataTable emptyTable = new("Items");
        emptyTable.Columns.Add("ID", typeof(int));
        emptyTable.Columns.Add("Name", typeof(string));
        dataSet.Tables.Add(emptyTable);

        form.Controls.Add(dataGrid);
        dataGrid.SetDataBinding(dataSet, "Items");

        CurrencyManager manager = Assert.IsAssignableFrom<CurrencyManager>(form.BindingContext[dataSet, "Items"]);

        Assert.Equal(0, manager.Count);
        Assert.Equal(-1, dataGrid.CurrentRowIndex);
    }

    [StaFact]
    public void DataGrid_DataSourceChanged_SetWithHandler_CallsDataSourceChanged()
    {
        using Form form = new();
        using DataGrid dataGrid = new();
        using DataSet dataSet = CreateDemoDataSet();

        form.Controls.Add(dataGrid);

        int callCount = 0;
        EventHandler handler = (sender, e) =>
        {
            Assert.Same(dataGrid, sender);
            Assert.Same(EventArgs.Empty, e);
            callCount++;
        };
        dataGrid.DataSourceChanged += handler;

        dataGrid.SetDataBinding(dataSet, "Customers");

        Assert.Equal(1, callCount);

        // Set same source.
        dataGrid.SetDataBinding(dataSet, "Customers");

        Assert.Equal(1, callCount);

        // Remove handler.
        dataGrid.DataSourceChanged -= handler;
        dataGrid.SetDataBinding(dataSet, "Orders");

        Assert.Equal(1, callCount);
    }

    [StaFact]
    public void DataGrid_TableStyles_AddDemoStyles_MapsCustomersAndOrdersColumns()
    {
        using Form form = new();
        using DataGrid dataGrid = new();
        using DataSet dataSet = CreateDemoDataSet();

        form.Controls.Add(dataGrid);
        dataGrid.SetDataBinding(dataSet, "Customers");

        AddDemoTableStyles(form, dataGrid, dataSet);

        Assert.Equal(2, dataGrid.TableStyles.Count);

        DataGridTableStyle customersStyle = Assert.IsType<DataGridTableStyle>(dataGrid.TableStyles["Customers"]);
        DataGridTableStyle ordersStyle = Assert.IsType<DataGridTableStyle>(dataGrid.TableStyles["Orders"]);

        Assert.Equal(Color.LightGray, customersStyle.AlternatingBackColor);
        Assert.Equal(Color.LightBlue, ordersStyle.AlternatingBackColor);

        DataGridBoolColumn boolColumn = Assert.IsType<DataGridBoolColumn>(customersStyle.GridColumnStyles["Current"]);
        Assert.Equal("IsCurrent Customer", boolColumn.HeaderText);
        Assert.Equal(150, boolColumn.Width);

        DataGridTextBoxColumn customerNameColumn = Assert.IsType<DataGridTextBoxColumn>(customersStyle.GridColumnStyles["custName"]);
        Assert.Equal("Customer Name", customerNameColumn.HeaderText);
        Assert.Equal(250, customerNameColumn.Width);

        DataGridTextBoxColumn orderAmountColumn = Assert.IsType<DataGridTextBoxColumn>(ordersStyle.GridColumnStyles["OrderAmount"]);
        Assert.Equal("c", orderAmountColumn.Format);
        Assert.Equal("Total", orderAmountColumn.HeaderText);
        Assert.Equal(100, orderAmountColumn.Width);
    }

    [StaFact]
    public void DataGrid_TableStyles_Clear_RemovesAllStyles()
    {
        using Form form = new();
        using DataGrid dataGrid = new();
        using DataSet dataSet = CreateDemoDataSet();

        form.Controls.Add(dataGrid);
        dataGrid.SetDataBinding(dataSet, "Customers");

        AddDemoTableStyles(form, dataGrid, dataSet);

        Assert.Equal(2, dataGrid.TableStyles.Count);

        dataGrid.TableStyles.Clear();

        Assert.Empty(dataGrid.TableStyles);
    }

    [StaFact]
    public void DataGrid_ColumnStartedEditing_ValidControl_Success()
    {
        using DataGrid dataGrid = new();
        using Control control = new();

        dataGrid.ColumnStartedEditing(control);
    }

    [StaFact]
    public void DataGrid_ColumnStartedEditing_NullControl_Nop()
    {
        using DataGrid dataGrid = new();

        dataGrid.ColumnStartedEditing(null);
    }

    private sealed class SubDataGrid : DataGrid
    {
        public new ScrollBar HorizScrollBar => base.HorizScrollBar;

        public new ScrollBar VertScrollBar => base.VertScrollBar;
    }

    private static DataSet CreateDemoDataSet()
    {
        DataSet dataSet = new("myDataSet");

        DataTable customersTable = new("Customers");
        DataTable ordersTable = new("Orders");

        DataColumn customerIdColumn = new("CustID", typeof(int));
        DataColumn customerNameColumn = new("CustName", typeof(string));
        DataColumn currentColumn = new("Current", typeof(bool));
        customersTable.Columns.Add(customerIdColumn);
        customersTable.Columns.Add(customerNameColumn);
        customersTable.Columns.Add(currentColumn);

        DataColumn orderCustomerIdColumn = new("CustID", typeof(int));
        DataColumn orderDateColumn = new("orderDate", typeof(DateTime));
        DataColumn orderAmountColumn = new("OrderAmount", typeof(decimal));
        ordersTable.Columns.Add(orderAmountColumn);
        ordersTable.Columns.Add(orderCustomerIdColumn);
        ordersTable.Columns.Add(orderDateColumn);

        dataSet.Tables.Add(customersTable);
        dataSet.Tables.Add(ordersTable);
        dataSet.Relations.Add(new DataRelation("custToOrders", customerIdColumn, orderCustomerIdColumn));

        for (int i = 1; i < 4; i++)
        {
            DataRow customerRow = customersTable.NewRow();
            customerRow["CustID"] = i;
            customerRow["CustName"] = $"Customer{i}";
            customerRow["Current"] = i < 3;
            customersTable.Rows.Add(customerRow);

            for (int j = 1; j < 6; j++)
            {
                DataRow orderRow = ordersTable.NewRow();
                orderRow["CustID"] = i;
                orderRow["orderDate"] = new DateTime(2001, i, j * 2);
                orderRow["OrderAmount"] = (decimal)(i * 10) + ((decimal)j / 10);
                ordersTable.Rows.Add(orderRow);
            }
        }

        return dataSet;
    }

    private static void AddDemoTableStyles(Form form, DataGrid dataGrid, DataSet dataSet)
    {
        DataGridTableStyle customersStyle = new()
        {
            MappingName = "Customers",
            AlternatingBackColor = Color.LightGray
        };

        DataGridColumnStyle currentColumn = new DataGridBoolColumn
        {
            MappingName = "Current",
            HeaderText = "IsCurrent Customer",
            Width = 150
        };
        customersStyle.GridColumnStyles.Add(currentColumn);

        DataGridColumnStyle customerNameColumn = new DataGridTextBoxColumn
        {
            MappingName = "custName",
            HeaderText = "Customer Name",
            Width = 250
        };
        customersStyle.GridColumnStyles.Add(customerNameColumn);

        DataGridTableStyle ordersStyle = new()
        {
            MappingName = "Orders",
            AlternatingBackColor = Color.LightBlue
        };

        DataGridTextBoxColumn orderDateColumn = new()
        {
            MappingName = "OrderDate",
            HeaderText = "Order Date",
            Width = 100
        };
        ordersStyle.GridColumnStyles.Add(orderDateColumn);

        PropertyDescriptorCollection properties = form.BindingContext[dataSet, "Customers.custToOrders"].GetItemProperties();
        PropertyDescriptor? orderAmountProperty = properties["OrderAmount"];
        Assert.NotNull(orderAmountProperty);

        DataGridTextBoxColumn orderAmountColumn = new(orderAmountProperty, "c", true)
        {
            MappingName = "OrderAmount",
            HeaderText = "Total",
            Width = 100
        };
        ordersStyle.GridColumnStyles.Add(orderAmountColumn);

        dataGrid.TableStyles.Add(customersStyle);
        dataGrid.TableStyles.Add(ordersStyle);
    }
}
