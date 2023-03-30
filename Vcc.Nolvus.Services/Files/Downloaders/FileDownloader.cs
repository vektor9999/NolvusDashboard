using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Services.Files.Downloaders
{
    public class FileDownloader : BaseFileDownloader
    {        
        public override async Task DownloadFile(string UrlAddress, string Location)
        {            
            try
            {
                FileName = new FileInfo(Location).Name;                

                SW.Start();

                await Client.DownloadFileTaskAsync(new Uri(UrlAddress), Location);                

                SW.Stop();
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }
    }
}
