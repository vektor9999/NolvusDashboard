using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Dashboard.Core
{
    public static class SettingsCache
    {
        public static string GameDirectory { get; set; } = string.Empty;
        public static string NexusApiKey { get; set; } = string.Empty;
        public static string NolvusUser { get; set; } = string.Empty;
        public static string NolvusPassword { get; set; } = string.Empty;
    }
}
