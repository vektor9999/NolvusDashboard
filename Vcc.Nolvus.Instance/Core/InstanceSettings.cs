using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Instance.Core
{
    public class InstanceSettings : IInstanceSettings
    {
        public string Ratio { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Width { get; set; } = string.Empty;
        public bool EnableArchiving { get; set; }
        public string CDN { get; set; } = "Nexus CDN";
        public string LgCode { get; set; } = "EN";
        public string LgName { get; set; } = "English";
        public string GameDataDir
        {
            get
            {
                return Path.Combine(ServiceSingleton.Instances.WorkingInstance.StockGame, "Data");
            }
        }

        public string GameDataDirSl
        {
            get
            {
                return Path.Combine(ServiceSingleton.Instances.WorkingInstance.StockGame, "Data\\");
            }
        }

        public InstanceSettings()
        {
            EnableArchiving = true;
        }

        public void Load(XmlNode Node)
        {
            Ratio = Node["Ratio"].InnerText.Trim();
            Height = Node["Height"].InnerText.Trim();
            Width = Node["Width"].InnerText.Trim();
            EnableArchiving = System.Convert.ToBoolean(Node["EnableArchiving"].InnerText.Trim());
            CDN = Node["CDN"].InnerText.Trim();
            LgCode = Node["LgCode"].InnerText.Trim();
            LgName = Node["LgName"].InnerText.Trim();

            ServiceSingleton.Logger.Log(string.Format("Instance ratio : {0}", Ratio));
            ServiceSingleton.Logger.Log(string.Format("Instance height : {0}", Height));
            ServiceSingleton.Logger.Log(string.Format("Instance width : {0}", Width));
            ServiceSingleton.Logger.Log(string.Format("Instance enable archiving : {0}", EnableArchiving));
            ServiceSingleton.Logger.Log(string.Format("Instance CDN : {0}", CDN));
            ServiceSingleton.Logger.Log(string.Format("Instance LgCode : {0}", LgCode));
            ServiceSingleton.Logger.Log(string.Format("Instance LgName : {0}", LgName));
        }        

        public void Save(XmlWriter XMLWriter)
        {
            XMLWriter.WriteStartElement("Settings");

            XMLWriter.WriteStartElement("Ratio");
            XMLWriter.WriteString(Ratio.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("Height");
            XMLWriter.WriteString(Height.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("Width");
            XMLWriter.WriteString(Width.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("EnableArchiving");
            XMLWriter.WriteString(EnableArchiving.ToString().Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("CDN");
            XMLWriter.WriteString(CDN.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("LgCode");
            XMLWriter.WriteString(LgCode.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("LgName");
            XMLWriter.WriteString(LgName.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteEndElement();
        }
    }
}
