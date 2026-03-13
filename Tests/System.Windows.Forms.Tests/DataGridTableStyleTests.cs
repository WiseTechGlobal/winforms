using System.Drawing;

namespace System.Windows.Forms.Tests;

public class DataGridTableStyleTests
{
    [StaFact]
    public void DataGridTableStyle_Ctor_Default()
    {
        using DataGridTableStyle style = new();

        Assert.True(style.AllowSorting);
        Assert.Equal(SystemColors.Window, style.AlternatingBackColor);
        Assert.Equal(SystemColors.Window, style.BackColor);
        Assert.True(style.ColumnHeadersVisible);
        Assert.Null(style.DataGrid);
        Assert.Equal(SystemColors.WindowText, style.ForeColor);
        Assert.Empty(style.GridColumnStyles);
        Assert.Same(style.GridColumnStyles, style.GridColumnStyles);
        Assert.Equal(SystemColors.Control, style.GridLineColor);
        Assert.Equal(DataGridLineStyle.Solid, style.GridLineStyle);
        Assert.Equal(SystemColors.Control, style.HeaderBackColor);
        Assert.Same(Control.DefaultFont, style.HeaderFont);
        Assert.Equal(SystemColors.ControlText, style.HeaderForeColor);
        Assert.Equal(SystemColors.HotTrack, style.LinkColor);
        Assert.Equal(SystemColors.HotTrack, style.LinkHoverColor);
        Assert.Empty(style.MappingName);
        Assert.Equal(75, style.PreferredColumnWidth);
        Assert.Equal(Control.DefaultFont.Height + 3, style.PreferredRowHeight);
        Assert.False(style.ReadOnly);
        Assert.True(style.RowHeadersVisible);
        Assert.Equal(35, style.RowHeaderWidth);
        Assert.Equal(SystemColors.ActiveCaption, style.SelectionBackColor);
        Assert.Equal(SystemColors.ActiveCaptionText, style.SelectionForeColor);
        Assert.Null(style.Site);
    }

    [StaFact]
    public void DataGridTableStyle_AllowSorting_Set_GetReturnsExpected()
    {
        using DataGridTableStyle style = new();

        style.AllowSorting = false;

        Assert.False(style.AllowSorting);

        // Set same.
        style.AllowSorting = false;

        Assert.False(style.AllowSorting);

        // Set different.
        style.AllowSorting = true;

        Assert.True(style.AllowSorting);
    }

    [StaFact]
    public void DataGridTableStyle_AllowSorting_SetWithHandler_CallsAllowSortingChanged()
    {
        using DataGridTableStyle style = new();
        int callCount = 0;
        EventHandler handler = (sender, e) =>
        {
            Assert.Same(style, sender);
            Assert.Same(EventArgs.Empty, e);
            callCount++;
        };
        style.AllowSortingChanged += handler;

        // Set different.
        style.AllowSorting = false;

        Assert.False(style.AllowSorting);
        Assert.Equal(1, callCount);

        // Set same.
        style.AllowSorting = false;

        Assert.False(style.AllowSorting);
        Assert.Equal(1, callCount);

        // Set different.
        style.AllowSorting = true;

        Assert.True(style.AllowSorting);
        Assert.Equal(2, callCount);

        // Remove handler.
        style.AllowSortingChanged -= handler;
        style.AllowSorting = false;

        Assert.Equal(2, callCount);
    }

    [StaFact]
    public void DataGridTableStyle_AllowSorting_SetDefaultTableStyle_ThrowsArgumentException()
    {
        using DataGridTableStyle style = new(isDefaultTableStyle: true);

        Assert.Throws<ArgumentException>(() => style.AllowSorting = false);
        Assert.True(style.AllowSorting);
    }

    [StaFact]
    public void DataGridTableStyle_AlternatingBackColor_Set_GetReturnsExpected()
    {
        using DataGridTableStyle style = new();

        style.AlternatingBackColor = Color.Red;

        Assert.Equal(Color.Red, style.AlternatingBackColor);

        // Set same.
        style.AlternatingBackColor = Color.Red;

        Assert.Equal(Color.Red, style.AlternatingBackColor);

        // Set different.
        style.AlternatingBackColor = Color.Blue;

        Assert.Equal(Color.Blue, style.AlternatingBackColor);
    }

    [StaFact]
    public void DataGridTableStyle_AlternatingBackColor_SetWithHandler_CallsAlternatingBackColorChanged()
    {
        using DataGridTableStyle style = new();
        int callCount = 0;
        EventHandler handler = (sender, e) =>
        {
            Assert.Same(style, sender);
            Assert.Same(EventArgs.Empty, e);
            callCount++;
        };
        style.AlternatingBackColorChanged += handler;

        // Set different.
        style.AlternatingBackColor = Color.Red;

        Assert.Equal(Color.Red, style.AlternatingBackColor);
        Assert.Equal(1, callCount);

        // Set same.
        style.AlternatingBackColor = Color.Red;

        Assert.Equal(Color.Red, style.AlternatingBackColor);
        Assert.Equal(1, callCount);

        // Set different.
        style.AlternatingBackColor = Color.Blue;

        Assert.Equal(Color.Blue, style.AlternatingBackColor);
        Assert.Equal(2, callCount);

        // Remove handler.
        style.AlternatingBackColorChanged -= handler;
        style.AlternatingBackColor = Color.Red;

        Assert.Equal(2, callCount);
    }

    [StaFact]
    public void DataGridTableStyle_AlternatingBackColor_SetEmpty_ThrowsArgumentException()
    {
        using DataGridTableStyle style = new();

        Assert.Throws<ArgumentException>(() => style.AlternatingBackColor = Color.Empty);
    }

    [StaFact]
    public void DataGridTableStyle_AlternatingBackColor_SetDefaultTableStyle_ThrowsArgumentException()
    {
        using DataGridTableStyle style = new(isDefaultTableStyle: true);

        Assert.Throws<ArgumentException>(() => style.AlternatingBackColor = Color.Red);
        Assert.Equal(SystemColors.Window, style.AlternatingBackColor);
    }

    [StaFact]
    public void DataGridTableStyle_BackColor_SetWithHandler_CallsBackColorChanged()
    {
        using DataGridTableStyle style = new();
        int callCount = 0;
        EventHandler handler = (sender, e) =>
        {
            Assert.Same(style, sender);
            Assert.Same(EventArgs.Empty, e);
            callCount++;
        };
        style.BackColorChanged += handler;

        // Set different.
        style.BackColor = Color.Red;

        Assert.Equal(Color.Red, style.BackColor);
        Assert.Equal(1, callCount);

        // Set same.
        style.BackColor = Color.Red;

        Assert.Equal(Color.Red, style.BackColor);
        Assert.Equal(1, callCount);

        // Set different.
        style.BackColor = Color.Blue;

        Assert.Equal(Color.Blue, style.BackColor);
        Assert.Equal(2, callCount);

        // Remove handler.
        style.BackColorChanged -= handler;
        style.BackColor = Color.Red;

        Assert.Equal(2, callCount);
    }

    [StaFact]
    public void DataGridTableStyle_BackColor_SetEmpty_ThrowsArgumentException()
    {
        using DataGridTableStyle style = new();

        Assert.Throws<ArgumentException>(() => style.BackColor = Color.Empty);
    }

    [StaFact]
    public void DataGridTableStyle_MappingName_Set_GetReturnsExpected()
    {
        using DataGridTableStyle style = new();

        style.MappingName = "Customers";

        Assert.Equal("Customers", style.MappingName);

        // Set same.
        style.MappingName = "Customers";

        Assert.Equal("Customers", style.MappingName);

        // Set different.
        style.MappingName = "Orders";

        Assert.Equal("Orders", style.MappingName);

        // Set empty.
        style.MappingName = string.Empty;

        Assert.Empty(style.MappingName);
    }

    [StaFact]
    public void DataGridTableStyle_PreferredColumnWidth_Set_GetReturnsExpected()
    {
        using DataGridTableStyle style = new();

        style.PreferredColumnWidth = 200;

        Assert.Equal(200, style.PreferredColumnWidth);

        // Set same.
        style.PreferredColumnWidth = 200;

        Assert.Equal(200, style.PreferredColumnWidth);

        // Restore default.
        style.PreferredColumnWidth = 75;

        Assert.Equal(75, style.PreferredColumnWidth);
    }

    [StaFact]
    public void DataGridTableStyle_ReadOnly_Set_GetReturnsExpected()
    {
        using DataGridTableStyle style = new();

        style.ReadOnly = true;

        Assert.True(style.ReadOnly);

        // Set same.
        style.ReadOnly = true;

        Assert.True(style.ReadOnly);

        // Set different.
        style.ReadOnly = false;

        Assert.False(style.ReadOnly);
    }

    [StaFact]
    public void DataGridTableStyle_ReadOnly_SetWithHandler_CallsReadOnlyChanged()
    {
        using DataGridTableStyle style = new();
        int callCount = 0;
        EventHandler handler = (sender, e) =>
        {
            Assert.Same(style, sender);
            Assert.Same(EventArgs.Empty, e);
            callCount++;
        };
        style.ReadOnlyChanged += handler;

        // Set different.
        style.ReadOnly = true;

        Assert.True(style.ReadOnly);
        Assert.Equal(1, callCount);

        // Set same.
        style.ReadOnly = true;

        Assert.True(style.ReadOnly);
        Assert.Equal(1, callCount);

        // Set different.
        style.ReadOnly = false;

        Assert.False(style.ReadOnly);
        Assert.Equal(2, callCount);

        // Remove handler.
        style.ReadOnlyChanged -= handler;
        style.ReadOnly = true;

        Assert.Equal(2, callCount);
    }

    [StaFact]
    public void DataGridTableStyle_GridLineStyle_Set_GetReturnsExpected()
    {
        using DataGridTableStyle style = new();

        style.GridLineStyle = DataGridLineStyle.None;

        Assert.Equal(DataGridLineStyle.None, style.GridLineStyle);

        // Set same.
        style.GridLineStyle = DataGridLineStyle.None;

        Assert.Equal(DataGridLineStyle.None, style.GridLineStyle);

        // Set different.
        style.GridLineStyle = DataGridLineStyle.Solid;

        Assert.Equal(DataGridLineStyle.Solid, style.GridLineStyle);
    }

    [StaFact]
    public void DataGridTableStyle_GridColumnStyles_AddAndRemove_UpdatesCollection()
    {
        using DataGridTableStyle style = new()
        {
            MappingName = "Items"
        };

        DataGridTextBoxColumn column = new()
        {
            MappingName = "Name",
            HeaderText = "Item Name",
            Width = 150
        };

        style.GridColumnStyles.Add(column);

        Assert.Single(style.GridColumnStyles);
        Assert.Same(column, style.GridColumnStyles["Name"]);

        style.GridColumnStyles.Remove(column);

        Assert.Empty(style.GridColumnStyles);
    }
}
