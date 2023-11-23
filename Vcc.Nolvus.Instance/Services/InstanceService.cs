using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Instance.Core;

namespace Vcc.Nolvus.Instance.Services
{
    public class InstanceService : IInstanceService
    {
        #region Fields

        private readonly object SyncRoot = new object();
        string _DataFilePath = string.Empty;
        public List<NolvusInstance> Instances = new List<NolvusInstance>();

        #endregion

        #region Properties

        public string InstancesDirectory
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Instances");
            }
        }

        public string ArchivesDirectory
        {
            get
            {
                return Path.Combine(InstancesDirectory, "ARCHIVE");
            }
        }

        protected string InstancesDataFile
        {
            get
            {
                return Path.Combine(InstancesDirectory, "InstancesData.xml");
            }
        }

        public bool InstanceFileExists
        {
            get
            {
                return File.Exists(InstancesDataFile);
            }
        }

        public INolvusInstance WorkingInstance { get; set; }
        public IInstanceOptions Options { get; set; }

        public List<INolvusInstance> InstanceList { get { return Instances.Cast<INolvusInstance>().ToList(); } }

        #endregion

        #region Methods

        public InstanceService()
        {
            Directory.CreateDirectory(InstancesDirectory);
            Directory.CreateDirectory(ArchivesDirectory);
        }

        private XmlDocument InitializeStorage()
        {
            XmlDocument Storage = new XmlDocument();

            if (!InstanceFileExists)
            {
                if (!Directory.Exists(InstancesDirectory))
                {
                    Directory.CreateDirectory(InstancesDirectory);
                }
                
                Save();
            }

            Storage.Load(InstancesDataFile);            

            return Storage;
        }      

        public void Load()
        {
            lock (SyncRoot)
            {
                Instances.Clear();

                var Storage = InitializeStorage();

                foreach (XmlNode InstanceNode in Storage.SelectNodes("Instances/Instance"))
                {
                    NolvusInstance Instance = new NolvusInstance();

                    Instance.Load(InstanceNode);

                    Instances.Add(Instance);
                }                                                                                               
            }
        }

        public void Save()
        {
            lock (SyncRoot)
            {
                if (InstanceFileExists)
                {
                    File.Copy(InstancesDataFile, Path.Combine(InstancesDirectory, "InstancesData.bak"), true);
                }

                Encoding Utf8noBOM = new UTF8Encoding(false);
                XmlWriterSettings Settings = new XmlWriterSettings();
                Settings.Indent = true;
                Settings.Encoding = Utf8noBOM;

                using (MemoryStream Output = new MemoryStream())
                {
                    using (XmlWriter XMLWriter = XmlWriter.Create(Output, Settings))
                    {
                        XMLWriter.WriteStartDocument();
                        XMLWriter.WriteComment("Warning DO NOT MODIFY this file yourself, you will break your installation!!!");
                        XMLWriter.WriteStartElement("Instances");                        

                        foreach (NolvusInstance Instance in Instances)
                        {
                            Instance.Save(XMLWriter);
                        }

                        XMLWriter.WriteEndElement();
                        XMLWriter.WriteEndDocument();
                    }


                    File.WriteAllBytes(InstancesDataFile, Output.ToArray());                    
                }
            }
        }

        public List<INolvusInstance> InstancesToResume
        {
            get
            {
                return Instances.Where(x => x.Status.InstallStatus != InstanceInstallStatus.Installed).Cast<INolvusInstance>().ToList();
            }            
        }

        public bool CheckInstances(out string Status)
        {
            foreach (var Instance in Instances)
            {
                var CheckStatus = Instance.Check();

                switch (CheckStatus)
                {
                    case InstanceCheckStatus.NoId:
                        Status = "Instance " + Instance.Name + " has no valid Id!";
                        return false;

                    case InstanceCheckStatus.VersionError:
                        Status = "Instance " + Instance.Name + " has no valid version!";
                        return false;

                }
            }

            Status = string.Empty;

            return true;
        }

        public bool InstanceExists(string Name)
        {            
            return Instances.Where(x => x.Name == Name).FirstOrDefault() != null;
        }

        public void RemoveInstance(INolvusInstance Instance)
        {
            Instances.Remove(Instance as NolvusInstance);

            Save();
        }

        public void PrepareInstanceForInstall()
        {
            if (WorkingInstance != null)
            {
                if(!InstanceExists(WorkingInstance.Name))
                {
                    Instances.Add(WorkingInstance as NolvusInstance);
                }

                WorkingInstance.Status.InstallStatus = InstanceInstallStatus.Installing;

                Save();
            }
            else
            {
                throw new Exception("No working instance selected!");
            }
        }

        public void PrepareInstanceForUpdate()
        {
            if (WorkingInstance != null)
            {                
                WorkingInstance.Status.InstallStatus = InstanceInstallStatus.Updating;

                Save();
            }
            else
            {
                throw new Exception("No working instance selected!");
            }
        }

        public void FinalizeInstance()
        {
            WorkingInstance.Status.InstallStatus = InstanceInstallStatus.Installed;
            WorkingInstance.Status.InstalledMods.Clear();
            WorkingInstance.Version = ServiceSingleton.Packages.LoadedVersion;

            Save();

            WorkingInstance = null;
        }

        public bool Empty
        {
            get { return Instances.Count == 0; }
        }             

        public string GetValueFromKey(string Key)
        {
            string EnvValue = Key;

            try
            {                     
                System.Reflection.PropertyInfo PropToCompare = typeof(INolvusInstance).GetProperty(Key);

                if (PropToCompare == null)
                {
                    PropToCompare = typeof(IInstanceSettings).GetProperty(Key);

                    if (PropToCompare == null)
                    {
                        PropToCompare = typeof(IInstancePerformance).GetProperty(Key);

                        if (PropToCompare == null)
                        {
                            PropToCompare = typeof(IInstanceOptions).GetProperty(Key);

                            EnvValue = (string)PropToCompare.GetValue(ServiceSingleton.Instances.WorkingInstance.Options);
                        }
                        else
                        {
                            EnvValue = (string)PropToCompare.GetValue(ServiceSingleton.Instances.WorkingInstance.Performance);
                        }
                    }
                    else
                    {
                        EnvValue = (string)PropToCompare.GetValue(ServiceSingleton.Instances.WorkingInstance.Settings);
                    }
                }
                else
                {
                    EnvValue = (string)PropToCompare.GetValue(ServiceSingleton.Instances.WorkingInstance);
                }
            }
            catch(Exception ex)
            {
                ServiceSingleton.Logger.Log(string.Format("Error during environment value checking (Key : {0}) with message {1}", Key, ex.Message));
                throw ex;
            }

            return EnvValue;   
        }

        #endregion
    }
}

