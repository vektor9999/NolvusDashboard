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
        public string CombatAnimation { get; set; } = "Fantasy";
        public string StancesPerksTree { get; set; } = "TRUE";
        public string DeleveledEnemies { get; set; } = "TRUE";
        public string Exhaustion { get; set; } = "TRUE";
        public string NerfPA { get; set; } = "Player Only";
        public string EnemiesResistance { get; set; } = "TRUE";
        public string Boss { get; set; } = "TRUE";
        public string Poise { get; set; } = "TRUE";
        public string Gore { get; set; } = "FALSE";
        public string CombatScaling { get; set; } = "Hard";


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

            if (Node["CombatAnimation"] != null)
            {
                CombatAnimation = Node["CombatAnimation"].InnerText.Trim();
            }

            if (Node["StancesPerksTree"] != null)
            {
                StancesPerksTree = Node["StancesPerksTree"].InnerText.Trim();
            }

            if (Node["DeleveledEnemies"] != null)
            {
                DeleveledEnemies = Node["DeleveledEnemies"].InnerText.Trim();
            }

            if (Node["Exhaustion"] != null)
            {
                Exhaustion = Node["Exhaustion"].InnerText.Trim();
            }

            if (Node["NerfPA"] != null)
            {
                if (Node["NerfPA"].InnerText == "TRUE")
                {
                    NerfPA = "Player Only";
                }
                else if (Node["NerfPA"].InnerText == "FALSE")
                {
                    NerfPA = "None";
                }
                else 
                {
                    NerfPA = Node["NerfPA"].InnerText.Trim();
                }                
            }

            if (Node["EnemiesResistance"] != null)
            {
                EnemiesResistance = Node["EnemiesResistance"].InnerText.Trim();
            }

            if (Node["Boss"] != null)
            {
                Boss = Node["Boss"].InnerText.Trim();
            }

            if (Node["Poise"] != null)
            {
                Poise = Node["Poise"].InnerText.Trim();
            }

            if (Node["Gore"] != null)
            {
                Gore = Node["Gore"].InnerText.Trim();
            }

            if (Node["CombatScaling"] != null)
            {
                CombatScaling = Node["CombatScaling"].InnerText.Trim();
            }

            ServiceSingleton.Logger.Log(string.Format("Instance nudity : {0}", Nudity));
            ServiceSingleton.Logger.Log(string.Format("Instance enb : {0}", AlternateENB));
            ServiceSingleton.Logger.Log(string.Format("Instance fantasy mode : {0}", FantasyMode));
            ServiceSingleton.Logger.Log(string.Format("Instance hardcore mode : {0}", HardcoreMode));
            ServiceSingleton.Logger.Log(string.Format("Instance alternate levelling : {0}", AlternateLeveling));
            ServiceSingleton.Logger.Log(string.Format("Instance alternate start : {0}", AlternateStart));
            ServiceSingleton.Logger.Log(string.Format("Instance skin type : {0}", SkinType));
            ServiceSingleton.Logger.Log(string.Format("Combat animation : {0}", CombatAnimation));
            ServiceSingleton.Logger.Log(string.Format("Stances perks tree : {0}", StancesPerksTree));
            ServiceSingleton.Logger.Log(string.Format("Deleveled enemies : {0}", DeleveledEnemies));
            ServiceSingleton.Logger.Log(string.Format("Exhaustion : {0}", Exhaustion));
            ServiceSingleton.Logger.Log(string.Format("Nerf power attacks : {0}", NerfPA));
            ServiceSingleton.Logger.Log(string.Format("Enemies resistance : {0}", EnemiesResistance));
            ServiceSingleton.Logger.Log(string.Format("Boss encounter : {0}", Boss));
            ServiceSingleton.Logger.Log(string.Format("Poise : {0}", Poise));
            ServiceSingleton.Logger.Log(string.Format("Gore : {0}", Gore));
            ServiceSingleton.Logger.Log(string.Format("Combat Scaling : {0}", CombatScaling));
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

            XMLWriter.WriteStartElement("CombatAnimation");
            XMLWriter.WriteString(CombatAnimation.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("StancesPerksTree");
            XMLWriter.WriteString(StancesPerksTree.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("DeleveledEnemies");
            XMLWriter.WriteString(DeleveledEnemies.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("Exhaustion");
            XMLWriter.WriteString(Exhaustion.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("NerfPA");
            XMLWriter.WriteString(NerfPA.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("EnemiesResistance");
            XMLWriter.WriteString(EnemiesResistance.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("Boss");
            XMLWriter.WriteString(Boss.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("Poise");
            XMLWriter.WriteString(Poise.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("Gore");
            XMLWriter.WriteString(Gore.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteStartElement("CombatScaling");
            XMLWriter.WriteString(CombatScaling.Trim());
            XMLWriter.WriteEndElement();

            XMLWriter.WriteEndElement();
        }

    }
    
}
