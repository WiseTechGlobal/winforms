namespace System.Windows.Forms.Tests
{
    using System.Drawing;

    [TestFixture]
    [SingleThreaded]
    public class MenuSizeCalculationTests
    {
        [Test]
        public void Form_WithMenu_HasCorrectWindowSize()
        {
            // Arrange
            using var formWithMenu = new Form();
            using var formWithoutMenu = new Form();
            
            var menu = new MainMenu();
            menu.MenuItems.Add(new MenuItem("File"));
            formWithMenu.Menu = menu;
            
            var clientSize = new Size(400, 300);
            
            // Act
            formWithMenu.ClientSize = clientSize;
            formWithoutMenu.ClientSize = clientSize;
            
            // Assert
            Assert.That(formWithMenu.ClientSize, Is.EqualTo(clientSize), "Form with menu should have correct client size");
            Assert.That(formWithoutMenu.ClientSize, Is.EqualTo(clientSize), "Form without menu should have correct client size");
            
            // The key test: form with menu should be taller due to menu bar
            Assert.That(formWithMenu.Size.Height, Is.GreaterThan(formWithoutMenu.Size.Height), 
                "Form with menu should be taller than form without menu (accounts for menu bar height)");
                
            // Width should be the same (menu doesn't affect width)
            Assert.That(formWithMenu.Size.Width, Is.EqualTo(formWithoutMenu.Size.Width), 
                "Form width should not be affected by menu presence");
        }

        [Test]
        public void Form_WithEmptyMenu_SameHeightAsFormWithoutMenu()
        {
            // Arrange
            using var formWithEmptyMenu = new Form();
            using var formWithoutMenu = new Form();
            
            var emptyMenu = new MainMenu(); // No menu items
            formWithEmptyMenu.Menu = emptyMenu;
            
            var clientSize = new Size(400, 300);
            
            // Act
            formWithEmptyMenu.ClientSize = clientSize;
            formWithoutMenu.ClientSize = clientSize;
            
            // Assert
            // According to the implementation, empty menus should not affect window height
            Assert.That(formWithEmptyMenu.Size.Height, Is.EqualTo(formWithoutMenu.Size.Height), 
                "Form with empty menu should have same height as form without menu");
        }

        [Test]
        public void Form_MenuAddedAfterCreation_AdjustsSize()
        {
            // Arrange
            using var form = new Form();
            var clientSize = new Size(400, 300);
            form.ClientSize = clientSize;
            
            var initialHeight = form.Size.Height;
            
            // Act - Add menu with items
            var menu = new MainMenu();
            menu.MenuItems.Add(new MenuItem("File"));
            form.Menu = menu;
            form.ClientSize = clientSize; // Trigger recalculation
            
            // Assert
            Assert.That(form.Size.Height, Is.GreaterThan(initialHeight), 
                "Form height should increase when menu with items is added");
            Assert.That(form.ClientSize, Is.EqualTo(clientSize), 
                "Client size should remain consistent after menu addition");
        }

        [Test]
        public void Form_MenuRemovedAfterCreation_AdjustsSize()
        {
            // Arrange
            using var form = new Form();
            var menu = new MainMenu();
            menu.MenuItems.Add(new MenuItem("File"));
            form.Menu = menu;
            
            var clientSize = new Size(400, 300);
            form.ClientSize = clientSize;
            var heightWithMenu = form.Size.Height;
            
            // Act - Remove menu
            form.Menu = null;
            form.ClientSize = clientSize; // Trigger recalculation
            
            // Assert
            Assert.That(form.Size.Height, Is.LessThan(heightWithMenu), 
                "Form height should decrease when menu is removed");
            Assert.That(form.ClientSize, Is.EqualTo(clientSize), 
                "Client size should remain consistent after menu removal");
        }

        [Test]
        public void Form_NonTopLevel_MenuDoesNotAffectSize()
        {
            // Arrange
            using var parentForm = new Form();
            using var childForm = new Form();
            
            var menu = new MainMenu();
            menu.MenuItems.Add(new MenuItem("File"));
            childForm.Menu = menu;
            childForm.TopLevel = false;
            childForm.Parent = parentForm;
            
            var clientSize = new Size(200, 150);
            
            // Create a comparable form without menu but also non-toplevel
            using var childFormNoMenu = new Form();
            childFormNoMenu.TopLevel = false;
            childFormNoMenu.Parent = parentForm;
            
            // Act
            childForm.ClientSize = clientSize;
            childFormNoMenu.ClientSize = clientSize;
            
            // Assert
            // Non-top-level forms should not account for menus in sizing
            Assert.That(childForm.Size.Height, Is.EqualTo(childFormNoMenu.Size.Height), 
                "Non-top-level forms should not be affected by menu presence");
        }

        [Test]
        public void MDIChild_MenuDoesNotAffectSize()
        {
            // Arrange
            using var parentForm = new Form();
            parentForm.IsMdiContainer = true;
            
            using var mdiChild1 = new Form();
            using var mdiChild2 = new Form();
            
            var menu = new MainMenu();
            menu.MenuItems.Add(new MenuItem("File"));
            mdiChild1.Menu = menu;
            
            mdiChild1.MdiParent = parentForm;
            mdiChild2.MdiParent = parentForm;
            
            var clientSize = new Size(200, 150);
            
            // Act
            mdiChild1.ClientSize = clientSize;
            mdiChild2.ClientSize = clientSize;
            
            // Assert
            // MDI children should not account for menus in sizing
            Assert.That(mdiChild1.Size.Height, Is.EqualTo(mdiChild2.Size.Height), 
                "MDI child forms should not be affected by menu presence");
        }

        [Test]
        public void Form_MenuWithMultipleItems_SameHeightAsMenuWithOneItem()
        {
            // Arrange
            using var formWithOneMenuItem = new Form();
            using var formWithMultipleMenuItems = new Form();
            
            var menuOne = new MainMenu();
            menuOne.MenuItems.Add(new MenuItem("File"));
            formWithOneMenuItem.Menu = menuOne;
            
            var menuMultiple = new MainMenu();
            menuMultiple.MenuItems.Add(new MenuItem("File"));
            menuMultiple.MenuItems.Add(new MenuItem("Edit"));
            menuMultiple.MenuItems.Add(new MenuItem("View"));
            formWithMultipleMenuItems.Menu = menuMultiple;
            
            var clientSize = new Size(400, 300);
            
            // Act
            formWithOneMenuItem.ClientSize = clientSize;
            formWithMultipleMenuItems.ClientSize = clientSize;
            
            // Assert
            // Number of menu items shouldn't affect form height (all in same menu bar)
            Assert.That(formWithMultipleMenuItems.Size.Height, Is.EqualTo(formWithOneMenuItem.Size.Height), 
                "Forms should have same height regardless of number of menu items");
        }

        [Test]
        public void Form_MenuWithSubmenus_SameHeightAsMenuWithoutSubmenus()
        {
            // Arrange
            using var formWithSubmenu = new Form();
            using var formWithoutSubmenu = new Form();
            
            var menuWithSubmenu = new MainMenu();
            var fileMenu = new MenuItem("File");
            fileMenu.MenuItems.Add(new MenuItem("New"));
            fileMenu.MenuItems.Add(new MenuItem("Open"));
            menuWithSubmenu.MenuItems.Add(fileMenu);
            formWithSubmenu.Menu = menuWithSubmenu;
            
            var menuWithoutSubmenu = new MainMenu();
            menuWithoutSubmenu.MenuItems.Add(new MenuItem("File"));
            formWithoutSubmenu.Menu = menuWithoutSubmenu;
            
            var clientSize = new Size(400, 300);
            
            // Act
            formWithSubmenu.ClientSize = clientSize;
            formWithoutSubmenu.ClientSize = clientSize;
            
            // Assert
            // Submenus shouldn't affect form height (they're dropdowns)
            Assert.That(formWithSubmenu.Size.Height, Is.EqualTo(formWithoutSubmenu.Size.Height), 
                "Forms should have same height regardless of submenu complexity");
        }

        [Test]
        public void Form_SetClientSizeMultipleTimes_ConsistentBehavior()
        {
            // Test that the HasMenu logic is consistently applied
            
            // Arrange
            using var form = new Form();
            var menu = new MainMenu();
            menu.MenuItems.Add(new MenuItem("File"));
            form.Menu = menu;
            
            var clientSize1 = new Size(300, 200);
            var clientSize2 = new Size(500, 400);
            
            // Act & Assert
            form.ClientSize = clientSize1;
            Assert.That(form.ClientSize, Is.EqualTo(clientSize1), "First client size setting should work correctly");
            var height1 = form.Size.Height;
            
            form.ClientSize = clientSize2;
            Assert.That(form.ClientSize, Is.EqualTo(clientSize2), "Second client size setting should work correctly");
            var height2 = form.Size.Height;
            
            // The height difference should be proportional to client size difference
            var clientHeightDiff = clientSize2.Height - clientSize1.Height;
            var windowHeightDiff = height2 - height1;
            Assert.That(windowHeightDiff, Is.EqualTo(clientHeightDiff), 
                "Window height difference should equal client height difference (menu bar height is constant)");
        }

        [Test]
        public void Form_MenuRemovedBeforeHandleCreated_SizeUpdatesImmediately()
        {
            // This test verifies the fix for the issue where setting Menu to null 
            // before handle creation didn't update the form size immediately
            
            // Arrange
            using var form = new Form();
            
            // Create menu with items
            var menu = new MainMenu();
            menu.MenuItems.Add(new MenuItem("File"));
            menu.MenuItems.Add(new MenuItem("Edit"));
            
            // Set menu first
            form.Menu = menu;
            
            // Set client size - this should trigger FormStateSetClientSize
            var targetClientSize = new Size(400, 300);
            form.ClientSize = targetClientSize;
            
            // Capture size with menu (before handle is created)
            var sizeWithMenu = form.Size;
            
            // Act - Remove menu before handle is created
            // This should trigger the fix in line 760: if FormStateSetClientSize == 1 && !IsHandleCreated
            form.Menu = null;
            
            // Assert
            var sizeWithoutMenu = form.Size;
            
            // The form size should be updated immediately when menu is removed
            Assert.That(sizeWithoutMenu.Height, Is.LessThan(sizeWithMenu.Height), 
                "Form height should decrease immediately when menu is removed before handle creation");
            
            Assert.That(form.ClientSize, Is.EqualTo(targetClientSize), 
                "Client size should remain the target size after menu removal");
            
            // Width should remain the same
            Assert.That(sizeWithoutMenu.Width, Is.EqualTo(sizeWithMenu.Width), 
                "Form width should not change when menu is removed");
        }
    }
}
