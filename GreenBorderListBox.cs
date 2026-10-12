using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp5
{
    public class GreenBorderListBox : ListBox
    {
        public GreenBorderListBox()
        {
            this.BackColor = Color.FromArgb(30, 30, 60); // Dark background
            this.ForeColor = Color.White; // White text
            this.BorderStyle = BorderStyle.None; // Remove default border
            this.DrawMode = DrawMode.OwnerDrawFixed; // Custom drawing for items
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw green border
            using (Pen pen = new Pen(Color.LimeGreen, 2))
            {
                Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();
                e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, Brushes.White, e.Bounds);
                e.DrawFocusRectangle();
            }
        }
    }
}