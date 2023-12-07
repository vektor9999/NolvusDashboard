using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Components.Controls
{
    public class ModsBox : ListBox
    {
        public double ScalingFactor { get; set; } = 1;
                
        public ModsBox()
        {
            this.SetStyle(
               ControlStyles.OptimizedDoubleBuffer |
               ControlStyles.ResizeRedraw |
               ControlStyles.UserPaint,
               true);

            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 35;
            BackColor = Color.FromArgb(54, 54, 54);
            SelectionMode = SelectionMode.None;                                                            
        }        

        private int GetGlobalProgress(int Value)
        {
            return ((Width - 100) / 100) * Value;            
        }

        protected override void OnPaint(PaintEventArgs e)
        {           
            Region iRegion = new Region(e.ClipRectangle);
            e.Graphics.FillRegion(new SolidBrush(this.BackColor), iRegion);
            if (this.Items.Count > 0)
            {
                for (int i = 0; i < this.Items.Count; ++i)
                {
                    System.Drawing.Rectangle irect = this.GetItemRectangle(i);
                    if (e.ClipRectangle.IntersectsWith(irect))
                    {
                        if ((this.SelectionMode == SelectionMode.One && this.SelectedIndex == i)
                        || (this.SelectionMode == SelectionMode.MultiSimple && this.SelectedIndices.Contains(i))
                        || (this.SelectionMode == SelectionMode.MultiExtended && this.SelectedIndices.Contains(i)))
                        {
                            OnDrawItem(new DrawItemEventArgs(e.Graphics, this.Font,
                                irect, i,
                                DrawItemState.Selected, this.ForeColor,
                                this.BackColor));
                        }
                        else
                        {
                            OnDrawItem(new DrawItemEventArgs(e.Graphics, this.Font,
                                irect, i,
                                DrawItemState.Default, this.ForeColor,
                                this.BackColor));
                        }
                        iRegion.Complement(irect);
                    }
                }
            }
            base.OnPaint(e);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {                                       
            FontFamily FamilyTitle = new FontFamily("Segoe UI Light");

            Font IFont = new Font(FamilyTitle, (float)9, FontStyle.Bold);
            Font InfoFont = new Font(FontFamily.GenericSansSerif, (float)7, FontStyle.Regular);
            Font StatusFont = e.Font;

            if (ScalingFactor > 1)
            {
                IFont = new Font(FamilyTitle, (float)(9 * ScalingFactor), FontStyle.Bold, GraphicsUnit.Pixel);
                InfoFont = new Font(FontFamily.GenericSansSerif, (float)(7 * ScalingFactor), FontStyle.Regular, GraphicsUnit.Pixel);
                StatusFont = new Font(e.Font.FontFamily, (float)(e.Font.Size * ScalingFactor), GraphicsUnit.Pixel);
            }           

            if (e.Index != -1 && !DesignMode)
            {
                var Top = 0;

                if (e.Index > 0)
                {
                    Top = (ItemHeight * e.Index);
                }
                
                var Progress = Items[e.Index] as ModProgress;                

                if (!Progress.HasError)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Orange)), 105, 5 + Top, GetGlobalProgress(Progress.GlobalDone), 30);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Red)), 105, 5 + Top, GetGlobalProgress(100), 30);
                }                

                e.Graphics.DrawRectangle(Pens.Orange, 3, 5 + Top, Progress.PercentDone, 1);
                e.Graphics.DrawString(Progress.Name, IFont, Brushes.White, 105, 3 + Top);

                if (Progress.Image != null) e.Graphics.DrawImage(Progress.Image, 3, 5 + Top);

                if ( Progress.Mbs != 0)
                {
                    e.Graphics.DrawString(string.Format("{0}MB/s", Progress.Mbs.ToString("0.0")), InfoFont, Brushes.White, 105, 10 + Top, new StringFormat(StringFormatFlags.DirectionRightToLeft));
                }                

                if (!Progress.HasError)
                {
                    e.Graphics.DrawString(string.Format("{0}%", Progress.PercentDone), InfoFont, Brushes.White, 105, 22 + Top, new StringFormat(StringFormatFlags.DirectionRightToLeft));
                }                

                if (!Progress.HasError)
                {
                    e.Graphics.DrawString(Progress.Action, InfoFont, Brushes.White, 3, 21 + Top);
                }
                else
                {
                    e.Graphics.DrawString("Error", InfoFont, Brushes.Red, 3, 21 + Top);
                }               

                Brush Brush = Brushes.Orange;

                if (Progress.HasError) Brush = Brushes.Red;
                
                e.Graphics.DrawString(Progress.Status, StatusFont, Brush, 105, 20 + Top);                                
            }       
        }
    }
}
