// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Windows.Forms;

namespace WTG.System.Windows.Forms.System.Windows.Forms
{
    public static class GridTablesFactory
    {
        /// <summary>
        ///  Takes a DataView and creates an intelligent mapping of DataView storage
        ///  types into available DataColumn types.
        /// </summary>
        public static DataGridTableStyle[] CreateGridTables(DataGridTableStyle gridTable, object dataSource, string dataMember, BindingContext bindingManager)
        {
            return new DataGridTableStyle[] { gridTable };
        }
    }
}
