using System.Drawing;
using System.Windows.Forms;

namespace Demo;

public partial class MenuStackForm : Form
{
    private readonly ContextMenu _surfaceContextMenu;
    private readonly ContextMenu _treeViewContextMenu;
    private int _dynamicMenuGeneration;

    public MenuStackForm()
    {
        InitializeComponent();

        Menu = CreateMainMenu();

        _surfaceContextMenu = CreateSurfaceContextMenu();
        _demoSurface.ContextMenu = _surfaceContextMenu;

        _treeViewContextMenu = CreateTreeViewContextMenu();
        _menuTreeView.ContextMenu = _treeViewContextMenu;

        InitializeTreeView();
        AppendLog("Menu stack demo ready.");
    }

    private MainMenu CreateMainMenu()
    {
        MenuItem fileMenuItem = new("File");
        MenuItem newMenuItem = new("New");
        MenuItem newProjectItem = new("Project...", OnMenuActionClicked);
        MenuItem newRepositoryItem = new("Repository...", OnMenuActionClicked);
        MenuItem newFileItem = new("File...", OnMenuActionClicked);
        MenuItem openMenuItem = new("Open", OnMenuActionClicked)
        {
            Shortcut = Shortcut.AltF12,
            ShowShortcut = true
        };
        MenuItem saveMenuItem = new("Save", OnMenuActionClicked, Shortcut.CtrlS)
        {
            Checked = true,
            RadioCheck = true
        };
        MenuItem exitMenuItem = new("Close Demo", (_, _) => Close());

        newMenuItem.MenuItems.Add(newProjectItem);
        newMenuItem.MenuItems.Add(newRepositoryItem);
        newMenuItem.MenuItems.Add(newFileItem);

        fileMenuItem.MenuItems.Add(newMenuItem);
        fileMenuItem.MenuItems.Add(openMenuItem);
        fileMenuItem.MenuItems.Add(saveMenuItem);
        fileMenuItem.MenuItems.Add(new MenuItem("-"));
        fileMenuItem.MenuItems.Add(exitMenuItem);

        MenuItem viewMenuItem = new("View");
        viewMenuItem.MenuItems.Add(new MenuItem("Toolbox", OnMenuActionClicked));
        viewMenuItem.MenuItems.Add(new MenuItem("Terminal", OnMenuActionClicked));
        viewMenuItem.MenuItems.Add(new MenuItem("Output", OnMenuActionClicked));

        MenuItem dynamicMenuItem = new("Dynamic");
        dynamicMenuItem.Popup += DynamicMenuItem_Popup;
        dynamicMenuItem.MenuItems.Add(new MenuItem("Dynamic code has not run yet"));

        MenuItem ownerDrawMenuItem = new("Owner Draw Demo");
        ownerDrawMenuItem.MenuItems.Add(new MenuItem("Standard Item", OnMenuActionClicked));
        ownerDrawMenuItem.MenuItems.Add(new MenuItem("-"));
        ownerDrawMenuItem.MenuItems.Add(CreateOwnerDrawMenuItem("Custom Draw Item 1"));
        ownerDrawMenuItem.MenuItems.Add(CreateOwnerDrawMenuItem("Custom Draw Item 2", createNestedItems: true));

        MenuItem contextActionsItem = new("Context Actions");
        contextActionsItem.MenuItems.Add(new MenuItem("Show Surface Menu", ShowSurfaceMenuMenuItem_Click));
        contextActionsItem.MenuItems.Add(new MenuItem("Clear Log", (_, _) => _eventLog.Items.Clear()));

        AttachMenuTracing(fileMenuItem);
        AttachMenuTracing(viewMenuItem);
        AttachMenuTracing(dynamicMenuItem);
        AttachMenuTracing(ownerDrawMenuItem);
        AttachMenuTracing(contextActionsItem);

        MainMenu mainMenu = new();
        mainMenu.MenuItems.Add(fileMenuItem);
        mainMenu.MenuItems.Add(viewMenuItem);
        mainMenu.MenuItems.Add(dynamicMenuItem);
        mainMenu.MenuItems.Add(ownerDrawMenuItem);
        mainMenu.MenuItems.Add(contextActionsItem);

        return mainMenu;
    }

    private ContextMenu CreateSurfaceContextMenu()
    {
        ContextMenu contextMenu = new();

        MenuItem inspectItem = new("Inspect Surface", OnContextMenuActionClicked);
        MenuItem toggleHighlightItem = new("Toggle Highlight", ToggleSurfaceHighlightMenuItem_Click);
        MenuItem timestampItem = new("Insert Timestamp", InsertTimestampMenuItem_Click);

        contextMenu.MenuItems.Add(inspectItem);
        contextMenu.MenuItems.Add(toggleHighlightItem);
        contextMenu.MenuItems.Add(new MenuItem("-"));
        contextMenu.MenuItems.Add(timestampItem);

        contextMenu.Popup += (_, _) => AppendLog("Surface context menu opened.");

        return contextMenu;
    }

    private ContextMenu CreateTreeViewContextMenu()
    {
        ContextMenu contextMenu = new();
        MenuItem inspectTreeItem = new("Inspect TreeView", (_, _) => AppendLog("Inspect TreeView context action executed."));
        MenuItem expandAllItem = new("Expand All Nodes", (_, _) =>
        {
            _menuTreeView.ExpandAll();
            AppendLog("TreeView context action: expand all nodes.");
        });
        MenuItem collapseAllItem = new("Collapse All Nodes", (_, _) =>
        {
            _menuTreeView.CollapseAll();
            AppendLog("TreeView context action: collapse all nodes.");
        });

        contextMenu.MenuItems.Add(inspectTreeItem);
        contextMenu.MenuItems.Add(new MenuItem("-"));
        contextMenu.MenuItems.Add(expandAllItem);
        contextMenu.MenuItems.Add(collapseAllItem);
        contextMenu.Popup += (_, _) => AppendLog("TreeView context menu opened.");

        return contextMenu;
    }

    private OwnerDrawMenuItem CreateOwnerDrawMenuItem(string text, bool createNestedItems = false)
    {
        OwnerDrawMenuItem item = new()
        {
            Text = text
        };

        item.Click += OnOwnerDrawItemClicked;

        if (!createNestedItems)
        {
            OwnerDrawMenuItem subItem1 = new()
            {
                Text = "Submenu Item 1-1"
            };
            OwnerDrawMenuItem subItem2 = new()
            {
                Text = "Submenu Item 1-2"
            };

            subItem1.Click += OnOwnerDrawItemClicked;
            subItem2.Click += OnOwnerDrawItemClicked;

            item.MenuItems.Add(subItem1);
            item.MenuItems.Add(subItem2);

            return item;
        }

        OwnerDrawMenuItem nestedItemA = new()
        {
            Text = "Nested Custom Item A"
        };
        OwnerDrawMenuItem nestedItemB = new()
        {
            Text = "Nested Custom Item B"
        };
        MenuItem deepSubmenu = new("Deep Submenu");
        OwnerDrawMenuItem deepItem1 = new()
        {
            Text = "Deep Custom Item 1"
        };
        OwnerDrawMenuItem deepItem2 = new()
        {
            Text = "Deep Custom Item 2"
        };

        nestedItemA.Click += OnOwnerDrawItemClicked;
        nestedItemB.Click += OnOwnerDrawItemClicked;
        deepItem1.Click += OnOwnerDrawItemClicked;
        deepItem2.Click += OnOwnerDrawItemClicked;

        deepSubmenu.MenuItems.Add(deepItem1);
        deepSubmenu.MenuItems.Add(deepItem2);

        item.MenuItems.Add(nestedItemA);
        item.MenuItems.Add(nestedItemB);
        item.MenuItems.Add(new MenuItem("-"));
        item.MenuItems.Add(deepSubmenu);

        return item;
    }

    private void AttachMenuTracing(MenuItem rootItem)
    {
        rootItem.Select += (_, _) => AppendLog($"Selected menu item: {rootItem.Text}");

        if (rootItem.MenuItems.Count > 0)
        {
            rootItem.Popup += (_, _) => AppendLog($"Popup menu opened: {rootItem.Text}");
        }

        foreach (MenuItem childItem in rootItem.MenuItems)
        {
            AttachMenuTracing(childItem);
        }
    }

    private void InitializeTreeView()
    {
        TreeNode rootNode = new("Menu Demo Root")
        {
            Nodes =
            {
                new TreeNode("Context Menu Node"),
                new TreeNode("Owner Draw Node")
                {
                    Nodes =
                    {
                        new TreeNode("Nested Node A"),
                        new TreeNode("Nested Node B")
                    }
                },
                new TreeNode("Shortcut Node")
            }
        };

        _menuTreeView.Nodes.Clear();
        _menuTreeView.Nodes.Add(rootNode);
        rootNode.Expand();

        AddContextMenusToNodes(_menuTreeView.Nodes);
    }

    private void AddContextMenusToNodes(TreeNodeCollection nodes)
    {
        foreach (TreeNode node in nodes)
        {
            ContextMenu nodeContextMenu = new();
            MenuItem inspectNodeItem = new($"Inspect {node.Text}", (_, _) =>
            {
                _surfaceMessageLabel.Text = "Inspecting node: " + node.Text;
                AppendLog($"TreeNode.ContextMenu action: inspect {node.Text}");
            });
            MenuItem toggleCheckedItem = new($"Toggle check state for {node.Text}", (_, _) =>
            {
                node.Checked = !node.Checked;
                AppendLog($"TreeNode.ContextMenu action: {(node.Checked ? "checked" : "unchecked")} {node.Text}");
            });
            MenuItem runNodeItem = new($"Run action for {node.Text}", (_, _) => MessageBox.Show(this, $"Action executed for {node.Text}", Text));

            nodeContextMenu.MenuItems.Add(inspectNodeItem);
            nodeContextMenu.MenuItems.Add(toggleCheckedItem);
            nodeContextMenu.MenuItems.Add(runNodeItem);
            nodeContextMenu.Popup += (_, _) => AppendLog($"TreeNode.ContextMenu opened for {node.Text}");
            node.ContextMenu = nodeContextMenu;

            if (node.Nodes.Count > 0)
            {
                AddContextMenusToNodes(node.Nodes);
            }
        }
    }

    private void DynamicMenuItem_Popup(object? sender, EventArgs e)
    {
        if (sender is not MenuItem dynamicMenuItem)
        {
            return;
        }

        _dynamicMenuGeneration++;
        dynamicMenuItem.MenuItems.Clear();

        for (int i = 1; i <= 5; i++)
        {
            MenuItem item = new($"Dynamic Item {_dynamicMenuGeneration}.{i}");
            item.Click += OnMenuActionClicked;
            dynamicMenuItem.MenuItems.Add(item);
        }

        AppendLog($"Dynamic menu rebuilt #{_dynamicMenuGeneration}.");
    }

    private void OnMenuActionClicked(object? sender, EventArgs e)
    {
        if (sender is not MenuItem menuItem)
        {
            return;
        }

        menuItem.Checked = !menuItem.Checked && menuItem.Text.EndsWith("...", StringComparison.Ordinal);
        AppendLog($"Menu click: {menuItem.Text}");
        MessageBox.Show(this, $"{menuItem.Text} clicked.", Text);
    }

    private void OnOwnerDrawItemClicked(object? sender, EventArgs e)
    {
        if (sender is not MenuItem menuItem)
        {
            return;
        }

        AppendLog($"Owner-draw click: {menuItem.Text}");
        MessageBox.Show(this, $"Owner-draw item clicked: {menuItem.Text}", Text);
    }

    private void OnContextMenuActionClicked(object? sender, EventArgs e)
    {
        if (sender is not MenuItem menuItem)
        {
            return;
        }

        AppendLog($"Context action: {menuItem.Text}");
        MessageBox.Show(this, $"{menuItem.Text} executed.", Text);
    }

    private void ToggleSurfaceHighlightMenuItem_Click(object? sender, EventArgs e)
    {
        bool useHighlight = _demoSurface.BackColor != Color.LightGoldenrodYellow;
        _demoSurface.BackColor = useHighlight ? Color.LightGoldenrodYellow : Color.WhiteSmoke;

        AppendLog(useHighlight ? "Surface highlight enabled." : "Surface highlight cleared.");
    }

    private void InsertTimestampMenuItem_Click(object? sender, EventArgs e)
    {
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        _surfaceMessageLabel.Text = "Last context action at " + timestamp;
        AppendLog("Timestamp inserted: " + timestamp);
    }

    private void ShowSurfaceMenuMenuItem_Click(object? sender, EventArgs e)
    {
        ShowSurfaceContextMenu();
    }

    private void ShowSurfaceContextMenuButton_Click(object? sender, EventArgs e)
    {
        ShowSurfaceContextMenu();
    }

    private void ClearLogButton_Click(object? sender, EventArgs e)
    {
        _eventLog.Items.Clear();
        AppendLog("Log cleared.");
    }

    private void MenuTreeView_NodeMouseClick(object? sender, TreeNodeMouseClickEventArgs e)
    {
        if (e.Node is null)
        {
            return;
        }

        _menuTreeView.SelectedNode = e.Node;
        AppendLog($"Tree node clicked: {e.Node.Text}");
    }

    private void ShowSurfaceContextMenu()
    {
        Point screenPoint = _demoSurface.PointToClient(MousePosition);
        Point menuPoint = screenPoint.X >= 0 && screenPoint.Y >= 0
            ? screenPoint
            : new Point(_demoSurface.Width / 2, _demoSurface.Height / 2);

        _surfaceContextMenu.Show(_demoSurface, menuPoint);
    }

    private void AppendLog(string message)
    {
        _eventLog.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {message}");

        if (_eventLog.Items.Count > 200)
        {
            _eventLog.Items.RemoveAt(_eventLog.Items.Count - 1);
        }
    }
}
