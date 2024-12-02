using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Api.Installer.Library;

namespace Vcc.Nolvus.Components.Controls
{
    public class NolvusListBox : ListBox
    {
        public double ScalingFactor { get; set; } = 1;
                
        public NolvusListBox()
        {
            this.SetStyle(
               ControlStyles.OptimizedDoubleBuffer |
               ControlStyles.ResizeRedraw |
               ControlStyles.UserPaint,
               true);

            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 40;
            BackColor = Color.FromArgb(54, 54, 54);
            SelectionMode = SelectionMode.One;                                                            
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
                        if ((this.SelectionMode == SelectionMode.One && this.SelectedIndex == i))
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

        private Image GetImageFromUrl(string Url)
        {            
            return new Bitmap(new WebClient().OpenRead(Url));
        }        

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            var NolvusList = Items[e.Index] as INolvusVersionDTO;

            FontFamily FamilyTitle = new FontFamily("Segoe UI Light");
            Font IFont = new Font(FamilyTitle, (float)12, FontStyle.Bold);
            Font InfoFont = new Font(new FontFamily("Microsoft Sans Serif") , (float)8.25, FontStyle.Regular);
            Font VersionFont = new Font(new FontFamily("Microsoft Sans Serif"), (float)7, FontStyle.Regular);
            Font BetaFont = new Font(new FontFamily("Microsoft Sans Serif"), (float)8.25, FontStyle.Bold);
            Font DescriptionFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Font StatusFont = e.Font;

            if (e.Index < 0) return;

            var Top = 0;

            if (e.Index > 0)
            {
                Top = (ItemHeight * e.Index);
            }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e = new DrawItemEventArgs(e.Graphics,
                                               e.Font,
                                               e.Bounds,
                                               e.Index,
                                               e.State ^ DrawItemState.Selected,
                                               Color.FromArgb(54, 54, 54),
                                               Color.FromArgb(204, 122, 0));
                                                
            }            

            e.DrawBackground();            

            e.Graphics.DrawRectangle(Pens.Silver, 3, 5 + Top, 150, 95);
            e.Graphics.DrawImage(NolvusList.ImageObject, 3, 5 + Top, 150, 95);
            e.Graphics.DrawString(NolvusList.Name, IFont, Brushes.White, 155, 3 + Top);
      

            Rectangle DescriptionRectangle = new Rectangle(156, 30 + Top, e.Bounds.Width - 155, 50);
            e.Graphics.DrawRectangle(Pens.Transparent, DescriptionRectangle);
            e.Graphics.DrawString(NolvusList.Description, InfoFont, Brushes.White, DescriptionRectangle, StringFormat.GenericDefault);

            Rectangle VersionRect = new Rectangle(new Point(4, 6 + Top), new Size(40, 15));

            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(54, 54, 54)), VersionRect);
            e.Graphics.DrawString(string.Format("v {0}", NolvusList.Version), VersionFont, Brushes.White, 6, 7 + Top);

            if (NolvusList.IsBeta)
            {
                Rectangle StatusRect = new Rectangle(new Point(110, 93 + Top), new Size(40, 15));

                e.Graphics.FillRectangle(new SolidBrush(Color.Orange), StatusRect);
                e.Graphics.DrawString("Beta", BetaFont, Brushes.White, 115, 93 + Top);
            }        
            
            if (NolvusList.Maintenance)
            {
                Rectangle MaintenanceRect = new Rectangle(new Point(6, 93 + Top), new Size(80, 15));

                e.Graphics.FillRectangle(new SolidBrush(Color.OrangeRed), MaintenanceRect);
                e.Graphics.DrawString("Maintenance", BetaFont, Brushes.White, 8, 93 + Top);
            }    
        }
    }
}
