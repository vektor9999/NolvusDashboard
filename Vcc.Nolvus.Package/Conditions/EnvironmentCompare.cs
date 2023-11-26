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
    public class EnvironmentCompare : CompareCondition
    {
        public override void Load(XmlNode Node)
        {
            base.Load(Node);
        }
        public override bool IsValid(string GamePath, string InstallDir)
        {            
            string Value = ServiceSingleton.Instances.GetValueFromKey(DataToCompare);

            bool Valid = false;

            switch (this.Operator)
            {
                case 0: Valid = Value == ValueToCompare;
                    break;
                case 1:
                    Valid = Value != ValueToCompare;
                    break;

            }

            return Valid;
        }
    }
}
