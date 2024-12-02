using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Misc
{
    public class GPUObject
    {
        public string VRAM { get; set; } = string.Empty;
        public string GPU { get; set; } = string.Empty;
        public bool Supported { get; set; } = false;
        public byte[] Image { get; set; } = null;
    }
}
