using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp5
{
    public class GreenBorderComboBox : ComboBox
    {
        public GreenBorderComboBox()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.BackColor = Color.FromArgb(30, 30, 60);
            this.ForeColor = Color.White;
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.DropDownStyle = ComboBoxStyle.DropDownList; // Match your image’s dropdown style
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