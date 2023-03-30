using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;

namespace Vcc.Nolvus.Package.Services
{
    public class QueueWatcher
    {
        private List<IInstallableElement> _List;

        public QueueWatcher(List<IInstallableElement> ListToWatch)
        {
            _List = ListToWatch;
        }

        public Task WaitingForCompletion()
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    if (_List.Count == 1)
                    {                        
                        break;
                    }

                    await Task.Delay(10);
                }
            });
        }
    }
}
