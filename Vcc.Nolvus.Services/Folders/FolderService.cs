using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Services.Folders
{
    public class FolderService : IFolderService
    {
        public const string PathSection = "Path";
        public const string GamePath = "GamePath";        

        public string CacheDirectory
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cache");
            }
        }

        public string DownloadDirectory
        {
            get
            {
                return Path.Combine(CacheDirectory, "Downloads");
            }
        }

        public string ExtractDirectory
        {
            get
            {
                try
                {
                    var cp = ServiceSingleton.Settings.GetIniValue("CustomPath", "Extract");
                    return cp == null ? Path.Combine(CacheDirectory, "Extract") : cp;
                }
                catch
                {
                    return Path.Combine(CacheDirectory, "Extract");
                }
            }
        }

        public string LibDirectory
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib");
            }
        }

        public string NexusCacheDirectory
        {
            get
            {
                return Path.Combine(CacheDirectory, "Nexus");
            }
        }

        public string WebCacheDirectory
        {
            get
            {
                return Path.Combine(CacheDirectory, "Web");
            }
        }

        public string ResourcesDirectory
        {
            get
            {
                return  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            }
        }

        public string InstancesDirectory
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Instances");
            }
        }

        public string UpdaterExe
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NolvusUpdater.exe");
            }
        }        

        public string PackageFile
        {
            get
            {
                return Path.Combine(ExtractDirectory, "InstallPackage.xml");
            }
        }

        public string GameDirectory
        {
            get { return ServiceSingleton.Settings.GetIniValue(PathSection, GamePath); }
        }        

        public FolderService()
        {            
            Directory.CreateDirectory(CacheDirectory);
            Directory.CreateDirectory(DownloadDirectory);
            Directory.CreateDirectory(ExtractDirectory);
            Directory.CreateDirectory(NexusCacheDirectory);
            Directory.CreateDirectory(WebCacheDirectory);
            Directory.CreateDirectory(InstancesDirectory);                       
        }        
    }
}
