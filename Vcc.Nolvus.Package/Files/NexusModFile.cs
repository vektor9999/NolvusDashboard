using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.NexusApi;
using System.Xml;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Package.Mods;

namespace Vcc.Nolvus.Package.Files
{
    public class NexusModFile : ModFile
    {
        #region Properties

        public string NexusId { get; set; }
        public string FileId { get; set; }

        #endregion

        #region Methods

        public override void Load(XmlNode Node, InstallableElement InstallableElement)
        {
            base.Load(Node, InstallableElement);

            FileId = Node["NexusFileId"].InnerText;
            NexusId = Node["NexusModId"].InnerText;
        }

        private string GetDomain()
        {
            var Domain = string.Empty;

            if (Element is NexusMod)
            {
                Domain = (Element as NexusMod).Domain;
            }
            else if (Element is NexusSoftware)
            {
                Domain = (Element as NexusSoftware).Domain;
            }

            return Domain;
        }

        private async Task<string> GetDownloadLink()
        {
            if (NexusApi.ApiManager.AccountInfo.IsPremium)
            {   
                var Links = await ApiManager.GetDownloadLinks(GetDomain(), System.Convert.ToInt32(NexusId), System.Convert.ToInt32(FileId));

                NexusApi.Responses.ModFileDownloadLink NexusLink = Links.Where(x => x.ShortName == ServiceSingleton.Instances.WorkingInstance.Settings.CDN).FirstOrDefault();

                return NexusLink.Uri.ToString();
            }
            else
            {
                return DownloadLink;
            }
        }

        protected override async Task DoDownload(string Link, DownloadProgressChangedHandler OnProgress)
        {
            var Tsk = Task.Run(async () =>
            {
                try
                {                    
                    await base.DoDownload(await GetDownloadLink(), OnProgress);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            await Tsk;           
        }        

        #endregion
    }
}
