using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Vcc.Nolvus.Core.Services;
using ZetaLongPaths;

namespace Vcc.Nolvus.Package.Rules
{
    public class FileCopy : CopyRule
    {
        public string NewFileName { get; set; }       

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

                ZlpFileInfo FileSource = new ZlpFileInfo(Path.Combine(ExtractDir, Source));

                if (!CopyToRoot)
                {
                    Destination = Path.Combine(Destination, DestinationDirectory);

                    ZlpIOHelper.CreateDirectory(Destination);                    
                }

                ZlpFileInfo FileDest = new ZlpFileInfo(Path.Combine(Destination, (NewFileName != string.Empty) ? NewFileName : FileSource.Name));

                FileSource.CopyTo(FileDest.FullName, true);                


            }
        }
    }
}
