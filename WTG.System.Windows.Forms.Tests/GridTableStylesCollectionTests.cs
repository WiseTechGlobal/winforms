
using System.Windows.Forms;

namespace WTG.System.Windows.Forms.Tests
{
    public class GridTableStylesCollectionTests
    {
        [Fact]
        public void GridTableStylesCollection_Add_DataGridTableStyle_Success()
        {
            var dataGrid = new DataGrid();
            GridTableStylesCollection collection = dataGrid.TableStyles;
            var style = new DataGridTableStyle();
            Assert.Equal(0, collection.Add(style));
            Assert.Same(style, Assert.Single(collection));
            Assert.Same(dataGrid, style.DataGrid);
        }
    }
}
