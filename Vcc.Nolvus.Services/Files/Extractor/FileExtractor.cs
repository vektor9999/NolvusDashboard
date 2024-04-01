using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Core.Services;

namespace Vcc.Nolvus.Services.Files.Extractor
{
    public class FileExtractor
    {
        private readonly ExtractProgress ExtractProgress;
        public event ExtractProgressChangedHandler ExtractProgressChanged;
        private string FileName;
        public FileExtractor()
        {
            ExtractProgress = new ExtractProgress();
        }

        private void TriggerProgressEvent(int Percent, string FileName)
        {
            if (ExtractProgressChanged != null)
            {
                ExtractProgress.ProgressPercentage = Percent;
                ExtractProgress.FileName = FileName;

                ExtractProgressChanged(this, ExtractProgress);
            }
        }

        public async Task ExtractFile(string File, string Output, ExtractProgressChangedHandler OnProgress)
        {
            var Tsk = Task.Run(() => 
            {
                FileName = new FileInfo(File).Name;

                try
                {
                    ExtractProgressChanged += OnProgress;

                    Process SevenZipProcess = new Process
                    {
                        StartInfo = { WorkingDirectory = ServiceSingleton.Folders.LibDirectory, FileName = "cmd.exe", CreateNoWindow = true, UseShellExecute = false, WindowStyle = ProcessWindowStyle.Hidden }
                    };

                    var Args = string.Format("\"" + Path.Combine(ServiceSingleton.Folders.LibDirectory, "7z.exe") + "\" x -bsp1 -y \"{0}\" -o\"{1}\" -mmt=off", File, Output);

                    SevenZipProcess.StartInfo.Arguments = "/c \"" + Args + "\"";
                    SevenZipProcess.StartInfo.RedirectStandardOutput = true;
                    SevenZipProcess.StartInfo.RedirectStandardError = true;

                    List<string> ErrorOutput = new List<string>();

                    SevenZipProcess.OutputDataReceived += delegate (object s, DataReceivedEventArgs e) {
                        if (e.Data != null && e.Data.Length >= 4 && e.Data[3] == '%')
                        {
                            var PercentAsInt = 0;

                            if (int.TryParse(e.Data.Substring(0, 3), out PercentAsInt))
                            {
                                TriggerProgressEvent(PercentAsInt, FileName);
                            }
                        }
                    };
                    SevenZipProcess.ErrorDataReceived += delegate (object s, DataReceivedEventArgs e) {
                        if (e.Data != null)
                        {
                            ErrorOutput.Add(e.Data);
                        }
                    };

                    SevenZipProcess.Start();
                    SevenZipProcess.BeginOutputReadLine();
                    SevenZipProcess.BeginErrorReadLine();
                    SevenZipProcess.WaitForExit();

                    if (SevenZipProcess.ExitCode != 0)
                    {
                        throw new Exception(string.Format("Error during file extraction {0} with error {1}!", FileName, string.Join(Environment.NewLine, Output.ToArray())));
                    }
                    else
                    {
                        TriggerProgressEvent(100, FileName);
                    }
                }
                catch (Exception ex)
                {
                    ServiceSingleton.Logger.Log(ex.Message);
                    throw ex;
                }

            });

            await Tsk;
        }
    }
}
