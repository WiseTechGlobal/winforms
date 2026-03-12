namespace System.Windows.Forms.Tests
{
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class MainMenuTests
    {
        [StaFact]
        public void MainMenu_SetAndGet_ReturnsCorrectValue()
        {
            // Arrange
            using Form form = new();
            MainMenu mainMenu = new();

            // Act
            form.Menu = mainMenu;

            // Assert
            Assert.Same(mainMenu, form.Menu);
        }

        [StaFact]
        public void MainMenu_AddMenuItem_ReturnsCorrectMenuItem()
        {
            // Arrange
            MainMenu mainMenu = new();
            MenuItem menuItem = new("Test Item");

            // Act
            mainMenu.MenuItems.Add(menuItem);

            // Assert
            Assert.Single(mainMenu.MenuItems);
            Assert.Same(menuItem, mainMenu.MenuItems[0]);
        }

        [StaFact]
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
            Assert.Empty(fileMenuItem.MenuItems);

            // Create the handle so we can send Windows messages
            var handle = form.Handle; // Forces handle creation
            
            // Act - Simulate WM_INITMENUPOPUP message to trigger popup event
            // This is what Windows sends when a menu is about to be displayed
            const uint WM_INITMENUPOPUP = 0x0117;
            
            // Send the message to trigger the popup event
            // The wParam contains the handle to the menu, lParam contains position info
            SendMessage(handle, WM_INITMENUPOPUP, fileMenuItem.Handle, IntPtr.Zero);

            // Assert
            Assert.True(popupEventFired, "Popup event should have been fired");
            Assert.Single(fileMenuItem.MenuItems);
            
            if (addedMenuItem is not null)
            {
                Assert.Same(addedMenuItem, fileMenuItem.MenuItems[0]);
                Assert.Equal("Dynamic Item", fileMenuItem.MenuItems[0].Text);
            }
            
            // Clean up
            form.Dispose();
        }

        [StaFact]
        public void MainMenu_FileMenuWithSubmenu_PopupAddsItemToFileMenu()
        {
            // Arrange
            using Form form = new();
            MainMenu mainMenu = new();
            MenuItem fileMenuItem = new("File");
            MenuItem submenu = new("Submenu");
            
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
            Assert.Single(fileMenuItem.MenuItems);
            Assert.Same(submenu, fileMenuItem.MenuItems[0]);

            // Create the handle so we can send Windows messages
            var handle = form.Handle; // Forces handle creation
            
            // Act - Simulate WM_INITMENUPOPUP message to the File menu
            const uint WM_INITMENUPOPUP = 0x0117;
            
            // Send the message to trigger the popup event on the File menu
            SendMessage(handle, WM_INITMENUPOPUP, fileMenuItem.Handle, IntPtr.Zero);

            // Assert
            Assert.True(popupEventFired, "Popup event should have been fired");
            Assert.Equal(2, fileMenuItem.MenuItems.Count);
            Assert.Same(submenu, fileMenuItem.MenuItems[0]);
            
            if (addedMenuItem is not null)
            {
                Assert.Same(addedMenuItem, fileMenuItem.MenuItems[1]);
                Assert.Equal("Dynamic Item Added to File", fileMenuItem.MenuItems[1].Text);
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    }
}
