using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Services.Files.Extractor
{
    public class FileExtractor
    {
        private readonly ExtractProgress ExtractProgress;
        public event ExtractProgressChangedHandler ExtractProgressChanged;
        private string FileName;
        public FileExtractor()
        {
            ExtractProgress = new ExtractProgress();
        }

        public async Task ExtractFile(string File, string Output, ExtractProgressChangedHandler OnProgress)
        {
            var Tsk = Task.Run(() =>
            {
                FileName = new FileInfo(File).Name;

                try
                {
                    ExtractProgressChanged += OnProgress;

                    SevenZip.SevenZipExtractor.SetLibraryPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "7z.dll"));

                    SevenZip.SevenZipExtractor Zip = new SevenZip.SevenZipExtractor(File);

                    try
                    {
                        Zip.Extracting += Zip_Extracting;

                        Zip.ExtractArchive(Output);
                    }
                    finally
                    {
                        Zip.Dispose();
                    }
                }
                catch (Exception ex)
                {                    
                    throw ex;
                }
            });

            await Tsk;
        }

        private void Zip_Extracting(object sender, SevenZip.ProgressEventArgs e)
        {
            if (ExtractProgressChanged != null)
            {
                ExtractProgress.ProgressPercentage = e.PercentDone;
                ExtractProgress.FileName = FileName;

                ExtractProgressChanged(this, ExtractProgress);
            }
        }
    }
}
