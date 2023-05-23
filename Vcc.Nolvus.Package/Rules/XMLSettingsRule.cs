using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Package.Rules
{    
    public class XMLSettingsRule : Rule
    {
        public string FileName { get; set; }
        public string Key { get; set; }
        public bool Variable { get; set; }
        public string Value { get; set; }

        public override void Load(XmlNode Node)
        {
            base.Load(Node);
            FileName = Node["FileName"].InnerText;
            Key = Node["Key"].InnerText;
            Variable = System.Convert.ToBoolean(Node["Variable"].InnerText);
            Value = Node["Value"].InnerText;
        }

        public override void Execute(string GamePath, string ExtractDir, string ModDir, string InstanceDir)
        {
            if (this.CanExecute(GamePath, ModDir))
            {               
                string EnvValue = Value;

                if (Variable)
                {
                    EnvValue = ServiceSingleton.Instances.GetValueFromKey(Value);
                }

                XmlDocument XMLFile = new XmlDocument();
                XMLFile.Load(Path.Combine(ModDir, FileName));

                XmlNode KeyNode = XMLFile.SelectSingleNode(Key);

                KeyNode.InnerText = EnvValue;

                XMLFile.Save(Path.Combine(ModDir, FileName));              
            }
        }
    }
}
