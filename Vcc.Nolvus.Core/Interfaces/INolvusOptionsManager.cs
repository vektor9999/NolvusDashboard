using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface INolvusOptionsManager
    {        
        void AddOption(string Name, string FriendlyName, string Category, string DataType, string Value);
        string GetOptionValue(string Name);
        void SetOptionValue(string Name, string Value);
        INolvusOption GetOption(string Name);
        Task LoadFromServer(INolvusInstance Instance);
        Task SynchronizeOptions(INolvusInstance Instance);
        List<INolvusOption> GetOptions();
        List<string> GetOptionCategories();
        List<INolvusOption> GetOptionsByCategory(string Category);        
        Task Load(XmlNode Node, INolvusInstance Instance);
        void Save(XmlWriter XMLWriter);
    }
}
