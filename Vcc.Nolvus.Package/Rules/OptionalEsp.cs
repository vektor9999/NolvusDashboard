using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Vcc.Nolvus.Package.Rules
{
    public class OptionalEsp : Rule
    {
        public string EspName { get; set; }

        public override void Load(XmlNode Node)
        {
            base.Load(Node);
            EspName = Node["EspName"].InnerText;
        }

        public override void Execute(string GamePath, string ExtractDir, string ModDir, string InstanceDir)
        {
            if (CanExecute(GamePath, ModDir))
            {
                Directory.CreateDirectory(Path.Combine(ModDir, "optional"));

                FileInfo FileSource = new FileInfo(Path.Combine(ModDir, EspName));
                FileInfo FileDest = new FileInfo(Path.Combine(ModDir, "optional", EspName));

                FileSource.CopyTo(FileDest.FullName, true);

                FileSource.Delete();
            }
        }
    }
}
