using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Misc;
using ZetaLongPaths;

namespace Vcc.Nolvus.Services.ENB
{
    public class ENBService : IENBService
    {
        private List<IENBPreset> EnbPresets
        {
            get
            {
                return ServiceSingleton.Packages.AllMods.Where(x => x is IENBPreset).Cast<IENBPreset>().ToList();
            }
        }

        private List<IReshade> Reshade
        {
            get
            {
                return ServiceSingleton.Packages.AllMods.Where(x => x is IReshade).Cast<IReshade>().ToList();
            }
        }   

        private List<IENBPreset> CurrentEnbPreset(string Preset)
        {
            return EnbPresets.Where(x => x.GetFieldValueByKey("EnbCode") == Preset).ToList();
        }

        public IENBPreset CurrentPreset(string Preset)
        {
            return CurrentEnbPreset(Preset).FirstOrDefault();
        }

        public async Task<List<IENBPreset>> GetEnbPresets()
        {
            return await Task.Run(() =>
            {
                return EnbPresets.Select(x =>
                {
                    x.Image = ServiceSingleton.Lib.ResizeKeepAspectRatio(ServiceSingleton.Lib.GetImageFromWebStream(x.ImagePath), 150, 95);
                    x.Description = ENBs.GetAvailableENBsForV6().Where(y => y.Code == x.GetFieldValueByKey("EnbCode")).FirstOrDefault().Description;
                    return x;
                }).ToList();

            });
        }

        public List<IMod> GetEnbMods(bool Installable, string EnbPreset)
        {
            return ServiceSingleton.Packages.AllMods.Where(x => x.Conditions.Any(y => y.DataToCompare == Strings.AlternateENB) && x.IsInstallable(EnbPreset) == Installable).Concat(ServiceSingleton.Packages.AllMods.Where(x => x.GetFieldValueByKey("ReinstallIfEnbChange") == "TRUE" && x.IsInstallable())).ToList();
        }        

        public async Task<List<IMod>> PrepareModsToUpdate(string CurrentENB, string NewENB)
        {
            return await Task.Run(() => {
                try
                {                    
                    var ModsToUpdate = GetEnbMods(true, NewENB).Select(x =>
                    {
                        x.Action = ElementAction.Add;
                        return x;
                    }).Concat(GetEnbMods(false, NewENB).Except(GetEnbMods(false, CurrentENB)).Select(x =>
                    {
                        x.Action = ElementAction.Remove;
                        return x;
                    })
                    ).Concat(Reshade).Except(CurrentEnbPreset(CurrentENB)).ToList();


                    return ModsToUpdate;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            });
        }

        public async Task DeleteENB(Action<string, int> Progress = null)
        {
            var Tsk = Task.Run(() =>
            {
                try
                {
                    string EnbCacheFolder = Path.Combine(ServiceSingleton.Instances.WorkingInstance.StockGame, "enbcache");
                    string EnbSeriesFolder = Path.Combine(ServiceSingleton.Instances.WorkingInstance.StockGame, "enbseries");
                    string EnbLocalIni = Path.Combine(ServiceSingleton.Instances.WorkingInstance.StockGame, "enblocal.ini");
                    string EnbSeriesIni = Path.Combine(ServiceSingleton.Instances.WorkingInstance.StockGame, "enbseries.ini");

                    if (ZlpIOHelper.DirectoryExists(EnbCacheFolder))
                    {
                        ServiceSingleton.Files.RemoveDirectory(EnbCacheFolder, true);
                    }

                    if (ZlpIOHelper.DirectoryExists(EnbSeriesFolder))
                    {
                        var Files = ServiceSingleton.Files.GetFiles(EnbSeriesFolder);

                        int Total = Files.Count;
                        int Counter = 0;

                        foreach (var File in Files)
                        {
                            if (File.Name != Strings.EnbAntialiasingFile && File.Name != Strings.EnbFrameGenFile)
                            {
                                File.Delete();
                                Progress("Preparing ENB update...", System.Convert.ToInt16(Math.Round(((double)++Counter / Total * 100))));
                            }
                        }

                        ServiceSingleton.Files.RemoveDirectory(EnbSeriesFolder, true);
                    }

                    if (ZlpIOHelper.FileExists(EnbLocalIni))
                    {
                        ZlpIOHelper.DeleteFile(EnbLocalIni);
                    }

                    if (ZlpIOHelper.FileExists(EnbSeriesIni))
                    {
                        ZlpIOHelper.DeleteFile(EnbSeriesIni);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            await Tsk;
        }
    }
}
