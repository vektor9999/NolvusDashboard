using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public Task<string> GenerateReportToClipBoard(ModObjectList ModObjects, Action<string, int> Progress)
        {
            return Task.Run(async () =>
            {                
                var Instance = ServiceSingleton.Instances.WorkingInstance;

                Progress("Generating report", 10);

                #region Hardware

                var Result = "HARDWARE";
                Result += Environment.NewLine;
                Result += Environment.NewLine;

                Result += "CPU : " + await ServiceSingleton.Globals.GetCPUInfo();
                Result += Environment.NewLine;
                Result += "GPU : " + string.Join(Environment.NewLine, ServiceSingleton.Globals.GetVideoAdapters().ToArray());
                Result += Environment.NewLine;
                Result += "RAM : " + await ServiceSingleton.Globals.GetRamCount() + " GB";
                Result += Environment.NewLine;
                Result += Environment.NewLine;

                #endregion

                Progress("Generating report", 20);
                #region Instance

                Result += "INSTANCE CONFIGURATION";
                Result += Environment.NewLine;
                Result += Environment.NewLine;

                Result += string.Format("Name : {0}, selected profile : {1}", Instance.Name, ModObjects.Profile);
                Result += Environment.NewLine;
                Result += string.Format("Version : {0}", Instance.Version);
                Result += Environment.NewLine;
                Result += string.Format("Install directory : {0}", Instance.InstallDir);
                Result += Environment.NewLine;
                Result += string.Format("Archive directory : {0}", Instance.ArchiveDir);
                Result += Environment.NewLine;

                if (Instance.Settings.EnableArchiving)
                {
                    Result += "Enable archiving : Yes";
                }
                else
                {
                    Result += "Enable archiving : No";
                }

                Result += Environment.NewLine;
                Result += Environment.NewLine;

                #endregion

                Progress("Generating report", 30);

                #region Instance Settings

                Result += "INSTANCE SETTINGS";
                Result += Environment.NewLine;
                Result += Environment.NewLine;

                Result += string.Format("Resolution : {0}", Instance.Settings.Width + "x" + Instance.Settings.Height);
                Result += Environment.NewLine;
                Result += string.Format("Screen ratio : {0}", Instance.Settings.Ratio);
                Result += Environment.NewLine;                
                Result += string.Format("CDN : {0}", Instance.Settings.CDN);
                Result += Environment.NewLine;
                Result += string.Format("Language : {0}", Instance.Settings.LgName);
                Result += Environment.NewLine;
                Result += Environment.NewLine;


                #endregion

                Progress("Generating report", 40);

                #region Performance

                Result += "PERFORMANCE SETTINGS";
                Result += Environment.NewLine;
                Result += Environment.NewLine;

                Result += string.Format("Variant : {0}", Instance.Performance.Variant);
                Result += Environment.NewLine;

                Result += string.Format("Anti aliasing : {0}", Instance.Performance.AntiAliasing);
                Result += Environment.NewLine;                

                if (Instance.Performance.Variant == "Redux")
                {
                    Result += "Lods : Redux";
                }
                else
                {
                    Result += "Lods : " + Instance.Performance.LODs;
                }

                Result += Environment.NewLine;                

                switch (Instance.Performance.IniSettings)
                {
                    case "0":
                        Result += "Ini level : Low";
                        break;
                    case "1":
                        Result += "Ini level : Medium";
                        break;
                    case "2":
                        Result += "Ini level : High";
                        break;
                }

                Result += Environment.NewLine;

                if (Instance.Performance.AdvancedPhysics == "TRUE")
                {
                    Result += "Advanced Physics : Yes";
                }
                else
                {
                    Result += "Advanced Physics : No";
                }

                Result += Environment.NewLine;

                if (Instance.Performance.RayTracing == "TRUE")
                {
                    Result += "Ray tracing : Yes";
                }
                else
                {
                    Result += "Ray tracing : No";
                }

                Result += Environment.NewLine;

                if (Instance.Performance.FPSStabilizer == "TRUE")
                {
                    Result += "FPS Stabilizer : Yes";
                }
                else
                {
                    Result += "FPS Stabilizer : No";
                }

                Result += Environment.NewLine;

                if (Instance.Performance.DownScaling == "TRUE")
                {
                    Result += "Downscaling : Yes";
                    Result += Environment.NewLine;
                    Result += "Downscaling resolution : " + Instance.Performance.DownWidth + "x" + Instance.Performance.DownHeight;
                }
                else
                {
                    Result += "Downscaling : No";
                }

                Result += Environment.NewLine;
                Result += Environment.NewLine;

                #endregion

                Progress("Generating report", 50);

                #region ENB

                Result += "ENB SETTINGS";
                Result += Environment.NewLine;
                Result += Environment.NewLine;

                Result += "ENB Preset :"  + ENBs.GetENBByCode(Instance.Options.AlternateENB);

                Result += Environment.NewLine;
                Result += Environment.NewLine;

                #endregion

                Progress("Generating report", 60);

                #region Options

                Result += "INSTANCE OPTIONS";
                Result += Environment.NewLine;
                Result += Environment.NewLine;

                if (Instance.Options.HardcoreMode == "TRUE")
                {
                    Result += "Hardcore mode : Yes";
                }
                else
                {
                    Result += "Hardcore mode : No";
                }

                Result += Environment.NewLine;

                if (Instance.Options.AlternateLeveling == "TRUE")
                {
                    Result += "Alternate leveling : Yes";
                }
                else
                {
                    Result += "Alternate leveling : No";
                }

                Result += Environment.NewLine;

                if (Instance.Options.AlternateStart == "TRUE")
                {
                    Result += "Alternate start : Yes";
                }
                else
                {
                    Result += "Alternate start : No";
                }

                Result += Environment.NewLine;

                if (Instance.Options.FantasyMode == "TRUE")
                {
                    Result += "Fantasy mode : Yes";
                }
                else
                {
                    Result += "Fantasy mode : No";
                }

                Result += Environment.NewLine;

                if (Instance.Options.Nudity == "TRUE")
                {
                    Result += "Nudity : Yes";
                }
                else
                {
                    Result += "Nudity : No";
                }

                Result += Environment.NewLine;

                Result += string.Format("Skin type : {0}", Instance.Options.SkinType);

                Result += Environment.NewLine;
                Result += Environment.NewLine;

                #endregion

                Progress("Generating report", 70);

                #region Status

                Result += "INSTANCE STATUS";
                Result += Environment.NewLine;
                Result += Environment.NewLine;

                var AddedMods = ModObjects.AddedModsCount;
                var RemovedMods = ModObjects.RemovedModsCount;
                var VersionMismatch = ModObjects.VersionMismatchCount;

                if ( AddedMods > 0 || RemovedMods > 0)
                {
                    Result += "List has been modified";
                }
                else
                {
                    Result += "Vanilla Nolvus";
                }

                Result += Environment.NewLine;
                Result += Environment.NewLine;

                Result += string.Format("Added mods : {0}", AddedMods);
                Result += Environment.NewLine;
                Result += string.Format("Removed mods : {0}", RemovedMods);
                Result += Environment.NewLine;
                Result += string.Format("Version changed : {0}", VersionMismatch);

                #endregion

                Progress("Generating report", 100);

                return Result;
            });           
        }

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

                Progress("Generating report", 10);

                #region Hardware                 

                CurrentLine += DrawHeader("HARDWARE CONFIGURATION", Graphics, Report.PageSettings, CurrentLine);                

                CurrentLine += DrawString("CPU : ", await ServiceSingleton.Globals.GetCPUInfo(), Graphics, Report.PageSettings, CurrentLine);
                CurrentLine += DrawString("GPU : ", string.Join(Environment.NewLine, ServiceSingleton.Globals.GetVideoAdapters().ToArray()), Graphics, Report.PageSettings, CurrentLine);
                CurrentLine += DrawString("RAM : ", string.Format("{0} GB", await ServiceSingleton.Globals.GetRamCount()), Graphics, Report.PageSettings, CurrentLine);

                #endregion

                Progress("Generating report", 20);                

                #region Instance Settings

                CurrentLine += 20;

                CurrentLine += DrawHeader("INSTANCE CONFIGURATION", Graphics, Report.PageSettings, CurrentLine);

                CurrentLine += DrawString("Name : ", string.Format("{0}, selected profile : {1}", Instance.Name, ModObjects.Profile != null? ModObjects.Profile : "Default"), Graphics, Report.PageSettings, CurrentLine);

                CurrentLine += DrawString("Version : ", Instance.Version, Graphics, Report.PageSettings, CurrentLine);

                CurrentLine += DrawString("Install directory : ", Instance.InstallDir, Graphics, Report.PageSettings, CurrentLine);

                CurrentLine += DrawString("Archive directory : ", Instance.ArchiveDir, Graphics, Report.PageSettings, CurrentLine);


                if (Instance.Settings.EnableArchiving)
                {
                    CurrentLine += DrawString("Enable archiving : ", "Yes", Graphics, Report.PageSettings, CurrentLine);
                }
                else
                {
                    CurrentLine += DrawString("Enable archiving : ", "No", Graphics, Report.PageSettings, CurrentLine);
                }

                CurrentLine += DrawString("Resolution : ", string.Format("{0}x{1}", Instance.Settings.Width, Instance.Settings.Height), Graphics, Report.PageSettings, CurrentLine);
                CurrentLine += DrawString("Screen ratio : ", Instance.Settings.Ratio, Graphics, Report.PageSettings, CurrentLine);
                CurrentLine += DrawString("CDN : ", Instance.Settings.CDN, Graphics, Report.PageSettings, CurrentLine);
                CurrentLine += DrawString("Language : ", Instance.Settings.LgName, Graphics, Report.PageSettings, CurrentLine);

                #endregion

                Progress("Generating report", 30);

                #region Performance

                CurrentLine += 20;

                CurrentLine += DrawHeader("PERFORMANCE SETTINGS", Graphics, Report.PageSettings, CurrentLine);

                CurrentLine += DrawString("Variant : ", Instance.Performance.Variant, Graphics, Report.PageSettings, CurrentLine);

                Graphics = PageBreak(Report, ref CurrentLine).Graphics;

                CurrentLine += DrawString("Anti aliasing : ", Instance.Performance.AntiAliasing, Graphics, Report.PageSettings, CurrentLine);
               
                if (Instance.Performance.Variant == "Redux")
                {
                    CurrentLine += DrawString("Lods : ", "Redux", Graphics, Report.PageSettings, CurrentLine);
                }
                else
                {
                    CurrentLine += DrawString("Lods : ", Instance.Performance.LODs, Graphics, Report.PageSettings, CurrentLine);                    
                }                

                switch (Instance.Performance.IniSettings)
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
                

                if (Instance.Performance.AdvancedPhysics == "TRUE")
                {
                    CurrentLine += DrawString("Advanced Physics : ", "Yes", Graphics, Report.PageSettings, CurrentLine);
                }
                else
                {
                    CurrentLine += DrawString("Advanced Physics : ", "No", Graphics, Report.PageSettings, CurrentLine);
                }
                

                if (Instance.Performance.RayTracing == "TRUE")
                {
                    CurrentLine += DrawString("Ray tracing : ", "Yes", Graphics, Report.PageSettings, CurrentLine);
                }
                else
                {
                    CurrentLine += DrawString("Ray tracing : ", "No", Graphics, Report.PageSettings, CurrentLine);
                }                

                if (Instance.Performance.FPSStabilizer == "TRUE")
                {
                    CurrentLine += DrawString("FPS Stabilizer : ", "Yes", Graphics, Report.PageSettings, CurrentLine);
                }
                else
                {
                    CurrentLine += DrawString("FPS Stabilizer : ", "No", Graphics, Report.PageSettings, CurrentLine);
                }
                

                if (Instance.Performance.DownScaling == "TRUE")
                {
                    CurrentLine += DrawString("Downscaling : ", "Yes", Graphics, Report.PageSettings, CurrentLine);
                    CurrentLine += DrawString("Downscaling resolution : ", string.Format("{0}x{1}", Instance.Performance.DownWidth, Instance.Performance.DownHeight), Graphics, Report.PageSettings, CurrentLine);
                }
                else
                {
                    CurrentLine += DrawString("Downscaling : ", "No", Graphics, Report.PageSettings, CurrentLine);
                }

                #endregion

                Progress("Generating report", 40);

                #region ENB

                CurrentLine += 20;

                CurrentLine += DrawHeader("ENB SETTINGS", Graphics, Report.PageSettings, CurrentLine);

                CurrentLine += DrawString("ENB Preset : ", ENBs.GetENBByCode(Instance.Options.AlternateENB), Graphics, Report.PageSettings, CurrentLine);

                #endregion

                Progress("Generating report", 50);

                #region Options

                CurrentLine += 20;

                CurrentLine += DrawHeader("INSTANCE OPTIONS", Graphics, Report.PageSettings, CurrentLine);

                if (Instance.Options.HardcoreMode == "TRUE")
                {
                    CurrentLine += DrawString("Hardcore mode : ", "Yes", Graphics, Report.PageSettings, CurrentLine);
                }
                else
                {
                    CurrentLine += DrawString("Hardcore mode : ", "No", Graphics, Report.PageSettings, CurrentLine);
                }                

                if (Instance.Options.AlternateLeveling == "TRUE")
                {
                    CurrentLine += DrawString("Alternate leveling : ", "Yes", Graphics, Report.PageSettings, CurrentLine);
                }
                else
                {
                    CurrentLine += DrawString("Alternate leveling : ", "No", Graphics, Report.PageSettings, CurrentLine);
                }                

                if (Instance.Options.AlternateStart == "TRUE")
                {
                    CurrentLine += DrawString("Alternate start : ", "Yes", Graphics, Report.PageSettings, CurrentLine);
                }
                else
                {
                    CurrentLine += DrawString("Alternate start : ", "No", Graphics, Report.PageSettings, CurrentLine);
                }                

                if (Instance.Options.FantasyMode == "TRUE")
                {
                    CurrentLine += DrawString("Fantasy mode : ", "Yes", Graphics, Report.PageSettings, CurrentLine);
                }
                else
                {
                    CurrentLine += DrawString("Fantasy mode : ", "No", Graphics, Report.PageSettings, CurrentLine);
                }

                if (Instance.Options.Nudity == "TRUE")
                {
                    CurrentLine += DrawString("Nudity : ", "Yes", Graphics, Report.PageSettings, CurrentLine);
                }
                else
                {
                    CurrentLine += DrawString("Nudity : ", "No", Graphics, Report.PageSettings, CurrentLine);
                }

                CurrentLine += DrawString("Skin type : ", Instance.Options.SkinType, Graphics, Report.PageSettings, CurrentLine);

                #endregion

                Progress("Generating report", 60);

                #region Status

                CurrentLine += 20;

                CurrentLine += DrawHeader("INSTANCE STATUS", Graphics, Report.PageSettings, CurrentLine);

                var AddedMods = ModObjects.AddedModsCount;
                var RemovedMods = ModObjects.RemovedModsCount;
                var VersionMismatch = ModObjects.VersionMismatchCount;

                if (AddedMods > 0 || RemovedMods > 0)
                {
                    CurrentLine += DrawString("List status : ", "List has been modified", Graphics, Report.PageSettings, CurrentLine);
                }
                else
                {
                    CurrentLine += DrawString("List status : ", "Vanilla Nolvus", Graphics, Report.PageSettings, CurrentLine);
                }



                CurrentLine += DrawString("Added mods : ", AddedMods.ToString(), Graphics, Report.PageSettings, CurrentLine);
                CurrentLine += DrawString("Removed mods : ", RemovedMods.ToString(), Graphics, Report.PageSettings, CurrentLine);
                CurrentLine += DrawString("Version changed : ", VersionMismatch.ToString(), Graphics, Report.PageSettings, CurrentLine);

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
                        CurrentLine += DrawStringTextOnly(Mod.Name, Graphics, Report.PageSettings, CurrentLine);

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

                Progress("Generating report", 90);

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

                Progress("Generating report", 100);

                Report.Save(Path.Combine(ServiceSingleton.Folders.ReportDirectory, string.Format("{0}-v{1}.pdf", Instance.Name, Instance.Version)));

                return Report;
            });
         }
    }
}
