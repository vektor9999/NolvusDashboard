using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Vcc.Nolvus.Core.Services;
using System.Threading.Tasks;
using ZetaLongPaths;

namespace Vcc.Nolvus.Package.Mods
{
    public class BsaUnPacking
    {
        public string FileName { get; set; }

        private ZlpFileInfo GetBsaToUnpack(string ExtractDir, string BSAFile)
        {            
            return  ServiceSingleton.Files.GetFiles(ExtractDir).Where(x => x.Name == BSAFile).FirstOrDefault();
        }

        public async Task UnPack(string ExtractDir)
        {
            var Tsk = Task.Run(() => 
            {
                Process UnPackingProcess = new Process();
                var BSArchDir = Path.Combine(ServiceSingleton.Instances.WorkingInstance.InstallDir, "TOOLS", "BSArch");
                var BSAFile = GetBsaToUnpack(ExtractDir, FileName);

                if (BSAFile != null)
                {
                    UnPackingProcess.StartInfo.WorkingDirectory = BSArchDir;
                    UnPackingProcess.StartInfo.FileName = "cmd.exe";
                    UnPackingProcess.StartInfo.CreateNoWindow = true;
                    UnPackingProcess.StartInfo.UseShellExecute = false;
                    UnPackingProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    
                    string CommandLine = string.Format("\"" + Path.Combine(BSArchDir, "bsarch.exe") + "\" unpack \"{0}\" \"{1}\"", BSAFile.FullName, BSAFile.DirectoryName);

                    ServiceSingleton.Logger.Log(string.Format("Unpacking command line : {0}", CommandLine));

                    UnPackingProcess.StartInfo.Arguments = "/c \"" + CommandLine + "\"";

                    UnPackingProcess.StartInfo.RedirectStandardOutput = true;
                    UnPackingProcess.StartInfo.RedirectStandardError = true;

                    List<String> Output = new List<string>();

                    UnPackingProcess.OutputDataReceived += new DataReceivedEventHandler((s, e) =>
                    {
                        if (e.Data != null)
                        {
                            Output.Add((string)e.Data);
                        }
                    });

                    UnPackingProcess.ErrorDataReceived += new DataReceivedEventHandler((s, e) =>
                    {
                        if (e.Data != null)
                        {
                            Output.Add((String)e.Data);
                        }
                    });

                    UnPackingProcess.Start();
                    UnPackingProcess.BeginOutputReadLine();
                    UnPackingProcess.BeginErrorReadLine();

                    UnPackingProcess.WaitForExit();

                    if (UnPackingProcess.ExitCode == 0)
                    {
                        File.Delete(BSAFile.FullName);
                    }
                    else
                    {
                        throw new Exception("Failed to unpack file : " + FileName + "==>" + String.Join(Environment.NewLine, Output.ToArray()));
                    }
                }
                else
                {
                    throw new Exception("Failed to unpack file : " + FileName + "==> File not found");
                }
                
            });

            await Tsk;

           
        }
    }
}
