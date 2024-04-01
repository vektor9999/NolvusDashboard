using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Services;
using System.Xml;
using ZetaLongPaths;

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
                string Dest = string.Empty;

                if (Destination == 0)
                {
                    Dest = ModDir;
                }
                else if (Destination == 1)
                {
                    Dest = GamePath;
                }
                else
                {
                    Dest = InstanceDir;
                }

                if (!IsDirectory)
                {
                    if (ZlpIOHelper.FileExists(Path.Combine(Dest, Source)))
                    {
                        ZlpIOHelper.DeleteFile(Path.Combine(Dest, Source));
                    }                    
                }
                else
                {
                    if (ZlpIOHelper.DirectoryExists((Path.Combine(Dest, Source))))
                    {
                        ServiceSingleton.Files.RemoveDirectory(Path.Combine(Dest, Source), true);
                    }                    
                }
            }
        }
    }
}
