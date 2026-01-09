using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;

namespace Vcc.Nolvus.Instance.Core
{
    public class InstanceStatusField : IInstanceStatusField
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;        
    }
}
