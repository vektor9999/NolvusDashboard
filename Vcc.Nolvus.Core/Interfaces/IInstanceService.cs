using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IInstanceService : INolvusService
    {
        INolvusInstance WorkingInstance { get; set; }
        IInstanceOptions Options { get; set; }        
        Task Load();
        void Save();
        bool Empty { get; }
        bool CheckInstances(out string Status);
        List<INolvusInstance> InstancesToResume { get; }
        string InstancesDirectory { get; }
        string ArchivesDirectory { get; }
        bool InstanceExists(string Name);
        bool InstanceExists(string Name, string Tag);
        void RemoveInstance(INolvusInstance Instance);
        List<INolvusInstance> InstanceList { get; }
        void PrepareInstanceForInstall();
        void PrepareInstanceForUpdate();
        void PrepareInstanceForEnb();
        void PrepareInstanceForOption();
        void FinalizeInstance();
        bool PropertyExists(string Name);
        string GetValueFromKey(string Key);        
        void UnloadWorkingIntance();        
    }
}
