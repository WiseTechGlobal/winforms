namespace System.Windows.Forms.Tests
{
    using System.Drawing;

    [TestFixture]
    [SingleThreaded]
    public class TreeViewTests
    {
        [Test]
        public void ContextMenu_SetAndGet_ReturnsCorrectValue()
        {
            // Arrange
            var treeView = new TreeView();
            var contextMenu = new ContextMenu();

            // Act
            treeView.ContextMenu = contextMenu;

            // Assert
            Assert.That(treeView.ContextMenu, Is.EqualTo(contextMenu));
        }

        // Commenting out this test, as it doesn't work in DAT
        //[Test]
        //public void ContextMenu_ShowsOnRightClick()
        //{
        //    // Arrange
        //    var form = new Form();
        //    var treeView = new TreeView();
        //    var contextMenu = new ContextMenu();
        //    contextMenu.MenuItems.Add(new MenuItem("Test Item"));
        //    treeView.ContextMenu = contextMenu;
        //    treeView.Bounds = new Rectangle(10, 10, 200, 200);
        //    form.Controls.Add(treeView);

        //    bool contextMenuShown = false;
        //    contextMenu.Popup += (sender, e) => contextMenuShown = true;

        //    // Ensure the Form and TreeView are created and visible
        //    form.Load += async (sender, e) =>
        //    {
        //        // Wait for the tree view to be created
        //        await Task.Delay(500);

        //        var clickPointRelativeToTreeView = treeView.Bounds.Location + new Size(treeView.Bounds.Width / 2, treeView.Bounds.Y + treeView.Bounds.Height / 2);
        //        var clickPointRelativeToScreen = treeView.PointToScreen(clickPointRelativeToTreeView);

        //        // Simulate right-click event on the TreeNode
        //        Cursor.Position = clickPointRelativeToScreen;
        //        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.RightDown, (uint)clickPointRelativeToScreen.X, (uint)clickPointRelativeToScreen.Y);
        //        await Task.Delay(100);
        //        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.RightUp, (uint)clickPointRelativeToScreen.X, (uint)clickPointRelativeToScreen.Y);

        //        // Wait 1 second for the context menu to show
        //        await Task.Delay(1000);

        //        var clickPointOutsideContextMenuRelative = new Point(50, 50);
        //        var clickPointOutsideContextMenuAbsolute = treeView.PointToScreen(clickPointOutsideContextMenuRelative);

        //        Cursor.Position = clickPointOutsideContextMenuAbsolute;
        //        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown, (uint)clickPointOutsideContextMenuAbsolute.X, (uint)clickPointOutsideContextMenuAbsolute.Y);
        //        await Task.Delay(100);
        //        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp, (uint)clickPointOutsideContextMenuAbsolute.X, (uint)clickPointOutsideContextMenuAbsolute.Y);

        //        // Wait 1 second for the context menu to close
        //        await Task.Delay(1000);

        //        // Assert
        //        Assert.IsTrue(contextMenuShown);
        //        form.Close();
        //        return;
        //    };

        //    // Show the form
        //    form.ShowDialog();
        //}

        [Test]
        public void ContextMenu_ContainsExpectedItems()
        {
            // Arrange
            var treeView = new TreeView();
            var contextMenu = new ContextMenu();
            var menuItem = new MenuItem("Test Item");
            contextMenu.MenuItems.Add(menuItem);
            treeView.ContextMenu = contextMenu;

            // Act
            var items = treeView.ContextMenu.MenuItems;

            // Assert
            Assert.That(items.Count, Is.EqualTo(1));
            Assert.That(items[0].Text, Is.EqualTo("Test Item"));
        }
    }
}
