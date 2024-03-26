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
            button1.Size = new Size(120, 24);
            button1.Text = "Change Appearance";
            button1.Click += new System.EventHandler(Button1_Click);

            button2.Location = new Point(150, 16);
            button2.Size = new Size(120, 24);
            button2.Text = "Get Binding Manager";
            button2.Click += new System.EventHandler(Button2_Click);

            myDataGrid.Location = new Point(24, 50);
            myDataGrid.Size = new Size(600, 400);
            myDataGrid.CaptionText = "Microsoft DataGrid Control";
            myDataGrid.MouseUp += new MouseEventHandler(Grid_MouseUp);

            this.Controls.Add(button1);
            this.Controls.Add(button2);
            this.Controls.Add(myDataGrid);
        }

        #endregion
    }
}
