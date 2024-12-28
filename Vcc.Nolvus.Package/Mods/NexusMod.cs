using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Vcc.Nolvus.Package.Files;
using System.Xml;

namespace Vcc.Nolvus.Package.Mods
{
    public class NexusMod : Mod
    {
        public string NexusId { get; set; }
        public string Domain { get; set; }

        public override void Load(XmlNode Node, List<InstallableElement> Elements)
        {
            base.Load(Node, Elements);
            NexusId = Node["NexusId"].InnerText;
            Domain = Node["Domain"].InnerText;
        }

        protected override void CreateElementIni()
        {
            if (Display)
            {                
                File.WriteAllText(Path.Combine(MoDirectoryFullName, "meta.ini"), string.Format(MetaIni, NexusId, Version, GetInstallFileName().Replace("\\", "/"), (Files.First() as NexusModFile).NexusId));
            }            
        }        
    }
}
