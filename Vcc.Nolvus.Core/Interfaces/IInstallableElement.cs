using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Events;


namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IInstallableElement
    {
        string Name { get; set; }
        string Version { get; set; }
        string ImagePath { get; set; }
        string Author { get; set; }        
        Task Install(CancellationToken Token, ModInstallSettings Settings = null);
        ModProgress Progress { get; }        
    }
}
