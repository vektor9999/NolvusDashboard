using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Events
{
    public class FileDownloadRequestEvent : EventArgs
    {
        private string _DownloadUrl;

        public string DownloadUrl
        {
            get
            {
                return _DownloadUrl;
            }
        }

        public FileDownloadRequestEvent(string Url)
        {
            _DownloadUrl = Url;
        }
    }

    public delegate void OnFileDownloadRequestedHandler(object sender, FileDownloadRequestEvent EventArgs);
    public delegate void OnBrowserClosedHandler(object sender, EventArgs EventArgs);

}
