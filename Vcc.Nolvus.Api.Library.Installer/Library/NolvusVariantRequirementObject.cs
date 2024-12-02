using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing;

namespace Vcc.Nolvus.Api.Installer.Library
{
    public interface INolvusVariantRequirementDTO : IInstallerApiDTO
    {
        string CPU { get; }
        int VRAM { get; }
        int InstallationSize { get; }
        int DownloadSize { get; }
        bool SREX { get; }
        int Width { get; }
        int Height { get; }
        string GPUVendor { get; }
        string GPUName { get; }
        int GPUVram { get; }
        int GPUIndex { get; }
        string Lods { get; }
    }

    public class NolvusVariantRequirementObject : InstallerApiObject
    {
        public string CPU { get; set; }
        public int VRAM { get; set; }
        public int InstallationSize { get; set; }
        public int DownloadSize { get; set; }
        public bool SREX { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string GPUVendor { get; set; }
        public string GPUName { get; set; }
        public int GPUVram { get; set; }
        public int GPUIndex { get; set; }
        public string Lods { get; set; }
    }

    public class NolvusVariantRequirementDTO : NolvusVariantRequirementObject, INolvusVariantRequirementDTO
    {
        public string Id { get; set; }
    }
}
