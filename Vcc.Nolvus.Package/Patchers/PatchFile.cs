using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.StockGame.Patcher;

namespace Vcc.Nolvus.Package.Patchers
{
    public class PatchFile
    {
        public string OriginFileName { get; set; }
        public string DestinationFileName { get; set; }
        public string PatchFileName { get; set; }
        public string HashBefore { get; set; }
        public string HashAfter { get; set; }

        public void Load(XmlNode Node)
        {
            OriginFileName = Node["OriginFileName"].InnerText;
            DestinationFileName = Node["DestinationFileName"].InnerText;
            PatchFileName = Node["PatchFileName"].InnerText;
            HashBefore = Node["HashBefore"].InnerText;
            HashAfter = Node["HashAfter"].InnerText;
        }

        private FileInfo CopyPatchedFile(FileInfo Source, FileInfo Destination)
        {
            FileInfo Result;

            if (Source.Name == Destination.Name)
            {
                Result = Destination.CopyTo(Source.FullName, true);
            }
            else
            {
                Result = Destination.CopyTo(Path.Combine(Source.DirectoryName, DestinationFileName), true);
            }

            return Result;
        }
        public async Task Patch(string ModDir, string GameDir, string ExtractDir)
        {            
            var Tsk = Task.Run(async ()=>
            {
                try
                {
                    var PatcherManager = new PatcherManager(ServiceSingleton.Folders.DownloadDirectory, ServiceSingleton.Folders.LibDirectory);

                    var Dir = ModDir;

                    if (!Directory.Exists(ModDir))
                    {
                        Dir = GameDir;
                    }

                    var SourceFileToPatch = ServiceSingleton.Files.GetFiles(Dir).Where(x => x.Name == OriginFileName).Where(y => ServiceSingleton.Files.GetHash(y.FullName) == HashBefore).FirstOrDefault();                    

                    if (SourceFileToPatch != null)
                    {
                        var DestinationFileToPatch = new FileInfo(Path.Combine(ExtractDir, DestinationFileName));

                        await PatcherManager.PatchFile(SourceFileToPatch.FullName, DestinationFileToPatch.FullName, Path.Combine(ExtractDir, PatchFileName));

                        if (ServiceSingleton.Files.GetHash(CopyPatchedFile(SourceFileToPatch, DestinationFileToPatch).FullName) != HashAfter)
                        {
                            throw new Exception("Hash for file : " + DestinationFileName + " does not match!");
                        }
                    }
                    else
                    {
                        throw new Exception("File name to patch does not exist (" + OriginFileName + ") hash : " + HashBefore);
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
