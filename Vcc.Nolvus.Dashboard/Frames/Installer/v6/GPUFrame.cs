using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Api.Installer.Services;
using Vcc.Nolvus.Api.Installer.Library;
using Vcc.Nolvus.Instance.Core;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Forms;
using Syncfusion.Windows.Forms.Tools;

namespace Vcc.Nolvus.Dashboard.Frames.Installer.v6
{
    public partial class GPUFrame : DashboardFrame
    {
        public GPUFrame()
        {
            InitializeComponent();
        }

        public GPUFrame(IDashboard Dashboard, FrameParameters Params)
            : base(Dashboard, Params)            
        {
            InitializeComponent();

            GPUsGrid.ThemeName = "Office2016Black";
            GPUsGrid.ShowRowHeader = true;
            GPUsGrid.Style.RowHeaderStyle.SelectionMarkerThickness = 100;
            GPUsGrid.Style.RowHeaderStyle.SelectionMarkerColor = Color.Orange;
            GPUsGrid.Style.RowHeaderStyle.SelectionBackColor = Color.FromArgb(68, 68, 68);

            GPUsGrid.Style.CurrentCellStyle.BackColor = Color.Orange;
            GPUsGrid.Style.CurrentCellStyle.TextColor = Color.White;
            GPUsGrid.Style.CurrentCellStyle.BorderColor = Color.Orange;

            GPUsGrid.AllowSorting = false;
        }
        

        protected override async Task OnLoadAsync()
        {
            INolvusVariantRequirementDTO VariantRequirement = Parameters["VariantRequirement"] as INolvusVariantRequirementDTO;
            INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

            LblVariant.Text = Instance.Performance.Variant;
            LblResolution.Text = Instance.GetSelectedResolution();
            LblSREX.Text = Instance.Performance.SREX == "TRUE" ? "Yes" : "No";

            var GPUs = await ApiManager.Service.Installer.GetGPUs();           

            GPUsGrid.DataSource = GPUs.Select(x => {

                return new GPUObject(){
                    VRAM = x.VRAM.ToString(),
                    GPU = string.Format("{0} {1}", x.Vendor, x.Name),
                    Supported = x.VRAM >= VariantRequirement.VRAM ? true : false,
                    Image = x.VRAM >= VariantRequirement.VRAM ? ServiceSingleton.Lib.ImageToByteArray(Properties.Resources.Check_01_24) : ServiceSingleton.Lib.ImageToByteArray(Properties.Resources.Cross_Close_24)
                };
                                
            }).ToList();

            ServiceSingleton.Dashboard.Info("Supported GPUS");
        }               
              
        private void BtnBack_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrameAsync<v6.PerformanceFrame>();
        }        

        private void GPUsGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            var GPU = e.DataRow.RowData as GPUObject;

            if (e.Column.MappingName == "Image")
            {
                if (GPU.Supported)
                {
                    e.Style.BackColor = Color.LimeGreen;
                }
                else
                {
                    e.Style.BackColor = Color.OrangeRed;
                }
                
            }
        }
    }
}
