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
        public string CDN { get; set; } = string.Empty;
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

        public XmlNode Save(XmlDocument Storage)
        {
            var SettingsNode = Storage.CreateNode("element", "Settings", "");

            XmlNode RatioNode = Storage.CreateNode("element", "Ratio", "");
            RatioNode.InnerText = Ratio.Trim();
            SettingsNode.AppendChild(RatioNode);

            XmlNode HeightNode = Storage.CreateNode("element", "Height", "");
            HeightNode.InnerText = Height.Trim();
            SettingsNode.AppendChild(HeightNode);

            XmlNode WidthNode = Storage.CreateNode("element", "Width", "");
            WidthNode.InnerText = Width.Trim();
            SettingsNode.AppendChild(WidthNode);

            XmlNode EnableArchivingNode = Storage.CreateNode("element", "EnableArchiving", "");
            EnableArchivingNode.InnerText = EnableArchiving.ToString().Trim();
            SettingsNode.AppendChild(EnableArchivingNode);

            XmlNode CDNNode = Storage.CreateNode("element", "CDN", "");
            CDNNode.InnerText = CDN.Trim();
            SettingsNode.AppendChild(CDNNode);

            XmlNode LgCodeNode = Storage.CreateNode("element", "LgCode", "");
            LgCodeNode.InnerText = LgCode.Trim();
            SettingsNode.AppendChild(LgCodeNode);

            XmlNode LgNameNode = Storage.CreateNode("element", "LgName", "");
            LgNameNode.InnerText = LgName.Trim();
            SettingsNode.AppendChild(LgNameNode);

            return SettingsNode;
        }
    }
}
