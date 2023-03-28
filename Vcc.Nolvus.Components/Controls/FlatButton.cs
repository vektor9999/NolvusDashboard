using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Vcc.Nolvus.Components.Controls
{
    public partial class FlatButton : Button
    {
        public FlatButton()
        {
            BackColor = Color.FromArgb(54, 54, 54);
            ForeColor = Color.Orange;
            Font = new Font("Segoe UI Semibold", 9);
            CurrentBackColor = BackColor;
        }

        private Color CurrentBackColor;

        private Color onHoverBackColor = Color.FromArgb(83, 83, 83);
        public Color OnHoverBackColor
        {
            get { return onHoverBackColor; }
            set { onHoverBackColor = value; Invalidate(); }
        }

        private Color _BorderColor = Color.White;
        public Color BorderColor
        {
            get { return _BorderColor; }
            set { _BorderColor = value; Invalidate(); }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            CurrentBackColor = onHoverBackColor;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            CurrentBackColor = BackColor;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            CurrentBackColor = Color.FromArgb(120, 120, 120);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            CurrentBackColor = BackColor;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.FillRectangle(new SolidBrush(CurrentBackColor), 0, 0, Width, Height);

            Pen blackPen = new Pen(BorderColor, 1);

            // Create rectangle.
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            // Draw rectangle to screen.
            pevent.Graphics.DrawRectangle(blackPen, rect);

            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(pevent.Graphics, Text, Font, new Point(Width + 3, Height / 2), ForeColor, flags);
        }
    }
}