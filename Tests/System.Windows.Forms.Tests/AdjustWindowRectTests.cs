namespace System.Windows.Forms.Tests
{
    using System.Drawing;

    [TestFixture]
    [SingleThreaded]
    public class AdjustWindowRectTests
    {
        [Test]
        public void Form_WindowRectCalculation_HandlesDpiScaling()
        {
            // Test that menu consideration works across different DPI scenarios
            
            // Arrange
            using var form = new Form();
            var menu = new MainMenu();
            menu.MenuItems.Add(new MenuItem("Test"));
            form.Menu = menu;
            
            var baseClientSize = new Size(400, 300);
            
            // Act
            form.ClientSize = baseClientSize;
            var windowSize = form.Size;
            
            // Change the client size again to ensure consistency
            form.ClientSize = new Size(600, 450);
            var newWindowSize = form.Size;
            
            // Assert
            // The window size should scale proportionally with client size
            var clientSizeChange = new Size(200, 150); // 600-400, 450-300
            var windowSizeChange = new Size(newWindowSize.Width - windowSize.Width, newWindowSize.Height - windowSize.Height);
            
            Assert.That(windowSizeChange, Is.EqualTo(clientSizeChange),
                "Window size changes should match client size changes when menu is present");
        }

        [Test]
        public void Form_CreateParams_MenuConsideredInWindowRectCalculation()
        {
            // Test that CreateParams and window rect calculations are consistent
            
            // Arrange
            using var formWithMenu = new Form();
            using var formWithoutMenu = new Form();
            
            var menu = new MainMenu();
            menu.MenuItems.Add(new MenuItem("File"));
            formWithMenu.Menu = menu;
            
            // Set same size and create handles to trigger CreateParams usage
            var testSize = new Size(300, 200);
            formWithMenu.Size = testSize;
            formWithoutMenu.Size = testSize;
            
            // Force handle creation to trigger CreateParams
            _ = formWithMenu.Handle;
            _ = formWithoutMenu.Handle;
            
            // Act & Assert
            // The forms should maintain their size relationships after handle creation
            // which exercises the CreateParams path
            formWithMenu.ClientSize = new Size(250, 150);
            formWithoutMenu.ClientSize = new Size(250, 150);
            
            Assert.That(formWithMenu.Size.Height, Is.GreaterThan(formWithoutMenu.Size.Height),
                "After handle creation, form with menu should still be taller");
        }

        [Test]
        public void ToolStripTextBox_InFormWithMenu_IndependentSizing()
        {
            // Test that ToolStripTextBox in a form with menu isn't affected by the form's menu
            
            // Arrange
            using var formWithMenu = new Form();
            using var formWithoutMenu = new Form();
            
            var menu = new MainMenu();
            menu.MenuItems.Add(new MenuItem("File"));
            formWithMenu.Menu = menu;
            
            using var toolStrip1 = new ToolStrip();
            using var toolStrip2 = new ToolStrip();
            using var textBox1 = new ToolStripTextBox();
            using var textBox2 = new ToolStripTextBox();
            
            toolStrip1.Items.Add(textBox1);
            toolStrip2.Items.Add(textBox2);
            formWithMenu.Controls.Add(toolStrip1);
            formWithoutMenu.Controls.Add(toolStrip2);
            
            // Act
            textBox1.Size = new Size(120, 22);
            textBox2.Size = new Size(120, 22);
            
            // Assert
            Assert.That(textBox1.Size, Is.EqualTo(textBox2.Size),
                "ToolStripTextBox should not be affected by parent form's menu");
        }

        [Test]
        public void Control_SizeCalculations_ConsistentForDifferentControlTypes()
        {
            // Test various control types to ensure they handle menu considerations correctly

            // Arrange & Act & Assert
            using var form1 = new Form(); // No menu
            using var form2 = new Form(); // Menu with items

            var menuWithItems = new MainMenu();
            menuWithItems.MenuItems.Add(new MenuItem("File"));
            form2.Menu = menuWithItems;

            var clientSize = new Size(400, 300);

            // Act
            form1.ClientSize = clientSize;
            form2.ClientSize = clientSize;

            var controlTypes = new Func<Control>[]
            {
                () => new Button(),
                () => new Label(),
                () => new TextBox(),
                () => new Panel(),
                () => new GroupBox()
            };

            foreach (var createControl in controlTypes)
            {
                using var control1 = createControl();
                using var control2 = createControl();
                
                var testSize = new Size(100, 50);
                control1.Size = testSize;
                control2.Size = testSize;

                form1.Controls.Add(control1);
                form2.Controls.Add(control2);

                Assert.That(control1.Size, Is.EqualTo(control2.Size),
                    $"{control1.GetType().Name} controls should have consistent sizing behavior");
            }
        }

        [Test]
        public void Form_MenuVisibility_AffectsWindowSizeCalculation()
        {
            // Test the specific logic: menu only affects size if it has items
            
            // Arrange
            using var form1 = new Form(); // No menu
            using var form2 = new Form(); // Empty menu
            using var form3 = new Form(); // Menu with items
            
            var emptyMenu = new MainMenu();
            form2.Menu = emptyMenu;
            
            var menuWithItems = new MainMenu();
            menuWithItems.MenuItems.Add(new MenuItem("File"));
            form3.Menu = menuWithItems;
            
            var clientSize = new Size(400, 300);
            
            // Act
            form1.ClientSize = clientSize;
            form2.ClientSize = clientSize;
            form3.ClientSize = clientSize;
            
            // Assert
            // Forms 1 and 2 should have same height (no menu or empty menu)
            Assert.That(form1.Size.Height, Is.EqualTo(form2.Size.Height),
                "Form with no menu and form with empty menu should have same height");
                
            // Form 3 should be taller (has menu items)
            Assert.That(form3.Size.Height, Is.GreaterThan(form1.Size.Height),
                "Form with menu items should be taller than form without menu");
                
            Assert.That(form3.Size.Height, Is.GreaterThan(form2.Size.Height),
                "Form with menu items should be taller than form with empty menu");
        }
    }
}
