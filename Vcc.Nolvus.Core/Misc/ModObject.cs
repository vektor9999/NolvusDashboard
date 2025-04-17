using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Misc
{
    public enum ModObjectStatus { OK, NotInstalled, Error, VersionMisMatch, CustomInstalled, InstalledIniMissing, MetaIniError, Disabled }

    internal class NameComparer : EqualityComparer<ModObject>
    {
        public override bool Equals(ModObject Obj1, ModObject Obj2)
        {
            if (Object.ReferenceEquals(Obj1, Obj2)) return true;
            if (Object.ReferenceEquals(Obj1, null) || Object.ReferenceEquals(Obj2, null))
                return false;
            
            return String.Equals(Obj1.Name, Obj2.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode(ModObject Obj)
        {
            if (Object.ReferenceEquals(Obj, null)) return 0;            
            int HashName = Obj.Name == null ? 0 : Obj.Name.ToLower().GetHashCode();
            return HashName;
        }
    }

    internal class VersionComparer : EqualityComparer<ModObject>
    {
        public override bool Equals(ModObject Obj1, ModObject Obj2)
        {
            if (Object.ReferenceEquals(Obj1, Obj2)) return true;
            if (Object.ReferenceEquals(Obj1, null) || Object.ReferenceEquals(Obj2, null))
                return false;

            return String.Equals(Obj1.Name, Obj2.Name, StringComparison.OrdinalIgnoreCase) && Obj1.Version == Obj2.Version && Obj1.Selected == Obj2.Selected;
        }

        public override int GetHashCode(ModObject Obj)
        {
            if (Object.ReferenceEquals(Obj, null)) return 0;
            int HashName = Obj.Version == null ? 0 : Obj.Name.ToLower().GetHashCode() ^ Obj.Version.GetHashCode() ^ Obj.Selected.GetHashCode();
            return HashName;


        }


    }

    public class ModObject
    {
        public bool Selected { get; set; }
        public int Priority { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Category { get; set; }
        public string StatusText { get; set; }
        public ModObjectStatus Status { get; set; }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;
            var Element = (ModObject)obj;
            return Element.Name == Name;
        }

        public override int GetHashCode() => new { Name }.GetHashCode();
        public override string ToString()
        {
            return string.Format("[{0}] {1}", Category, Name);
        }
    }

    public class ModObjectList
    {
        private List<ModObject> _List = new List<ModObject>();
        public string Profile { get; set; }
        public int AddedModsCount
        {
            get { return _List.Where(x => x.Status == ModObjectStatus.CustomInstalled).ToList().Count; }
        }

        public int RemovedModsCount
        {
            get { return _List.Where(x => x.Status == ModObjectStatus.NotInstalled).ToList().Count; }
        }

        public int VersionMismatchCount
        {
            get { return _List.Where(x => x.Status == ModObjectStatus.VersionMisMatch).ToList().Count; }
        }

        public int IniParsingErrorCount
        {
            get { return _List.Where(x => x.Status == ModObjectStatus.MetaIniError).ToList().Count; }
        }

        public int InstalledIniMissingCount
        {
            get { return _List.Where(x => x.Status == ModObjectStatus.InstalledIniMissing).ToList().Count; }
        }

        public int DisabledModsCount
        {
            get { return _List.Where(x => x.Status == ModObjectStatus.Disabled).ToList().Count; }
        }

        public List<ModObject> AddedMods
        {
            get { return _List.Where(x => x.Status == ModObjectStatus.CustomInstalled).ToList(); }
        }

        public List<ModObject> RemovedMods
        {
            get { return _List.Where(x => x.Status == ModObjectStatus.NotInstalled).ToList(); }
        }

        public List<ModObject> VersionMismatchMods
        {
            get { return _List.Where(x => x.Status == ModObjectStatus.VersionMisMatch).ToList(); }
        }

        public List<ModObject> IniParsingErrorMods
        {
            get { return _List.Where(x => x.Status == ModObjectStatus.MetaIniError).ToList(); }
        }

        public List<ModObject> InstalledIniMissingMods
        {
            get { return _List.Where(x => x.Status == ModObjectStatus.InstalledIniMissing).ToList(); }
        }

        public List<ModObject> DisabledMods
        {
            get { return _List.Where(x => x.Status == ModObjectStatus.Disabled).ToList(); }
        }

        public bool HasMods
        {
            get
            {
                return _List.Count > 0;
            }
        }

        public List<ModObject> List
        {
            get
            {
                return _List;
            }
        }

        public ModObjectList Merge(List<ModObject> Mo2List, List<ModObject> NolvusList)
        {            
            _List = Mo2List.Select(Mo2Mod => {
                var NolvusMod = NolvusList.Where(x => x.Name.ToLower() == Mo2Mod.Name.ToLower()).FirstOrDefault();

                if (NolvusMod == null)
                {
                    Mo2Mod.Status = ModObjectStatus.CustomInstalled;
                    Mo2Mod.StatusText = "Not from Nolvus";
                }
                else
                {
                    if (!Mo2Mod.Selected)
                    {
                        Mo2Mod.Status = ModObjectStatus.Disabled;
                        Mo2Mod.StatusText = "Mod is disabled";
                    }
                    else if (Mo2Mod.Version != NolvusMod.Version)
                    {
                        if (Mo2Mod.Status != ModObjectStatus.MetaIniError && Mo2Mod.Status != ModObjectStatus.InstalledIniMissing)
                        {
                            Mo2Mod.Status = ModObjectStatus.VersionMisMatch;
                            Mo2Mod.StatusText = string.Format("Expected version : {0}", NolvusMod.Version);
                        }
                        
                    }
                }

                return Mo2Mod;
            })
            .ToList().Concat(NolvusList.Except(Mo2List, new NameComparer()).ToList().Select(x =>
            {
                x.Status = ModObjectStatus.NotInstalled;
                x.StatusText = "Not installed";
                x.Category = "UNINSTALLED NOLVUS MODS";

                return x;
            })).ToList();

            return this;
        }
    }
}
