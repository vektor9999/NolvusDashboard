using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Package.Conditions
{
    public class EnvironmentCondition : InstallCondition
    {
        public string DataToCompare { get; set; }        
        public string ValueToCompare { get; set; }

        public override void Load(XmlNode Node)
        {
            base.Load(Node);
            DataToCompare = Node["DataToCompare"].InnerText;
            ValueToCompare = Node["ValueToCompare"].InnerText;
        }

        public override bool IsValid()
        {            
            string Value = ServiceSingleton.Instances.GetValueFromKey(DataToCompare);              

            bool Valid = false;

            switch (this.Operator)
            {
                case 0:
                    Valid = Value == ValueToCompare;                    
                    
                    break;
                case 1:
                    Valid = Value != ValueToCompare;                   

                    break;
            }            

            return Valid;
        }
    }
}
