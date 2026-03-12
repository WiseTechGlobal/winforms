#if NET
using System.Drawing;
using System.Windows.Forms;
#endif

namespace Demo
{
    internal sealed class OwnerDrawMenuItem : MenuItem
    {
        public OwnerDrawMenuItem() : base()
        {
            OwnerDraw = true;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            // Get the drawing area
            Rectangle bounds = e.Bounds;
            Graphics g = e.Graphics;
            
            // Determine colors based on item state
            Color backColor = e.BackColor;
            Color textColor = e.ForeColor;
            
            // Custom styling for selected/highlighted state
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                backColor = Color.LightBlue;
                textColor = Color.DarkBlue;
            }
            
            // Draw background
            using (Brush backBrush = new SolidBrush(backColor))
            {
                g.FillRectangle(backBrush, bounds);
            }
            
            // Draw an icon area (simple colored rectangle as demo)
            Rectangle iconRect = new Rectangle(bounds.X + 4, bounds.Y + 2, 16, 16);
            using (Brush iconBrush = new SolidBrush(Color.Green))
            {
                g.FillRectangle(iconBrush, iconRect);
            }
            
            // Draw a simple "check mark" in the icon area
            using (Pen checkPen = new Pen(Color.White, 2))
            {
                Point[] checkPoints = {
                    new (iconRect.X + 3, iconRect.Y + 8),
                    new (iconRect.X + 7, iconRect.Y + 12),
                    new (iconRect.X + 13, iconRect.Y + 4)
                };
                g.DrawLines(checkPen, checkPoints);
            }
            
            // Calculate text area (leaving space for icon and margins)
            Rectangle textRect = new Rectangle(
                bounds.X + 24, // Start after icon area (4 + 16 + 4 spacing)
                bounds.Y + 1,
                bounds.Width - 28, // Total margin: 4 (left) + 16 (icon) + 4 (spacing) + 4 (right) = 28
                bounds.Height - 2
            );
            
            // Draw the menu text
            using (Brush textBrush = new SolidBrush(textColor))
            {
                StringFormat format = new StringFormat()
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
                
                Font font = e.Font ?? SystemInformation.MenuFont;
                g.DrawString(Text, font, textBrush, textRect, format);
            }
            
            // Draw focus rectangle if the item has focus
            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
            {
                e.DrawFocusRectangle();
            }
            
            // Draw a subtle shadow effect at the bottom
            using (Pen shadowPen = new Pen(Color.FromArgb(50, 0, 0, 0)))
            {
                g.DrawLine(shadowPen, bounds.X, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
            }
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            Font font = SystemInformation.MenuFont;
            
            string text = Text ?? string.Empty;
            if (string.IsNullOrEmpty(text))
            {
                text = " ";
            }
            
            var stringSize = e.Graphics.MeasureString(text, font);
            e.ItemWidth = (int)Math.Ceiling(stringSize.Width) + 28;
            
            int minHeightForIcon = 20;
            int textHeight = (int)Math.Ceiling(stringSize.Height) + 4;
            
            e.ItemHeight = Math.Max(minHeightForIcon, textHeight);
        }
    }
}
