using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Api.Installer.Library;
using Vcc.Nolvus.Api.Installer.Services;

namespace Vcc.Nolvus.Instance.Options
{
    public class NolvusOptionsManager : INolvusOptionsManager
    {
        private ConcurrentDictionary<string, INolvusOption> _List = new ConcurrentDictionary<string, INolvusOption>();
        
        public NolvusOptionsManager()
        {

        }

        public async Task LoadFromServer(INolvusInstance Instance)
        {
            var ServerOptions = await ApiManager.Service.Installer.GetVariantOptions(Instance.SettingsManager.Variant, Instance.Id);

            foreach (var ServerOption in ServerOptions)
            {
                NolvusOption Option = new NolvusOption();
                Option.LoadFromServer(ServerOption);
                AddOption(Option);
            }
        }        

        public async Task SynchronizeOptions(INolvusInstance Instance)
        {
            var ServerOptionsCount = await ApiManager.Service.Installer.OptionsCount(Instance.SettingsManager.Variant, Instance.Id);

            if (ServerOptionsCount != ToList().Count)
            {
                var ServerOptions = await ApiManager.Service.Installer.GetVariantOptions(Instance.SettingsManager.Variant, Instance.Id);

                if (ServerOptionsCount > ToList().Count)
                {
                    ServerOptions.Where(x => !ToList().Any(y => y.Name == x.Name)).ToList().ForEach(x => AddOption(x));
                }
                else
                {
                    ToList().Where(x => !ServerOptions.Any(y => y.Name == x.Name)).ToList().ForEach(x => RemoveOption(x));
                }
            }
        }

        public List<string> GetOptionCategories()
        {
            return ToList().OrderBy(x=>x.Category).Select(x => x.Category).Distinct().ToList();           
        }

        public List<INolvusOption> GetOptionsByCategory(string Category)
        {
            return ToList().Where(x => x.Category == Category).OrderBy(x => x.Name).ToList();
        }        

        public List<INolvusOption> GetOptions()
        {
            return ToList().OrderBy(x => x.Name).OrderBy(x => x.Category).ToList();
        }

        private List<INolvusOption> ToList()
        {
            return _List.Values.OrderBy(x => x.Name).ToList();
        }

        private void AddOption(INolvusOption Option)
        {
            _List.TryAdd(Option.Name, Option);
        }

        private void RemoveOption(INolvusOption Option)
        {
            INolvusOption Ignored;
            _List.TryRemove(Option.Name, out Ignored);
        }

        private void AddOption(INolvusOptionDTO Option)
        {
            NolvusOption NewOption = new NolvusOption();
            NewOption.LoadFromServer(Option);
            AddOption(NewOption);
        }

        public void AddOption(string Name, string FriendlyName, string Category, string DataType, string Value)
        {
            AddOption(new NolvusOption
            {
                Name = Name,
                FriendlyName = FriendlyName,
                Category = Category,
                DataType = DataType,
                Value = Value                
            });
        }

        public string GetOptionValue(string Name)
        {
            INolvusOption Result;

            if(_List.TryGetValue(Name, out Result))
            {
                return Result.Value;
            }

            throw new Exception(string.Format("Option {0} is not defined", Name));
        }

        public INolvusOption GetOption(string Name)
        {
            INolvusOption Result;

            if (_List.TryGetValue(Name, out Result))
            {
                return Result;
            }

            throw new Exception(string.Format("Option {0} is not defined", Name));
        }

        public void SetOptionValue(string Name, string Value)
        {
            INolvusOption Option;

            if (_List.TryGetValue(Name, out Option))
            {
                Option.Value = Value;
            }
            else
            {
                throw new Exception(string.Format("Option {0} is not defined", Name));
            }
        }

        public async Task Load(XmlNode Node, INolvusInstance Instance)
        {
            var Options = Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Option").ToList();

            foreach(var OptionNode in Options)
            {
                NolvusOption Option = new NolvusOption();
                Option.Load(OptionNode);
                AddOption(Option);
            }

            await SynchronizeOptions(Instance);                      
        }

        public void Save(XmlWriter XMLWriter)
        {
            XMLWriter.WriteStartElement("Options");

            foreach(var Option in ToList())
            {
                Option.Save(XMLWriter);
            }

            XMLWriter.WriteEndElement();
        }
    }
}
