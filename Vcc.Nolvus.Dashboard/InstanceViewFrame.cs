using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Syncfusion.WinForms.Controls;
using Syncfusion.Data;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.ListView.Enums;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Vcc.Nolvus.Dashboard.Core;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Frames;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Api.Installer;

namespace Vcc.Nolvus.Dashboard
{
    public partial class InstanceViewFrame : DashboardFrame
    {       
        public InstanceViewFrame()
        {
            InitializeComponent();
        }

        public InstanceViewFrame(IDashboard Dashboard, FrameParameters Params) 
            : base(Dashboard, Params)            
        {
            InitializeComponent();

            PnlHeader.BackColor = Color.FromArgb(92, 184, 92);
            PnlHeader.Paint += PnlHeader_Paint;           

            ModsGrid.ThemeName = "Office2016Black";
            ModsGrid.ShowRowHeader = true;
            ModsGrid.Style.RowHeaderStyle.SelectionMarkerThickness = 100;
            ModsGrid.Style.RowHeaderStyle.SelectionMarkerColor = Color.Orange;
            ModsGrid.Style.RowHeaderStyle.SelectionBackColor = Color.FromArgb(68, 68, 68);

            ModsGrid.Style.CurrentCellStyle.BackColor = Color.Orange;
            ModsGrid.Style.CurrentCellStyle.TextColor = Color.White;
            ModsGrid.Style.CurrentCellStyle.BorderColor = Color.Orange;

            ModsGrid.GroupColumnDescriptions.Add(new GroupColumnDescription() { ColumnName = "Category", SortGroupRecords = false });

            ModsGrid.AllowSorting = false;

        }

        private void PnlHeader_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, PnlHeader.ClientRectangle,
              Color.White, 3, ButtonBorderStyle.Solid, // left
              Color.White, 3, ButtonBorderStyle.Solid, // top
              Color.White, 3, ButtonBorderStyle.Solid, // right
              Color.White, 3, ButtonBorderStyle.Solid);// bottom
        }

        //private void LoadGrids(List<ModObject> Mods)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke((System.Action<List<ModObject>>)LoadGrids, Mods);
        //        return;
        //    }

        //    ModsGrid.SourceType = typeof(ModObject);
        //    ModsGrid.DataSource = Mods;

        //    ModsGrid.ExpandAllGroup();
        //}

        //private void ShowLoading()
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke((System.Action)ShowLoading);
        //        return;
        //    }

        //    PicBoxLoading.Show();
        //}

        //private void HideLoading()
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke((System.Action)HideLoading);
        //        return;
        //    }

        //    PicBoxLoading.Hide();
        //}

        private void SetPlayText(string Text)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<string>)SetPlayText, Text);
                return;
            }

            this.BtnPlay.Text = Text;
            this.BtnPlay.Enabled = true;
        }

        public void SetStepInfo(string Text)
        {
            if (InvokeRequired)
            {
                Invoke((Action<string>)SetStepInfo, Text);
                return;
            }

            this.LblStepText.Text = Text;
        }

        //public override void Initialize()
        //{
        //    SetStepInfo(string.Empty);

        //    LblHeader.Text = InstallerEnvironment.Instance.Name + " v" + InstallerEnvironment.Instance.Version;

        //    this.MainForm.SetStripInfo("Instance mods for " + InstallerEnvironment.Instance.Name + " v" + InstallerEnvironment.Instance.Version);

        //    Loaded(true);
            
        //    LoadMods().ContinueWith(List => 
        //    {
        //        if (List.IsFaulted)
        //        {
        //            this.ShowError("Error during mods loading", List.Exception.InnerException.Message, List.Exception.InnerException.StackTrace, null, false);
        //        }
        //        else
        //        {
        //            LoadGrids(List.Result);
        //            SetStepInfo(string.Empty);
        //            SetProgress(0);
        //            HideLoading();
        //        }
        //    }, TaskScheduler.FromCurrentSynchronizationContext());    
                     
        //}     

        private void CalculateProgress(int WorkCount)
        {
            //int Progress = 0;

            //double RelativePercent = (100 / (double)WorkCount);

            //this.SumOfOverallRelativePercent = SumOfOverallRelativePercent + RelativePercent;

            //if (RelativePercent < 1)
            //{
            //    if (SumOfOverallRelativePercent >= InternalOverallProgression)
            //    {
            //        Progress = Progress + InternalOverallProgression;
            //        this.SetProgress(Progress);
            //        this.SetStepInfo("Loading mods (" + Progress + "%)...");
            //        InternalOverallProgression++;
            //    }
            //}
            //else
            //{
            //    Progress = Progress + (int)SumOfOverallRelativePercent;
            //    this.SetStepInfo("Loading mods (" + Progress + "%)...");
            //    this.SetProgress(Progress);
            //}
        }

        //public Task<List<ModObject>> LoadMods()
        //{
        //    TaskCompletionSource<List<ModObject>> Tcs = new TaskCompletionSource<List<ModObject>>();

        //    SetStepInfo("Loading mods...");

        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            string ModListFile = Path.Combine(InstallerEnvironment.Instance.InstallDir + "\\MODS\\profiles\\" + InstallerEnvironment.Instance.Name, "modlist.txt");

        //            List<string> Mods = System.IO.File.ReadAllLines(ModListFile).ToList();

        //            Mods.Reverse();                    

        //            List<ModObject> ModList = new List<ModObject>();

        //            string Category = string.Empty;                    

        //            this.SetMaximumProgress(100);

        //            int Counter = 0;

        //            int Max = 0;
        //            int StartIndex = 0;

        //            if (InstallerEnvironment.Instance.Game == "SE")
        //            {
        //                StartIndex = 8;
        //                Max = Mods.Count - 9;
        //            }
        //            else
        //            {
        //                StartIndex = 78;
        //                Max = Mods.Count - 79;
        //            }

        //            for (int i = StartIndex; i < Mods.Count - 1; i++)
        //            {
        //                string _Mod = Mods[i].Substring(1, Mods[i].Length - 1);

        //                if (_Mod.Contains("_separator"))
        //                {
        //                    Category = _Mod.Replace("_separator", string.Empty);
        //                }
        //                else
        //                {
        //                    ModObject ModObject = new ModObject();

        //                    ModObject.Priority = ModList.Count + 1;
        //                    ModObject.Name = _Mod;
        //                    ModObject.Category = Category;

        //                    var Parser = new FileIniDataParser();

        //                    try
        //                    {

        //                        IniData Data = Parser.ReadFile(Path.Combine(InstallerEnvironment.Instance.InstallDir + "\\MODS\\mods\\" + ModObject.Name, "meta.ini"));

        //                        ModObject.Version = Data["General"]["version"];

        //                        Mod Mod = _PackageManager.GetMod(ModObject.Name);

        //                        if (Mod != null)
        //                        {
        //                            ModObject.Status = ModStatus.OK;

        //                            if (ModObject.Version != Mod.Version)
        //                            {
        //                                ModObject.Status = ModStatus.VersionDiffer;
        //                                ModObject.StatusText = string.Format("Version mismatch expected v{0}", Mod.Version);
        //                            }
        //                            else
        //                            {
        //                                ModObject.StatusText = "OK";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ModObject.Status = ModStatus.CustomInstalled;
        //                            ModObject.StatusText = "Not from the guide";
        //                        }
        //                    }
        //                    catch(Exception Ex)
        //                    {
        //                        ModObject.Status = ModStatus.Error;
        //                        ModObject.StatusText = "Can not parse file : " + InstallerEnvironment.Instance.InstallDir + "\\MODS\\mods\\" + ModObject.Name + "\\meta.ini" + " (" + Ex.Message + ")";
        //                    }

        //                    ModList.Add(ModObject);
        //                }

        //                Counter++;
        //                this.CalculateProgress(Max);
        //            }

        //            this.SetStepInfo("Loading mods (100%)...");
        //            this.SetProgress(100);

        //            Tcs.SetResult(ModList);
        //        }
        //        catch (Exception e)
        //        {
        //            Tcs.SetException(e);
        //        }
        //    });

        //    return Tcs.Task;
        //}

        //public Task ApplyLoadOrder()
        //{
        //    TaskCompletionSource<object> Tcs = new TaskCompletionSource<object>();            

        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            string LoadOrderFile = InstallerEnvironment.Instance.InstallDir + "\\MODS\\profiles\\" + InstallerEnvironment.Instance.Name + "\\loadorder.txt";
        //            string PluginsFile = InstallerEnvironment.Instance.InstallDir + "\\MODS\\profiles\\" + InstallerEnvironment.Instance.Name + "\\plugins.txt";

        //            List<string> LoadOrderLines = new List<string>();

        //            #region Load Order

        //            if (InstallerEnvironment.Instance.Game == "SE")
        //            {
        //                LoadOrderLines.Add("# This file was automatically generated by Mod Organizer.");
        //                LoadOrderLines.Add("Skyrim.esm");
        //                LoadOrderLines.Add("Update.esm");
        //                LoadOrderLines.Add("Dawnguard.esm");
        //                LoadOrderLines.Add("HearthFires.esm");
        //                LoadOrderLines.Add("Dragonborn.esm");
        //                LoadOrderLines.Add("ccBGSSSE001-Fish.esm");
        //                LoadOrderLines.Add("ccQDRSSE001-SurvivalMode.esl");
        //                LoadOrderLines.Add("ccBGSSSE037-Curios.esl");
        //                LoadOrderLines.Add("ccBGSSSE025-AdvDSGS.esm");
        //            }
        //            else
        //            {
        //                LoadOrderLines.Add("# This file was automatically generated by Mod Organizer.");
        //                LoadOrderLines.Add("Skyrim.esm");
        //                LoadOrderLines.Add("Update.esm");
        //                LoadOrderLines.Add("Dawnguard.esm");
        //                LoadOrderLines.Add("HearthFires.esm");
        //                LoadOrderLines.Add("Dragonborn.esm");
        //                LoadOrderLines.Add("ccasvsse001-almsivi.esm");
        //                LoadOrderLines.Add("ccBGSSSE001-Fish.esm");
        //                LoadOrderLines.Add("ccbgssse002-exoticarrows.esl");
        //                LoadOrderLines.Add("ccbgssse003-zombies.esl");
        //                LoadOrderLines.Add("ccbgssse004-ruinsedge.esl");
        //                LoadOrderLines.Add("ccbgssse005-goldbrand.esl");
        //                LoadOrderLines.Add("ccbgssse006-stendarshammer.esl");
        //                LoadOrderLines.Add("ccbgssse007-chrysamere.esl");
        //                LoadOrderLines.Add("ccbgssse010-petdwarvenarmoredmudcrab.esl");
        //                LoadOrderLines.Add("ccbgssse011-hrsarmrelvn.esl");
        //                LoadOrderLines.Add("ccbgssse012-hrsarmrstl.esl");
        //                LoadOrderLines.Add("ccbgssse014-spellpack01.esl");
        //                LoadOrderLines.Add("ccbgssse019-staffofsheogorath.esl");
        //                LoadOrderLines.Add("ccbgssse020-graycowl.esl");
        //                LoadOrderLines.Add("ccbgssse021-lordsmail.esl");
        //                LoadOrderLines.Add("ccmtysse001-knightsofthenine.esl");
        //                LoadOrderLines.Add("ccQDRSSE001-SurvivalMode.esl");
        //                LoadOrderLines.Add("cctwbsse001-puzzledungeon.esm");
        //                LoadOrderLines.Add("cceejsse001-hstead.esm");
        //                LoadOrderLines.Add("ccqdrsse002-firewood.esl");
        //                LoadOrderLines.Add("ccbgssse018-shadowrend.esl");
        //                LoadOrderLines.Add("ccbgssse035-petnhound.esl");
        //                LoadOrderLines.Add("ccfsvsse001-backpacks.esl");
        //                LoadOrderLines.Add("cceejsse002-tower.esl");
        //                LoadOrderLines.Add("ccedhsse001-norjewel.esl");
        //                LoadOrderLines.Add("ccvsvsse002-pets.esl");
        //                LoadOrderLines.Add("ccBGSSSE037-Curios.esl");
        //                LoadOrderLines.Add("ccbgssse034-mntuni.esl");
        //                LoadOrderLines.Add("ccbgssse045-hasedoki.esl");
        //                LoadOrderLines.Add("ccbgssse008-wraithguard.esl");
        //                LoadOrderLines.Add("ccbgssse036-petbwolf.esl");
        //                LoadOrderLines.Add("ccffbsse001-imperialdragon.esl");
        //                LoadOrderLines.Add("ccmtysse002-ve.esl");
        //                LoadOrderLines.Add("ccbgssse043-crosselv.esl");
        //                LoadOrderLines.Add("ccvsvsse001-winter.esl");
        //                LoadOrderLines.Add("cceejsse003-hollow.esl");
        //                LoadOrderLines.Add("ccbgssse016-umbra.esm");
        //                LoadOrderLines.Add("ccbgssse031-advcyrus.esm");
        //                LoadOrderLines.Add("ccbgssse038-bowofshadows.esl");
        //                LoadOrderLines.Add("ccbgssse040-advobgobs.esl");
        //                LoadOrderLines.Add("ccbgssse050-ba_daedric.esl");
        //                LoadOrderLines.Add("ccbgssse052-ba_iron.esl");
        //                LoadOrderLines.Add("ccbgssse054-ba_orcish.esl");
        //                LoadOrderLines.Add("ccbgssse058-ba_steel.esl");
        //                LoadOrderLines.Add("ccbgssse059-ba_dragonplate.esl");
        //                LoadOrderLines.Add("ccbgssse061-ba_dwarven.esl");
        //                LoadOrderLines.Add("ccpewsse002-armsofchaos.esl");
        //                LoadOrderLines.Add("ccbgssse041-netchleather.esl");
        //                LoadOrderLines.Add("ccedhsse002-splkntset.esl");
        //                LoadOrderLines.Add("ccbgssse064-ba_elven.esl");
        //                LoadOrderLines.Add("ccbgssse063-ba_ebony.esl");
        //                LoadOrderLines.Add("ccbgssse062-ba_dwarvenmail.esl");
        //                LoadOrderLines.Add("ccbgssse060-ba_dragonscale.esl");
        //                LoadOrderLines.Add("ccbgssse056-ba_silver.esl");
        //                LoadOrderLines.Add("ccbgssse055-ba_orcishscaled.esl");
        //                LoadOrderLines.Add("ccbgssse053-ba_leather.esl");
        //                LoadOrderLines.Add("ccbgssse051-ba_daedricmail.esl");
        //                LoadOrderLines.Add("ccbgssse057-ba_stalhrim.esl");
        //                LoadOrderLines.Add("ccbgssse066-staves.esl");
        //                LoadOrderLines.Add("ccbgssse067-daedinv.esm");
        //                LoadOrderLines.Add("ccbgssse068-bloodfall.esl");
        //                LoadOrderLines.Add("ccbgssse069-contest.esl");
        //                LoadOrderLines.Add("ccvsvsse003-necroarts.esl");
        //                LoadOrderLines.Add("ccvsvsse004-beafarmer.esl");
        //                LoadOrderLines.Add("ccBGSSSE025-AdvDSGS.esm");
        //                LoadOrderLines.Add("ccffbsse002-crossbowpack.esl");
        //                LoadOrderLines.Add("ccbgssse013-dawnfang.esl");
        //                LoadOrderLines.Add("ccrmssse001-necrohouse.esl");
        //                LoadOrderLines.Add("ccedhsse003-redguard.esl");
        //                LoadOrderLines.Add("cceejsse004-hall.esl");
        //                LoadOrderLines.Add("cceejsse005-cave.esm");
        //                LoadOrderLines.Add("cckrtsse001_altar.esl");
        //                LoadOrderLines.Add("cccbhsse001-gaunt.esl");
        //                LoadOrderLines.Add("ccafdsse001-dwesanctuary.esm");
        //            }

        //            #endregion

        //            List<string> PluginsLines = new List<string>();

        //            PluginsLines.Add("# This file was automatically generated by Mod Organizer.");

        //            this.SetMaximumProgress(_PackageManager.LoadOrder.Count);

        //            int Counter = 1;

        //            List<string> OptionalEsps = _PackageManager.GetOptionalEsps();

        //            foreach (string Esp in _PackageManager.LoadOrder)
        //            {
        //                if (OptionalEsps.Where(x => x == Esp).FirstOrDefault() == null)
        //                {
        //                    LoadOrderLines.Add(Esp);
        //                    PluginsLines.Add("*" + Esp);
        //                }
        //                else
        //                {

        //                }

        //                this.SetProgress(Counter);
        //                Counter++;
        //            }

        //            System.IO.File.WriteAllLines(LoadOrderFile, LoadOrderLines.ToArray());
        //            System.IO.File.WriteAllLines(PluginsFile, PluginsLines.ToArray());

        //            Tcs.SetResult(new object());
        //        }
        //        catch (Exception e)
        //        {
        //            Tcs.SetException(e);
        //        }
        //    });

        //    return Tcs.Task;
        //}

        //public Task ApplyInstallOrder()
        //{
        //    TaskCompletionSource<object> Tcs = new TaskCompletionSource<object>();

        //    Task.Run(() =>
        //    {
        //        try
        //        {
        //            List<InstallableElement> Elements = _PackageManager.GetInstallList();
        //            List<string> Lines = new List<string>();

        //            #region Install Order

        //            if (InstallerEnvironment.Instance.Game == "SE")
        //            {
        //                Lines.Add("-0. MASTER FILES_separator");
        //                Lines.Add("*Creation Club: ccQDRSSE001-SurvivalMode");
        //                Lines.Add("*Creation Club: ccBGSSSE037-Curios");
        //                Lines.Add("*Creation Club: ccBGSSSE025-AdvDSGS");
        //                Lines.Add("*Creation Club: ccBGSSSE001-Fish");
        //                Lines.Add("*DLC: HeartFires");
        //                Lines.Add("*DLC: Dragonborn");
        //                Lines.Add("*DLC: Dawnguard");
        //            }
        //            else
        //            {
        //                Lines.Add("-0. MASTER FILES_separator");
        //                Lines.Add("*Creation Club: ccvsvsse004-beafarmer");
        //                Lines.Add("*Creation Club: ccvsvsse003-necroarts");
        //                Lines.Add("*Creation Club: ccvsvsse002-pets");
        //                Lines.Add("*Creation Club: ccvsvsse001-winter");
        //                Lines.Add("*Creation Club: cctwbsse001-puzzledungeon");
        //                Lines.Add("*Creation Club: ccrmssse001-necrohouse");
        //                Lines.Add("*Creation Club: ccqdrsse002-firewood");
        //                Lines.Add("*Creation Club: ccpewsse002-armsofchaos");
        //                Lines.Add("*Creation Club: ccmtysse002-ve");
        //                Lines.Add("*Creation Club: ccmtysse001-knightsofthenine");
        //                Lines.Add("*Creation Club: cckrtsse001_altar");
        //                Lines.Add("*Creation Club: ccfsvsse001-backpacks");
        //                Lines.Add("*Creation Club: ccffbsse002-crossbowpack");
        //                Lines.Add("*Creation Club: ccffbsse001-imperialdragon");
        //                Lines.Add("*Creation Club: cceejsse005-cave");
        //                Lines.Add("*Creation Club: cceejsse004-hall");
        //                Lines.Add("*Creation Club: cceejsse003-hollow");
        //                Lines.Add("*Creation Club: cceejsse002-tower");
        //                Lines.Add("*Creation Club: cceejsse001-hstead");
        //                Lines.Add("*Creation Club: ccedhsse003-redguard");
        //                Lines.Add("*Creation Club: ccedhsse002-splkntset");
        //                Lines.Add("*Creation Club: ccedhsse001-norjewel");
        //                Lines.Add("*Creation Club: cccbhsse001-gaunt");
        //                Lines.Add("*Creation Club: ccbgssse069-contest");
        //                Lines.Add("*Creation Club: ccbgssse068-bloodfall");
        //                Lines.Add("*Creation Club: ccbgssse067-daedinv");
        //                Lines.Add("*Creation Club: ccbgssse066-staves");
        //                Lines.Add("*Creation Club: ccbgssse064-ba_elven");
        //                Lines.Add("*Creation Club: ccbgssse063-ba_ebony");
        //                Lines.Add("*Creation Club: ccbgssse062-ba_dwarvenmail");
        //                Lines.Add("*Creation Club: ccbgssse061-ba_dwarven");
        //                Lines.Add("*Creation Club: ccbgssse060-ba_dragonscale");
        //                Lines.Add("*Creation Club: ccbgssse059-ba_dragonplate");
        //                Lines.Add("*Creation Club: ccbgssse058-ba_steel");
        //                Lines.Add("*Creation Club: ccbgssse057-ba_stalhrim");
        //                Lines.Add("*Creation Club: ccbgssse056-ba_silver");
        //                Lines.Add("*Creation Club: ccbgssse055-ba_orcishscaled");
        //                Lines.Add("*Creation Club: ccbgssse054-ba_orcish");
        //                Lines.Add("*Creation Club: ccbgssse053-ba_leather");
        //                Lines.Add("*Creation Club: ccbgssse052-ba_iron");
        //                Lines.Add("*Creation Club: ccbgssse051-ba_daedricmail");
        //                Lines.Add("*Creation Club: ccbgssse050-ba_daedric");
        //                Lines.Add("*Creation Club: ccbgssse045-hasedoki");
        //                Lines.Add("*Creation Club: ccbgssse043-crosselv");
        //                Lines.Add("*Creation Club: ccbgssse041-netchleather");
        //                Lines.Add("*Creation Club: ccbgssse040-advobgobs");
        //                Lines.Add("*Creation Club: ccbgssse038-bowofshadows");
        //                Lines.Add("*Creation Club: ccbgssse036-petbwolf");
        //                Lines.Add("*Creation Club: ccbgssse035-petnhound");
        //                Lines.Add("*Creation Club: ccbgssse034-mntuni");
        //                Lines.Add("*Creation Club: ccbgssse031-advcyrus");
        //                Lines.Add("*Creation Club: ccbgssse021-lordsmail");
        //                Lines.Add("*Creation Club: ccbgssse020-graycowl");
        //                Lines.Add("*Creation Club: ccbgssse019-staffofsheogorath");
        //                Lines.Add("*Creation Club: ccbgssse018-shadowrend");
        //                Lines.Add("*Creation Club: ccbgssse016-umbra");
        //                Lines.Add("*Creation Club: ccbgssse014-spellpack01");
        //                Lines.Add("*Creation Club: ccbgssse013-dawnfang");
        //                Lines.Add("*Creation Club: ccbgssse012-hrsarmrstl");
        //                Lines.Add("*Creation Club: ccbgssse011-hrsarmrelvn");
        //                Lines.Add("*Creation Club: ccbgssse010-petdwarvenarmoredmudcrab");
        //                Lines.Add("*Creation Club: ccbgssse008-wraithguard");
        //                Lines.Add("*Creation Club: ccbgssse007-chrysamere");
        //                Lines.Add("*Creation Club: ccbgssse006-stendarshammer");
        //                Lines.Add("*Creation Club: ccbgssse005-goldbrand");
        //                Lines.Add("*Creation Club: ccbgssse004-ruinsedge");
        //                Lines.Add("*Creation Club: ccbgssse003-zombies");
        //                Lines.Add("*Creation Club: ccbgssse002-exoticarrows");
        //                Lines.Add("*Creation Club: ccasvsse001-almsivi");
        //                Lines.Add("*Creation Club: ccafdsse001-dwesanctuary");
        //                Lines.Add("*Creation Club: ccQDRSSE001-SurvivalMode");
        //                Lines.Add("*Creation Club: ccBGSSSE037-Curios");
        //                Lines.Add("*Creation Club: ccBGSSSE025-AdvDSGS");
        //                Lines.Add("*Creation Club: ccBGSSSE001-Fish");
        //                Lines.Add("*DLC: HearthFires");
        //                Lines.Add("*DLC: Dragonborn");
        //                Lines.Add("*DLC: Dawnguard");

        //            }

        //            #endregion

        //            string ModListFile = InstallerEnvironment.Instance.InstallDir + "\\MODS\\profiles\\" + InstallerEnvironment.Instance.Name + "\\modlist.txt";                    

        //            this.SetMaximumProgress(Elements.Count);

        //            int Counter = 1;

        //            foreach (InstallableElement Element in Elements)
        //            {
        //                if (Element is MOElement && (Element as MOElement).Display)
        //                {
        //                    string Name = (Element as MOElement).MoDirectoryName;

        //                    Lines.Add("+" + Name);

        //                    this.SetProgress(Counter);
        //                    Counter++;
        //                }
                        
        //            }

        //            Lines.Add("# This file was automatically generated by Mod Organizer.");

        //            Lines.Reverse();

        //            System.IO.File.WriteAllLines(ModListFile, Lines.ToArray());

        //            Tcs.SetResult(new object());
        //        }
        //        catch (Exception e)
        //        {
        //            Tcs.SetException(e);
        //        }
        //    });

        //    return Tcs.Task;
        //}

        private int GetTotalRecordsCount(Group group)
        {
            int count = 0;

            if (group.Groups != null)
            {
                foreach (var g in group.Groups)
                {
                    if (g.Groups != null)
                        foreach (var g1 in g.Groups)
                            count += GetTotalRecordsCount(g1);

                    if (g.Records != null)
                        count += g.Records.Count;
                }
            }
            else
                if (group.Records != null)
                count += group.Records.Count;

            return count;
        }

        private void ModsGrid_DrawCell(object sender, Syncfusion.WinForms.DataGrid.Events.DrawCellEventArgs e)
        {
            if (e.DataRow.RowType == RowType.CaptionCoveredRow && !string.IsNullOrEmpty(e.DisplayText))
            {
                var displayText = string.Empty;
                var group = (e.DataRow.RowData as Syncfusion.Data.Group);

                if (group != null)
                {
                    if (group.Groups != null)
                    {
                        int Mods = 0;
                        Mods = GetTotalRecordsCount(group);
                        displayText = group.Key.ToString() + " ( " + Mods + " Mods (" + group.Groups.Count + " Sub Categories)";
                    }
                    else if (group.Records != null)
                    {
                        displayText = group.Key.ToString() + " ( " + group.Records.Count + " Mods )";                        
                    }

                    e.DisplayText = displayText;
                }
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            //InstallerEnvironment.Instance = null;
            //ShowFrame(typeof(InstancesFrame), true);
        }

        private void BtnExpand_Click(object sender, EventArgs e)
        {
            ModsGrid.ExpandAllGroup();
        }

        private void BtnCollapse_Click(object sender, EventArgs e)
        {
            ModsGrid.CollapseAllGroup();
        }

        private void ModsGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            //if (e.Column.MappingName == "StatusText")
            //{
               
            //    if ((e.DataRow.RowData as ModObject).Status == ModStatus.OK)
            //    {
            //        e.Style.TextColor = Color.LimeGreen;
            //    }
            //    else if ((e.DataRow.RowData as ModObject).Status == ModStatus.VersionDiffer)
            //    {
            //        e.Style.TextColor = Color.Orange;
            //    }
            //    else
            //    {
            //        e.Style.TextColor = Color.Red;
            //    }             
            //}
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            //Process ModOrganizer = Process.Start(InstallerEnvironment.Instance.InstallDir + "\\MO2\\ModOrganizer.exe");

            //this.BtnPlay.Text = "Running...";
            //this.BtnPlay.Enabled = false;

            //Task.Run(() =>
            //{
            //    ModOrganizer.WaitForExit();

            //    if (ModOrganizer.ExitCode == 0)
            //    {
            //        this.SetPlayText("Play");
            //    }
            //});
        }

        private void BtnIntegrity_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            //if (InstallerEnvironment.Instance.Game == "SE")
            //{
            //    ShowFrame(typeof(InstanceSettingsFrame), true);
            //}
            //else
            //{
            //    ShowFrame(typeof(v5.InstanceSettingsFrame), true);
            //}            
        }

        private void BtnLoadOrder_Click(object sender, EventArgs e)
        {
            //Process[] ModOrganizerProcesses = Process.GetProcessesByName("ModOrganizer");

            //if (ModOrganizerProcesses.Length == 0)
            //{
            //    SetStepInfo("Applying Install order...");

            //    ApplyInstallOrder().ContinueWith(InstallOrder =>
            //    {
            //        if (InstallOrder.IsFaulted)
            //        {
            //            this.ShowError("Error applying Install order", InstallOrder.Exception.InnerException.Message, InstallOrder.Exception.InnerException.StackTrace, null, false);
            //        }
            //        else
            //        {
            //            SetProgress(0);

            //            SetStepInfo("Applying load order...");

            //            ApplyLoadOrder().ContinueWith(LoadOrder =>
            //            {
            //                SetProgress(0);
            //                SetStepInfo(string.Empty);
            //            });
            //        }
            //    });
            //}
            //else
            //{
            //    NolvusMessageBox.ShowMessage("Mod Organizer 2", "An instance of Mod Organizer 2 is already running! Close it first.", MessageBoxType.Error);
            //}            
        }
    }
}
