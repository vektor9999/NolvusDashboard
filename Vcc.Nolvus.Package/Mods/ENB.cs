using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;


namespace Vcc.Nolvus.Package.Mods
{
    public class ENB : Mod
    {
        protected override async Task DoCopy()
        {
            var Tsk = Task.Run(() =>
            {
                try
                {
                    try
                    {
                        INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                        ServiceSingleton.Logger.Log(string.Format("Installing ENB binaries", Name));

                        var ENBCacheDir = Path.Combine(Instance.StockGame, "enbcache");

                        if (Directory.Exists(ENBCacheDir))
                        {
                            ServiceSingleton.Files.RemoveDirectory(ENBCacheDir, true);
                        }

                        var Rules = FetchRules();
                        var Counter = 0;                        

                        foreach (var Rule in Rules)
                        {
                            Rule.Execute(Instance.StockGame, 
                                         Path.Combine(ServiceSingleton.Folders.ExtractDirectory, ExtractSubDir), 
                                         Instance.StockGame, 
                                         Instance.InstallDir);

                            CopyingProgress(++Counter, Rules.Count);
                        }                        
                    }
                    finally
                    {
                        ServiceSingleton.Files.RemoveDirectory(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, ExtractSubDir), true);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            await Tsk;
        }

        protected override async Task DoPatch()
        {
            var Tsk = Task.Run(async () =>
            {
                try
                {
                    if (Patcher != null)
                    {
                        await Patcher.PatchFiles(ServiceSingleton.Instances.WorkingInstance.InstallDir, ServiceSingleton.Instances.WorkingInstance.StockGame, DownloadingProgress, ExtractingProgress, PatchingProgress);
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
