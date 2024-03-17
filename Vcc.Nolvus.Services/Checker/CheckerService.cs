using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Services.Checker
{
    public class CheckerService : ICheckerService
    {
        public async Task<ModObjectList> CheckModList(List<ModObject> Mo2List, List<ModObject> NolvusList, Action<string> Status)
        {
            return await Task.Run(() =>
            {
                Status("Checking differences...");                
                return new ModObjectList().Merge(Mo2List, NolvusList);              
            });
        }
    }
}
