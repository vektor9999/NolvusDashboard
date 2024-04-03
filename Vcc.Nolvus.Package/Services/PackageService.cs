using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Misc;
using Vcc.Nolvus.Package.Mods;
using Vcc.Nolvus.Package.Errors;
using Vcc.Nolvus.Api.Installer.Library;

namespace Vcc.Nolvus.Package.Services
{   
    public class PackageService : IPackageService
    {
        private const string SoftWareKey = "InstallationManifest/Softwares/Soft";        
        private const string CategoryKey = "InstallationManifest/Categories/Category";        
        private const string VersionKey = "InstallationManifest/Settings/Guide/Version";
        private const string LoadOrderKey = "InstallationManifest/LoadOrder/Esp";
        
        #region Fields

        XmlDocument _Storage = new XmlDocument();        
        List<InstallableElement> Elements = new List<InstallableElement>();
        SemaphoreSlim SemaphoreSlim;
        SemaphoreSlim SemaphoreSlimBeforeDownload;
        QueueWatcher QueueWatcher;
        bool _Processing = false;
        private Dictionary<string, object> _Softwares = new Dictionary<string, object>();

        #endregion

        #region Properties

        public List<string> LoadOrder { get; set; } = new List<string>();
        protected XmlDocument Storage
        {
            get { return _Storage; }
        }
        public List<IInstallableElement> InstallingModsQueue { get; set; } = new List<IInstallableElement>();
        public IProgressQueue ProgressQueue { get; set; } = new ProgressQueue();
        private ErrorHandler _ErrorHandler { get; set; }
        public string LoadedVersion { get; set; }        
        public IErrorHandler ErrorHandler
        {
            get
            {
                return _ErrorHandler;
            }
        }       
        public bool Processing
        {
            get
            {
                return _Processing;
            }
        }
        public IModOrganizer ModOrganizer2
        {
            get
            {
                return Softwares.Where(x => x.Name == ModOrganizer.SoftwareId).FirstOrDefault() as IModOrganizer;
            }            
        }

        private List<InstallableElement> ModsToInstall
        {
            get
            {
                return Elements.Where(x => x.IsInstallable() && (x is Mod || x is Software)).ToList();
            }

        }

        private List<InstallableElement> CategoriesToInstall
        {
            get
            {
                return Elements.Where(x => x.IsInstallable() && x is Category).ToList();
            }

        }

        private List<ISoftware> Softwares
        {
            get
            {
                return Elements.Where(x => x.IsInstallable() && x is Software).Cast<ISoftware>().ToList();
            }
        }

        public List<IMOElement> InstallList
        {
            get
            {
                return Elements.Where(x => x.IsInstallable() && (x is MOElement) && (x as MOElement).Display).Cast<IMOElement>().ToList();
            }

        }        

        public List<string> OptionalEsps
        {
            get
            {
                return Elements.Where(x => !x.IsInstallable() && x is Mod).Cast<Mod>().SelectMany(x => x.Esps).Select(x => x.FileName).ToList();
            }
            
        }

        public int ModsCount
        {
            get
            {
                return ModsToInstall.Count;
            }
        }

        public double InstallProgression
        {
            get
            {
                return Math.Floor(((double)ServiceSingleton.Instances.WorkingInstance.Status.InstalledMods.Count / ModsCount) * 100);
            }
        }

        #endregion

        public PackageService()
        {            
        }

        #region Methods               

        private async Task<string> DownloadPackage(IInstallPackageDTO Package, bool Install, Action<string, int> Progress = null)
        {
            return await Task.Run(async () => 
            {                
                var DownloadedFile = Path.Combine(ServiceSingleton.Folders.DownloadDirectory, "InstallPackage.zip");
                var ExtractedFile = Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "InstallPackage.xml");
                var Link = Package.InstallLink;

                if (!Install)
                {                    
                    ExtractedFile = Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "UpdatePackage.xml");
                    Link = Package.UpdateLink;
                }

                try
                {                    
                    try
                    {
                        await ServiceSingleton.Files.DownloadFile(Link, DownloadedFile, (s, e) =>
                        {
                            Progress("Downloading package", e.ProgressPercentage);
                        });

                        await ServiceSingleton.Files.ExtractFile(DownloadedFile, ServiceSingleton.Folders.ExtractDirectory, (s, e) =>
                        {
                            Progress("Extracting package", e.ProgressPercentage);
                        });
                    }
                    finally
                    {
                        File.Delete(DownloadedFile);
                    }

                    return ExtractedFile;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }

        public async Task Load(IInstallPackageDTO Package, Action<string, int> Progress = null)
        {
            var Tsk = Task.Run(async () =>
            {                
                try
                {                    
                    try
                    {                        
                        await Load(await DownloadPackage(Package, true, Progress), (s, p) =>
                        {
                            Progress(s, p);
                        });                       
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                finally
                {
                    File.Delete(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "InstallPackage.xml"));
                }
            });

            await Tsk;
        }
        private async Task Load(string PackageFile, Action<string, int> Progress = null)
        {            
            var Tsk = Task.Run(() => 
            {
                try
                {
                    Elements.Clear();

                    Storage.Load(PackageFile);                    

                    XmlNodeList SoftwaresNode = Storage.SelectNodes(SoftWareKey);
                    XmlNodeList CategoriesNode = Storage.SelectNodes(CategoryKey);
                    XmlNodeList Esps = Storage.SelectNodes(LoadOrderKey);

                    int Total = SoftwaresNode.Count + CategoriesNode.Count + Esps.Count;
                    int Counter = 0;

                    foreach (XmlNode SoftNode in SoftwaresNode)
                    {
                        Software Soft = Activator.CreateInstance(Type.GetType("Vcc.Nolvus.Package.Mods." + SoftNode["Type"].InnerText)) as Software;

                        Soft.Load(SoftNode, Elements);                        

                        Progress(string.Format("Loading softwares for version {0}", Storage.SelectSingleNode(VersionKey).InnerText), System.Convert.ToInt16(Math.Round(((double)++Counter / Total * 100))));
                    }

                    Counter = 1;

                    foreach (XmlNode CatNode in CategoriesNode)
                    {
                        Category Category = new Category();

                        Category.Load(CatNode, Elements);
                        
                        Progress(string.Format("Loading mods for version {0}", Storage.SelectSingleNode(VersionKey).InnerText), System.Convert.ToInt16(Math.Round(((double)++Counter / Total * 100))));
                    }                    

                    LoadOrder.Clear();

                    foreach (XmlNode EspNode in Esps)
                    {
                        LoadOrder.Add(EspNode.InnerText);
                        
                        Progress(string.Format("Loading load order for version {0}", Storage.SelectSingleNode(VersionKey).InnerText), System.Convert.ToInt16(Math.Round(((double)++Counter / Total * 100))));
                    }

                    LoadedVersion = Storage.SelectSingleNode(VersionKey).InnerText;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            await Tsk;                       
        }

        public async Task Merge(IEnumerable<IInstallPackageDTO> Packages, Action<string, int> Progress = null)
        {
            var Tsk = Task.Run(async () => 
            {
                try
                {
                    try
                    {
                        Elements.Clear();

                        foreach (var Package in Packages.ToList())
                        {
                            await Merge(await DownloadPackage(Package, false, Progress), Progress);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                finally
                {
                    File.Delete(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "UpdatePackage.xml"));
                }
            });

            await Tsk;            
        }

        private async Task Merge(string PackageFile, Action<string, int> Progress = null)
        {

            var Tsk = Task.Run(() => 
            {
                try
                {
                    Storage.Load(PackageFile);                    

                    XmlNodeList CategoryElements = Storage.SelectNodes(CategoryKey);
                    XmlNodeList Esps = Storage.SelectNodes(LoadOrderKey);

                    int Counter = 0;
                    int Total = CategoryElements.Count + Esps.Count;

                    foreach (XmlNode CatNode in CategoryElements)
                    {
                        Category Category = Elements.Where(x => x.Name == CatNode["Name"].InnerText && x is Category).FirstOrDefault() as Category;

                        if (Category == null)
                        {
                            Category = new Category();
                        }

                        Category.Load(CatNode, Elements);

                        Progress(string.Format("Loading mods for version {0}", Storage.SelectSingleNode(VersionKey).InnerText), System.Convert.ToInt16(Math.Round(((double)++Counter / Total * 100))));
                    }

                    LoadOrder.Clear();

                    foreach (XmlNode EspNode in Esps)
                    {
                        LoadOrder.Add(EspNode.InnerText);

                        Progress(string.Format("Loading load order for version {0}", Storage.SelectSingleNode(VersionKey).InnerText), System.Convert.ToInt16(Math.Round(((double)++Counter / Total * 100))));
                    }

                    LoadedVersion = Storage.SelectSingleNode(VersionKey).InnerText;               
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            await Tsk;            
        }                

        private async Task AddModToQueue(InstallableElement Mod)
        {
            var Tsk = Task.Run(async () => 
            {                
                ProgressQueue.Add(await Mod.PrepareProgress());
                InstallingModsQueue.Add(Mod);
                ServiceSingleton.Logger.Log("Mod : " + Mod.Name + " added to queue.");
            });

            await Tsk;
        }

        private void RemoveModFromQueue(InstallableElement Mod)
        {
            ProgressQueue.Remove(Mod.Progress);
            InstallingModsQueue.Remove(Mod);
            ServiceSingleton.Logger.Log("Mod : " + Mod.Name + " removed from queue.");
        }

        private async Task RequestManualDownloadLinkIfAny(InstallableElement Mod, ModInstallSettings Settings)
        {
            await SemaphoreSlimBeforeDownload.WaitAsync();

            if (!NexusApi.ApiManager.AccountInfo.IsPremium)
            {
                await Mod.RequestManualNexusDownloadLink(Settings.Browser);
            }
            
            SemaphoreSlimBeforeDownload.Release();
        }

        private void SaveInstance(InstallableElement Mod)
        {
            ServiceSingleton.Instances.WorkingInstance.Status.InstalledMods.Add(Mod.Name);
            ServiceSingleton.Instances.Save();
        }

        public async Task InstallModList(ModInstallSettings Settings)
        {
            try
            {
                _Processing = true;

                SemaphoreSlim = new SemaphoreSlim(ServiceSingleton.Settings.ProcessCount);
                SemaphoreSlimBeforeDownload = new SemaphoreSlim(1);

                _ErrorHandler = new ErrorHandler(ServiceSingleton.Settings.ErrorsThreshold) {
                    CancelTasks = new TaskCompletionSource<object>(),
                    CancelTokenSource = new CancellationTokenSource()
                };               

                QueueWatcher = new QueueWatcher(InstallingModsQueue);

                foreach (var Category in CategoriesToInstall)
                {
                    await Category.Install(_ErrorHandler.Token);
                }

                Settings.OnStartInstalling();                

                var Tasks = ModsToInstall.Where(x => !ServiceSingleton.Instances.WorkingInstance.Status.InstalledMods.Any(y => y == x.Name)).OrderBy(i => i.Index).ToList().Select(async Mod =>
                {
                    await SemaphoreSlim.WaitAsync();

                    try
                    {
                        if (!_ErrorHandler.Cancelling)
                        {
                            await RequestManualDownloadLinkIfAny(Mod, Settings);

                            await AddModToQueue(Mod);

                            await Mod.Install(_ErrorHandler.Token, Settings);

                            SaveInstance(Mod);

                            RemoveModFromQueue(Mod);

                            Settings.OnModInstalled(Mod);

                            SemaphoreSlim.Release();
                        }
                    }
                    catch (Exception ex)
                    {
                        RemoveModFromQueue(Mod);

                        SemaphoreSlim.Release();

                        if (!_ErrorHandler.Cancelling)
                        {
                            _ErrorHandler.AddFaultyMod(Mod, ex);

                            Settings.OnModError(_ErrorHandler.ErrorsCount);

                            if (_ErrorHandler.ThresholdEnabled && _ErrorHandler.ThresholdReached)
                            {
                                Settings.OnMaxErrors();

                                _ErrorHandler.CancelInstall();

                                await QueueWatcher.WaitingForCompletion();

                                _ErrorHandler.Exit();
                            }
                        }
                    }
                }).ToList();

                await Task.WhenAny(Task.WhenAll(Tasks), _ErrorHandler.CancelTasks.Task);                

                if (_ErrorHandler.HasErrors)
                {
                    ProgressQueue.Clear();
                    _ErrorHandler.ThrowException();
                }

                ServiceSingleton.Logger.Log("List is installed");
            }
            finally
            {
                _Processing = false;
            }
        }        

        public async Task<List<ModObject>> GetModsMetaData(Action<string, int> Progress = null)
        {
            return await Task.Run(() =>
            {                
                var Counter = 0;

                var Result = ServiceSingleton.Game.GamePluginAsObjects();
                var InstallList = Elements.Where(x => x.IsInstallable() && (x is Mod) && (x as Mod).Display).Cast<IMod>().ToList();

                Result.AddRange(
                    InstallList.Select(x =>
                    {
                        var Mod = new ModObject
                        {
                            Selected = true,
                            Priority = Counter,
                            Name = x.Name,
                            Category = x.Category.Name,
                            Version = x.Version,
                            Status = ModObjectStatus.OK,
                            StatusText = "OK"
                        };

                        Progress("Loading mods from nolvus package", System.Convert.ToInt16(Math.Round(((double)++Counter / InstallList.Count * 100))));

                        return Mod;

                    }).ToList()
                );

                return Result;                        
            });
        }

        #endregion
    }
}
