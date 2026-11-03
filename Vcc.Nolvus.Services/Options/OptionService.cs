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

namespace Vcc.Nolvus.Services.Options
{
    public class OptionService : IOptionService
    {
        private List<IMod> GetMods(bool Installable, string Option)
        {
            return ServiceSingleton.Packages.AllMods.Where(x => x.Conditions.Any(y => y.DataToCompare == Option) && x.IsInstallable() == Installable).ToList();
        }

        private List<IMod> GetModsFromKey(string Option)
        {
            return ServiceSingleton.Packages.AllMods.Where(x => x.HasFieldValueByKey("ReinstallIfOptionChange", Option) && x.IsInstallable()).ToList();
        }

        public async Task<List<IMod>> PrepareModsToUpdate(string Option, string CurrentValue, string NewValue)
        {
            return await Task.Run(() =>
            {
                try
                {
                    ServiceSingleton.Logger.Log("Option Manager is preparing mods to update...");

                    ServiceSingleton.Instances.WorkingInstance.OptionsManager.SetOptionValue(Option, CurrentValue);

                    var ModsToInstallWithCurrentValue = GetMods(true, Option);
                    var ModsToRemoveWithCurrentValue = GetMods(false, Option);

                    ServiceSingleton.Instances.WorkingInstance.OptionsManager.SetOptionValue(Option, NewValue);

                    var ModsToInstallWithNewValue = GetMods(true, Option);                                      
                    var ModsToRemoveWithNewValue = GetMods(false, Option);

                    ServiceSingleton.Instances.WorkingInstance.OptionsManager.SetOptionValue(Option, CurrentValue);

                    var ModsToInstallFromKey = GetModsFromKey(Option).Select(x =>
                    {
                        x.Action = ElementAction.Add;
                        return x;
                    });

                    var ModsToInstall = ModsToInstallWithNewValue.Except(ModsToInstallWithCurrentValue).Select(x =>
                    {
                        x.Action = ElementAction.Add;
                        return x;
                    });

                    var ModsToRemove = ModsToRemoveWithNewValue.Except(ModsToRemoveWithCurrentValue).Select(x =>
                    {
                        x.Action = ElementAction.Remove;
                        return x;
                    });


                    var ModsToUpdate = ModsToInstall.Concat(ModsToRemove).Concat(ModsToInstallFromKey).ToList();                  

                    ServiceSingleton.Logger.Log(string.Format("Mods to update : {0}", ModsToUpdate.Count));
                    ServiceSingleton.Logger.Log(string.Format("About to list mods to update related to option {0}...", Option));

                    foreach (var Mod in ModsToUpdate.OrderBy(x=>x.Name).ToList())
                    {
                        ServiceSingleton.Logger.Log(string.Format("Mod : {0}, Action : {1}", Mod.Name, Mod.Action == ElementAction.Add ? "Add" : "Remove"));
                    }

                    return ModsToUpdate;                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }
    }
}
