﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Events;
using ZetaLongPaths;

namespace Vcc.Nolvus.Core.Interfaces
{
    public interface IFileService : INolvusService
    {
        Task DownloadFile(string UrlAddress, string Location, DownloadProgressChangedHandler OnProgress);
        Task ExtractFile(string File, string Output, ExtractProgressChangedHandler OnProgress);
        void CopyFiles(string SourcePath, string TargetPath, bool IncludeRoot);
        List<ZlpFileInfo> GetFiles(string Directory);
        List<string> GetFilesPath(string Directory);
        void RemoveDirectory(string DirectoryPath, bool RemoveDirectory);
        Task<string> GetCRC32(ZlpFileInfo File, Action<string, int> Progress);
        ZlpFileInfo GetFileFromDirectory(string Directory, string FileName);
        bool FileExists(string Directory, string FileName);
        bool FileExists(string Directory, string FileName, out string FullPath);
        string GetHash(string FileName);
        bool IsDirectoryEmpty(string Dir);
        Task AuthenticateToMegaApi(string Username, string password);
    }
}
