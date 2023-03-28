using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Vcc.Nolvus.Package.Rules
{
    public class FileCopy : CopyRule
    {
        public string NewFileName { get; set; }       

        public string FileName
        {
            get
            {
                return new FileInfo(Source).Name;
            }
        }

        public override void Load(XmlNode Node)
        {
            base.Load(Node);
            NewFileName = Node["NewFileName"].InnerText;
        }

        public override void Execute(string GamePath, string ExtractDir, string ModDir, string InstanceDir)
        {
            if (CanExecute(GamePath, ModDir))
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

                FileInfo FileSource = new FileInfo(Path.Combine(ExtractDir,this.Source));

                if (!CopyToRoot)
                {
                    Destination = Path.Combine(Destination, DestinationDirectory);

                    Directory.CreateDirectory(Destination);
                }

                FileInfo FileDest = new FileInfo(Path.Combine(Destination, (NewFileName != string.Empty) ? NewFileName : FileSource.Name));                
                
                FileSource.CopyTo(FileDest.FullName, true);

            }            
        }
    }
}
