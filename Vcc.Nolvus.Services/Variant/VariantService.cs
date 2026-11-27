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
    public class VariantService : IVariantService
    {
        private List<IMod> GetMods(bool Installable, string Variant)
        {
            return ServiceSingleton.Packages.AllMods.Where(x => x.Conditions.Any(y => y.DataToCompare == Variant) && x.IsInstallable() == Installable).ToList();
        }

        private List<IMod> GetModsFromKey(string Value)
        {
            return ServiceSingleton.Packages.AllMods.Where(x => x.HasFieldValueByKey("ReinstallIfVariantChange", Value) && x.IsInstallable()).ToList();
        }

        public async Task<List<IMod>> PrepareModsToUpdate(string Option, string CurrentValue, string NewValue)
        {
            return await Task.Run(() =>
            {
                try
                {
                    ServiceSingleton.Logger.Log("Variant Manager is preparing mods to update...");

                    ServiceSingleton.Instances.WorkingInstance.SettingsManager.Variant = CurrentValue;

                    var ModsToInstallWithCurrentValue = GetMods(true, Option);
                    var ModsToRemoveWithCurrentValue = GetMods(false, Option);

                    ServiceSingleton.Instances.WorkingInstance.SettingsManager.Variant = NewValue;

                    var ModsToInstallWithNewValue = GetMods(true, Option);
                    var ModsToRemoveWithNewValue = GetMods(false, Option);

                    ServiceSingleton.Instances.WorkingInstance.SettingsManager.Variant = CurrentValue;

                    var ModsToInstallFromKey = GetModsFromKey("TRUE").Select(x =>
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
                    

                    var ModsToUpdate = ModsToInstall
                            .Concat(ModsToRemove)
                            .Concat(ModsToInstallFromKey.Where(x => !ModsToRemove.Any(r => r.Name == x.Name && r.Action == ElementAction.Remove)))
                            .ToList();

                    ServiceSingleton.Logger.Log(string.Format("Mods to update : {0}", ModsToUpdate.Count));
                    ServiceSingleton.Logger.Log(string.Format("About to list mods to update related to variant {0}...",  NewValue));

                    ModsToUpdate.OrderBy(x => x.Name).ToList().ForEach(x => ServiceSingleton.Logger.Log(string.Format("Mod : {0}, Action : {1}", x.Name, x.Action == ElementAction.Add ? "Add" : "Remove")));

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
