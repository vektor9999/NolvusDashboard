using System;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Package.Rules;

namespace Vcc.Nolvus.Package.Mods
{
    public class BSArch : NexusSoftware
    {
        protected override async Task DoCopy()
        {
            var Tsk = Task.Run(() =>
            {
                try
                {
                    try
                    {
                        var InstallDirectory = Path.Combine(ServiceSingleton.Instances.WorkingInstance.InstallDir, "TOOLS", Name);

                        Directory.CreateDirectory(InstallDirectory);

                        var Rules = new DirectoryCopy().CreateFileRules(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, ExtractSubDir), 2);

                        var Counter = 0;

                        foreach (var Rule in Rules)
                        {
                            Rule.Execute(ServiceSingleton.Instances.WorkingInstance.StockGame, Path.Combine(ServiceSingleton.Folders.ExtractDirectory, ExtractSubDir), InstallDirectory, InstallDirectory);
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
    }
}
