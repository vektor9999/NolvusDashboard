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
                        var Rules = FetchRules();
                        var Counter = 0;

                        INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                        foreach (var Rule in Rules)
                        {
                            Rule.Execute(Instance.StockGame, Path.Combine(ServiceSingleton.Folders.ExtractDirectory, ExtractSubDir), Instance.StockGame , Instance.InstallDir);
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
                        await Patcher.PatchFiles(ServiceSingleton.Instances.WorkingInstance.StockGame, DownloadingProgress, ExtractingProgress, PatchingProgress);
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
