using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Vcc.Nolvus.Package.Conditions
{
    public class FileExistsCondition : CompareCondition
    {
        public override bool IsValid(string GamePath, string InstallDir)
        {
            throw new NotImplementedException();
        }
    }
}
