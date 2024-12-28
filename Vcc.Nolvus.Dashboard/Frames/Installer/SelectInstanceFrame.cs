using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using Vcc.Nolvus.Api.Installer.Services;
using Vcc.Nolvus.Api.Installer.Library;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Instance.Core;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Dashboard.Forms;
using Vcc.Nolvus.Dashboard.Frames.Instance;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Dashboard.Frames.Installer
{
    public partial class SelectInstanceFrame : DashboardFrame
    {        
        public SelectInstanceFrame()
        {
            InitializeComponent();
        }

        public SelectInstanceFrame(IDashboard Dashboard, FrameParameters Params)
            :base(Dashboard, Params)
        {
            InitializeComponent();
        }

        private int InstanceIndex(IEnumerable<INolvusVersionDTO> Versions)
        {           
            INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

            if (Instance != null)
            {
                var Index = Versions.ToList().FindIndex(x => x.Name == Instance.Name);

                return Index == -1 ? Versions.ToList().Count - 1 : Index;
            }

            return 0;
        }

        private int LgIndex(List<LgCode> Lgs)
        {
            INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

            if (Instance != null)
            {
                var Index = Lgs.FindIndex(x => x.Code == Instance.Settings.LgCode);

                return Index == -1 ? 0 : Index;
            }

            return 0;            
        }

        private void SetDataSource(IEnumerable<INolvusVersionDTO> Source)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<IEnumerable<INolvusVersionDTO>>)SetDataSource, Source);
                return;
            }
            
            NolvusListBox.DataSource = Source;            
            NolvusListBox.SelectedIndex = InstanceIndex(Source);
            PicLoading.Hide();
            NolvusListBox.Show();
        }

        private async Task LoadAvailableLists(IEnumerable<INolvusVersionDTO> Lists)
        {
            await Task.Run(() =>
            {
                SetDataSource(Lists.Select(x => {
                    x.ImageObject = ServiceSingleton.Lib.SetImageOpacity(ServiceSingleton.Lib.GetImageFromUrl(x.Image), 0.50F);
                    return x;
                }).ToList());
            });
        }

        private void SwitchInstance(INolvusVersionDTO NolvusInstance)
        {
            if (ServiceSingleton.Instances.WorkingInstance == null || ServiceSingleton.Instances.WorkingInstance.Name != NolvusInstance.Name)
            {
                ServiceSingleton.Instances.WorkingInstance = new NolvusInstance(NolvusInstance);                
            }
        }

        private void LoadLanguages()
        {           
            List<LgCode> LgList = new List<LgCode>();

            LgCode Lg1 = new LgCode { Code = "EN", Name = "English" };
            LgCode Lg2 = new LgCode { Code = "FR", Name = "French" };
            LgCode Lg3 = new LgCode { Code = "IT", Name = "Italian" };
            LgCode Lg4 = new LgCode { Code = "DE", Name = "German" };
            LgCode Lg5 = new LgCode { Code = "ES", Name = "Spanish" };
            LgCode Lg6 = new LgCode { Code = "RU", Name = "Russian" };
            LgCode Lg7 = new LgCode { Code = "PL", Name = "Polish" };

            LgList.Add(Lg1);
            LgList.Add(Lg2);
            LgList.Add(Lg3);
            LgList.Add(Lg4);
            LgList.Add(Lg5);
            LgList.Add(Lg6);
            LgList.Add(Lg7);

            DrpDwnLg.DataSource = LgList;
            DrpDwnLg.DisplayMember = "Name";
            DrpDwnLg.ValueMember = "Code";

            DrpDwnLg.SelectedIndex = LgIndex(LgList);            
        }

        protected override async Task OnLoadedAsync()
        {
            try
            {
                ServiceSingleton.Dashboard.Title("Nolvus Dashboard - [Instance Auto Installer]");
                ServiceSingleton.Dashboard.Info("Instance Prerequisites");

                BtnCancel.Visible = !Parameters.IsEmpty && Parameters["Cancel"] != null;

                LoadLanguages();

                await LoadAvailableLists(await ApiManager.Service.Installer.GetNolvusVersions());

                SwitchInstance(NolvusListBox.SelectedItem as INolvusVersionDTO);

                NolvusListBox.SelectedIndexChanged += NolvusListBox_SelectedIndexChanged;                
            }
            catch (Exception ex)
            {
                await ServiceSingleton.Dashboard.Error("Error during instance selection loading", ex.Message, ex.StackTrace);
            }
        }        

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            INolvusVersionDTO InstanceToInstall = NolvusListBox.SelectedItem as INolvusVersionDTO;

            if (InstanceToInstall.Maintenance)
            {
                NolvusMessageBox.ShowMessage("Maintenance", "The nolvus instance " + InstanceToInstall.Name + " is under maintenance. Unable to install.", MessageBoxType.Error);
            }
            else
            {
                if (ServiceSingleton.Instances.InstanceExists(InstanceToInstall.Name))
                {
                    NolvusMessageBox.ShowMessage("Invalid Instance", "The nolvus instance " + InstanceToInstall.Name + " is already installed!", MessageBoxType.Error);
                }
                else
                {
                    if (InstanceToInstall.IsBeta && NolvusMessageBox.ShowConfirmation("Disclaimer", string.Format("{0} is in BETA state.\n\n\nDon't Install it if :\n\n- You are expecting the full polished version.\n\n- You want to do a full playthrough.\n\n\nInstall it only if :\n\n- You want to help us reporting bugs.\n\n- You want to give us some feedbacks.\n\n\nDo you want to continue?", InstanceToInstall.Name), 390, 470)  == DialogResult.Yes)
                    {
                        INolvusInstance WorkingInstance = ServiceSingleton.Instances.WorkingInstance;

                        WorkingInstance.Settings.LgCode = (DrpDwnLg.SelectedItem as LgCode).Code;
                        WorkingInstance.Settings.LgName = (DrpDwnLg.SelectedItem as LgCode).Name;

                        ServiceSingleton.Dashboard.LoadFrame<PathFrame>();
                    }                    
                }
            }            
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ServiceSingleton.Dashboard.LoadFrame<InstancesFrame>();
        }

        private void NolvusListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SwitchInstance(NolvusListBox.SelectedItem as INolvusVersionDTO);            
        }       
    }
}
