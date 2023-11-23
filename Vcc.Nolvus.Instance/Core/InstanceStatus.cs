using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Interfaces;

namespace Vcc.Nolvus.Instance.Core
{
    public class InstanceStatus : IInstanceStatus
    {
        #region Properties

        public string CurrentMod { get; set; } = string.Empty;
        public InstanceInstallStatus InstallStatus { get; set; } = InstanceInstallStatus.None;
        public int TotalMods { get; set; }
        public List<string> InstalledMods { get; set; } = new List<string>();

        #endregion

        public void Load(XmlNode Node)
        {
            TotalMods = System.Convert.ToInt16(Node["TotalMods"].InnerText.Trim());
            InstallStatus = (InstanceInstallStatus)Enum.Parse(typeof(InstanceInstallStatus), Node["InstallStatus"].InnerText);

            var InstalledModsNode = Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "InstalledMods").FirstOrDefault();

            foreach (XmlNode InstalledModNode in InstalledModsNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "InstalledMod"))
            {
                InstalledMods.Add(InstalledModNode.InnerText);
            }
        }       

        public void Save(XmlWriter XMLWriter)
        {
            XMLWriter.WriteStartElement("Status");

            XMLWriter.WriteStartElement("InstallStatus");
            XMLWriter.WriteString(InstallStatus.ToString().Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("TotalMods");
            XMLWriter.WriteString(TotalMods.ToString().Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("InstalledMods");

            foreach (string InstalledMod in InstalledMods)
            {
                XMLWriter.WriteStartElement("InstalledMod");
                XMLWriter.WriteString(InstalledMod.Trim());
                XMLWriter.WriteEndElement();
            }

            XMLWriter.WriteEndElement();

            XMLWriter.WriteEndElement();
        }
    }
}
