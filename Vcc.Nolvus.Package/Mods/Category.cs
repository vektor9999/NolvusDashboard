using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Package.Mods
{
    public class Category : MOElement, ICategory
    {
        #region Fields

        public List<Mod> Mods = new List<Mod>();

        #endregion

        #region Properties

        public override string MoDirectoryName
        {
            get { return Name + "_separator"; }
        }

        public override string ArchiveFolder
        {
            get { return string.Empty; }
        }

        #endregion

        #region Methods        

        public override void Load(XmlNode Node, List<InstallableElement> Elements)
        {            
            Name = Node["Name"].InnerText;
            Display = true;

            Elements.Add(this);

            foreach (XmlNode ModNode in Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Mods").FirstOrDefault().ChildNodes.Cast<XmlNode>().ToList())
            {
                Mod Mod = Activator.CreateInstance(Type.GetType("Vcc.Nolvus.Package.Mods." + ModNode["Type"].InnerText)) as Mod;

                Mod.Category = this;
                Mod.Load(ModNode, Elements);                

                Mods.Add(Mod);                             
            }
        }

        public override bool IsInstallable()
        {
            return true;
        }

        public override bool IsInstallable(string Value)
        {
            return true;
        }

        protected override void CreateElementIni()
        {
            File.WriteAllText(Path.Combine(MoDirectoryFullName, "meta.ini"), string.Format(MetaIni, "0", Version, string.Empty, "0"));
        }

        protected override Task DoUnpack()
        {
            return Task.CompletedTask;
        }

        protected override Task DoCopy()
        {
            return Task.CompletedTask;
        }

        protected override Task DoPatch()
        {
            return Task.CompletedTask;
        }
        
        public override async Task Install(CancellationToken Token, ModInstallSettings Settings = null)
        {
            var Tsk = Task.Run(() =>
            {
                if (!Directory.Exists(MoDirectoryFullName))
                {
                    Directory.CreateDirectory(MoDirectoryFullName);

                    this.CreateElementIni();

                    if (ServiceSingleton.Instances.WorkingInstance.Settings.EnableArchiving) Directory.CreateDirectory(Path.Combine(ServiceSingleton.Instances.WorkingInstance.ArchiveDir, Name));                    
                }
            });

            await Tsk;
        }

        public override Task Remove()
        {
            return Task.CompletedTask;
        }

        #endregion

    }
}
