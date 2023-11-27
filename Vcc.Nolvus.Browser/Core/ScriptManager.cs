using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Browser.Core
{
    public static class ScriptManager
    {
        public static string ScrollToButton          = "(function () { var el = document.getElementById('slowDownloadButton'); if (el) { el.scrollIntoView({ behavior: 'smooth', block: 'end', inline: 'nearest' }); } })();";
        public static string IsDownloadAvailable     = "(function () { let val = 0; if (document.getElementById('slowDownloadButton') != null) { val = 1 }; return val; })();";
        public static string IsFileDeleted           = "(function() { let val = 0; if (document.getElementsByClassName('info-content') && document.getElementsByClassName('info-content')[0].innerHTML.indexOf('This file has been removed') !== -1) { val = 1 }; return val; })();";
        public static string IsModNotFound           = "(function() { let val = 0; if (document.getElementsByClassName('info-content') && document.getElementsByClassName('info-content')[0].innerHTML.indexOf('Not found') !== -1) { val = 1 }; return val; })();";
        public static string IsLoginNeeded           = "(function() { let val = 0; if (document.getElementsByClassName('replaced-login-link')[0] != null) { val = 1 }; return val; })();";
        public static string NexusManualDownloadInit = "document.getElementById('slowDownloadButton').style.border = '5px dashed green';";
        public static string RedirectToLogin         = "document.getElementsByClassName('replaced-login-link')[0].click();";

        public static string ReadScript(string FileName)
        {
            return System.IO.File.ReadAllText(FileName);
        }

        public static string GetIsDownloadAvailable()
        {            
            return ScriptManager.IsDownloadAvailable;
        }

        public static string GetIsFileDeleted()
        {            
            return ScriptManager.IsFileDeleted;
        }

        public static string GetIsModNotFound()
        {            
            return ScriptManager.IsModNotFound;
        }

        public static string GetIsLoginNeeded()
        {            
            return ScriptManager.IsLoginNeeded;
        }

        public static string GetNexusManualDownloadInit()
        {            
            return ScriptManager.NexusManualDownloadInit;
        }

        public static string GetNexusManualDownload()
        {            
            return ScriptManager.ScrollToButton;
        }

        public static string GetRedirectToLogin()
        {            
            return ScriptManager.RedirectToLogin;
        }

        public static string GetHandleVector(string Url)
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\HandleVector.js").Replace("{0}", Url);
        }

        public static string GetVectorLogin()
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\VectorLogin.js");
        }

        public static string GetVectorDownload(string Url)
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\VectorDownload.js").Replace("{0}", Url);
        }

        public static string GetVectorDownLoadInit(string FileName)
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\VectorDownloadInit.js").Replace("{0}", FileName);
        }

        public static string GetHandleENBDev(string FileName)
        {            
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\HandleENBDev.js").Replace("{0}", FileName);
        }

        public static string GetHandleAFK()
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\HandleAFK.js");
        }

        public static string GetAFKDownLoad()
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\AFKDownLoad.js");
        }

        public static string GetHandleLoverslab()
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\HandleLoverslab.js");
        }

        public static string GetLoverslabDownLoad()
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\LoverslabDownLoad.js");
        }

        public static string GetLoverslabDownLoadInit(string FileName)
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\LoverslabDownloadInit.js").Replace("{0}", FileName);
        }
    }
}
