using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Package.Rules;
using Vcc.Nolvus.Package.Conditions;
using Vcc.Nolvus.Package.Patchers;
using System.Xml;

namespace Vcc.Nolvus.Package.Mods
{
    public class Mod : MOElement
    {
        #region Fields
        
        public List<InstallCondition> InstallConditions = new List<InstallCondition>();
        public Category Category;
        public Patcher Patcher;
        public List<Rule> Rules = new List<Rule>();
        public List<BsaUnPacking> Bsas = new List<BsaUnPacking>();
        public List<Esp> Esps = new List<Esp>();

        #endregion

        #region Properties              

        public override string MoDirectoryName
        {
            get
            {
                return Name;
            }
        }

        public override string ArchiveFolder
        {
            get
            {
                return Path.Combine(ServiceSingleton.Instances.WorkingInstance.ArchiveDir, this.Category.Name);
            }
        }

        #endregion

        public Mod()
        {
            Action = ElementAction.None;
        }

        #region Methods

        public override void Load(XmlNode Node, List<InstallableElement> Elements)
        {
            base.Load(Node, Elements);

            ElementAction ElementAction = (ElementAction)Enum.Parse(typeof(ElementAction), Node["Action"].InnerText);

            var Mod = Elements.Where(x => x.Name == Name && x is Mod).FirstOrDefault();
            
            if (Mod != null)
            {
                Elements.Remove(Mod);
            }
            
            Action = ElementAction;

            #region Rules

            Rules.Clear();

            XmlNode RulesNode = Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Rules").FirstOrDefault();

            if (RulesNode != null)
            {
                foreach (XmlNode RuleNode in RulesNode.ChildNodes.Cast<XmlNode>().ToList())
                {
                    Rule Rule = Activator.CreateInstance(Type.GetType("Vcc.Nolvus.Package.Rules." + RuleNode["Type"].InnerText)) as Rule;

                    Rule.Load(RuleNode);

                    Rules.Add(Rule);
                }
            }

            #endregion

            #region Install Conditions

            InstallConditions.Clear();

            XmlNode InstallConditionsNode = Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "InstallConditions").FirstOrDefault();

            if (InstallConditionsNode != null)
            {
                foreach (XmlNode InstallConditionNode in InstallConditionsNode.ChildNodes.Cast<XmlNode>().ToList())
                {
                    InstallCondition InstallCondition = Activator.CreateInstance(Type.GetType("Vcc.Nolvus.Package.Conditions." + InstallConditionNode["Type"].InnerText)) as InstallCondition;

                    InstallCondition.Load(InstallConditionNode);

                    InstallConditions.Add(InstallCondition);
                }
            }

            #endregion

            #region Patcher

            Patcher = null;

            XmlNode PatcherNode = Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Patcher").FirstOrDefault();

            if (PatcherNode != null)
            {
                Patcher = new Patcher();

                Patcher.Load(PatcherNode);
            }

            #endregion

            #region Bsas

            Bsas.Clear();

            XmlNode BsasNode = Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Bsas").FirstOrDefault();

            if (BsasNode != null)
            {
                foreach (XmlNode BsaNode in BsasNode.ChildNodes.Cast<XmlNode>().ToList())
                {
                    BsaUnPacking Bsa = new BsaUnPacking();

                    Bsa.FileName = BsaNode["FileName"].InnerText;
                    Bsas.Add(Bsa);
                }
            }

            #endregion

            #region Esps

            Esps.Clear();

            XmlNode EspsNode = Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "Esps").FirstOrDefault();

            if (EspsNode != null)
            {
                List<XmlNode> EspNodes = EspsNode.ChildNodes.Cast<XmlNode>().ToList();

                foreach (XmlNode EspNode in EspNodes)
                {
                    Esp Esp = new Esp();

                    Esp.FileName = EspNode["FileName"].InnerText;
                    Esps.Add(Esp);
                }
            }

            #endregion

            Elements.Add(this);                        
        }

        public override string ToString()
        {
            return this.Name + " v" + this.Version;
        }
        protected override void CreateElementIni()
        {
            if (this.Display)
            {
                File.WriteAllText(Path.Combine(MoDirectoryFullName, "meta.ini"), string.Format(MetaIni, "0", Version, GetInstallFileName().Replace("\\", "/"), "0"));
            }            
        }
        public override bool IsInstallable(bool Log)
        {
            foreach (var Condition in InstallConditions)
            {
                if (!Condition.IsValid(Log))
                {                    
                    return false;
                }
            }            

            return true;
        }
        private void PrepareDirectrory()
        {
            if (Display)
            {
                if (Directory.Exists(MoDirectoryFullName))
                {
                    ServiceSingleton.Files.RemoveDirectory(MoDirectoryFullName, true);
                }

                Directory.CreateDirectory(MoDirectoryFullName);
            }
        }
        protected string GetInstallFileName()
        {
            if (ServiceSingleton.Instances.WorkingInstance.Settings.EnableArchiving)
            {
                return Path.Combine(ArchiveFolder, this.Files.First().FileName);
            }

            return string.Empty;
        }
        protected List<Rule> FetchRules()
        {
            var Result = new List<Rule>();

            if (Rules.Where(x => x.Force).Count() > 0)
            {
                return Rules;
            }
            else
            {
                var DirectoryRules = Rules.Where(x => x is DirectoryCopy).ToList();
                var PriorityRules  = Rules.Where(x => x.IsPriority && !(x is DirectoryCopy)).ToList();
                               
                if (DirectoryRules.Count == 0 && PriorityRules.Count == 0)
                {
                    Result.AddRange(new DirectoryCopy().CreateFileRules(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, ExtractSubDir), 0));
                }
                else
                {
                    foreach (var Rule in DirectoryRules)
                    {
                        Result.AddRange((Rule as DirectoryCopy).CreateFileRules(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, ExtractSubDir), (Rule as DirectoryCopy).Destination));
                    }

                    Result.AddRange(PriorityRules);
                }
                                
                Result.AddRange(Rules.Where(x => !x.Force && !x.IsPriority).ToList());
            }

            return Result;
        }
        protected override async Task DoUnpack()
        {
            var Tsk = Task.Run(async () =>
            {
                try
                {                                          
                    var Counter = 0;

                    foreach (BsaUnPacking BSA in this.Bsas)
                    {                        
                        await BSA.UnPack(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, ExtractSubDir));

                        UnpackingProgress(BSA.FileName, ++Counter, Bsas.Count);
                    }                   
                }
                catch (Exception ex)
                {
                    ServiceSingleton.Files.RemoveDirectory(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, ExtractSubDir), true);
                    throw ex;
                }
            });

            await Tsk;
        }
        protected override async Task DoCopy()
        {
            var Tsk = Task.Run(() =>
            {
                try
                {
                    try
                    {
                        this.PrepareDirectrory();
                        var Rules = FetchRules();
                        var Counter = 0;

                        foreach (var Rule in Rules)
                        {
                            Rule.Execute(ServiceSingleton.Instances.WorkingInstance.StockGame, Path.Combine(ServiceSingleton.Folders.ExtractDirectory, ExtractSubDir), MoDirectoryFullName, ServiceSingleton.Instances.WorkingInstance.InstallDir);
                            CopyingProgress(++Counter, Rules.Count);
                        }

                        this.CreateElementIni();
                    }
                    finally
                    {
                        ServiceSingleton.Files.RemoveDirectory(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, ExtractSubDir), true);
                    }                                          
                }
                catch (Exception ex)
                {
                    throw ex;                    
                }
            });

            await Tsk;
        }
        protected override async Task DoPatch()
        {
            var Tsk = Task.Run(async () => 
            {
                try
                {                                        
                    if (Patcher != null)
                    {                        
                        await Patcher.PatchFiles(MoDirectoryFullName, ServiceSingleton.Instances.WorkingInstance.StockGame, DownloadingProgress, ExtractingProgress, PatchingProgress);
                    }                                        
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            await Tsk;
        }
        public override async Task Remove()
        {
            var Tsk = Task.Run(() =>
            {
                try
                {
                    if (Directory.Exists(MoDirectoryFullName))
                    {
                        ServiceSingleton.Files.RemoveDirectory(MoDirectoryFullName, true);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            await Tsk;
        }

        #endregion
    }
}

