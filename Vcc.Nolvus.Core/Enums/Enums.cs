using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Enums
{
    public enum WebSite { Nexus, Nolvus, Vector, EnbDev, Mega, AFK, Loverslab, Other };
    public enum InstanceCheck { NoInstance, InstancesToResume, InstalledInstances, ErrorInInstances };
    public enum InstallMode { Install, Resume, ResumeInstall, ResumeUpdate, Update, ResumeAndUpdateInstall, ResumeAndUpdateUpdate, View };
    public enum MessageBoxType { Info, Warning, Error, Question };
    public enum InstanceAction { Delete, Reinstall, Cancel }
    public enum ModInstallStatus { Downloading, Hashing, Extracting, UnPacking, Installing, Patching, Archiving }
    public enum ElementAction { None, Add, Update, Move, Remove };
    public enum InstanceInstallStatus { None, Installing, Installed, Updating };
    public enum ModStatus { OK, VersionDiffer, CustomInstalled, Error };
    public enum InstanceCheckStatus { OK, VersionError, NoId }   
    public enum StockGameProcessStep { GameFileInfoLoading, PatchingInfoLoading, GameFilesChecking, GameFilesCopy, GameFilesPatching, PatchGameFile, CheckPatchedGameFile}
}

