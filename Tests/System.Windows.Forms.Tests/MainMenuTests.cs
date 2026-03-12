namespace System.Windows.Forms.Tests
{
    using System.Drawing;
    using System.Runtime.InteropServices;

    [TestFixture]
    [SingleThreaded]
    public class MainMenuTests
    {
        [Test]
        public void MainMenu_SetAndGet_ReturnsCorrectValue()
        {
            // Arrange
            var form = new Form();
            var mainMenu = new MainMenu();

            // Act
            form.Menu = mainMenu;

            // Assert
            Assert.That(form.Menu, Is.EqualTo(mainMenu));
        }

        [Test]
        public void MainMenu_AddMenuItem_ReturnsCorrectMenuItem()
        {
            // Arrange
            var mainMenu = new MainMenu();
            var menuItem = new MenuItem("Test Item");

            // Act
            mainMenu.MenuItems.Add(menuItem);

            // Assert
            Assert.That(mainMenu.MenuItems.Count, Is.EqualTo(1));
            Assert.That(mainMenu.MenuItems[0], Is.EqualTo(menuItem));
        }

        [Test]
        public void MainMenu_FileMenuPopup_AddsMenuItemOnPopup()
        {
            // Arrange
            using var form = new Form();
            var mainMenu = new MainMenu();
            var fileMenuItem = new MenuItem("File");
            
            // Add popup event handler that adds a menu item when fired
            bool popupEventFired = false;
            MenuItem? addedMenuItem = null;
            
            fileMenuItem.Popup += (sender, e) =>
            {
                popupEventFired = true;
                addedMenuItem = new MenuItem("Dynamic Item");
                fileMenuItem.MenuItems.Add(addedMenuItem);
            };

            mainMenu.MenuItems.Add(fileMenuItem);
            form.Menu = mainMenu;
            form.Size = new Size(400, 300);

            // Initially, the File menu should have no items
            Assert.That(fileMenuItem.MenuItems.Count, Is.EqualTo(0));

            // Create the handle so we can send Windows messages
            var handle = form.Handle; // Forces handle creation
            
            // Act - Simulate WM_INITMENUPOPUP message to trigger popup event
            // This is what Windows sends when a menu is about to be displayed
            const uint WM_INITMENUPOPUP = 0x0117;
            
            // Send the message to trigger the popup event
            // The wParam contains the handle to the menu, lParam contains position info
            IntPtr result = SendMessage(handle, WM_INITMENUPOPUP, fileMenuItem.Handle, IntPtr.Zero);

            // Assert
            Assert.That(popupEventFired, Is.True, "Popup event should have been fired");
            Assert.That(fileMenuItem.MenuItems.Count, Is.EqualTo(1), "File menu should have one item after popup");
            
            if (addedMenuItem is not null)
            {
                Assert.That(fileMenuItem.MenuItems[0], Is.EqualTo(addedMenuItem), "The added item should be in the file menu");
                Assert.That(fileMenuItem.MenuItems[0].Text, Is.EqualTo("Dynamic Item"), "The added item should have the correct text");
            }
            
            // Clean up
            form.Dispose();
        }

        [Test]
        public void MainMenu_FileMenuWithSubmenu_PopupAddsItemToFileMenu()
        {
            // Arrange
            var form = new Form();
            var mainMenu = new MainMenu();
            var fileMenuItem = new MenuItem("File");
            var submenu = new MenuItem("Submenu");
            
            // Add the submenu to the File menu first
            fileMenuItem.MenuItems.Add(submenu);
            
            // Add popup event handler to the File menu (not the submenu)
            bool popupEventFired = false;
            MenuItem? addedMenuItem = null;
            
            fileMenuItem.Popup += (sender, e) =>
            {
                popupEventFired = true;
                addedMenuItem = new MenuItem("Dynamic Item Added to File");
                fileMenuItem.MenuItems.Add(addedMenuItem);
            };

            mainMenu.MenuItems.Add(fileMenuItem);
            form.Menu = mainMenu;
            form.Size = new Size(400, 300);

            // Initially, the File menu should have 1 item (the submenu)
            Assert.That(fileMenuItem.MenuItems.Count, Is.EqualTo(1));
            Assert.That(fileMenuItem.MenuItems[0], Is.EqualTo(submenu));

            // Create the handle so we can send Windows messages
            var handle = form.Handle; // Forces handle creation
            
            // Act - Simulate WM_INITMENUPOPUP message to the File menu
            const uint WM_INITMENUPOPUP = 0x0117;
            
            // Send the message to trigger the popup event on the File menu
            IntPtr result = SendMessage(handle, WM_INITMENUPOPUP, fileMenuItem.Handle, IntPtr.Zero);

            // Assert
            Assert.That(popupEventFired, Is.True, "Popup event should have been fired");
            Assert.That(fileMenuItem.MenuItems.Count, Is.EqualTo(2), "File menu should have two items after popup");
            Assert.That(fileMenuItem.MenuItems[0], Is.EqualTo(submenu), "The original submenu should still be there");
            
            if (addedMenuItem is not null)
            {
                Assert.That(fileMenuItem.MenuItems[1], Is.EqualTo(addedMenuItem), "The added item should be in the file menu");
                Assert.That(fileMenuItem.MenuItems[1].Text, Is.EqualTo("Dynamic Item Added to File"), "The added item should have the correct text");
            }
            
            // Clean up
            form.Dispose();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    }
}
