// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Windows.Forms;

namespace System.Windows.Forms.Legacy.Tests
{
    public class GridTableStylesCollectionTests
    {
        [StaFact]
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
