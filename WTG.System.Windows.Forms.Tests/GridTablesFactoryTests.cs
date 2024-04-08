using System.Windows.Forms;
using WTG.System.Windows.Forms.System.Windows.Forms;

namespace WTG.System.Windows.Forms.Tests
{
    public class GridTablesFactoryTests
    {
        public static IEnumerable<object[]> CreateGridTables_TestData()
        {
            yield return new object[] { null, null, null, null, new DataGridTableStyle[] { null } };
            var style = new DataGridTableStyle();
            yield return new object[] { style, new(), string.Empty, new BindingContext(), new DataGridTableStyle[] { style } };
            yield return new object[] { style, new(), "dataMember", new BindingContext(), new DataGridTableStyle[] { style } };
        }

        [Theory]
        [MemberData(nameof(CreateGridTables_TestData))]
        public void GridTablesFactory_CreateGridTables_Invoke_ReturnsExpected(DataGridTableStyle gridTable, object dataSource, string dataMember, BindingContext bindingManager, DataGridTableStyle[] expected)
        {
            Assert.Equal(expected, GridTablesFactory.CreateGridTables(gridTable, dataSource, dataMember, bindingManager));
        }
    }
}
