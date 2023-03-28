using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IModWrapper : IContainerControl
    {
        Task Execute(CancellationToken Token);
        void Dispose();
        bool IsDisposed { get; }
    }
}
