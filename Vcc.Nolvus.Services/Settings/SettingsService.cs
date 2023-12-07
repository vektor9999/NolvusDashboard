using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;
using Vcc.Nolvus.Core.Interfaces;

namespace Vcc.Nolvus.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        public const string IniFile = "NolvusDashboard.ini";
        public const string ProcessSection = "Process";
        public const string Count = "Count";
        public const string Retry = "Retry";
        public const string EThreshold = "ErrorsTreshold";
        public const string MiscSection = "Misc";
        public const string ForceAntiAliasing = "ForceAA";
        public const string Interval = "RefreshInterval";

        public void StoreIniValue(string Section, string Parameter, string Value)
        {
            var Parser = new FileIniDataParser();

            IniData Data = Parser.ReadFile(IniFile);

            Data[Section][Parameter] = Value;

            Parser.WriteFile(IniFile, Data);
        }

        public void StoreIniValue(string File, string Section, string Parameter, string Value)
        {
            var Parser = new FileIniDataParser();

            IniData Data = Parser.ReadFile(File);

            Data[Section][Parameter] = Value;

            Parser.WriteFile(File, Data);
        }

        public string GetIniValue(string Section, string Parameter)
        {
            var Parser = new FileIniDataParser();

            IniData Data = Parser.ReadFile(IniFile);

            return Data[Section][Parameter];
        }

        public string GetIniValue(string File, string Section, string Parameter)
        {
            var Parser = new FileIniDataParser();

            IniData Data = Parser.ReadFile(File);

            return Data[Section][Parameter];
        }

        public bool IniKeyExists(string Section, string Key)
        {
            var Parser = new FileIniDataParser();

            IniData Data = Parser.ReadFile(IniFile);

            var _Section = Data.Sections.Where(x => x.SectionName == Section).FirstOrDefault();

            if (_Section != null)
            {
                return _Section.Keys.Where(x => x.KeyName == Key).FirstOrDefault() != null;
            }
            else
            {
                return false;
            }
        }

        public int ProcessCount
        {
            get
            {
                try
                {
                    var p = System.Convert.ToInt16(GetIniValue(ProcessSection, Count));
                    return p == 0 ? Environment.ProcessorCount : p;
                }
                catch
                {
                    return Environment.ProcessorCount;
                }
            }
        }

        public int RetryCount
        {
            get
            {
                try
                {
                    var r = System.Convert.ToInt16(GetIniValue(ProcessSection, Retry));
                    return r == 0 ? 3 : r;
                }
                catch
                {
                    return 3;
                }
            }
        }

        public bool ForceAA
        {
            get
            {
                try
                {
                    return System.Convert.ToBoolean(GetIniValue(MiscSection, ForceAntiAliasing));
                    
                }
                catch
                {
                    return false;
                }
            }
        }

        public int RefreshInterval
        {
            get
            {
                try
                {
                    var r = System.Convert.ToInt16(GetIniValue(MiscSection, Interval));
                    return r == 0 || r > 1000 ? 10 : r;
                }
                catch
                {
                    return 10;
                }
            }
        }

        public int ErrorsThreshold
        {
            get
            {
                try
                {
                    var r = System.Convert.ToInt16(GetIniValue(ProcessSection, EThreshold));
                    return r > 100 ? 50 : r;
                }
                catch
                {
                    return 50;
                }
            }
        }
    }
}
