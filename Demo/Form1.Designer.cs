using System.Drawing;
using System.Windows.Forms;

namespace Demo
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeStatusBar()
        {
            StatusBar mainStatusBar = new StatusBar();

            mainStatusBar.Dock = DockStyle.Bottom;
            mainStatusBar.Height = 70;

            StatusBarPanel statusPanel = new StatusBarPanel();
            StatusBarPanel datetimePanel = new StatusBarPanel();

            // Set first panel properties and add to StatusBar  
            statusPanel.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            statusPanel.Text = "Status Bar Example";
            statusPanel.AutoSize = StatusBarPanelAutoSize.Spring;
            mainStatusBar.Panels.Add(statusPanel);

            // Set second panel properties and add to StatusBar  
            datetimePanel.BorderStyle = StatusBarPanelBorderStyle.Raised;

            datetimePanel.Text = System.DateTime.Today.ToLongDateString();
            datetimePanel.AutoSize = StatusBarPanelAutoSize.Contents;
            mainStatusBar.Panels.Add(datetimePanel);

            mainStatusBar.ShowPanels = true;

            Controls.Add(mainStatusBar);
        }

        private void InitializeDataGrid()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.myDataGrid = new DataGrid();

            button1.Location = new Point(24, 16);
            button1.Size = new Size(380, 54);
            button1.Text = "Change Appearance";
            button1.Click += new System.EventHandler(Button1_Click);

            button2.Location = new Point(400, 16);
            button2.Size = new Size(420, 54);
            button2.Text = "Get Binding Manager";
            button2.Click += new System.EventHandler(Button2_Click);

            myDataGrid.Location = new Point(24, 100);
            myDataGrid.Size = new Size(600, 400);
            myDataGrid.CaptionText = "Microsoft DataGrid Control";
            myDataGrid.MouseUp += new MouseEventHandler(Grid_MouseUp);

            this.Controls.Add(button1);
            this.Controls.Add(button2);
            this.Controls.Add(myDataGrid);
        }

        private void InitializeMenu()
        {
            // Create the main menu
            mainMenu = new MainMenu();

            // Create menu items
            fileMenuItem = new MenuItem("File");
            newMenuItem = new MenuItem("New");
            openMenuItem = new MenuItem("Open");
            saveMenuItem = new MenuItem("Save");
            exitMenuItem = new MenuItem("Exit");
            viewMenuItem = new MenuItem("View");
            toolboxMenuItem = new MenuItem("Toolbox");
            terminalMenuItem = new MenuItem("Terminal");
            outputMenuItem = new MenuItem("Output");

            newProjectItem = new MenuItem("Project...");
            newRepositoryItem = new MenuItem("Repository...");
            newFileItem = new MenuItem("File...");

            newMenuItem.MenuItems.Add(newProjectItem);
            newMenuItem.MenuItems.Add(newRepositoryItem);
            newMenuItem.MenuItems.Add(newFileItem);

            openMenuItem.Shortcut = Shortcut.AltF12;
            openMenuItem.ShowShortcut = true;

            saveMenuItem.Checked = true;
            saveMenuItem.RadioCheck = true;

            // Add sub-menu items to the "File" menu item
            fileMenuItem.MenuItems.Add(newMenuItem);
            fileMenuItem.MenuItems.Add(openMenuItem);
            fileMenuItem.MenuItems.Add(saveMenuItem);
            fileMenuItem.MenuItems.Add(exitMenuItem);

            viewMenuItem.MenuItems.Add(toolboxMenuItem);
            viewMenuItem.MenuItems.Add(terminalMenuItem);
            viewMenuItem.MenuItems.Add(outputMenuItem);

            // Add "File" and "View" menu item to the main menu
            mainMenu.MenuItems.Add(fileMenuItem);
            mainMenu.MenuItems.Add(viewMenuItem);

            // Set the form's main menu
            this.Menu = mainMenu;

            newProjectItem.Click += NewProjectItem_Click;
            newRepositoryItem.Click += NewRepositoryItem_Click;
            newFileItem.Click += NewFileItem_Click;

            // Add event handlers for menu items
            //newMenuItem.Click += NewMenuItem_Click;
            openMenuItem.Click += OpenMenuItem_Click;
            saveMenuItem.Click += SaveMenuItem_Click;
            exitMenuItem.Click += ExitMenuItem_Click;

            toolboxMenuItem.Click += ToolboxMenuItem_Click;
            terminalMenuItem.Click += TerminalMenuItem_Click;
            outputMenuItem.Click += OutputMenuItem_Click;

            this.Controls.Add(mainMenu);
        }

        private void InitializeMenuStrip()
        {
            // Create a MenuStrip
            MenuStrip menuStrip = new MenuStrip();

            // Create "File" menu item
            ToolStripMenuItem fileMenuItem = new ToolStripMenuItem("File");
            fileMenuItem.DropDownItems.Add("New");
            fileMenuItem.DropDownItems.Add("Open");
            fileMenuItem.DropDownItems.Add("Save");
            fileMenuItem.DropDownItems.Add("Exit");
            menuStrip.Items.Add(fileMenuItem);

            // Create "Edit" menu item
            ToolStripMenuItem editMenuItem = new ToolStripMenuItem("Edit");
            editMenuItem.DropDownItems.Add("Cut");
            editMenuItem.DropDownItems.Add("Copy");
            editMenuItem.DropDownItems.Add("Paste");
            menuStrip.Items.Add(editMenuItem);

            // Attach the MenuStrip to the form
            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Text = "WTG WinForms Demo";
        }

        #endregion
    }
}
