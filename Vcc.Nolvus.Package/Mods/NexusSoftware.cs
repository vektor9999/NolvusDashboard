using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Package.Mods
{
    public abstract class NexusSoftware : Software
    {
        public string Domain { get; set; }

        public override void Load(XmlNode Node, List<InstallableElement> Elements)
        {
            base.Load(Node, Elements);
            Domain = Node["Domain"].InnerText;
        }
    }
}
