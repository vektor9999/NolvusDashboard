using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface INolvusOption
    {
        string Name { get; }
        string FriendlyName { get; }
        string Category { get; }
        string Value { get; set; }
        string DataType { get; }        
        string FormattedValue { get; }
        void Load(XmlNode Node);
        void Save(XmlWriter XMLWriter);
    }
}
