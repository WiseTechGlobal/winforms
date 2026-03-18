using System.Drawing;
using System.Windows.Forms;

namespace Demo
{
    partial class DataGridForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private GroupBox classicFeaturesGroupBox;
        private Label classicFeaturesLabel;
        private GroupBox classicOptionsGroupBox;

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

        private void InitializeDataGrid()
        {
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            button4 = new System.Windows.Forms.Button();
            button5 = new System.Windows.Forms.Button();
            button6 = new System.Windows.Forms.Button();
            myDataGrid = new DataGrid();
            featureListBox = new System.Windows.Forms.ListBox();
            selectionSummaryLabel = new System.Windows.Forms.Label();
            captionVisibleCheckBox = new System.Windows.Forms.CheckBox();
            parentRowsVisibleCheckBox = new System.Windows.Forms.CheckBox();
            rowHeadersVisibleCheckBox = new System.Windows.Forms.CheckBox();
            readOnlyCheckBox = new System.Windows.Forms.CheckBox();
            allowNavigationCheckBox = new System.Windows.Forms.CheckBox();
            classicOptionsHintLabel = new System.Windows.Forms.Label();

            button1.Location = new Point(24, 60);
            button1.Size = new Size(160, 30);
            button1.Text = "Change Appearance";
            button1.Click += new System.EventHandler(Button1_Click);

            button2.Location = new Point(192, 60);
            button2.Size = new Size(160, 30);
            button2.Text = "Get Binding Manager";
            button2.Click += new System.EventHandler(Button2_Click);

            button3.Location = new Point(360, 60);
            button3.Size = new Size(140, 30);
            button3.Text = "Use Flat Mode";
            button3.Click += new System.EventHandler(Button3_Click);

            button4.Location = new Point(508, 60);
            button4.Size = new Size(116, 30);
            button4.Text = "Next Customer";
            button4.Click += new System.EventHandler(Button4_Click);

            button5.Location = new Point(632, 60);
            button5.Size = new Size(116, 30);
            button5.Text = "First Customer";
            button5.Click += new System.EventHandler(Button5_Click);

            button6.Location = new Point(756, 60);
            button6.Size = new Size(140, 30);
            button6.Text = "Use Wide Columns";
            button6.Click += new System.EventHandler(Button6_Click);

            myDataGrid.Location = new Point(24, 100);
            myDataGrid.Size = new Size(620, 440);
            myDataGrid.CaptionText = "Microsoft DataGrid Control";
            myDataGrid.PreferredColumnWidth = 75;
            myDataGrid.MouseUp += new MouseEventHandler(Grid_MouseUp);

            selectionSummaryLabel.BorderStyle = BorderStyle.FixedSingle;
            selectionSummaryLabel.Location = new Point(24, 548);
            selectionSummaryLabel.Size = new Size(620, 70);
            selectionSummaryLabel.Text = "Classic demo summary will appear after the grid is initialized.";
            selectionSummaryLabel.TextAlign = ContentAlignment.MiddleLeft;

            featureListBox.BorderStyle = BorderStyle.None;
            featureListBox.Dock = DockStyle.Fill;
            featureListBox.IntegralHeight = false;

            classicFeaturesGroupBox = new GroupBox();
            classicFeaturesLabel = new Label();
            classicOptionsGroupBox = new GroupBox();
            classicFeaturesGroupBox.Location = new Point(668, 100);
            classicFeaturesGroupBox.Size = new Size(320, 268);
            classicFeaturesGroupBox.Text = "Basic DataGrid Features";

            classicFeaturesLabel.Dock = DockStyle.Top;
            classicFeaturesLabel.Height = 70;
            classicFeaturesLabel.Text = "This panel lists the DataGrid-specific behaviors exercised by this demo without relying on other legacy control families.";

            classicFeaturesGroupBox.Controls.Add(featureListBox);
            classicFeaturesGroupBox.Controls.Add(classicFeaturesLabel);

            classicOptionsGroupBox.Location = new Point(668, 384);
            classicOptionsGroupBox.Size = new Size(320, 234);
            classicOptionsGroupBox.Text = "Try Classic Options";

            captionVisibleCheckBox.Location = new Point(16, 28);
            captionVisibleCheckBox.Size = new Size(160, 24);
            captionVisibleCheckBox.Text = "Caption Visible";
            captionVisibleCheckBox.CheckedChanged += new System.EventHandler(CaptionVisibleCheckBox_CheckedChanged);

            parentRowsVisibleCheckBox.Location = new Point(16, 58);
            parentRowsVisibleCheckBox.Size = new Size(160, 24);
            parentRowsVisibleCheckBox.Text = "Parent Rows Visible";
            parentRowsVisibleCheckBox.CheckedChanged += new System.EventHandler(ParentRowsVisibleCheckBox_CheckedChanged);

            rowHeadersVisibleCheckBox.Location = new Point(16, 88);
            rowHeadersVisibleCheckBox.Size = new Size(160, 24);
            rowHeadersVisibleCheckBox.Text = "Row Headers Visible";
            rowHeadersVisibleCheckBox.CheckedChanged += new System.EventHandler(RowHeadersVisibleCheckBox_CheckedChanged);

            readOnlyCheckBox.Location = new Point(16, 118);
            readOnlyCheckBox.Size = new Size(160, 24);
            readOnlyCheckBox.Text = "Read Only";
            readOnlyCheckBox.CheckedChanged += new System.EventHandler(ReadOnlyCheckBox_CheckedChanged);

            allowNavigationCheckBox.Location = new Point(16, 148);
            allowNavigationCheckBox.Size = new Size(160, 24);
            allowNavigationCheckBox.Text = "Allow Navigation";
            allowNavigationCheckBox.CheckedChanged += new System.EventHandler(AllowNavigationCheckBox_CheckedChanged);

            classicOptionsHintLabel.Location = new Point(16, 180);
            classicOptionsHintLabel.Size = new Size(288, 42);
            classicOptionsHintLabel.Text = "Toggle caption, parent rows, row headers, read-only mode, and relation navigation while switching rows and column widths.";

            classicOptionsGroupBox.Controls.Add(captionVisibleCheckBox);
            classicOptionsGroupBox.Controls.Add(parentRowsVisibleCheckBox);
            classicOptionsGroupBox.Controls.Add(rowHeadersVisibleCheckBox);
            classicOptionsGroupBox.Controls.Add(readOnlyCheckBox);
            classicOptionsGroupBox.Controls.Add(allowNavigationCheckBox);
            classicOptionsGroupBox.Controls.Add(classicOptionsHintLabel);

            Controls.Add(button1);
            Controls.Add(button2);
            Controls.Add(button3);
            Controls.Add(button4);
            Controls.Add(button5);
            Controls.Add(button6);
            Controls.Add(myDataGrid);
            Controls.Add(selectionSummaryLabel);
            Controls.Add(classicFeaturesGroupBox);
            Controls.Add(classicOptionsGroupBox);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            SuspendLayout();
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1024, 680);
            Text = "DataGrid Demo";
            ResumeLayout(false);
        }

        #endregion
    }
}