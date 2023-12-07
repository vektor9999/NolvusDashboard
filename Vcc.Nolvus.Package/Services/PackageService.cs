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

        #endregion

        #region Properties

        public List<string> LoadOrder { get; set; } = new List<string>();
        protected XmlDocument Storage
        {
            get { return _Storage; }
        }
        public List<IInstallableElement> InstallingModsQueue { get; set; } = new List<IInstallableElement>();
        public List<ModProgress> ProgressQueue { get; set; } = new List<ModProgress>();
        private ErrorHandler _ErrorHandler { get; set; } = new ErrorHandler(ServiceSingleton.Settings.ErrorsThreshold);
        public string LoadedVersion { get; set; }
        public List<string> GameBaseModsList
        {
            get
            {
                var Lines = new List<string>();

                Lines.Add("-0. MASTER FILES_separator");
                Lines.Add("*Creation Club: ccvsvsse004-beafarmer");
                Lines.Add("*Creation Club: ccvsvsse003-necroarts");
                Lines.Add("*Creation Club: ccvsvsse002-pets");
                Lines.Add("*Creation Club: ccvsvsse001-winter");
                Lines.Add("*Creation Club: cctwbsse001-puzzledungeon");
                Lines.Add("*Creation Club: ccrmssse001-necrohouse");
                Lines.Add("*Creation Club: ccqdrsse002-firewood");
                Lines.Add("*Creation Club: ccpewsse002-armsofchaos");
                Lines.Add("*Creation Club: ccmtysse002-ve");
                Lines.Add("*Creation Club: ccmtysse001-knightsofthenine");
                Lines.Add("*Creation Club: cckrtsse001_altar");
                Lines.Add("*Creation Club: ccfsvsse001-backpacks");
                Lines.Add("*Creation Club: ccffbsse002-crossbowpack");
                Lines.Add("*Creation Club: ccffbsse001-imperialdragon");
                Lines.Add("*Creation Club: cceejsse005-cave");
                Lines.Add("*Creation Club: cceejsse004-hall");
                Lines.Add("*Creation Club: cceejsse003-hollow");
                Lines.Add("*Creation Club: cceejsse002-tower");
                Lines.Add("*Creation Club: cceejsse001-hstead");
                Lines.Add("*Creation Club: ccedhsse003-redguard");
                Lines.Add("*Creation Club: ccedhsse002-splkntset");
                Lines.Add("*Creation Club: ccedhsse001-norjewel");
                Lines.Add("*Creation Club: cccbhsse001-gaunt");
                Lines.Add("*Creation Club: ccbgssse069-contest");
                Lines.Add("*Creation Club: ccbgssse068-bloodfall");
                Lines.Add("*Creation Club: ccbgssse067-daedinv");
                Lines.Add("*Creation Club: ccbgssse066-staves");
                Lines.Add("*Creation Club: ccbgssse064-ba_elven");
                Lines.Add("*Creation Club: ccbgssse063-ba_ebony");
                Lines.Add("*Creation Club: ccbgssse062-ba_dwarvenmail");
                Lines.Add("*Creation Club: ccbgssse061-ba_dwarven");
                Lines.Add("*Creation Club: ccbgssse060-ba_dragonscale");
                Lines.Add("*Creation Club: ccbgssse059-ba_dragonplate");
                Lines.Add("*Creation Club: ccbgssse058-ba_steel");
                Lines.Add("*Creation Club: ccbgssse057-ba_stalhrim");
                Lines.Add("*Creation Club: ccbgssse056-ba_silver");
                Lines.Add("*Creation Club: ccbgssse055-ba_orcishscaled");
                Lines.Add("*Creation Club: ccbgssse054-ba_orcish");
                Lines.Add("*Creation Club: ccbgssse053-ba_leather");
                Lines.Add("*Creation Club: ccbgssse052-ba_iron");
                Lines.Add("*Creation Club: ccbgssse051-ba_daedricmail");
                Lines.Add("*Creation Club: ccbgssse050-ba_daedric");
                Lines.Add("*Creation Club: ccbgssse045-hasedoki");
                Lines.Add("*Creation Club: ccbgssse043-crosselv");
                Lines.Add("*Creation Club: ccbgssse041-netchleather");
                Lines.Add("*Creation Club: ccbgssse040-advobgobs");
                Lines.Add("*Creation Club: ccbgssse038-bowofshadows");
                Lines.Add("*Creation Club: ccbgssse036-petbwolf");
                Lines.Add("*Creation Club: ccbgssse035-petnhound");
                Lines.Add("*Creation Club: ccbgssse034-mntuni");
                Lines.Add("*Creation Club: ccbgssse031-advcyrus");
                Lines.Add("*Creation Club: ccbgssse021-lordsmail");
                Lines.Add("*Creation Club: ccbgssse020-graycowl");
                Lines.Add("*Creation Club: ccbgssse019-staffofsheogorath");
                Lines.Add("*Creation Club: ccbgssse018-shadowrend");
                Lines.Add("*Creation Club: ccbgssse016-umbra");
                Lines.Add("*Creation Club: ccbgssse014-spellpack01");
                Lines.Add("*Creation Club: ccbgssse013-dawnfang");
                Lines.Add("*Creation Club: ccbgssse012-hrsarmrstl");
                Lines.Add("*Creation Club: ccbgssse011-hrsarmrelvn");
                Lines.Add("*Creation Club: ccbgssse010-petdwarvenarmoredmudcrab");
                Lines.Add("*Creation Club: ccbgssse008-wraithguard");
                Lines.Add("*Creation Club: ccbgssse007-chrysamere");
                Lines.Add("*Creation Club: ccbgssse006-stendarshammer");
                Lines.Add("*Creation Club: ccbgssse005-goldbrand");
                Lines.Add("*Creation Club: ccbgssse004-ruinsedge");
                Lines.Add("*Creation Club: ccbgssse003-zombies");
                Lines.Add("*Creation Club: ccbgssse002-exoticarrows");
                Lines.Add("*Creation Club: ccasvsse001-almsivi");
                Lines.Add("*Creation Club: ccafdsse001-dwesanctuary");
                Lines.Add("*Creation Club: ccqdrsse001-survivalmode");
                Lines.Add("*Creation Club: ccbgssse037-curios");
                Lines.Add("*Creation Club: ccbgssse025-advdsgs");
                Lines.Add("*Creation Club: ccbgssse001-fish");
                Lines.Add("*DLC: HearthFires");
                Lines.Add("*DLC: Dragonborn");
                Lines.Add("*DLC: Dawnguard");

                return Lines;
            }
        }

        public List<string> GameBasePluginsList
        {
            get
            {
                var Lines = new List<string>();

                Lines.Add("Skyrim.esm");
                Lines.Add("Update.esm");
                Lines.Add("Dawnguard.esm");
                Lines.Add("HearthFires.esm");
                Lines.Add("Dragonborn.esm");
                Lines.Add("ccasvsse001-almsivi.esm");
                Lines.Add("ccbgssse001-Fish.esm");
                Lines.Add("ccbgssse002-exoticarrows.esl");
                Lines.Add("ccbgssse003-zombies.esl");
                Lines.Add("ccbgssse004-ruinsedge.esl");
                Lines.Add("ccbgssse005-goldbrand.esl");
                Lines.Add("ccbgssse006-stendarshammer.esl");
                Lines.Add("ccbgssse007-chrysamere.esl");
                Lines.Add("ccbgssse010-petdwarvenarmoredmudcrab.esl");
                Lines.Add("ccbgssse011-hrsarmrelvn.esl");
                Lines.Add("ccbgssse012-hrsarmrstl.esl");
                Lines.Add("ccbgssse014-spellpack01.esl");
                Lines.Add("ccbgssse019-staffofsheogorath.esl");
                Lines.Add("ccbgssse020-graycowl.esl");
                Lines.Add("ccbgssse021-lordsmail.esl");
                Lines.Add("ccmtysse001-knightsofthenine.esl");
                Lines.Add("ccqrdsse001-SurvivalMode.esl");
                Lines.Add("cctwbsse001-puzzledungeon.esm");
                Lines.Add("cceejsse001-hstead.esm");
                Lines.Add("ccqdrsse002-firewood.esl");
                Lines.Add("ccbgssse018-shadowrend.esl");
                Lines.Add("ccbgssse035-petnhound.esl");
                Lines.Add("ccfsvsse001-backpacks.esl");
                Lines.Add("cceejsse002-tower.esl");
                Lines.Add("ccedhsse001-norjewel.esl");
                Lines.Add("ccvsvsse002-pets.esl");
                Lines.Add("ccbgssse037-Curios.esl");
                Lines.Add("ccbgssse034-mntuni.esl");
                Lines.Add("ccbgssse045-hasedoki.esl");
                Lines.Add("ccbgssse008-wraithguard.esl");
                Lines.Add("ccbgssse036-petbwolf.esl");
                Lines.Add("ccffbsse001-imperialdragon.esl");
                Lines.Add("ccmtysse002-ve.esl");
                Lines.Add("ccbgssse043-crosselv.esl");
                Lines.Add("ccvsvsse001-winter.esl");
                Lines.Add("cceejsse003-hollow.esl");
                Lines.Add("ccbgssse016-umbra.esm");
                Lines.Add("ccbgssse031-advcyrus.esm");
                Lines.Add("ccbgssse038-bowofshadows.esl");
                Lines.Add("ccbgssse040-advobgobs.esl");
                Lines.Add("ccbgssse050-ba_daedric.esl");
                Lines.Add("ccbgssse052-ba_iron.esl");
                Lines.Add("ccbgssse054-ba_orcish.esl");
                Lines.Add("ccbgssse058-ba_steel.esl");
                Lines.Add("ccbgssse059-ba_dragonplate.esl");
                Lines.Add("ccbgssse061-ba_dwarven.esl");
                Lines.Add("ccpewsse002-armsofchaos.esl");
                Lines.Add("ccbgssse041-netchleather.esl");
                Lines.Add("ccedhsse002-splkntset.esl");
                Lines.Add("ccbgssse064-ba_elven.esl");
                Lines.Add("ccbgssse063-ba_ebony.esl");
                Lines.Add("ccbgssse062-ba_dwarvenmail.esl");
                Lines.Add("ccbgssse060-ba_dragonscale.esl");
                Lines.Add("ccbgssse056-ba_silver.esl");
                Lines.Add("ccbgssse055-ba_orcishscaled.esl");
                Lines.Add("ccbgssse053-ba_leather.esl");
                Lines.Add("ccbgssse051-ba_daedricmail.esl");
                Lines.Add("ccbgssse057-ba_stalhrim.esl");
                Lines.Add("ccbgssse066-staves.esl");
                Lines.Add("ccbgssse067-daedinv.esm");
                Lines.Add("ccbgssse068-bloodfall.esl");
                Lines.Add("ccbgssse069-contest.esl");
                Lines.Add("ccvsvsse003-necroarts.esl");
                Lines.Add("ccvsvsse004-beafarmer.esl");
                Lines.Add("ccbgssse025-AdvDSGS.esm");
                Lines.Add("ccffbsse002-crossbowpack.esl");
                Lines.Add("ccbgssse013-dawnfang.esl");
                Lines.Add("ccrmssse001-necrohouse.esl");
                Lines.Add("ccedhsse003-redguard.esl");
                Lines.Add("cceejsse004-hall.esl");
                Lines.Add("cceejsse005-cave.esm");
                Lines.Add("cckrtsse001_altar.esl");
                Lines.Add("cccbhsse001-gaunt.esl");
                Lines.Add("ccafdsse001-dwesanctuary.esm");

                return Lines;
            }
        }

        public IErrorHandler ErrorHandler
        {
            get
            {
                return _ErrorHandler;
            }
        }

        public double DownloadSpeed
        {
            get
            {
                try

                {
                    return ServiceSingleton.Packages.ProgressQueue.Sum(x => x.Mbs);
                }
                catch
                {
                    return 0;
                }
            }
        }

        #endregion

        public PackageService()
        {            
        }

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

        public List<InstallableElement> GetModsToInstall()
        {
            return Elements.Where(x => x.IsInstallable() && (x is Mod || x is Software)).ToList();            
        }

        public List<InstallableElement> GetCategoriesToInstall()
        {
            return Elements.Where(x => x.IsInstallable() && x is Category).ToList();
        }

        public List<IMOElement> GetInstallList()
        {
            return Elements.Where(x => x.IsInstallable() && (x is MOElement) && (x as MOElement).Display).Cast<IMOElement>().ToList();
        }

        public List<string> GetOptionalEsps()
        {
            return Elements.Where(x => !x.IsInstallable() && x is Mod).Cast<Mod>().SelectMany(x => x.Esps).Select(x => x.FileName).ToList();
        }

        public int ModsCount
        {
            get
            {
                return GetModsToInstall().Count;
            }
        }                                              

        public double InstallProgression
        {
            get
            {
                return Math.Floor(((double)ServiceSingleton.Instances.WorkingInstance.Status.InstalledMods.Count / ModsCount) * 100);
            }
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
            SemaphoreSlim = new SemaphoreSlim(ServiceSingleton.Settings.ProcessCount);
            SemaphoreSlimBeforeDownload = new SemaphoreSlim(1);            

            _ErrorHandler.Clear();
            _ErrorHandler.CancelTasks = new TaskCompletionSource<object>();
            _ErrorHandler.CancelTokenSource = new CancellationTokenSource();

            QueueWatcher = new QueueWatcher(InstallingModsQueue);
                          
            foreach (var Category in GetCategoriesToInstall())
            {
                await Category.Install(_ErrorHandler.Token);
            }

            Settings.OnStartInstalling();  

            var Tasks = GetModsToInstall().Where(x => !ServiceSingleton.Instances.WorkingInstance.Status.InstalledMods.Any(y => y == x.Name)).ToList().Select(async Mod =>
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
    }
}
