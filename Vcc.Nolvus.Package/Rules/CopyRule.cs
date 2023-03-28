using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Vcc.Nolvus.Package.Rules
{
    public abstract class CopyRule : Rule
    {
        public string Source { get; set; } = string.Empty;
        public int Destination { get; set; } = 0;
        public string DestinationDirectory { get; set; } = string.Empty;
        public bool CopyToRoot
        {
            get { return this.DestinationDirectory == string.Empty; }
        }

        public override void Load(XmlNode Node)
        {
            base.Load(Node);
            Source = Node["Source"].InnerText;
            Destination = System.Convert.ToInt16(Node["Destination"].InnerText);
            DestinationDirectory = Node["DestinationDirectory"].InnerText;
        }

        public override bool IsPriority
        {
            get
            {
                return true;
            }
        }        
    }
}
