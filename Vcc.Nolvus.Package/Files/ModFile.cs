using System;
using System.Xml;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Package.Mods;
using ZetaLongPaths;

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
                return (Element.Name + "-v" + Element.Version + new FileInfo(FileName).Extension);
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
            return FileName;
        }

        private bool Found(string Dir, out string FullPath)
        {
            FullPath = string.Empty;

            if (!(Dir != string.Empty))
            {
                return false;
            }

            if (!ServiceSingleton.Files.FileExists(Dir, FileName, out FullPath) && !ServiceSingleton.Files.FileExists(Dir, VersionedFileName, out FullPath))
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

            if (!ServiceSingleton.Files.FileExists(Dir, FileName, out FullPath) && !ServiceSingleton.Files.FileExists(Dir, VersionedFileName, out FullPath))
            {
                return false;
            }

            return true;
        }

        public bool Exist()
        {
            return Found(ServiceSingleton.Folders.DownloadDirectory) || Found(ServiceSingleton.Instances.WorkingInstance.ArchiveDir);
        }

        public ZlpFileInfo GetFileInfo()
        {
            return new ZlpFileInfo(LocationFileName);
        }

        public void Delete()
        {
            var FileInfo = GetFileInfo();

            if (FileInfo.Exists)
            {
                FileInfo.Delete();
            }
        }

        public async Task Move(string DestinationDirectory, Action<string, int> Progress)
        {
            var Tsk = Task.Run(() => 
            {
                var SourceFile = GetFileInfo();
                var ArchivedFile = new ZlpFileInfo(Path.Combine(DestinationDirectory, SourceFile.Name));

                if (!ZlpIOHelper.FileExists(ArchivedFile.FullName))
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
                await ServiceSingleton.Files.DownloadFile(Link, Path.Combine(ServiceSingleton.Folders.DownloadDirectory, FileName), OnProgress);
            }
            catch (Exception ex)
            {                
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

                    if (CRC32 == string.Empty || !FileInfo.Exists || FileInfo.Length == 0 || (CRC32 != string.Empty && CRC32 != await ServiceSingleton.Files.GetCRC32(FileInfo, HashProgress)))
                    {
                        ServiceSingleton.Logger.Log(string.Format("CRC check failed for file {0}", FileName));
                        ServiceSingleton.Logger.Log(string.Format("Deleting bad file {0}", FileName));

                        Delete();

                        return false;
                    }

                    ServiceSingleton.Logger.Log(string.Format("CRC validated for file {0}", FileName));

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
                Exception CaughtException = null;

                while (true)
                {
                    ServiceSingleton.Logger.Log(string.Format("Checking file {0}", FileName));

                    if (!Exist())
                    {
                        ServiceSingleton.Logger.Log(string.Format("File {0} not found!", FileName));
                        ServiceSingleton.Logger.Log(string.Format("Trying to download file {0} ({1}/{2})", FileName, Tries.ToString(), RetryCount.ToString()));

                        try
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
                        catch(Exception ex)
                        {
                            CaughtException = ex;

                            if (ex.InnerException != null) CaughtException = ex.InnerException;

                            ServiceSingleton.Logger.Log(string.Format("Error during file download {0} with error {1}", FileName, CaughtException.Message));
                        }
                    }
                    else
                    {
                        ServiceSingleton.Logger.Log(string.Format("File exists ({0}), about to check crc", LocationFileName));
                    }

                    if (await CRCCheck(HashProgress))
                    {
                        break;
                    }
                    else if (Tries == RetryCount)
                    {
                        ServiceSingleton.Logger.Log(string.Format("Download retry count reached for file {0}", FileName));

                        if (CaughtException != null)
                        {
                            throw new Exception(string.Format("Unable to download file {0} after {1} retries with error {2}!", FileName, RetryCount.ToString(), CaughtException.Message));
                        }
                        else
                        {
                            throw new Exception(string.Format("Unable to download file {0} after {1} retries!", FileName, RetryCount.ToString()));
                        }
                        
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
                if (!ZlpIOHelper.DirectoryExists(ArchivePath))
                {
                    ZlpIOHelper.CreateDirectory(ArchivePath);
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
