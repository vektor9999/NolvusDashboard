using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Instance.Settings
{
    public class NolvusSettingsManager : INolvusSettingsManager
    {
        #region Properties

        public string InstallDir { get; set; } = string.Empty;
        public string ArchiveDir { get; set; } = string.Empty;
        public string StockGame { get; set; } = string.Empty;
        public string Variant { get; set; } = "Ultra";
        public string Height { get; set; } = string.Empty;
        public string Width { get; set; } = string.Empty;
        public string DownScaling { get; set; } = "FALSE";
        public string DownHeight { get; set; } = string.Empty;
        public string DownWidth { get; set; } = string.Empty;
        public string IniSettings { get; set; } = "2";
        public bool EnableArchiving { get; set; }
        public string CDN { get; set; } = "Nexus CDN";
        public string LgCode { get; set; } = "EN";
        public string LgName { get; set; } = "English";                   
        public string GameDataDir
        {
            get
            {
                return Path.Combine(StockGame, "Data");
            }
        }
        public string GameDataDirSl
        {
            get
            {
                return Path.Combine(StockGame, "Data\\");
            }
        }
        public string DownScaledResolution
        {
            get
            {
                return this.DownWidth + "x" + this.DownHeight;
            }
        }
        public string GetSelectedResolution()
        {
            return string.Format("{0}x{1}", GetSelectedWidth(), GetSelectedHeight());
        }
        public string GetSelectedHeight()
        {
            if (DownScaling == "TRUE")
            {
                return DownHeight;
            }
            else
            {
                return Height;
            }
        }
        public string GetSelectedWidth()
        {
            if (DownScaling == "TRUE")
            {
                return DownWidth;
            }
            else
            {
                return Width;
            }
        }

        #endregion

        public NolvusSettingsManager()
        {
            EnableArchiving = true;            
        }

        #region Methods

        public void Load(XmlNode Node)
        {
            InstallDir = Node["InstallPath"].InnerText.Trim();
            ArchiveDir = Node["ArchivePath"].InnerText.Trim();
            StockGame = Node["StockGame"].InnerText.Trim();
            Variant = Node["Variant"].InnerText.Trim();
            Height = Node["Height"].InnerText.Trim();
            Width = Node["Width"].InnerText.Trim();
            DownScaling = Node["DownScaling"].InnerText.Trim();
            DownHeight = Node["DownHeight"].InnerText.Trim();
            DownWidth = Node["DownWidth"].InnerText.Trim();
            IniSettings = Node["IniSettings"].InnerText.Trim();
            EnableArchiving = System.Convert.ToBoolean(Node["EnableArchiving"].InnerText.Trim());
            CDN = Node["CDN"].InnerText.Trim();
            LgCode = Node["LgCode"].InnerText.Trim();
            LgName = Node["LgName"].InnerText.Trim();            

            ServiceSingleton.Logger.Log(string.Format("Instance Install Directory : {0}", InstallDir));
            ServiceSingleton.Logger.Log(string.Format("Instance Archive Directory : {0}", ArchiveDir));
            ServiceSingleton.Logger.Log(string.Format("Instance Stock Game Directory : {0}", StockGame));
            ServiceSingleton.Logger.Log(string.Format("Instance Variant : {0}", Variant));
            ServiceSingleton.Logger.Log(string.Format("Instance Height : {0}", Height));
            ServiceSingleton.Logger.Log(string.Format("Instance Width : {0}", Width));
            ServiceSingleton.Logger.Log(string.Format("Instance Downscaling : {0}", DownScaling));
            ServiceSingleton.Logger.Log(string.Format("Instance Downscaling Height : {0}", DownHeight));
            ServiceSingleton.Logger.Log(string.Format("Instance Downscaling Width : {0}", DownWidth));
            ServiceSingleton.Logger.Log(string.Format("Instance Ini Settings : {0}", IniSettings));
            ServiceSingleton.Logger.Log(string.Format("Instance Enable Archiving : {0}", EnableArchiving));
            ServiceSingleton.Logger.Log(string.Format("Instance CDN : {0}", CDN));
            ServiceSingleton.Logger.Log(string.Format("Instance LgCode : {0}", LgCode));
            ServiceSingleton.Logger.Log(string.Format("Instance LgName : {0}", LgName));            
        }        

        public void Save(XmlWriter XMLWriter)
        {
            XMLWriter.WriteStartElement("Settings");

            XMLWriter.WriteStartElement("InstallPath");
            XMLWriter.WriteString(InstallDir.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("ArchivePath");
            XMLWriter.WriteString(ArchiveDir.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("StockGame");
            XMLWriter.WriteString(StockGame.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("Variant");
            XMLWriter.WriteString(Variant.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("Height");
            XMLWriter.WriteString(Height.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("Width");
            XMLWriter.WriteString(Width.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("DownScaling");
            XMLWriter.WriteString(DownScaling.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("DownHeight");
            XMLWriter.WriteString(DownHeight.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("DownWidth");
            XMLWriter.WriteString(DownWidth.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("IniSettings");
            XMLWriter.WriteString(IniSettings.Trim());
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

        #endregion
    }
}
