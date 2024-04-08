
using System.Windows.Forms;
using WTG.System.Windows.Forms.Tests.Common;

namespace WTG.System.Windows.Forms.Tests
{
    public class DataGridCellTests
    {
        [Fact]
        public void DataGridCell_Ctor_Default()
        {
            var cell = new DataGridCell();
            Assert.Equal(0, cell.RowNumber);
            Assert.Equal(0, cell.ColumnNumber);
        }

        [Theory]
        [InlineData(-1, -2)]
        [InlineData(0, 0)]
        [InlineData(1, 2)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        public void DataGridCell_Ctor_Int_Int(int rowNumber, int columnNumber)
        {
            var cell = new DataGridCell(rowNumber, columnNumber);
            Assert.Equal(rowNumber, cell.RowNumber);
            Assert.Equal(columnNumber, cell.ColumnNumber);
        }

        [Theory]
        [CommonMemberData(nameof(CommonTestHelper.GetIntTheoryData))]
        public void DataGridCell_RowNumber_Set_GetReturnsExpected(int value)
        {
            var cell = new DataGridCell
            {
                RowNumber = value
            };
            Assert.Equal(value, cell.RowNumber);

            // Set same.
            cell.RowNumber = value;
            Assert.Equal(value, cell.RowNumber);
        }

        [Theory]
        [CommonMemberData(nameof(CommonTestHelper.GetIntTheoryData))]
        public void DataGridCell_ColumnNumber_Set_GetReturnsExpected(int value)
        {
            var cell = new DataGridCell
            {
                ColumnNumber = value
            };
            Assert.Equal(value, cell.ColumnNumber);

            // Set same.
            cell.ColumnNumber = value;
            Assert.Equal(value, cell.ColumnNumber);
        }

        public static IEnumerable<object[]> Equals_TestData()
        {
            yield return new object[] { new DataGridCell(1, 2), new DataGridCell(1, 2), true };
            yield return new object[] { new DataGridCell(1, 2), new DataGridCell(2, 2), false };
            yield return new object[] { new DataGridCell(1, 2), new DataGridCell(1, 3), false };

            yield return new object[] { new DataGridCell(1, 2), new(), false };
            yield return new object[] { new DataGridCell(1, 2), "null", false };
        }

        [Theory]
        [MemberData(nameof(Equals_TestData))]
        public void DataGridCell_Equals_Invoke_ReturnsExpected(DataGridCell cell, object other, bool expected)
        {
            if (other is DataGridCell otherCell)
            {
                Assert.Equal(expected, cell.GetHashCode().Equals(otherCell.GetHashCode()));
            }

            Assert.Equal(expected, cell.Equals(other));
        }

        [Fact]
        public void DataGridCell_ToString_Invoke_ReturnsExpected()
        {
            var cell = new DataGridCell(1, 2);
            Assert.Equal("DataGridCell {RowNumber = 1, ColumnNumber = 2}", cell.ToString());
        }
    }
}
