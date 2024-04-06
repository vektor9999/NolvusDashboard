using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Instance.Core
{
    public class InstanceOptions : IInstanceOptions
    {
        public string Nudity { get; set; } = "FALSE";
        public string AlternateENB { get; set; } = "FALSE";
        public string FantasyMode { get; set; } = "FALSE";
        public string HardcoreMode { get; set; } = "FALSE";
        public string AlternateLeveling { get; set; } = "FALSE";
        public string SkinType { get; set; } = "Smooth";
        public string AlternateStart { get; set; } = "FALSE";

        public void Load(XmlNode Node)
        {
            Nudity = Node["Nudity"].InnerText.Trim();
            AlternateENB = Node["AlternateENB"].InnerText.Trim();

            if (AlternateENB == "FALSE")
            {
                AlternateENB = "PICHO";
            }

            FantasyMode = Node["FantasyMode"].InnerText.Trim();
            HardcoreMode = Node["HardcoreMode"].InnerText.Trim();
            AlternateLeveling = Node["AlternateLeveling"].InnerText.Trim();

            if (Node["AlternateStart"] != null)
            {
                AlternateStart = Node["AlternateStart"].InnerText.Trim();
            }

            SkinType = Node["SkinType"].InnerText.Trim();

            ServiceSingleton.Logger.Log(string.Format("Instance nudity : {0}", Nudity));
            ServiceSingleton.Logger.Log(string.Format("Instance enb : {0}", AlternateENB));
            ServiceSingleton.Logger.Log(string.Format("Instance fantasy mode : {0}", FantasyMode));
            ServiceSingleton.Logger.Log(string.Format("Instance hardcore mode : {0}", HardcoreMode));
            ServiceSingleton.Logger.Log(string.Format("Instance alternate levelling : {0}", AlternateLeveling));
            ServiceSingleton.Logger.Log(string.Format("Instance alternate start : {0}", AlternateStart));
            ServiceSingleton.Logger.Log(string.Format("Instance skin type : {0}", SkinType));
        }      

        public void Save(XmlWriter XMLWriter)
        {
            XMLWriter.WriteStartElement("Options");

            XMLWriter.WriteStartElement("Nudity");
            XMLWriter.WriteString(Nudity.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("AlternateENB");
            XMLWriter.WriteString(AlternateENB.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("FantasyMode");
            XMLWriter.WriteString(FantasyMode.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("HardcoreMode");
            XMLWriter.WriteString(HardcoreMode.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("AlternateLeveling");
            XMLWriter.WriteString(AlternateLeveling.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("SkinType");
            XMLWriter.WriteString(SkinType.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("AlternateStart");
            XMLWriter.WriteString(AlternateStart.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteEndElement();
        }

    }
    
}
