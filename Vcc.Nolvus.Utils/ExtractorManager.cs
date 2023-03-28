using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Utils
{
    public class ExtractorManager
    {
        private readonly ExtractProgress ExtractProgress;
        public event ExtractProgressChangedEventHandler ExtractProgressChanged;
        private string _FileName;
        public ExtractorManager()
        {
            ExtractProgress = new ExtractProgress();
        }

        public async Task ExtractFile(string FileName, string Output, ExtractProgressChangedEventHandler OnProgress)
        {            
            var Tsk = Task.Run(() => 
            {
                _FileName = new FileInfo(FileName).Name;

                try
                {
                    ExtractProgressChanged += OnProgress;                    

                    SevenZip.SevenZipExtractor.SetLibraryPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "7z.dll"));

                    SevenZip.SevenZipExtractor Zip = new SevenZip.SevenZipExtractor(FileName);

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
                ExtractProgress.FileName = _FileName;

                ExtractProgressChanged(this, ExtractProgress);
            }
        }
    }
}
