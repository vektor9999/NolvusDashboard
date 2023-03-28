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

        public override bool IsValid(bool Log)
        {
            System.Reflection.PropertyInfo PropToCompare = typeof(IInstanceOptions).GetProperty(this.DataToCompare);

            string Value;

            if (PropToCompare == null)
            {
                PropToCompare = typeof(IInstancePerformance).GetProperty(this.DataToCompare);                

                if ( PropToCompare == null)
                {
                    PropToCompare = typeof(IInstanceSettings).GetProperty(this.DataToCompare);
                    Value = (string)PropToCompare.GetValue(ServiceSingleton.Instances.WorkingInstance.Settings);
                }
                else
                {
                    Value = (string)PropToCompare.GetValue(ServiceSingleton.Instances.WorkingInstance.Performance);
                }
            }
            else
            {
                Value = (string)PropToCompare.GetValue(ServiceSingleton.Instances.WorkingInstance.Options);
            }                      

            bool Valid = false;

            switch (this.Operator)
            {
                case 0:
                    Valid = Value == this.ValueToCompare;

                    if (Log)
                    {                        
                    }
                    
                    break;
                case 1:
                    Valid = Value != this.ValueToCompare;

                    if (Log)
                    {                        
                    }

                    break;
            }            

            return Valid;
        }
    }
}
