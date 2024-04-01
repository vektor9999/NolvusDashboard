using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using ZetaLongPaths;

namespace Vcc.Nolvus.Package.Rules
{
    public class RenameRule : Rule
    {
        public string OldName { get; set; }
        public string NewName { get; set; }
        public int Source { get; set; }
        public bool IsDirectory { get; set; }

        public override void Load(XmlNode Node)
        {
            base.Load(Node);
            OldName = Node["OldName"].InnerText;
            NewName = Node["NewName"].InnerText;
            Source = System.Convert.ToInt16(Node["Source"].InnerText);
            IsDirectory = System.Convert.ToBoolean(Node["IsDirectory"].InnerText);
        }

        public override void Execute(string GamePath, string ExtractDir, string ModDir, string InstanceDir)
        {
            if (CanExecute(GamePath, ModDir))
            {
                string Src;
                string Destination;

                if (Source == 0)
                {
                    Src = Path.Combine(ModDir, OldName);
                    Destination = Path.Combine(ModDir, NewName);
                }
                else
                {
                    Src = Path.Combine(GamePath, OldName);
                    Destination = Path.Combine(GamePath, NewName);
                }

                if (!IsDirectory)
                {
                    if (ZlpIOHelper.FileExists(Destination))
                    {
                        ZlpIOHelper.DeleteFile(Destination);
                    }

                    ZlpIOHelper.MoveFile(Src, Destination);
                }
                else
                {
                    ZlpIOHelper.MoveDirectory(Src, Destination);
                }
            }            
        }
    }
}
