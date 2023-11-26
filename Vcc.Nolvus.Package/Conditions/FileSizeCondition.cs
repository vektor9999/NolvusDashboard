using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Vcc.Nolvus.Package.Conditions
{
    public class FileSizeCondition : CompareCondition
    {
        public int Source { get; set; }

        public override void Load(XmlNode Node)
        {
            base.Load(Node);
            Source = System.Convert.ToInt16(Node["Source"].InnerText);
        }

        public override bool IsValid(string GamePath, string InstallDir)
        {
            bool Valid = false;

            string SourceDir = string.Empty;

            if (Source == 1)
            {
                SourceDir = GamePath;
            }
            else
            {
                SourceDir = InstallDir;
            }

            FileInfo File = new FileInfo(SourceDir + "\\" + DataToCompare);

            long Size = File.Length / 1024;

            switch (this.Operator)
            {
                case 0:
                    Valid =  Size == System.Convert.ToInt32(ValueToCompare);
                    break;
                case 1:
                    Valid = Size != System.Convert.ToInt32(ValueToCompare);
                    break;
                case 2:
                    Valid = Size > System.Convert.ToInt32(ValueToCompare);
                    break;
                case 3:
                    Valid = Size < System.Convert.ToInt32(ValueToCompare);
                    break;

            }

            return Valid;
        }
    }
}
