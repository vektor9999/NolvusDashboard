using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Events;

namespace Vcc.Nolvus.Package.Patchers
{
    public class Patcher
    {
        public string PatchArchive { get; set; }
        public string DownloadLink { get; set; }

        public List<PatchFile> Files = new List<PatchFile>();

        private FileInfo FileInfo
        {
            get
            {
                return new FileInfo(PatchArchive);
            }
        }

        private string PatcherFileDir
        {
            get
            {
                return FileInfo.Name.Replace(FileInfo.Extension, string.Empty);                
            }
        }        

        public void Load(XmlNode Node)
        {
            PatchArchive = Node["PatchArchive"].InnerText;
            DownloadLink = Node["DownloadLink"].InnerText;
            
            foreach (XmlNode PatchFileNode in Node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "PatchFiles").FirstOrDefault().ChildNodes.Cast<XmlNode>().ToList())
            {
                PatchFile PatchFile = new PatchFile();
                PatchFile.Load(PatchFileNode);                
                Files.Add(PatchFile);
            }
        }

        public async Task DownloadPatch(DownloadProgressChangedHandler OnProgress)
        {
            var Tsk = Task.Run(async () => 
            {
                try
                {
                    ServiceSingleton.Logger.Log(string.Format("Downloading patch file {0}", PatchArchive));
                    var PatchFilePath = Path.Combine(ServiceSingleton.Folders.DownloadDirectory, PatchArchive);

                    if (File.Exists(PatchFilePath))
                    {
                        File.Delete(PatchFilePath);
                    }

                    await ServiceSingleton.Files.DownloadFile(DownloadLink, PatchFilePath, OnProgress);                    
                }
                catch(Exception ex)
                {
                    ServiceSingleton.Logger.Log(string.Format("Error during patching file download {0} with error {1}", PatchArchive, ex.Message));
                    throw ex;
                }
            });

            await Tsk;           
        }

        public async Task ExtractPatch(ExtractProgressChangedHandler OnProgress)
        {
            var Tsk = Task.Run(async () =>
            {
                try
                {
                    ServiceSingleton.Logger.Log(string.Format("Extracting patch file {0}", PatchArchive));
                    ServiceSingleton.Files.RemoveDirectory(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, PatcherFileDir), true);
                    await ServiceSingleton.Files.ExtractFile(Path.Combine(ServiceSingleton.Folders.DownloadDirectory, PatchArchive), Path.Combine(ServiceSingleton.Folders.ExtractDirectory, PatcherFileDir), OnProgress);                                        
                }
                catch (Exception ex)
                {
                    ServiceSingleton.Logger.Log(string.Format("Error during patching file extract {0} with error {1}", PatchArchive, ex.Message));
                    throw ex;
                }
            });

            await Tsk;
        }

        public async Task PatchFiles(string ModDir, string GameDir, DownloadProgressChangedHandler DownloadProgress, ExtractProgressChangedHandler ExtractProgress, Action<string, int, int> PatchProgress)
        {
            var Tsk = Task.Run(async () =>
            {
                try
                {
                    try
                    {
                        var Counter = 0;
                        var Tries = 0;

                        while (true)
                        {
                            try
                            {
                                await DownloadPatch(DownloadProgress);
                                await ExtractPatch(ExtractProgress);
                                break;
                            }
                            catch(Exception ex)
                            {
                                if (Tries == ServiceSingleton.Settings.RetryCount)
                                {
                                    throw new Exception(string.Format("Unable to download file {0} after {1} retries with error {2}!", PatchArchive, ServiceSingleton.Settings.RetryCount.ToString(), ex.Message));
                                }
                                Tries++;
                            }
                        }                        
                        
                        foreach (var File in Files)
                        {
                            await File.Patch(ModDir, GameDir, Path.Combine(ServiceSingleton.Folders.ExtractDirectory, PatcherFileDir));
                            PatchProgress(File.OriginFileName, ++Counter, Files.Count);
                        }
                    }
                    finally
                    {
                        File.Delete(Path.Combine(ServiceSingleton.Folders.DownloadDirectory, PatchArchive));
                        ServiceSingleton.Files.RemoveDirectory(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, PatcherFileDir), true);
                    }
                }
                catch (Exception ex)
                {
                    ServiceSingleton.Logger.Log(string.Format("Error during file patching {0} with error {1}", PatchArchive, ex.Message));
                    throw ex;
                }
            });

            await Tsk;
        }
    }
}
