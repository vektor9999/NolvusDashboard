using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Services;
using System.Xml;

namespace Vcc.Nolvus.Package.Rules
{
    public class DeleteRule : Rule
    {
        public string Source { get; set; }
        public bool IsDirectory { get; set; }
        public int Destination { get; set; } = 0;
        public override void Load(XmlNode Node)
        {
            base.Load(Node);
            Source = Node["Source"].InnerText;
            IsDirectory = System.Convert.ToBoolean(Node["IsDirectory"].InnerText);

            Destination = 0;

            if (Node["Destination"] != null)
            {
                Destination = System.Convert.ToInt16(Node["Destination"].InnerText);
            }
            
        }

        public override void Execute(string GamePath, string ExtractDir, string ModDir, string InstanceDir)
        {
            if (this.CanExecute(GamePath, ModDir))
            {
                string Destination = string.Empty;

                if (this.Destination == 0)
                {
                    Destination = ModDir;
                }
                else if (this.Destination == 1)
                {
                    Destination = GamePath;
                }
                else
                {
                    Destination = InstanceDir;
                }

                if (!IsDirectory)
                {
                    if (File.Exists(Path.Combine(Destination, Source)))
                    {
                        File.Delete(Path.Combine(Destination, Source));
                    }                    
                }
                else
                {
                    if (Directory.Exists((Path.Combine(Destination, Source))))
                    {
                        ServiceSingleton.Files.RemoveDirectory(Path.Combine(Destination, Source), true);
                    }                    
                }
            }
        }
    }
}
