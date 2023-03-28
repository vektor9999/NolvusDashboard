using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Package.Rules
{
    public class SettingsRule : Rule
    {
        public string FileName { get; set; }
        public bool IsIni { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Section { get; set; }

        public override void Load(XmlNode Node)
        {
            base.Load(Node);
            FileName = Node["FileName"].InnerText;
            IsIni = System.Convert.ToBoolean(Node["IsIni"].InnerText);
            Key = Node["Key"].InnerText;
            Value = Node["Value"].InnerText;
            Section = Node["Section"].InnerText;
        }

        public override void Execute(string GamePath, string ExtractDir, string ModDir, string InstanceDir)
        {
            if (this.CanExecute(GamePath, ModDir))
            {                
                if (this.IsIni)
                {                    
                    ServiceSingleton.Settings.StoreIniValue(Path.Combine(ModDir, FileName), Section, Key, Value);
                }
                else
                {
                    string SettingsFile = Path.Combine(ModDir, FileName);

                    string[] Lines = System.IO.File.ReadAllLines(SettingsFile);

                    List<string> NewLines = new List<string>();

                    bool Found = false;

                    foreach(string Line in Lines)
                    {                        
                        string _Line = Line;

                        if (Line.Contains(this.Key) && Line.Substring(0,1) != "#" && !Found)
                        {
                            _Line = this.Key + " = " + this.Value;
                            Found = true;
                        }

                        NewLines.Add(_Line);
                    }

                    System.IO.File.WriteAllLines(SettingsFile, NewLines.ToArray());                    
                }
            }
        }
    }
}
