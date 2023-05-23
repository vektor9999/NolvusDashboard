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
        bool InstanceFileExists { get; }
        void Load();
        void Save();
        bool Empty { get; }
        bool CheckInstances(out string Status);
        List<INolvusInstance> InstancesToResume { get; }
        string InstancesDirectory { get; }
        string ArchivesDirectory { get; }
        bool InstanceExists(string Name);
        void RemoveInstance(INolvusInstance Instance);
        List<INolvusInstance> InstanceList { get; }
        void PrepareInstanceForInstall();
        void PrepareInstanceForUpdate();
        void FinalizeInstance();
        string GetValueFromKey(string Key);
    }
}
