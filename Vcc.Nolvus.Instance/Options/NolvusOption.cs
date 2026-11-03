using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Api.Installer.Library;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Instance.Options
{
    public class NolvusOption : INolvusOption
    {
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public string Category { get; set; }
        public string Value { get; set; }
        public string DataType { get; set; }        
        public string FormattedValue {
            get
            {
                if (DataType == "bool")
                {
                    return Value == "TRUE" ? "Yes" : "No";
                }
                else
                {
                    return Value;
                }
            }
        }

        public void LoadFromServer(INolvusOptionDTO ServerOption)
        {
            Name = ServerOption.Name;
            FriendlyName = ServerOption.FriendlyName;
            Category = ServerOption.Category;
            Value = ServerOption.DefaultValue;
            DataType = ServerOption.DataType;            
        }

        public override string ToString()
        {
            return Name;
        }

        public void Load(XmlNode Node)
        {
            Name = Node["Name"].InnerText.Trim();
            FriendlyName = Node["FriendlyName"].InnerText.Trim();
            Category = Node["Category"].InnerText.Trim();
            DataType = Node["DataType"].InnerText.Trim();
            Value = Node["Value"].InnerText.Trim();            

            ServiceSingleton.Logger.Log(string.Format("Option {0} : {1}", Name, Value));
        }

        public void Save(XmlWriter XMLWriter)
        {
            XMLWriter.WriteStartElement("Option");

            XMLWriter.WriteStartElement("Name");
            XMLWriter.WriteString(Name.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("FriendlyName");
            XMLWriter.WriteString(FriendlyName.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("Category");
            XMLWriter.WriteString(Category.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("DataType");
            XMLWriter.WriteString(DataType.Trim());
            XMLWriter.WriteEndElement();            

            XMLWriter.WriteStartElement("Value");
            XMLWriter.WriteString(Value.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteEndElement();
        }
    }
}
