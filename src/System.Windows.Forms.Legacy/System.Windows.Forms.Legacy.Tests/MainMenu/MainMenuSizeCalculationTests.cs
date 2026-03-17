using global::System.Drawing;
using global::System.Windows.Forms;

namespace System.Windows.Forms.Legacy.Tests;

public class MainMenuSizeCalculationTests
{
    [StaFact]
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
        Assert.Equal(clientSize, formWithMenu.ClientSize);
        Assert.Equal(clientSize, formWithoutMenu.ClientSize);

        // The key test: form with menu should be taller due to menu bar
        Assert.True(formWithMenu.Size.Height > formWithoutMenu.Size.Height,
            "Form with menu should be taller than form without menu (accounts for menu bar height)");

        // Width should be the same (menu doesn't affect width)
        Assert.Equal(formWithoutMenu.Size.Width, formWithMenu.Size.Width);
    }

    [StaFact]
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
        Assert.Equal(formWithoutMenu.Size.Height, formWithEmptyMenu.Size.Height);
    }

    [StaFact]
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
        Assert.True(form.Size.Height > initialHeight,
            "Form height should increase when menu with items is added");
        Assert.Equal(clientSize, form.ClientSize);
    }

    [StaFact]
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
        Assert.True(form.Size.Height < heightWithMenu,
            "Form height should decrease when menu is removed");
        Assert.Equal(clientSize, form.ClientSize);
    }

    [StaFact]
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
        Assert.Equal(childFormNoMenu.Size.Height, childForm.Size.Height);
    }

    [StaFact]
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
        Assert.Equal(mdiChild2.Size.Height, mdiChild1.Size.Height);
    }

    [StaFact]
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
        Assert.Equal(formWithOneMenuItem.Size.Height, formWithMultipleMenuItems.Size.Height);
    }

    [StaFact]
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
        Assert.Equal(formWithoutSubmenu.Size.Height, formWithSubmenu.Size.Height);
    }

    [StaFact]
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
        Assert.Equal(clientSize1, form.ClientSize);
        var height1 = form.Size.Height;

        form.ClientSize = clientSize2;
        Assert.Equal(clientSize2, form.ClientSize);
        var height2 = form.Size.Height;

        // The height difference should be proportional to client size difference
        var clientHeightDiff = clientSize2.Height - clientSize1.Height;
        var windowHeightDiff = height2 - height1;
        Assert.Equal(clientHeightDiff, windowHeightDiff);
    }

    [StaFact]
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
        Assert.True(sizeWithoutMenu.Height < sizeWithMenu.Height,
            "Form height should decrease immediately when menu is removed before handle creation");

        Assert.Equal(targetClientSize, form.ClientSize);

        // Width should remain the same
        Assert.Equal(sizeWithMenu.Width, sizeWithoutMenu.Width);
    }
}
