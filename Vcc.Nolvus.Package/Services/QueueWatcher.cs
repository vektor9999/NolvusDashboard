using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;

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
                ServiceSingleton.Logger.Log("Queue Watcher ==> Waiting for completion");

                while (true)
                {                    
                    if (_List.Count == 0)
                    {
                        ServiceSingleton.Logger.Log("Queue Watcher ==> Queue completed");
                        break;
                    }

                    await Task.Delay(10);
                }
            });
        }
    }
}
