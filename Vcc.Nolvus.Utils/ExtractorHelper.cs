using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Vcc.Nolvus.Utils
{
    public delegate void ExtractProgressChangedEventHandler(object sender, ExtractProgress progress);

    public static class ExtractorHelper
    {
        public static Task ExtractFile(string FileName, string Output, EventHandler<SevenZip.ProgressEventArgs> Extracting)
        {
            TaskCompletionSource<object> Tcs = new TaskCompletionSource<object>();

            SevenZip.SevenZipExtractor Zip = null;

            Task.Run(() =>
            {
                try
                {
                    SevenZip.SevenZipExtractor.SetLibraryPath(AppDomain.CurrentDomain.BaseDirectory + "lib\\7z.dll");                    

                    Zip = new SevenZip.SevenZipExtractor(FileName);

                    try
                    {
                        if (Extracting != null)
                        {
                            Zip.Extracting += Extracting;
                        }

                        Zip.ExtractArchive(Output);                                                
                    }
                    finally
                    {
                        Zip.Dispose();
                        Tcs.SetResult(new object());
                    }
                }
                catch (Exception e)
                {                    
                    Tcs.SetException(new ExtractException(e.Message));                      
                }                                 
            });
                                   
            return Tcs.Task;          
        }

        private static void ExtractProcess_Exited(object sender, EventArgs e)
        {
            
        }
    }
}
