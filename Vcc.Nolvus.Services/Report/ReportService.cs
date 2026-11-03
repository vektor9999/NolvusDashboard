using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Services.Report
{
    public class ReportService : IReportService
    {        
        protected PdfPage PageBreak(PdfDocument Report, ref float Line)
        {
            Line = 10;

            PdfPage Page = Report.Pages.Add();

            Page.Graphics.DrawRectangle(new PdfSolidBrush(new PdfColor(54, 54, 54)), new RectangleF(0, 0, Report.PageSettings.Width, Report.PageSettings.Height));

            return Page;
        }

        private float DrawHeader(string Text, PdfGraphics Graphics, PdfPageSettings PageSettings, float CurrentLine)
        {
            PdfPen PenWhiteRect = new PdfPen(Color.White);
            PenWhiteRect.DashStyle = PdfDashStyle.Solid;
            PenWhiteRect.Width = 3;

            Graphics.DrawRectangle(PdfBrushes.Orange, new RectangleF((PageSettings.Width /2) - 175, CurrentLine, 350, 75));
            Graphics.DrawRectangle(PenWhiteRect, new RectangleF((PageSettings.Width / 2) - 175, CurrentLine, 350, 75));

            Graphics.DrawString(Text, new PdfStandardFont(PdfFontFamily.Helvetica, 20, PdfFontStyle.Bold), PdfBrushes.White, new RectangleF((PageSettings.Width / 2) - 150, CurrentLine + 25, 300, 75), new PdfStringFormat(PdfTextAlignment.Center));

            return 100;
        }

        private float DrawString(string Label, string Text, PdfGraphics Graphics, PdfPageSettings PageSettings, float CurrentLine)
        {            
            Graphics.DrawString(Label, new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Regular), PdfBrushes.White, new RectangleF((PageSettings.Width / 2) - 175, CurrentLine, 350, 20), new PdfStringFormat(PdfTextAlignment.Left));
            Graphics.DrawString(Text, new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Regular), PdfBrushes.Orange, new RectangleF((PageSettings.Width / 2) - 80, CurrentLine, 300, 20), new PdfStringFormat(PdfTextAlignment.Left));

            return 20;
        }

        private float DrawStringTextOnly(string Text, PdfGraphics Graphics, PdfPageSettings PageSettings, float CurrentLine)
        {
            Graphics.DrawString(Text, new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Regular), PdfBrushes.White, new RectangleF((PageSettings.Width / 2) - 175, CurrentLine, 350, 20), new PdfStringFormat(PdfTextAlignment.Left));            

            return 20;
        }

        public Task<PdfDocument> GenerateReportToPdf(ModObjectList ModObjects, Image Image, Action<string, int> Progress)
        {
            return Task.Run(async () =>
            {                
                var Instance = ServiceSingleton.Instances.WorkingInstance;

                PdfDocument Report = new PdfDocument();

                Report.PageSettings.Orientation = PdfPageOrientation.Portrait;
                Report.PageSettings.Margins.All = 0;

                PdfPage Page = Report.Pages.Add();
                PdfGraphics Graphics = Page.Graphics;

                

                PdfPen PenWhite = new PdfPen(Color.White);
                PenWhite.DashStyle = PdfDashStyle.Solid;
                PenWhite.Width = 1;

                PdfPen PenOrange = new PdfPen(Color.Orange);
                PenOrange.DashStyle = PdfDashStyle.Solid;
                PenOrange.Width = 1;

                Page.Graphics.DrawRectangle(new PdfSolidBrush(new PdfColor(54, 54, 54)), new RectangleF(0, 0, Report.PageSettings.Width, Report.PageSettings.Height));

                float HalfWidth = Report.PageSettings.Width / 2;

                Graphics.DrawImage(new PdfBitmap(Image), HalfWidth - 225, 1, 450, 250);                

                float CurrentLine = 250;                

                CurrentLine += DrawString("Report Date : ", string.Format("{0} at {1}", DateTime.Now.ToString("MMMM dd, yyyy", new CultureInfo("en-US")), DateTime.Now.ToShortTimeString()), Graphics, Report.PageSettings, CurrentLine);

                Progress("Generating report", 10);

                #region Hardware                 

                CurrentLine += DrawHeader("HARDWARE CONFIGURATION", Graphics, Report.PageSettings, CurrentLine);                

                CurrentLine += DrawString("CPU : ", await ServiceSingleton.Globals.GetCPUInfo(), Graphics, Report.PageSettings, CurrentLine);
                CurrentLine += DrawString("GPU : ", string.Join(Environment.NewLine, ServiceSingleton.Globals.GetVideoAdapters().ToArray()), Graphics, Report.PageSettings, CurrentLine);

                var Ram = await ServiceSingleton.Globals.GetRamCount();

                if ( Ram != "RAM count not found")
                {
                    CurrentLine += DrawString("RAM : ", string.Format("{0} GB", Ram), Graphics, Report.PageSettings, CurrentLine);
                }
                else
                {
                    CurrentLine += DrawString("RAM : ", string.Format("{0}", Ram), Graphics, Report.PageSettings, CurrentLine);
                }
                

                #endregion

                Progress("Generating report", 20);                
                
                #region Instance Settings

                CurrentLine += 20;

                CurrentLine += DrawHeader("INSTANCE CONFIGURATION", Graphics, Report.PageSettings, CurrentLine);

                CurrentLine += DrawString("Name : ", string.Format("{0}, selected profile : {1}", Instance.Name, ModObjects.Profile != null? ModObjects.Profile : "Default"), Graphics, Report.PageSettings, CurrentLine);

                CurrentLine += DrawString("Version : ", Instance.Version, Graphics, Report.PageSettings, CurrentLine);

                CurrentLine += DrawString("Variant : ", Instance.SettingsManager.Variant, Graphics, Report.PageSettings, CurrentLine);

                CurrentLine += DrawString("Tag : ", Instance.Tag == string.Empty ? "-" : Instance.Tag, Graphics, Report.PageSettings, CurrentLine);

                CurrentLine += DrawString("Install directory : ", Instance.SettingsManager.InstallDir, Graphics, Report.PageSettings, CurrentLine);

                CurrentLine += DrawString("Archive directory : ", Instance.SettingsManager.ArchiveDir, Graphics, Report.PageSettings, CurrentLine);


                if (Instance.SettingsManager.EnableArchiving)
                {
                    CurrentLine += DrawString("Enable archiving : ", "Yes", Graphics, Report.PageSettings, CurrentLine);
                }
                else
                {
                    CurrentLine += DrawString("Enable archiving : ", "No", Graphics, Report.PageSettings, CurrentLine);
                }
                
                CurrentLine += DrawString("Resolution : ", string.Format("{0}x{1}", Instance.SettingsManager.Width, Instance.SettingsManager.Height), Graphics, Report.PageSettings, CurrentLine);
                
                CurrentLine += DrawString("Downscaling : ", Instance.SettingsManager.DownScaling == "TRUE" ? "Yes" : "No", Graphics, Report.PageSettings, CurrentLine);
                CurrentLine += DrawString("Resolution : ", Instance.SettingsManager.DownScaling == "TRUE" ? Instance.SettingsManager.DownScaledResolution : "-", Graphics, Report.PageSettings, CurrentLine);

                switch (Instance.SettingsManager.IniSettings)
                {
                    case "0":
                        CurrentLine += DrawString("Ini level : ", "Low", Graphics, Report.PageSettings, CurrentLine);
                        break;
                    case "1":
                        CurrentLine += DrawString("Ini level : ", "Medium", Graphics, Report.PageSettings, CurrentLine);
                        break;
                    case "2":
                        CurrentLine += DrawString("Ini level : ", "High", Graphics, Report.PageSettings, CurrentLine);
                        break;
                }

                CurrentLine += DrawString("CDN : ", Instance.SettingsManager.CDN, Graphics, Report.PageSettings, CurrentLine);
                CurrentLine += DrawString("Language : ", Instance.SettingsManager.LgName, Graphics, Report.PageSettings, CurrentLine);                

                #endregion

                Progress("Generating report", 30);

                #region Options

                Graphics = PageBreak(Report, ref CurrentLine).Graphics;

                var Categories = Instance.OptionsManager.GetOptionCategories();

                foreach(var Category in Categories)
                {
                    CurrentLine += 20;

                    if (CurrentLine + 100 > Page.Graphics.ClientSize.Height)
                    {
                        Graphics = PageBreak(Report, ref CurrentLine).Graphics;
                    }

                    CurrentLine += DrawHeader(string.Format("{0} Options", Category), Graphics, Report.PageSettings, CurrentLine);

                    var Options = Instance.OptionsManager.GetOptionsByCategory(Category);                    

                    foreach (var Option in Options)
                    {
                        if (CurrentLine + 20 > Page.Graphics.ClientSize.Height)
                        {
                            Graphics = PageBreak(Report, ref CurrentLine).Graphics;
                        }

                        CurrentLine += DrawString(string.Format("{0} : ", Option.FriendlyName), Option.FormattedValue, Graphics, Report.PageSettings, CurrentLine);
                    }                    
                }

                #endregion

                Progress("Generating report", 60);                

                #region Status                

                CurrentLine += 20;

                CurrentLine += DrawHeader("INSTANCE STATUS", Graphics, Report.PageSettings, CurrentLine);

                var AddedMods = ModObjects.AddedModsCount;
                var RemovedMods = ModObjects.RemovedModsCount;
                var VersionMismatch = ModObjects.VersionMismatchCount;
                var DisabledMods = ModObjects.DisabledModsCount;

                if (AddedMods > 0 || RemovedMods > 0 || DisabledMods > 0)
                {
                    CurrentLine += DrawString("List status : ", "List has been modified", Graphics, Report.PageSettings, CurrentLine);
                }
                else if (ServiceSingleton.Packages.ModOrganizer2.GetProfiles().Count > 1)
                {
                    CurrentLine += DrawString("List status : ", string.Format("List might be modified ({0} profiles found)", ServiceSingleton.Packages.ModOrganizer2.GetProfiles().Count), Graphics, Report.PageSettings, CurrentLine);

                    CurrentLine += DrawString("New profile(s) added : ", string.Join(",", ServiceSingleton.Packages.ModOrganizer2.GetProfiles().ToArray()), Graphics, Report.PageSettings, CurrentLine);

                }
                else
                {
                    CurrentLine += DrawString("List status : ", "Vanilla Nolvus", Graphics, Report.PageSettings, CurrentLine);
                }

                CurrentLine += DrawString("Added mods : ", AddedMods.ToString(), Graphics, Report.PageSettings, CurrentLine);
                CurrentLine += DrawString("Removed mods : ", RemovedMods.ToString(), Graphics, Report.PageSettings, CurrentLine);
                CurrentLine += DrawString("Version changed : ", VersionMismatch.ToString(), Graphics, Report.PageSettings, CurrentLine);
                CurrentLine += DrawString("Disabled mods : ", DisabledMods.ToString(), Graphics, Report.PageSettings, CurrentLine);

                #endregion

                Progress("Generating report", 70);

                #region AddedMods

                if (AddedMods > 0)
                {
                    Graphics = PageBreak(Report, ref CurrentLine).Graphics;

                    CurrentLine += 20;

                    CurrentLine += DrawHeader("ADDED MODS", Graphics, Report.PageSettings, CurrentLine);

                    foreach (var Mod in ModObjects.AddedMods)
                    {
                        CurrentLine += DrawStringTextOnly(Mod.Name + string.Format(" [{0}]", Mod.Selected ? "v" : "x"), Graphics, Report.PageSettings, CurrentLine);

                        if (CurrentLine + 20 > Page.Graphics.ClientSize.Height)
                        {
                            Graphics = PageBreak(Report, ref CurrentLine).Graphics;
                        }
                    }
                }

                #endregion

                Progress("Generating report", 80);

                #region RemovedMods

                if (RemovedMods > 0)
                {
                    Graphics = PageBreak(Report, ref CurrentLine).Graphics;

                    CurrentLine += 20;

                    CurrentLine += DrawHeader("REMOVED MODS", Graphics, Report.PageSettings, CurrentLine);

                    foreach (var Mod in ModObjects.RemovedMods)
                    {
                        CurrentLine += DrawStringTextOnly(Mod.Name, Graphics, Report.PageSettings, CurrentLine);

                        if (CurrentLine + 20 > Page.Graphics.ClientSize.Height)
                        {
                            Graphics = PageBreak(Report, ref CurrentLine).Graphics;
                        }
                    }
                }

                #endregion

                Progress("Generating report", 85);

                #region Version Mismatch

                if (VersionMismatch > 0)
                {
                    Graphics = PageBreak(Report, ref CurrentLine).Graphics;

                    CurrentLine += 20;

                    CurrentLine += DrawHeader("VERSION MISMATCH", Graphics, Report.PageSettings, CurrentLine);

                    foreach (var Mod in ModObjects.VersionMismatchMods)
                    {
                        CurrentLine += DrawStringTextOnly(string.Format("{0} , {1}", Mod.Name, Mod.StatusText), Graphics, Report.PageSettings, CurrentLine);

                        if (CurrentLine + 20 > Page.Graphics.ClientSize.Height)
                        {
                            Graphics = PageBreak(Report, ref CurrentLine).Graphics;
                        }
                    }
                }

                #endregion

                Progress("Generating report", 90);

                #region Disabled Mods

                if (DisabledMods > 0)
                {
                    Graphics = PageBreak(Report, ref CurrentLine).Graphics;

                    CurrentLine += 20;

                    CurrentLine += DrawHeader("DISABLED MODS", Graphics, Report.PageSettings, CurrentLine);

                    foreach (var Mod in ModObjects.DisabledMods)
                    {
                        CurrentLine += DrawStringTextOnly(Mod.Name, Graphics, Report.PageSettings, CurrentLine);

                        if (CurrentLine + 20 > Page.Graphics.ClientSize.Height)
                        {
                            Graphics = PageBreak(Report, ref CurrentLine).Graphics;
                        }
                    }
                }

                #endregion

                Progress("Generating report", 100);

                Report.Save(Path.Combine(ServiceSingleton.Folders.ReportDirectory, string.Format("{0}-v{1}.pdf", Instance.Name, Instance.Version)));

                return Report;
            });
         }
    }
}
