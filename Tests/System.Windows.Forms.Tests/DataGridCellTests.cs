namespace System.Windows.Forms.Tests;

public class DataGridCellTests
{
    [StaFact]
    public void DataGridCell_Ctor_Default()
    {
        DataGridCell cell = default;

        Assert.Equal(0, cell.RowNumber);
        Assert.Equal(0, cell.ColumnNumber);
    }

    [StaFact]
    public void DataGridCell_Ctor_Int_Int_Zero()
    {
        DataGridCell cell = new(0, 0);

        Assert.Equal(0, cell.RowNumber);
        Assert.Equal(0, cell.ColumnNumber);
    }

    [StaFact]
    public void DataGridCell_Ctor_Int_Int_Positive()
    {
        DataGridCell cell = new(1, 2);

        Assert.Equal(1, cell.RowNumber);
        Assert.Equal(2, cell.ColumnNumber);
    }

    [StaFact]
    public void DataGridCell_Ctor_Int_Int_Negative()
    {
        DataGridCell cell = new(-1, -2);

        Assert.Equal(-1, cell.RowNumber);
        Assert.Equal(-2, cell.ColumnNumber);
    }

    [StaFact]
    public void DataGridCell_RowNumber_Set_GetReturnsExpected()
    {
        DataGridCell cell = new()
        {
            RowNumber = 5
        };

        Assert.Equal(5, cell.RowNumber);

        // Set same.
        cell.RowNumber = 5;

        Assert.Equal(5, cell.RowNumber);

        // Set different.
        cell.RowNumber = 10;

        Assert.Equal(10, cell.RowNumber);
    }

    [StaFact]
    public void DataGridCell_ColumnNumber_Set_GetReturnsExpected()
    {
        DataGridCell cell = new()
        {
            ColumnNumber = 3
        };

        Assert.Equal(3, cell.ColumnNumber);

        // Set same.
        cell.ColumnNumber = 3;

        Assert.Equal(3, cell.ColumnNumber);

        // Set different.
        cell.ColumnNumber = 7;

        Assert.Equal(7, cell.ColumnNumber);
    }

    [StaFact]
    public void DataGridCell_Equals_SameRowAndColumn_ReturnsTrue()
    {
        DataGridCell cell1 = new(1, 2);
        DataGridCell cell2 = new(1, 2);

        Assert.True(cell1.Equals(cell2));
        Assert.Equal(cell1.GetHashCode(), cell2.GetHashCode());
    }

    [StaFact]
    public void DataGridCell_Equals_DifferentRow_ReturnsFalse()
    {
        DataGridCell cell1 = new(1, 2);
        DataGridCell cell2 = new(2, 2);

        Assert.False(cell1.Equals(cell2));
    }

    [StaFact]
    public void DataGridCell_Equals_DifferentColumn_ReturnsFalse()
    {
        DataGridCell cell1 = new(1, 2);
        DataGridCell cell2 = new(1, 3);

        Assert.False(cell1.Equals(cell2));
    }

    [StaFact]
    public void DataGridCell_Equals_NonDataGridCellObject_ReturnsFalse()
    {
        DataGridCell cell = new(1, 2);

        Assert.False(cell.Equals(new object()));
    }

    [StaFact]
    public void DataGridCell_Equals_Null_ReturnsFalse()
    {
        DataGridCell cell = new(1, 2);

        Assert.False(cell.Equals(null));
    }

    [StaFact]
    public void DataGridCell_ToString_ReturnsExpected()
    {
        DataGridCell cell = new(1, 2);

        Assert.Equal("DataGridCell {RowNumber = 1, ColumnNumber = 2}", cell.ToString());
    }

    [StaFact]
    public void DataGridCell_ToString_DefaultValues_ReturnsExpected()
    {
        DataGridCell cell = default;

        Assert.Equal("DataGridCell {RowNumber = 0, ColumnNumber = 0}", cell.ToString());
    }
}
