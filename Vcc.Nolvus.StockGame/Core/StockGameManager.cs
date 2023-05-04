using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Api.Installer.Library;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.StockGame.Meta;
using Vcc.Nolvus.StockGame.Patcher;

namespace Vcc.Nolvus.StockGame.Core
{
    public class StockGameManager
    {
        private const string NameKey = "GameManifest/Name";
        private const string ExeNameKey = "GameManifest/ExeName";
        private const string VersionKey = "GameManifest/Version";
        private const string FileKey = "GameManifest/Files/File";
        private const string InstructionKey = "GameManifest/Patcher/Instruction";        

        #region Fields

        XmlDocument _Storage = new XmlDocument();
        IGamePackageDTO _GamePackage;
        string _WorkingDir = string.Empty;
        string _LibDir = string.Empty;
        string _GameDir = string.Empty;
        string _StockGameDir = string.Empty;
        string _PatchDir = string.Empty;
        string _Language = string.Empty;
        string _LanguageCode = string.Empty;
        bool _KeepPatches = false;
        GameManifest _Package;
        PatcherManager _Patcher;

        #endregion

        #region Properties

        private XmlDocument Storage
        {
            get { return _Storage; }
        }       

        protected string ManifestFile
        {
            get
            {
                return Path.Combine(this._WorkingDir, "GameManifest.xml");
            }
        }

        public bool StorageExists
        {
            get
            {
                return File.Exists(this.ManifestFile);
            }
        }

        public GameManifest GamePackage
        {
            get
            {
                return _Package;
            }
        }

        #endregion

        #region Events

        public event DownloadProgressChangedHandler OnDownload;
        public event ExtractProgressChangedHandler OnExtract;        

        event OnItemProcessedHandler OnItemProcessedEvent;

        public event OnItemProcessedHandler OnItemProcessed
        {
            add
            {
                if (OnItemProcessedEvent != null)
                {
                    lock (OnItemProcessedEvent)
                    {
                        OnItemProcessedEvent += value;
                    }
                }
                else
                {
                    OnItemProcessedEvent = value;
                }
            }
            remove
            {
                if (OnItemProcessedEvent != null)
                {
                    lock (OnItemProcessedEvent)
                    {
                        OnItemProcessedEvent -= value;
                    }
                }
            }
        }

        event OnStepProcessedHandler OnStepProcessedEvent;

        public event OnStepProcessedHandler OnStepProcessed
        {
            add
            {
                if (OnStepProcessedEvent != null)
                {
                    lock (OnStepProcessedEvent)
                    {
                        OnStepProcessedEvent += value;
                    }
                }
                else
                {
                    OnStepProcessedEvent = value;
                }
            }
            remove
            {
                if (OnStepProcessedEvent != null)
                {
                    lock (OnStepProcessedEvent)
                    {
                        OnStepProcessedEvent -= value;
                    }
                }
            }
        }

        #endregion

        public StockGameManager(string WorkingDir, string LibDir, string PatchDir, string GameDir, INolvusInstance Instance, IGamePackageDTO GamePackage, bool KeepPatches = false)
        {            
            _WorkingDir = WorkingDir;
            _LibDir = LibDir;
            _PatchDir = PatchDir;
            _GameDir = GameDir;
            _GamePackage = GamePackage;
            _StockGameDir = Instance.StockGame;
            _Language = Instance.Settings.LgName;
            _LanguageCode = Instance.Settings.LgCode;
            _KeepPatches = KeepPatches;
            _Patcher = new PatcherManager(WorkingDir, LibDir, PatchDir);

            _Patcher.OnDownload += Downloading;
            _Patcher.OnExtract += Extracting;
            _Patcher.OnStepProcessed += StepProcessing;
            _Patcher.OnItemProcessed += ItemProcessing;                        
        }

        public StockGameManager(string WorkingDir, string LibDir, string PatchDir, string GameDir, string StockGameDir, string LgName, string LgCode, IGamePackageDTO GamePackage, bool KeepPatches = false)
        {
            _WorkingDir = WorkingDir;
            _LibDir = LibDir;
            _PatchDir = PatchDir;
            _GameDir = GameDir;
            _GamePackage = GamePackage;
            _StockGameDir = StockGameDir;
            _Language = LgName;
            _LanguageCode = LgCode;
            _KeepPatches = KeepPatches;
            _Patcher = new PatcherManager(WorkingDir, LibDir, PatchDir);

            _Patcher.OnDownload += Downloading;
            _Patcher.OnExtract += Extracting;
            _Patcher.OnStepProcessed += StepProcessing;
            _Patcher.OnItemProcessed += ItemProcessing;
        }

        #region Methods

        private void Downloading(object sender, DownloadProgress e)
        {
            if (OnDownload != null)
            {
                OnDownload(this, e);
            }
        }

        protected void Extracting(object sender, ExtractProgress e)
        {
            if (OnExtract != null)
            {
                OnExtract(this, e);
            }
        }

        private void StepProcessing(object sender, ItemProcessedEventArgs e)
        {
            this.StepProcessed(e.Step);
        }

        private void ItemProcessing(object sender, ItemProcessedEventArgs e)
        {
            this.ElementProcessed(e.Value, e.Total, e.Step);
        }

        private void ElementProcessed(int Value, int Total, string Step)
        {
            OnItemProcessedHandler Handler = this.OnItemProcessedEvent;
            ItemProcessedEventArgs Event = new ItemProcessedEventArgs(Value, Total, Step);
            if (Handler != null) Handler(this, Event);
        }

        private void StepProcessed(string Step)
        {
            OnStepProcessedHandler Handler = this.OnStepProcessedEvent;
            ItemProcessedEventArgs Event = new ItemProcessedEventArgs(0, 0, Step);
            if (Handler != null) Handler(this, Event);
        }

        public async Task Load()
        {            
            var Tsk = Task.Run(async () => 
            {
                string DownloadedFile = Path.Combine(_WorkingDir, _GamePackage.Name + ".zip");

                try
                {
                    try
                    {
                        this.StepProcessed("Initializing stock game installation");
                        this.StepProcessed("Using game directory : " + _GameDir);
                        this.StepProcessed("Using stock game directory : " + _StockGameDir);
                        this.StepProcessed("Using language : " + _Language);                        

                        await ServiceSingleton.Files.DownloadFile(_GamePackage.DownloadLink, DownloadedFile, Downloading);
                        await ServiceSingleton.Files.ExtractFile(DownloadedFile, _WorkingDir, Extracting);
                        await DoLoad();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                finally
                {
                    File.Delete(DownloadedFile);
                    File.Delete(ManifestFile);
                }
            });

            await Tsk;                                   
        }

        private async Task DoLoad()
        {
            var Tsk = Task.Run(() => 
            {
                try
                {
                    this.Storage.Load(ManifestFile);

                    _Package = new GameManifest();

                    _Package.Parse(this.Storage.SelectSingleNode("GameManifest"));

                    StepProcessed("Loading game meta data package using " + _Package.Name + " template");

                    XmlNodeList FileElements = this.Storage.SelectNodes(FileKey);

                    int FileCount = FileElements.Count;
                    int Counter = 1;

                    StepProcessed("Processing game files meta data");

                    foreach (XmlNode FileNode in FileElements)
                    {
                        _Package.AddFile(FileNode);

                        ElementProcessed(Counter, FileCount, "Loading game files info for " + _Package.Name);

                        Counter++;
                    }

                    XmlNodeList InstructionsElements = this.Storage.SelectNodes(InstructionKey);

                    int InstructionCount = InstructionsElements.Count;
                    Counter = 1;

                    StepProcessed("Processing patching instructions");

                    foreach (XmlNode InstructionNode in InstructionsElements)
                    {
                        _Package.AddInstruction(InstructionNode);

                        ElementProcessed(Counter, InstructionCount, "Loading patching info for " + _Package.Name);

                        Counter++;
                    }

                    StepProcessed("Game meta data package loaded");
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            });

            await Tsk;                    
        }       

        public async Task CheckIntegrity()
        {
            TaskCompletionSource<object> Tcs = new TaskCompletionSource<object>();

            var Tsk = Task.Run(() =>
            {
                try
                {
                    int Total = _Package.Files.Count;
                    int Counter = 1;

                    StepProcessed("Verifying game files integrity");

                    foreach (var GameFile in _Package.Files)
                    {
                        StepProcessed("Checking game file : " + GameFile.Name);

                        GameFile.Check(_GameDir, _LanguageCode);                        

                        ElementProcessed(Counter, Total, "Checking game files");                        

                        Counter++;
                    }

                    StepProcessed("Game files validated");                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            await Tsk;
        }

        public async Task CopyGameFiles()
        {            
            var Tsk =  Task.Run(() =>
            {
                try
                {
                    int Total = _Package.Files.Count;
                    int Counter = 1;

                    StepProcessed("Copying game files");

                    foreach (var GameFile in _Package.Files)
                    {
                        StepProcessed("Copying game file : " + GameFile.Name);

                        GameFile.Copy(_GameDir, _StockGameDir, _LanguageCode);

                        ElementProcessed(Counter, Total, "Copying game files");

                        Counter++;
                    }

                    StepProcessed("Game files copied");                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            await Tsk;
        }        

        public async Task PatchGameFiles()
        {            
            var Tsk = Task.Run(async () =>
            {
                try
                {
                    StepProcessed("Patching game files");                    
                                           
                    int Total = _Package.Instructions.Count;
                    int Counter = 1;                        

                    foreach (var Instruction in _Package.Instructions)
                    {                                                
                        await _Patcher.PatchFile(Instruction, _GameDir, _StockGameDir, _KeepPatches);

                        ElementProcessed(Counter, Total, "Patching game files");

                        Counter++;
                    }

                    StepProcessed("Game files patched");                                                           
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            await Tsk;
        }

        public async Task CreateStockGame()
        {
            await Load();
            await CheckIntegrity();
            await CopyGameFiles();
            await PatchGameFiles();            
        }

        #endregion
    }
}
