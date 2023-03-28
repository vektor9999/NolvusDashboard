using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Package.Conditions
{
    public abstract class RuleCondition
    {
        public abstract bool IsValid(string GamePath, string InstallDir);

        public abstract void Load(XmlNode Node);
    }
}
