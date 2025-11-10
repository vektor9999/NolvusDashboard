using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Events;
using ZetaLongPaths;

namespace Vcc.Nolvus.Package.Patchers
{
    public class Patcher
    {
        public string PatchArchive { get; set; }
        public string DownloadLink { get; set; }
        public string MirrorDownloadLink { get; set; }

        public List<PatchFile> Files = new List<PatchFile>();

        private FileInfo FileInfo
        {
            get
            {
                return new FileInfo(PatchArchive);
            }
        }

        public string PatcherFileDir
        {
            get
            {
                return FileInfo.Name.Replace(FileInfo.Extension, string.Empty);                
            }
        }

        public string BinPatchFolder
        {
            get
            {
                var Result = new ZlpDirectoryInfo(Path.Combine(ServiceSingleton.Folders.BinPatchDirectory, PatcherFileDir));

                if (!Result.Exists)
                {
                    ZlpIOHelper.CreateDirectory(Result.FullName);
                }

                return Result.FullName;
            }
        }

        public void Load(XmlNode Node)
        {
            PatchArchive = Node["PatchArchive"].InnerText;
            DownloadLink = Node["DownloadLink"].InnerText;

            if (Node["MirrorDownloadLink"] != null)
            {
                MirrorDownloadLink = Node["MirrorDownloadLink"].InnerText;
            }
            else
            {
                MirrorDownloadLink = string.Empty;
            }

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
                    await InternalDownloadPatch(DownloadLink, OnProgress);
                }
                catch(Exception ex)
                {
                    if (MirrorDownloadLink != string.Empty)
                    {
                        await InternalDownloadPatch(MirrorDownloadLink, OnProgress);
                    }
                    else
                    {
                        throw ex;
                    }       
                }
            });

            await Tsk;
        }

        private async Task InternalDownloadPatch(string Link, DownloadProgressChangedHandler OnProgress)
        {
            var Tsk = Task.Run(async () =>
            {                
                var Tries = 0;

                while (true)
                {
                    try
                    {
                        ServiceSingleton.Logger.Log(string.Format("Trying to downloading patch file {0} ({1}/{2})", PatchArchive, Tries.ToString(), ServiceSingleton.Settings.RetryCount.ToString()));                        

                        var PatchFilePath = Path.Combine(ServiceSingleton.Folders.DownloadDirectory, PatchArchive);

                        if (ZlpIOHelper.FileExists(PatchFilePath))
                        {
                            ZlpIOHelper.DeleteFile(PatchFilePath);
                        }

                        await ServiceSingleton.Files.DownloadFile(Link, PatchFilePath, OnProgress);

                        break;
                    }
                    catch (Exception ex)
                    {
                        var CaughtException = ex;

                        if (ex.InnerException != null) CaughtException = ex.InnerException;

                        ServiceSingleton.Logger.Log(string.Format("Error during patching file download {0} with error {1}", PatchArchive, CaughtException.Message));

                        if (Tries == ServiceSingleton.Settings.RetryCount)
                        {                            
                            throw new Exception(string.Format("Unable to download file {0} after {1} retries with error {2}!", PatchArchive, ServiceSingleton.Settings.RetryCount.ToString(), CaughtException.Message));
                        }

                        Tries++;
                    }
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

                        await DownloadPatch(DownloadProgress);
                        await ExtractPatch(ExtractProgress);                                          

                        foreach (var File in Files)
                        {
                            await File.Patch(ModDir, GameDir, Path.Combine(ServiceSingleton.Folders.ExtractDirectory, PatcherFileDir), BinPatchFolder);
                            PatchProgress(File.OriginFileName, ++Counter, Files.Count);
                        }
                    }
                    finally
                    {
                        ZlpIOHelper.DeleteFile(Path.Combine(ServiceSingleton.Folders.DownloadDirectory, PatchArchive));
                        ServiceSingleton.Files.RemoveDirectory(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, PatcherFileDir), true);
                        ServiceSingleton.Files.RemoveDirectory(Path.Combine(ServiceSingleton.Folders.BinPatchDirectory, PatcherFileDir), true);
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
