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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //this.components = new System.ComponentModel.Container();
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.ClientSize = new System.Drawing.Size(800, 450);
            //this.Text = "Form1";

            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Text = "WTG WinForms Demo";

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
        }

        #endregion
    }
}
