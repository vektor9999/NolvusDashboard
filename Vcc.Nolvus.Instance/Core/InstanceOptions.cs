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
        public string AlternateStart { get; set; } = "TRUE";

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
            ServiceSingleton.Logger.Log(string.Format("Instance hardcore mode : {0}", HardcoreMode));
            ServiceSingleton.Logger.Log(string.Format("Instance alternate levelling : {0}", AlternateLeveling));
            ServiceSingleton.Logger.Log(string.Format("Instance alternate start : {0}", AlternateStart));
            ServiceSingleton.Logger.Log(string.Format("Instance skin type : {0}", SkinType));
        }

        public XmlNode Save(XmlDocument Storage)
        {
            var OptionsNode = Storage.CreateNode("element", "Options", "");

            XmlNode NudityNode = Storage.CreateNode("element", "Nudity", "");
            NudityNode.InnerText = Nudity.Trim();
            OptionsNode.AppendChild(NudityNode);

            XmlNode AlternateENBNode = Storage.CreateNode("element", "AlternateENB", "");
            AlternateENBNode.InnerText = AlternateENB.Trim();
            OptionsNode.AppendChild(AlternateENBNode);

            XmlNode FantasyModeNode = Storage.CreateNode("element", "FantasyMode", "");
            FantasyModeNode.InnerText = FantasyMode.Trim();
            OptionsNode.AppendChild(FantasyModeNode);

            XmlNode HardcoreModeNode = Storage.CreateNode("element", "HardcoreMode", "");
            HardcoreModeNode.InnerText = HardcoreMode.Trim();
            OptionsNode.AppendChild(HardcoreModeNode);

            XmlNode AlternateLevelingNode = Storage.CreateNode("element", "AlternateLeveling", "");
            AlternateLevelingNode.InnerText = AlternateLeveling.Trim();
            OptionsNode.AppendChild(AlternateLevelingNode);

            XmlNode SkinTypeNode = Storage.CreateNode("element", "SkinType", "");
            SkinTypeNode.InnerText = SkinType.Trim();
            OptionsNode.AppendChild(SkinTypeNode);

            XmlNode AlternateStartNode = Storage.CreateNode("element", "AlternateStart", "");
            AlternateStartNode.InnerText = AlternateStart.Trim();
            OptionsNode.AppendChild(AlternateStartNode);

            return OptionsNode;
        }
    }

    
}
