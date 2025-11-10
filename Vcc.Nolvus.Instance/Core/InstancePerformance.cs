using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Instance.Core
{
    public class InstancePerformance : IInstancePerformance
    {
        #region Properties

        public string DownScaling { get; set; } = "FALSE";
        public string DownHeight { get; set; } = string.Empty;
        public string DownWidth { get; set; } = string.Empty;
        public string AdvancedPhysics { get; set; } = "TRUE";
        public string IniSettings { get; set; } = "2";
        public string AntiAliasing { get; set; } = "TAA";
        public string Variant { get; set; } = "Ultra";
        public string LODs { get; set; } = "Ultra";
        public string RayTracing { get; set; } = "TRUE";
        public string SREX { get; set; } = "FALSE";
        public string FrameGeneration { get; set; } = "FALSE";
        public string DownScaledResolution
        {
            get
            {
                return this.DownWidth + "x" + this.DownHeight;
            }
        }

        public string FPSStabilizer { get; set; } = "FALSE";

        #endregion

        public void Load(XmlNode Node)
        {
            AdvancedPhysics = Node["AdvancedPhysics"].InnerText.Trim();
            DownScaling = Node["DownScaling"].InnerText.Trim();
            DownHeight = Node["DownHeight"].InnerText.Trim();
            DownWidth = Node["DownWidth"].InnerText.Trim();
            IniSettings = Node["IniSettings"].InnerText.Trim();
            AntiAliasing = Node["AntiAliasing"].InnerText.Trim();
            Variant = Node["Variant"].InnerText.Trim();
            LODs = Node["LODs"].InnerText.Trim();
            RayTracing = Node["RayTracing"].InnerText.Trim();

            if (Node["FPSStabilizer"] != null)
            {
                FPSStabilizer = Node["FPSStabilizer"].InnerText.Trim();
            }

            if (Node["SREX"] != null)
            {
                SREX = Node["SREX"].InnerText.Trim();
            }

            if (Node["FrameGeneration"] != null)
            {
                FrameGeneration = Node["FrameGeneration"].InnerText.Trim();
            }

            ServiceSingleton.Logger.Log(string.Format("Instance advanced physics : {0}", AdvancedPhysics));
            ServiceSingleton.Logger.Log(string.Format("Instance downscaling : {0}", DownScaling));
            ServiceSingleton.Logger.Log(string.Format("Instance downscaling height : {0}", DownHeight));
            ServiceSingleton.Logger.Log(string.Format("Instance downscaling width : {0}", DownWidth));
            ServiceSingleton.Logger.Log(string.Format("Instance ini settings : {0}", IniSettings));
            ServiceSingleton.Logger.Log(string.Format("Instance anti aliasing : {0}", AntiAliasing));
            ServiceSingleton.Logger.Log(string.Format("Instance variant : {0}", Variant));
            ServiceSingleton.Logger.Log(string.Format("Instance lods : {0}", LODs));
            ServiceSingleton.Logger.Log(string.Format("Instance ray tracing : {0}", RayTracing));
            ServiceSingleton.Logger.Log(string.Format("Instance fps stabilizer : {0}", FPSStabilizer));
            ServiceSingleton.Logger.Log(string.Format("Instance SREX : {0}", SREX));
            ServiceSingleton.Logger.Log(string.Format("Instance FrameGeneration : {0}", FrameGeneration));
        }        

        public void Save(XmlWriter XMLWriter)
        {
            XMLWriter.WriteStartElement("Performance");

            XMLWriter.WriteStartElement("AdvancedPhysics");
            XMLWriter.WriteString(AdvancedPhysics.Trim());
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

            XMLWriter.WriteStartElement("AntiAliasing");
            XMLWriter.WriteString(AntiAliasing.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("Variant");
            XMLWriter.WriteString(Variant.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("LODs");
            XMLWriter.WriteString(LODs.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("RayTracing");
            XMLWriter.WriteString(RayTracing.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("FPSStabilizer");
            XMLWriter.WriteString(FPSStabilizer.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("SREX");
            XMLWriter.WriteString(SREX.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("FrameGeneration");
            XMLWriter.WriteString(FrameGeneration.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteEndElement();
        }
    }
}
