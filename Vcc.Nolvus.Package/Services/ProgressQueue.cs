using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Interfaces;

namespace Vcc.Nolvus.Package.Services
{
    public class ProgressQueue : IProgressQueue
    {        
        private ConcurrentDictionary<string, ModProgress> _List = new ConcurrentDictionary<string, ModProgress>();

        public void Add(ModProgress ModProgress)
        {            
            _List.TryAdd(ModProgress.Name, ModProgress);            
        }

        public void Remove(ModProgress ModProgress)
        {
            ModProgress Val = null;            
            _List.TryRemove(ModProgress.Name, out Val);          
        }
        
        public void Clear()
        {
            _List.Clear();            
        }

        public List<ModProgress> ToList()
        {
            return _List.Values.OrderBy(x=> x.Index).ToList();                        
        }

        public double DownloadSpeed
        {
            get
            {
                return _List.Sum(x => x.Value.Mbs);
            }
            
        }

    }
}
