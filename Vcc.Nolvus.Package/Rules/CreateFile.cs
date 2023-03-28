using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Vcc.Nolvus.Package.Rules
{
    public class CreateFile : CreateRule
    {
        public string DefaultText { get; set; }

        public override void Load(XmlNode Node)
        {
            base.Load(Node);
            if (Node["DefaultText"] != null) DefaultText = Node["DefaultText"].InnerText;            
        }

        public override void Execute(string GamePath, string ExtractDir, string ModDir, string InstanceDir)
        {
            if (CanExecute(GamePath, ModDir))
            {                
                if (DefaultText != string.Empty)
                {
                    if ( Source == 0)
                    {
                        File.WriteAllText(Path.Combine(ModDir, Name), DefaultText);
                    }
                    else
                    {
                        File.WriteAllText(Path.Combine(GamePath, Name), DefaultText);
                    }                    
                }
                else
                {
                    if (this.Source == 0)
                    {
                        File.Create(Path.Combine(ModDir, Name)).Dispose();
                    }
                    else
                    {
                        File.Create(Path.Combine(GamePath, Name)).Dispose();
                    }
                }
            }                     
        }
    }
}
