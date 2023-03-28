using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Vcc.Nolvus.Package.Conditions
{
    public abstract class CompareCondition : RuleCondition
    {
        public string DataToCompare { get; set; }
        public int Operator { get; set; }
        public string ValueToCompare { get; set; }

        public override void Load(XmlNode Node)
        {            
            DataToCompare = Node["DataToCompare"].InnerText;
            Operator = System.Convert.ToInt16(Node["Operator"].InnerText);
            ValueToCompare = Node["ValueToCompare"].InnerText;
        }        
    }
}
