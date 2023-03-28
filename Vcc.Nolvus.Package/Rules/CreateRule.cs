using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Vcc.Nolvus.Package.Rules
{
    public class CreateRule : Rule
    {
        public string Name { get; set; }
        public int Source { get; set; }

        public override void Load(XmlNode Node)
        {
            base.Load(Node);
            Name = Node["Name"].InnerText;
            Source = System.Convert.ToInt16(Node["Source"].InnerText);
        }

        public override void Execute(string GamePath, string ExtractDir, string ModDir, string InstanceDir)
        {
            throw new NotImplementedException();
        }
    }
}
