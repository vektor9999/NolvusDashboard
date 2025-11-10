using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.StockGame.Patcher;
using ZetaLongPaths;

namespace Vcc.Nolvus.Package.Patchers
{
    public class PatchFile
    {
        public string OriginFileName { get; set; }
        public string DestinationFileName { get; set; }
        public string PatchFileName { get; set; }
        public string HashBefore { get; set; }
        public string HashAfter { get; set; }
        public string Directory { get; set; }             

        

        public void Load(XmlNode Node)
        {
            OriginFileName = Node["OriginFileName"].InnerText;
            DestinationFileName = Node["DestinationFileName"].InnerText;
            PatchFileName = Node["PatchFileName"].InnerText;
            HashBefore = Node["HashBefore"].InnerText;
            HashAfter = Node["HashAfter"].InnerText;

            Directory = string.Empty;

            if (Node["Directory"] != null)
            {
                Directory = Node["Directory"].InnerText;
            }            
        }

        private ZlpFileInfo CopyPatchedFile(ZlpFileInfo Source, ZlpFileInfo Destination)
        {
            ZlpFileInfo Result;

            if (Source.Name == Destination.Name)
            {
                Destination.CopyTo(Source.FullName, true);
                Result = new ZlpFileInfo(Source.FullName);
            }
            else
            {
                Destination.CopyTo(Path.Combine(Source.DirectoryName, DestinationFileName), true);
                Result = new ZlpFileInfo(Path.Combine(Source.DirectoryName, DestinationFileName));
            }

            return Result;
        }

        private ZlpFileInfo CopyFileToPatch(ZlpFileInfo Source, string Destination)
        {            
            Source.CopyTo(Path.Combine(Destination, Source.Name), true);

            return new ZlpFileInfo(Path.Combine(Destination, Source.Name));
        }

        public async Task Patch(string ModDir, string GameDir, string ExtractDir, string BinPatchDir)
        {            
            var Tsk = Task.Run(async ()=>
            {
                try
                {
                    var PatcherManager = new PatcherManager(ServiceSingleton.Folders.DownloadDirectory, ServiceSingleton.Folders.LibDirectory, ServiceSingleton.Folders.PatchDirectory);

                    ServiceSingleton.Files.RemoveDirectory(BinPatchDir, false);

                    var Dir = ModDir;

                    if (!ZlpIOHelper.DirectoryExists(ModDir))
                    {
                        Dir = GameDir;
                    }

                    ZlpFileInfo SourceFileToPatch = null;

                    if (Directory == string.Empty)
                    {
                        SourceFileToPatch = ServiceSingleton.Files.GetFiles(Dir).Where(x => x.Name == DestinationFileName).Where(y => ServiceSingleton.Files.GetHash(y.FullName) == HashBefore).FirstOrDefault();
                    }
                    else
                    {
                        SourceFileToPatch = ServiceSingleton.Files.GetFiles(Dir).Where(x => x.FullName == ZlpPathHelper.Combine(Dir, Directory, DestinationFileName)).Where(y => ServiceSingleton.Files.GetHash(y.FullName) == HashBefore).FirstOrDefault();                        
                    }                    

                    if (SourceFileToPatch != null)
                    {
                        ServiceSingleton.Logger.Log(string.Format("Patching file {0}", SourceFileToPatch.Name));

                        var DestinationFileToPatch = new ZlpFileInfo(Path.Combine(ExtractDir, DestinationFileName));

                        var BinarySourceFileToPatch = CopyFileToPatch(SourceFileToPatch, BinPatchDir);

                        await PatcherManager.PatchFile(BinarySourceFileToPatch.FullName, DestinationFileToPatch.FullName, Path.Combine(ExtractDir, PatchFileName));

                        if (ServiceSingleton.Files.GetHash(CopyPatchedFile(SourceFileToPatch, DestinationFileToPatch).FullName) != HashAfter)
                        {
                            throw new Exception("Hash for file : " + DestinationFileName + " does not match!");
                        }
                    }
                    else
                    {
                        throw new Exception("File name to patch does not exist (" + DestinationFileName + ") hash : " + HashBefore + " in " + Dir);
                    }
                }
                catch(Exception ex)
                {                    
                    throw ex;
                }
            });

            await Tsk;
        }
    }
}
