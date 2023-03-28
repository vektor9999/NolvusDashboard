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

        public override void Load(XmlNode Node)
        {
            base.Load(Node);
            Source = Node["Source"].InnerText;
            IsDirectory = System.Convert.ToBoolean(Node["IsDirectory"].InnerText);
        }

        public override void Execute(string GamePath, string ExtractDir, string ModDir, string InstanceDir)
        {
            if (this.CanExecute(GamePath, ModDir))
            {
                if (!IsDirectory)
                {
                    File.Delete(Path.Combine(ModDir, Source));
                }
                else
                {
                    ServiceSingleton.Files.RemoveDirectory(Path.Combine(ModDir, Source), false);
                }
            }
        }
    }
}
