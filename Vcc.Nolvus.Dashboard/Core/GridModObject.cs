using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Dashboard.Core
{
    public enum GridModObjectStatus { OK, NotInstalled, Error, VersionMisMatch, CustomInstalled }
    public class GridModObject
    {
        public int Priority { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Category { get; set; }
        public string StatusText { get; set; }
        public GridModObjectStatus Status { get; set; }
    }
}
