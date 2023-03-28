using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Browser.Core
{
    public static class ScriptManager
    {
        public static string ReadScript(string FileName)
        {
            return System.IO.File.ReadAllText(FileName);
        }

        public static string GetIsDownloadAvailable()
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\IsDownloadAvailable.js");
        }

        public static string GetIsFileDeleted()
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\IsFileDeleted.js");
        }

        public static string GetIsModNotFound()
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\IsModNotFound.js");
        }

        public static string GetIsLoginNeeded()
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\IsLoginNeeded.js");
        }

        public static string GetNexusManualDownloadInit()
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\NexusManualDownloadInit.js");
        }

        public static string GetNexusManualDownload()
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\NexusManualDownload.js");
        }

        public static string GetRedirectToLogin()
        {
            return ScriptManager.ReadScript(AppDomain.CurrentDomain.BaseDirectory + "\\Scripts\\RedirectToLogin.js");
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
