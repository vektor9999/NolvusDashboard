using System;
using System.Xml;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Package.Mods;

namespace Vcc.Nolvus.Package.Files
{
    public class ModFile
    {
        #region Properties

        public string Name { get; set; }
        public string FileName { get; set; }
        public bool TakenFromArchive
        {
            get
            {
                return Found(ServiceSingleton.Instances.WorkingInstance.ArchiveDir);
            }
        }
        public string LocationFileName
        {
            get
            {
                if (TakenFromArchive)
                {
                    string Path;

                    Found(ServiceSingleton.Instances.WorkingInstance.ArchiveDir, out Path);

                    return Path;
                }
                else
                {
                    return Path.Combine(ServiceSingleton.Folders.DownloadDirectory, FileName);
                }
            }
        }
        public int Size { get; set; }
        public string DownloadLink { get; set; }
        public string CRC32 { get; set; }
        public bool ExtractInSubdirectory { get; set; }
        public InstallableElement Element { get; set; }
        public bool RequireManualDownload { get; set; }
        public string VersionedFileName
        {
            get
            {
                return (this.Element.Name + "-v" + this.Element.Version + new FileInfo(this.FileName).Extension);
            }
        }

        #endregion

        #region Methods

        public virtual void Load(XmlNode Node, InstallableElement InstallableElement)
        {
            Name = Node["Name"].InnerText;
            FileName = Node["FileName"].InnerText;
            Size = System.Convert.ToInt32(Node["Size"].InnerText);
            Element = InstallableElement;            
            CRC32 = Node["CRC32"].InnerText;
            DownloadLink = Node["DownloadLink"].InnerText;

            if (Node["RequireManualDownload"] != null)
            {
                RequireManualDownload = System.Convert.ToBoolean(Node["RequireManualDownload"].InnerText);
            }
            
        }

        public override string ToString()
        {
            return this.FileName;
        }

        private bool Found(string Dir, out string FullPath)
        {
            FullPath = string.Empty;

            if (!(Dir != string.Empty))
            {
                return false;
            }

            if (!ServiceSingleton.Files.FileExists(Dir, this.FileName, out FullPath) && !ServiceSingleton.Files.FileExists(Dir, this.VersionedFileName, out FullPath))
            {
                return false;
            }

            return true;
        }

        private bool Found(string Dir)
        {
            string FullPath = string.Empty;

            if (!(Dir != string.Empty))
            {
                return false;
            }

            if (!ServiceSingleton.Files.FileExists(Dir, this.FileName, out FullPath) && !ServiceSingleton.Files.FileExists(Dir, this.VersionedFileName, out FullPath))
            {
                return false;
            }

            return true;
        }

        public bool Exist()
        {
            return Found(ServiceSingleton.Folders.DownloadDirectory) || Found(ServiceSingleton.Instances.WorkingInstance.ArchiveDir);
        }

        public FileInfo GetFileInfo()
        {
            return new FileInfo(this.LocationFileName);
        }

        public void Delete()
        {
            var FileInfo = GetFileInfo();

            if (FileInfo.Exists)
            {
                FileInfo.Delete();
            }
        }

        public void Copy(string DestinationFileName)
        {
            GetFileInfo().CopyTo(DestinationFileName, true);
        }

        public async Task Move(string DestinationDirectory, Action<string, int> Progress)
        {
            var Tsk = Task.Run(() => 
            {
                var SourceFile = GetFileInfo();
                var ArchivedFile = new FileInfo(Path.Combine(DestinationDirectory, SourceFile.Name));

                if (!File.Exists(ArchivedFile.FullName))
                {
                    SourceFile.CopyTo(ArchivedFile, Progress);
                }

                SourceFile.Delete();
            });

            await Tsk;          
        }

        protected virtual async Task DoDownload(string Link, DownloadProgressChangedHandler OnProgress)
        {
            try
            {
                ServiceSingleton.Logger.Log(string.Format("Downloading file {0}", FileName));
                await ServiceSingleton.Files.DownloadFile(Link, Path.Combine(ServiceSingleton.Folders.DownloadDirectory, this.FileName), OnProgress);
            }
            catch (Exception ex)
            {
                ServiceSingleton.Logger.Log(string.Format("Error during file download {0} with error {1}", FileName, ex.Message));
                throw ex;
            }
        }

        public async Task<bool> CRCCheck(Action<string, int> HashProgress = null)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var FileInfo = GetFileInfo();

                    ServiceSingleton.Logger.Log(string.Format("Checking CRC for file {0}", FileName));

                    if (CRC32 == string.Empty || FileInfo.Length == 0 || (CRC32 != string.Empty && CRC32 != await ServiceSingleton.Files.GetCRC32(FileInfo, HashProgress)))
                    {
                        ServiceSingleton.Logger.Log(string.Format("CRC check failed for file {0}", FileName));
                        Delete();
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }
        
        public async Task Download(DownloadProgressChangedHandler OnProgress, Action<string, int> HashProgress, int RetryCount, Func<IBrowserInstance> Browser)
        {
            var Tsk = Task.Run(async () =>
            {
                var Tries = 0;

                while (true)
                {
                    if (!Exist())
                    {
                        if (RequireManualDownload)
                        {                                                       
                            await Browser().AwaitUserDownload(DownloadLink, FileName, OnProgress);
                        }
                        else
                        {
                            await DoDownload(DownloadLink, OnProgress);
                        }
                    }                   

                    if (await CRCCheck(HashProgress))
                    {
                        break;
                    }
                    else if (Tries == RetryCount)
                    {
                        throw new Exception(string.Format("Unable to download the file after {0} retries!", RetryCount.ToString()));
                    }

                    Tries++;
                }
            });

            await Tsk;
        }

        public async Task Extract(ExtractProgressChangedHandler OnProgress)
        {
            var Tsk = Task.Run(async() => 
            {
                try
                {
                    ServiceSingleton.Logger.Log(string.Format("Extracting file {0}", FileName));
                    await ServiceSingleton.Files.ExtractFile(LocationFileName, Path.Combine(ServiceSingleton.Folders.ExtractDirectory, Element.ExtractSubDir), OnProgress);
                }
                catch(Exception ex)
                {
                    ServiceSingleton.Logger.Log(string.Format("Error during file extract {0} with error {1}", FileName, ex.Message));
                    throw ex;
                }
            });

            await Tsk;
        }

        public async Task Archive(string ArchivePath, Action<string, int> Progress)
        {
            var Tsk = Task.Run(async () => 
            {
                if (!Directory.Exists(ArchivePath))
                {
                    Directory.CreateDirectory(ArchivePath);
                }                

                if (!TakenFromArchive)
                {                
                    await Move(ArchivePath, Progress);                                   
                }                
            });

            await Tsk;
        }

        #endregion        
    }
}
