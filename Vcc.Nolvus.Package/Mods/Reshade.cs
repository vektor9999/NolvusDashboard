using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Core.Interfaces;



namespace Vcc.Nolvus.Package.Mods
{
    public class Reshade : Mod
    {
        private const string StandardEffect = "https://github.com//crosire/reshade-shaders/archive/refs/heads/slim.zip";
        private const string LegacyEffect = "https://github.com/crosire/reshade-shaders/archive/master.zip";
        private const string LegacyEffectv5 = "https://github.com/crosire/reshade-shaders/archive/refs/heads/legacy.zip";
        private const string SweetFX = "https://github.com/CeeJayDK/SweetFX/archive/refs/heads/master.zip";
        private const string AstrayFX = "https://github.com/BlueSkyDefender/AstrayFX/archive/refs/heads/master.zip";
        private const string Prod80Effect = "https://github.com/prod80/prod80-ReShade-Repository/archive/refs/heads/master.zip";

        
        #region Reshade Ini

        private const string ReshadeIni = @"[APP]
Force10BitFormat=0
ForceFullscreen=0
ForceResolution=0,0
ForceVsync=0
ForceWindowed=0

[GENERAL]
EffectSearchPaths=.\reshade-shaders\Shaders,.\reshade-shaders\Shaders\qUINT,.\reshade-shaders\Shaders\PD80,.\reshade-shaders\Shaders\Depth3D,.\reshade-shaders\Shaders\AstrayFX,.\reshade-shaders\Shaders\OtisFX,.\reshade-shaders\Shaders\Daodan,.\reshade-shaders\Shaders\Fubax,.\reshade-shaders\Shaders\CorgiFX
IntermediateCachePath={0}
NoDebugInfo=0
NoEffectCache=0
NoReloadOnInit=0
NoReloadOnInitForNonVR=0
PerformanceMode=0
PreprocessorDefinitions=RESHADE_DEPTH_LINEARIZATION_FAR_PLANE=1000.0,RESHADE_DEPTH_INPUT_IS_UPSIDE_DOWN=0,RESHADE_DEPTH_INPUT_IS_REVERSED=1,RESHADE_DEPTH_INPUT_IS_LOGARITHMIC=0
PresetPath=.\Nolvus Reshade.ini
PresetTransitionDuration=1000
SkipLoadingDisabledEffects=0
TextureSearchPaths=.\reshade-shaders\Textures

[INPUT]
ForceShortcutModifiers=1
InputProcessing=2
KeyEffects=46,0,0,0
KeyNextPreset=0,0,0,0
KeyOverlay=36,0,0,0
KeyPerformanceMode=0,0,0,0
KeyPreviousPreset=0,0,0,0
KeyReload=0,0,0,0
KeyScreenshot=123,0,0,0

[OVERLAY]
ClockFormat=0
FPSPosition=1
NoFontScaling=1
SaveWindowState=0
ShowClock=0
ShowForceLoadEffectsButton=1
ShowFPS=0
ShowFrameTime=0
ShowScreenshotMessage=1
TutorialProgress=4
VariableListHeight=656.000000
VariableListUseTabs=0

[SCREENSHOT]
ClearAlpha=1
FileFormat=1
FileNaming=%AppName% %Date% %Time%
JPEGQuality=90
PostSaveCommand=
PostSaveCommandArguments=""%TargetPath%""
PostSaveCommandNoWindow=0
PostSaveCommandWorkingDirectory=.\
SaveBeforeShot=0
SaveOverlayShot=0
SavePath={1}
SavePresetFile=0

[STYLE]
Alpha=1.000000
ChildRounding=0.000000
ColFPSText=1.000000,1.000000,0.784314,1.000000
EditorFont=ProggyClean.ttf
EditorFontSize=13
EditorStyleIndex=0
Font=ProggyClean.ttf
FontSize=13
FPSScale=1.000000
FrameRounding=0.000000
GrabRounding=0.000000
PopupRounding=0.000000
ScrollbarRounding=0.000000
StyleIndex=2
TabRounding=4.000000
WindowRounding=0.000000";

        #endregion

      
        private async Task DownloadAndExtractShaders(string Url, string ShaderName)
        {

            var Tsk = Task.Run(async() => 
            {
                try
                {
                    var ShaderFile = Path.Combine(ServiceSingleton.Folders.DownloadDirectory, ShaderName + ".zip");

                    ServiceSingleton.Logger.Log(string.Format("Downloading shader {0}", ShaderName));
                    await ServiceSingleton.Files.DownloadFile(Url, ShaderFile, DownloadingProgress);

                    ServiceSingleton.Logger.Log(string.Format("Extracting shader {0}", ShaderName));
                    await ServiceSingleton.Files.ExtractFile(ShaderFile, Path.Combine(ServiceSingleton.Folders.ExtractDirectory, ShaderName), ExtractingProgress);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            });

            await Tsk;                        
        }

        
        protected override async Task DoExtract()
        {
            var Tsk = Task.Run(() => 
            {
                try
                {
                    Process ReshadeProcess = new Process
                    {
                        StartInfo = { WorkingDirectory = ServiceSingleton.Folders.DownloadDirectory, FileName = "cmd.exe", CreateNoWindow = true, UseShellExecute = false, WindowStyle = ProcessWindowStyle.Hidden }
                    };

                    string Args = string.Format("\"" + Path.Combine(ServiceSingleton.Folders.LibDirectory, "7z.exe") + "\" e \"{0}\" -o\"{1}\"", this.Files.First().LocationFileName, Path.Combine(ServiceSingleton.Folders.ExtractDirectory, Name));
                    
                    ReshadeProcess.StartInfo.Arguments = "/c \"" + Args + "\"";
                    ReshadeProcess.StartInfo.RedirectStandardOutput = true;
                    ReshadeProcess.StartInfo.RedirectStandardError = true;

                    List<string> Output = new List<string>();

                    ReshadeProcess.OutputDataReceived += delegate (object s, DataReceivedEventArgs e) {
                        if (e.Data != null)
                        {
                            Output.Add(e.Data);
                        }
                    };
                    ReshadeProcess.ErrorDataReceived += delegate (object s, DataReceivedEventArgs e) {
                        if (e.Data != null)
                        {
                            Output.Add(e.Data);
                        }
                    };

                    ReshadeProcess.Start();
                    ReshadeProcess.BeginOutputReadLine();
                    ReshadeProcess.BeginErrorReadLine();
                    ReshadeProcess.WaitForExit();

                    if (ReshadeProcess.ExitCode != 0)
                    {
                        throw new Exception("Error during reshade binaries extraction!" + Environment.NewLine + string.Join(Environment.NewLine, Output.ToArray()));
                    }                    
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            });

            await Tsk;
        }

        protected override async Task DoCopy()
        {
            var Tsk = Task.Run(async () => 
            {
                try
                {
                    try
                    {
                        INolvusInstance Instance = ServiceSingleton.Instances.WorkingInstance;

                        var ShadersDir = Path.Combine(Instance.StockGame, "reshade-shaders");

                        Directory.CreateDirectory(ShadersDir);

                        ServiceSingleton.Logger.Log("Installing reshade binaries");

                        File.Copy(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, Name, "ReShade64.dll"), Path.Combine(Instance.StockGame, "dxgi.dll"), true);

                        ServiceSingleton.Logger.Log("Reshade binaries installed");

                        await DownloadAndExtractShaders(StandardEffect, "Standard");
                        await DownloadAndExtractShaders(LegacyEffectv5, "Legacy");
                        await DownloadAndExtractShaders(SweetFX, "SweetFX");
                        await DownloadAndExtractShaders(AstrayFX, "AstrayFX");
                        await DownloadAndExtractShaders(Prod80Effect, "Prod80");

                        ServiceSingleton.Logger.Log("Copying Shaders");
                        ServiceSingleton.Files.CopyFiles(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "Standard", "reshade-shaders-slim", "Shaders"), Path.Combine(ShadersDir, "Shaders"), true);
                        ServiceSingleton.Files.CopyFiles(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "Standard", "reshade-shaders-slim", "Textures"), Path.Combine(ShadersDir, "Textures"), true);
                        CopyingProgress(1, 5);

                        ServiceSingleton.Files.CopyFiles(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "Legacy", "reshade-shaders-legacy", "Shaders"), Path.Combine(ShadersDir, "Shaders"), true);
                        ServiceSingleton.Files.CopyFiles(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "Legacy", "reshade-shaders-legacy", "Textures"), Path.Combine(ShadersDir, "Textures"), true);
                        CopyingProgress(2, 5);

                        ServiceSingleton.Files.CopyFiles(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "SweetFX", "SweetFX-master", "Shaders"), Path.Combine(ShadersDir, "Shaders"), true);
                        ServiceSingleton.Files.CopyFiles(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "SweetFX", "SweetFX-master", "Textures"), Path.Combine(ShadersDir, "Textures"), true);
                        CopyingProgress(3, 5);

                        ServiceSingleton.Files.CopyFiles(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "AstrayFX", "AstrayFX-master", "Shaders"), Path.Combine(ShadersDir, "Shaders", "AstrayFX"), true);
                        ServiceSingleton.Files.CopyFiles(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "AstrayFX", "AstrayFX-master", "Textures"), Path.Combine(ShadersDir, "Textures"), true);
                        CopyingProgress(4, 5);

                        ServiceSingleton.Files.CopyFiles(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "Prod80", "prod80-ReShade-Repository-master", "Shaders"), Path.Combine(ShadersDir, "Shaders", "PD80"), true);
                        ServiceSingleton.Files.CopyFiles(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "Prod80", "prod80-ReShade-Repository-master", "Textures"), Path.Combine(ShadersDir, "Textures"), true);
                        CopyingProgress(5, 5);

                        string ShotsDir = Path.Combine(Instance.InstallDir, "SHOTS");

                        Directory.CreateDirectory(ShotsDir);
                        File.WriteAllText(Path.Combine(Instance.StockGame, "ReShade.ini"), string.Format(ReshadeIni, Path.GetTempPath(), ShotsDir));

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                finally
                {
                    File.Delete(Path.Combine(ServiceSingleton.Folders.DownloadDirectory, "Standard.zip"));
                    File.Delete(Path.Combine(ServiceSingleton.Folders.DownloadDirectory, "Legacy.zip"));
                    File.Delete(Path.Combine(ServiceSingleton.Folders.DownloadDirectory, "SweetFX.zip"));
                    File.Delete(Path.Combine(ServiceSingleton.Folders.DownloadDirectory, "AstrayFX.zip"));
                    File.Delete(Path.Combine(ServiceSingleton.Folders.DownloadDirectory, "Prod80.zip"));
                    ServiceSingleton.Files.RemoveDirectory(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "Standard"), true);
                    ServiceSingleton.Files.RemoveDirectory(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "Legacy"), true);
                    ServiceSingleton.Files.RemoveDirectory(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "SweetFX"), true);
                    ServiceSingleton.Files.RemoveDirectory(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "AstrayFX"), true);
                    ServiceSingleton.Files.RemoveDirectory(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, "Prod80"), true);
                    ServiceSingleton.Files.RemoveDirectory(Path.Combine(ServiceSingleton.Folders.ExtractDirectory, Name), true);
                }
            });

            await Tsk;
        }       
    }
}
