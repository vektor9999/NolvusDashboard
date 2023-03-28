using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Vcc.Nolvus.Utils.Events;
using Force.Crc32;

namespace Vcc.Nolvus.Utils
{
    public static class FileInfoExtension
    {
        public static void CopyTo(this FileInfo file, FileInfo destination, Action<string, int> progressCallback)
        {
            const int bufferSize = 1024 * 1024;
            byte[] buffer = new byte[bufferSize], buffer2 = new byte[bufferSize];
            bool swap = false;
            int progress = 0, reportedProgress = 0, read = 0;
            long len = file.Length;
            float flen = len;
            Task writer = null;
            using (var source = file.OpenRead())
            using (var dest = destination.OpenWrite())
            {
                dest.SetLength(source.Length);
                for (long size = 0; size < len; size += read)
                {
                    if ((progress = ((int)((size / flen) * 100))) != reportedProgress)
                        progressCallback(file.Name, reportedProgress = progress);
                    read = source.Read(swap ? buffer : buffer2, 0, bufferSize);
                    writer?.Wait();
                    writer = dest.WriteAsync(swap ? buffer : buffer2, 0, read);
                    swap = !swap;
                }
                writer?.Wait();
            }
        }
    }

    public static class DirectoryUtil
    {
        static event DirectoryCopyingEventHandler DirectoryCopyingEvent;
        static event DirectoryDeletingEventHandler DirectoryDeletingEvent;

        public static event DirectoryCopyingEventHandler DirectoryCopying
        {
            add
            {
                if (DirectoryCopyingEvent != null)
                {
                    lock (DirectoryCopyingEvent)
                    {
                        DirectoryCopyingEvent += value;
                    }
                }
                else
                {
                    DirectoryCopyingEvent = value;
                }
            }
            remove
            {
                if (DirectoryCopyingEvent != null)
                {
                    lock (DirectoryCopyingEvent)
                    {
                        DirectoryCopyingEvent -= value;
                    }
                }
            }
        }

        public static event DirectoryDeletingEventHandler DirectoryDeleting
        {
            add
            {
                if (DirectoryDeletingEvent != null)
                {
                    lock (DirectoryDeletingEvent)
                    {
                        DirectoryDeletingEvent += value;
                    }
                }
                else
                {
                    DirectoryDeletingEvent = value;
                }
            }
            remove
            {
                if (DirectoryCopyingEvent != null)
                {
                    lock (DirectoryDeletingEvent)
                    {
                        DirectoryDeletingEvent -= value;
                    }
                }
            }
        }

        private static double SumOfOverallRelativePercent;
        private static int InternalOverallProgression;

        private static void DirectoryCopyProgress(int Value)
        {
            DirectoryCopyingEventHandler Handler = DirectoryUtil.DirectoryCopyingEvent;
            DirectoryProgressEvent Event = new DirectoryProgressEvent(Value);
            if (Handler != null) Handler(null, Event);
        }

        private static void DirectoryDeleteProgress(int Value, string FileName)
        {
            DirectoryDeletingEventHandler Handler = DirectoryUtil.DirectoryDeletingEvent;
            DirectoryDeletingProgressEvent Event = new DirectoryDeletingProgressEvent(Value, FileName);
            if (Handler != null) Handler(null, Event);
        }

        private static double DoCalculate(int WorkCount)
        {
            if (WorkCount > 0)
            {
                if (WorkCount <= 100)
                {
                    return (100 / WorkCount);
                }
                else
                {
                    return (100 / (double)WorkCount);

                }
            }

            return 0;
        }

        private static void CalculateCopyOverAll(int WorkCount)
        {
            int Progress = 0;

            double RelativePercent = DirectoryUtil.DoCalculate(WorkCount);

            DirectoryUtil.SumOfOverallRelativePercent = DirectoryUtil.SumOfOverallRelativePercent + RelativePercent;

            if (RelativePercent < 1)
            {
                if (DirectoryUtil.SumOfOverallRelativePercent >= DirectoryUtil.InternalOverallProgression)
                {
                    Progress = Progress + DirectoryUtil.InternalOverallProgression;
                    DirectoryUtil.DirectoryCopyProgress(Progress);
                    DirectoryUtil.InternalOverallProgression++;
                }
            }
            else
            {
                Progress = Progress + (int)DirectoryUtil.SumOfOverallRelativePercent;
                DirectoryUtil.DirectoryCopyProgress(Progress);
            }
        }

        private static void CalculateDeleteOverAll(int WorkCount, string FileName)
        {
            int Progress = 0;

            double RelativePercent = DirectoryUtil.DoCalculate(WorkCount);

            DirectoryUtil.SumOfOverallRelativePercent = DirectoryUtil.SumOfOverallRelativePercent + RelativePercent;

            if (RelativePercent < 1)
            {
                if (DirectoryUtil.SumOfOverallRelativePercent >= DirectoryUtil.InternalOverallProgression)
                {
                    Progress = Progress + DirectoryUtil.InternalOverallProgression;
                    DirectoryUtil.DirectoryDeleteProgress(Progress, FileName);
                    DirectoryUtil.InternalOverallProgression++;
                }
            }
            else
            {
                Progress = Progress + (int)DirectoryUtil.SumOfOverallRelativePercent;
                DirectoryUtil.DirectoryDeleteProgress(Progress, FileName);
            }
        }

        public static void CopyFilesRecursively(string SourcePath, string TargetPath, bool IncludeRoot, Action<string, int> Progress)
        {
            DirectoryUtil.SumOfOverallRelativePercent = 0;
            DirectoryUtil.InternalOverallProgression = 0;

            if (IncludeRoot)
            {
                Directory.CreateDirectory(TargetPath);
            }

            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(SourcePath, TargetPath));
            }

            int TotalFiles = Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories).Count();

            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
            {
                FileInfo FileSource = new FileInfo(newPath);
                FileInfo FileDest = new FileInfo(newPath.Replace(SourcePath, TargetPath));

                if (Progress != null)
                {
                    FileSource.CopyTo(FileDest, Progress);

                    DirectoryUtil.CalculateCopyOverAll(TotalFiles);
                }
                else
                {
                    FileSource.CopyTo(FileDest.FullName, true);
                }

            }
        }

        public static void CopyFilesRecursively(string SourcePath, string TargetPath, bool IncludeRoot)
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

        public static bool IsDirectoryEmpty(string Dir)
        {
            return Directory.GetFiles(Dir, "*.*", SearchOption.AllDirectories).Length == 0;
        }

        public static void Clean(string DirectoryPath, bool RemoveDirectory)
        {
            DirectoryInfo _Directory = new DirectoryInfo(DirectoryPath);

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

        public static void Clean2(string DirectoryPath, bool RemoveDirectory)
        {
            DirectoryUtil.SumOfOverallRelativePercent = 0;
            DirectoryUtil.InternalOverallProgression = 0;

            string[] Files = Directory.GetFiles(DirectoryPath, "*.*", SearchOption.AllDirectories);

            foreach (string File in Files)
            {
                FileInfo FileInfo = new FileInfo(File);

                FileInfo.Delete();
                DirectoryUtil.CalculateDeleteOverAll(Files.Length, FileInfo.Name);
            }

            DirectoryInfo _Directory = new DirectoryInfo(DirectoryPath);

            foreach (DirectoryInfo Directory in _Directory.GetDirectories())
            {
                Directory.Delete(true);
            }

            if (RemoveDirectory)
            {
                _Directory.Delete();
            }
        }

        public static FileInfo GetFileFromDirectory(string Directory, string FileName)
        {
            DirectoryInfo Di = new DirectoryInfo(Directory);

            return Di.EnumerateFiles("*.*", SearchOption.AllDirectories).Where(x => x.Name == FileName).FirstOrDefault();
        }

        public static List<FileInfo> GetFiles(string Directory)
        {
            return new DirectoryInfo(Directory).GetFiles(".", SearchOption.AllDirectories).ToList();
        }

        public static string GetCRC32(FileInfo File)
        {
            uint r = 0;
            using (var s = File.OpenRead())
            {
                byte[] buff = new byte[1024];
                int len = s.Read(buff, 0, buff.Length);
                r = Crc32Algorithm.Compute(buff, 0, len);
                while ((len = s.Read(buff, 0, buff.Length)) > 0)
                    r = Crc32Algorithm.Append(r, buff, 0, len);
            }

            return Convert.ToString(r, 16).ToUpper();
        }

        public static async Task<string> GetCRC32(FileInfo File, Action<string, int> Progress)
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
                        Counter = Counter + 1024;

                        int Percent = System.Convert.ToInt16(Math.Round(((double)Counter / s.Length * 100)));

                        if (Percent > Internal)
                        {
                            Internal = Percent;
                            Progress(File.Name, Percent);
                        }
                    }
                }

                return Convert.ToString(r, 16).ToUpper();
            });
           
        }

        //public static string GetCRC32(string File)
        //{
        //    return GetCRC32(new FileInfo(File));
        //}

        //public static bool FileCheckSumCheck(string Directory, string FileName, string CRC32, out string FullPath)
        //{
        //    FileInfo FileFromDirectory = GetFileFromDirectory(Directory, FileName);
        //    if (FileFromDirectory != null)
        //    {
        //        FullPath = FileFromDirectory.FullName;
        //        return (GetCRC32(FileFromDirectory) == CRC32);
        //    }
        //    FullPath = string.Empty;
        //    return false;
        //}

        public static bool FileExists(string Directory, string FileName)
        {
            return GetFileFromDirectory(Directory, FileName) != null;
        }

        public static bool FileExists(string Directory, string FileName, out string FullPath)
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

        public static bool FileExistsWithSize(string Directory, string FileName, int Size, out string FilePath)
        {
            FilePath = string.Empty;           

            if (Directory != string.Empty)
            {
                string[] Files = System.IO.Directory.GetFiles(Directory, "*.*", SearchOption.AllDirectories);

                foreach (string File in Files)
                {
                    FileInfo FileInfo = new FileInfo(File);

                    if (FileInfo.Name == FileName)
                    {
                        double FileSizeInKb = (double)FileInfo.Length / 1024;

                        double FileSizeInKbCeil = Math.Ceiling(FileSizeInKb);
                        double FileSizeInKbRound = Math.Round(FileSizeInKb);

                        if (Size == FileSizeInKbCeil || Size == FileSizeInKbRound)
                        {
                            FilePath = FileInfo.FullName;
                            return true;
                        }
                    }
                }

                return false;
            }
            else
            {
                return false;
            }                               
        }    

        public static bool FileExistsWithCRC(string Directory, string CRC32, out string FilePath)
        {
            FilePath = string.Empty;
         
            if (Directory != string.Empty)
            {
                string[] Files = System.IO.Directory.GetFiles(Directory, "*.*", SearchOption.AllDirectories);

                foreach (string File in Files)
                {
                    FileInfo FileInfo = new FileInfo(File);

                    if (DirectoryUtil.GetCRC32(FileInfo) == CRC32)
                    {
                        FilePath = FileInfo.FullName;
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
        }       

    }
}
