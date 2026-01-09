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
        public List<IInstanceStatusField> Fields { get; set; } = new List<IInstanceStatusField>();

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

            var FieldsNode = Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Fields").FirstOrDefault();

            if (FieldsNode != null)
            {
                foreach (XmlNode FieldNode in FieldsNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Field"))
                {
                    InstanceStatusField Field = new InstanceStatusField();

                    Field.Key = FieldNode["Key"].InnerText;
                    Field.Value = FieldNode["Value"].InnerText;

                    Fields.Add(Field);
                }
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

            XMLWriter.WriteStartElement("Fields");

            foreach (var Field in Fields)
            {
                XMLWriter.WriteStartElement("Field");                

                XMLWriter.WriteStartElement("Key");
                XMLWriter.WriteString(Field.Key.Trim());
                XMLWriter.WriteEndElement();

                XMLWriter.WriteStartElement("Value");
                XMLWriter.WriteString(Field.Value.Trim());
                XMLWriter.WriteEndElement();

                XMLWriter.WriteEndElement();
            }

            XMLWriter.WriteEndElement();


            XMLWriter.WriteEndElement();
        }

        public void AddField(string Key, string Value)
        {
            Fields.Add(new InstanceStatusField(){Key = Key, Value = Value });
        }

        public IInstanceStatusField GetFieldByKey(string Key)
        {
            return Fields.Where(x => x.Key == Key).FirstOrDefault();
        }
    }
}
