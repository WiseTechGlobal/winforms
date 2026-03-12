namespace System.Windows.Forms.Tests
{
    [TestFixture]
    [SingleThreaded]
    public class TreeNodeTests
    {
        [Test]
        public void Text_SetAndGet_ReturnsCorrectValue()
        {
            // Arrange
            var treeNode = new TreeNode();
            var text = "Test Node";

            // Act
            treeNode.Text = text;

            // Assert
            Assert.That(treeNode.Text, Is.EqualTo(text));
        }

        [Test]
        public void Nodes_AddAndGet_ReturnsCorrectNodes()
        {
            // Arrange
            var parentNode = new TreeNode();
            var childNode = new TreeNode("Child Node");

            // Act
            parentNode.Nodes.Add(childNode);

            // Assert
            Assert.That(parentNode.Nodes.Count, Is.EqualTo(1));
            Assert.That(parentNode.Nodes[0], Is.EqualTo(childNode));
        }

        [Test]
        public void Parent_SetAndGet_ReturnsCorrectParent()
        {
            // Arrange
            var parentNode = new TreeNode("Parent Node");
            var childNode = new TreeNode("Child Node");

            // Act
            parentNode.Nodes.Add(childNode);

            // Assert
            Assert.That(childNode.Parent, Is.EqualTo(parentNode));
        }

        [Test]
        public void Remove_RemovesNodeFromParent()
        {
            // Arrange
            var parentNode = new TreeNode("Parent Node");
            var childNode = new TreeNode("Child Node");
            parentNode.Nodes.Add(childNode);

            // Act
            parentNode.Nodes.Remove(childNode);

            // Assert
            Assert.That(parentNode.Nodes.Count, Is.EqualTo(0));
            Assert.That(childNode.Parent, Is.Null);
        }

        [Test]
        public void TreeView_SetAndGet_ReturnsCorrectTreeView()
        {
            // Arrange
            var treeView = new TreeView();
            var treeNode = new TreeNode("Test Node");

            // Act
            treeView.Nodes.Add(treeNode);

            // Assert
            Assert.That(treeNode.TreeView, Is.EqualTo(treeView));
        }

        // Commenting out this test, as it doesn't work in DAT
        //[Test]
        //public void ContextMenu_ShowsOnRightClick()
        //{
        //    // Arrange
        //    var form = new Form();
        //    var treeView = new TreeView();
        //    var treeNode = new TreeNode("Test Node");
        //    var contextMenu = new ContextMenu();
        //    contextMenu.MenuItems.Add(new MenuItem("Test Item"));
        //    treeNode.ContextMenu = contextMenu;
        //    treeView.Nodes.Add(treeNode);
        //    treeView.Bounds = new Rectangle(10, 10, 200, 200);
        //    form.Controls.Add(treeView);

        //    bool contextMenuShown = false;
        //    contextMenu.Popup += (sender, e) => contextMenuShown = true;

        //    // Ensure the Form and TreeView are created and visible
        //    form.Load += async (sender, e) =>
        //    {
        //        // Need to wait for the form to be created and visible
        //        await Task.Delay(500);

        //        // Get the bounds of the TreeNode
        //        var nodeBounds = treeNode.Bounds;
        //        var clickPointRelativeToTreeView = nodeBounds.Location + new Size(nodeBounds.Width / 2, nodeBounds.Y + nodeBounds.Height / 2);
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
    }
}
