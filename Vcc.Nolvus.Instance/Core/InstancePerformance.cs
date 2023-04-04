using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;

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
        public string AntiAliasing { get; set; } = "DLAA";
        public string Variant { get; set; } = "Ultra";
        public string LODs { get; set; } = "Ultra";
        public string RayTracing { get; set; } = "TRUE";
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
        }
        public XmlNode Save(XmlDocument Storage)
        {
            var PerformanceNode = Storage.CreateNode("element", "Performance", "");                                 

            XmlNode AdvancedPhysicsNode = Storage.CreateNode("element", "AdvancedPhysics", "");
            AdvancedPhysicsNode.InnerText = AdvancedPhysics.Trim();
            PerformanceNode.AppendChild(AdvancedPhysicsNode);

            XmlNode DownScalingNode = Storage.CreateNode("element", "DownScaling", "");
            DownScalingNode.InnerText = DownScaling.Trim();
            PerformanceNode.AppendChild(DownScalingNode);

            XmlNode DownHeightNode = Storage.CreateNode("element", "DownHeight", "");
            DownHeightNode.InnerText = DownHeight.Trim();
            PerformanceNode.AppendChild(DownHeightNode);

            XmlNode DownWidthtNode = Storage.CreateNode("element", "DownWidth", "");
            DownWidthtNode.InnerText = DownWidth.Trim();
            PerformanceNode.AppendChild(DownWidthtNode);

            XmlNode IniNode = Storage.CreateNode("element", "IniSettings", "");
            IniNode.InnerText = IniSettings.Trim();
            PerformanceNode.AppendChild(IniNode);

            XmlNode AntiAliasingNode = Storage.CreateNode("element", "AntiAliasing", "");
            AntiAliasingNode.InnerText = AntiAliasing.Trim();
            PerformanceNode.AppendChild(AntiAliasingNode);

            XmlNode VariantNode = Storage.CreateNode("element", "Variant", "");
            VariantNode.InnerText = Variant.Trim();
            PerformanceNode.AppendChild(VariantNode);

            XmlNode LODsNode = Storage.CreateNode("element", "LODs", "");
            LODsNode.InnerText = LODs.Trim();
            PerformanceNode.AppendChild(LODsNode);

            XmlNode RTNode = Storage.CreateNode("element", "RayTracing", "");
            RTNode.InnerText = RayTracing.Trim();
            PerformanceNode.AppendChild(RTNode);

            XmlNode FPSStabilizerNode = Storage.CreateNode("element", "FPSStabilizer", "");
            FPSStabilizerNode.InnerText = FPSStabilizer.Trim();
            PerformanceNode.AppendChild(FPSStabilizerNode);

            return PerformanceNode;
        }
    }
}
