using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Interfaces;
using System.Security.Cryptography;
using Vcc.Nolvus.Core.Events;
using Vcc.Nolvus.Services.Files.Downloaders;
using Vcc.Nolvus.Services.Files.Extractor;
using Force.Crc32;

namespace Vcc.Nolvus.Services.Files
{    
    public class FileService : IFileService
    {        
        private BaseFileDownloader CreateDownloader(string Url)
        {
            if (Url.Contains("drive.google.com"))
            {
                return new GoogleDriveFileDownloader();
            }
            else if (Url.Contains("mega.nz"))
            {
                return new MegaFileDownloader();
            }
            else
            {
                return new FileDownloader();
            }
        }       

        public async Task DownloadFile(string UrlAddress, string Location, DownloadProgressChangedHandler OnProgress)
        {
            using (var Downloader = CreateDownloader(UrlAddress))
            {
                Downloader.DownloadProgressChanged += OnProgress;
                await Downloader.DownloadFile(UrlAddress, Location);                
            }
        }

        public async Task ExtractFile(string File, string Output, ExtractProgressChangedHandler OnProgress)
        {
            await new FileExtractor().ExtractFile(File, Output, OnProgress);
        }

        public void CopyFiles(string SourcePath, string TargetPath, bool IncludeRoot)
        {            
            if (IncludeRoot)
            {
                Directory.CreateDirectory(TargetPath);
            }

            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(SourcePath, TargetPath));
            }            

            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
            {
                FileInfo FileSource = new FileInfo(newPath);
                FileInfo FileDest = new FileInfo(newPath.Replace(SourcePath, TargetPath));
                FileSource.CopyTo(FileDest.FullName, true);               
            }
        }

        public void RemoveDirectory(string DirectoryPath, bool RemoveDirectory)
        {
            DirectoryInfo _Directory = new DirectoryInfo(DirectoryPath);

            if (_Directory.Exists)
            {
                foreach (FileInfo File in _Directory.GetFiles())
                {
                    File.Delete();
                }

                foreach (DirectoryInfo Directory in _Directory.GetDirectories())
                {
                    Directory.Delete(true);
                }

                if (RemoveDirectory)
                {
                    _Directory.Delete();
                }
            }            
        }

        public async Task<string> GetCRC32(FileInfo File, Action<string, int> Progress)
        {
            return await Task.Run(() =>
            {
                uint r = 0;
                using (var s = File.OpenRead())
                {
                    byte[] buff = new byte[1024];
                    int len = s.Read(buff, 0, buff.Length);
                    r = Crc32Algorithm.Compute(buff, 0, len);
                    uint Counter = 0;
                    int Internal = 0;
                    while ((len = s.Read(buff, 0, buff.Length)) > 0)
                    {
                        r = Crc32Algorithm.Append(r, buff, 0, len);

                        if (Progress != null)
                        {
                            Counter = Counter + 1024;

                            int Percent = System.Convert.ToInt16(Math.Round(((double)Counter / s.Length * 100)));

                            if (Percent > Internal)
                            {
                                Internal = Percent;
                                Progress(File.Name, Percent);
                            }
                        }                        
                    }
                }

                return Convert.ToString(r, 16).ToUpper();
            });

        }

        public FileInfo GetFileFromDirectory(string Directory, string FileName)
        {
            DirectoryInfo Di = new DirectoryInfo(Directory);

            return Di.EnumerateFiles("*.*", SearchOption.AllDirectories).Where(x => x.Name == FileName).FirstOrDefault();
        }
        public List<FileInfo> GetFiles(string Directory)
        {
            return new DirectoryInfo(Directory).GetFiles(".", SearchOption.AllDirectories).ToList();
        }
        public bool FileExists(string Directory, string FileName)
        {
            return GetFileFromDirectory(Directory, FileName) != null;
        }

        public bool FileExists(string Directory, string FileName, out string FullPath)
        {
            FileInfo FileFromDirectory = GetFileFromDirectory(Directory, FileName);
            if (FileFromDirectory != null)
            {
                FullPath = FileFromDirectory.FullName;
                return true;
            }
            FullPath = string.Empty;
            return false;
        }

        public string GetHash(string FileName)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(FileName))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public bool IsDirectoryEmpty(string Dir)
        {
            return Directory.GetFiles(Dir, "*.*", SearchOption.AllDirectories).Length == 0;
        }
    }
}
