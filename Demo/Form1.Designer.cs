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
        private Button openMenuStackDemoButton;

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

            button1.Location = new Point(24, 60);
            button1.Size = new Size(200, 30);
            button1.Text = "Change Appearance";
            button1.Click += new System.EventHandler(Button1_Click);

            button2.Location = new Point(224, 60);
            button2.Size = new Size(200, 30);
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.openMenuStackDemoButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openMenuStackDemoButton
            // 
            this.openMenuStackDemoButton.Location = new System.Drawing.Point(650, 60);
            this.openMenuStackDemoButton.Name = "openMenuStackDemoButton";
            this.openMenuStackDemoButton.Size = new System.Drawing.Size(200, 30);
            this.openMenuStackDemoButton.TabIndex = 0;
            this.openMenuStackDemoButton.Text = "Open Menu Stack Demo";
            this.openMenuStackDemoButton.UseVisualStyleBackColor = true;
            this.openMenuStackDemoButton.Click += new System.EventHandler(this.OpenMenuStackDemoButton_Click);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.openMenuStackDemoButton);
            this.Text = "WTG WinForms Demo";
            this.ResumeLayout(false);
        }

        #endregion
    }
}
