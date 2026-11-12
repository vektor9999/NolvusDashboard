using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Package.Rules;


namespace Vcc.Nolvus.Package.Mods
{
    public class xEdit : NexusSoftware
    {        
        protected override async Task DoCopy()
        {
            var Tsk = Task.Run(async () =>
            {
                try
                {
                    try
                    {
                        var Instance = ServiceSingleton.Instances.WorkingInstance;

                        var InstallDirectory = Path.Combine(Instance.SettingsManager.InstallDir, "TOOLS", Name);
                        var CacheDir = Path.Combine(InstallDirectory, "Cache");                        

                        Directory.CreateDirectory(InstallDirectory);
                        Directory.CreateDirectory(CacheDir);
                        
                        var DirectoryRule = (Rules.First() as DirectoryCopy);
                        var FileRules = DirectoryRule.CreateFileRules(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, ExtractSubDir), DirectoryRule.Destination, string.Empty, string.Empty);                        

                        var Counter = 0;

                        foreach (var Rule in Rules)
                        {
                            Rule.Execute(Instance.SettingsManager.StockGame, Path.Combine(ServiceSingleton.Folders.ExtractDirectory, ExtractSubDir), Instance.SettingsManager.InstallDir, Instance.SettingsManager.InstallDir);
                            CopyingProgress(++Counter, Rules.Count);
                        }                        
                    }
                    finally
                    {
                        await ServiceSingleton.Files.RemoveDirectory(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, ExtractSubDir), true, CleaningProgress);
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
