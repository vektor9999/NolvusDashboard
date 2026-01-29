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
        void Load();
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
        void FinalizeInstance();
        string GetValueFromKey(string Key);
        bool WorkingOnInstance { get; }
        void UnloadWorkingIntance();
        //void SetCurrenTask(string Key, string Value);
        //void ClearCurrenTask();
    }
}
