using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Package.Rules
{
    public class EnvironmentSettingsRule : SettingsRule
    {
        public override void Execute(string GamePath, string ExtractDir, string ModDir, string InstanceDir)
        {
            if (this.CanExecute(GamePath, ModDir))
            {
                System.Reflection.PropertyInfo PropToCompare = typeof(IInstanceOptions).GetProperty(Value);                

                string EnvValue;

                if (PropToCompare == null)
                {
                    PropToCompare = typeof(IInstancePerformance).GetProperty(Value);

                    if (PropToCompare == null)
                    {
                        PropToCompare = typeof(IInstanceSettings).GetProperty(Value);
                        EnvValue = (string)PropToCompare.GetValue(ServiceSingleton.Instances.WorkingInstance.Settings);
                    }
                    else
                    {
                        EnvValue = (string)PropToCompare.GetValue(ServiceSingleton.Instances.WorkingInstance.Performance);
                    }
                }
                else
                {
                    EnvValue = (string)PropToCompare.GetValue(ServiceSingleton.Instances.WorkingInstance.Options);
                }
                

                if (this.IsIni)
                {                    
                    ServiceSingleton.Settings.StoreIniValue(Path.Combine(ModDir, FileName), Section, Key, EnvValue);
                }
                else
                {
                    string SettingsFile = ModDir + "\\" + FileName;

                    string[] Lines = System.IO.File.ReadAllLines(SettingsFile);

                    List<string> NewLines = new List<string>();

                    bool Found = false;

                    foreach (string Line in Lines)
                    {
                        string _Line = Line;

                        if (Line.Contains(this.Key) && Line.Substring(0, 2) != "# " && !Found)
                        {
                            _Line = this.Key + " = " + EnvValue;
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
