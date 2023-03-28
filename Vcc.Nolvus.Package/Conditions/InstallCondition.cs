using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Package.Conditions
{
    public abstract class InstallCondition
    {
        public int Operator { get; set; }

        public abstract bool IsValid(bool Log);

        public virtual void Load(XmlNode Node)
        {
            Operator = System.Convert.ToInt16(Node["Operator"].InnerText);
        }
    }
}
