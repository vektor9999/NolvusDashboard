using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ZetaLongPaths;

namespace Vcc.Nolvus.Core.Events
{
    public static class FileInfoExtension
    {
        public static void CopyTo(this ZlpFileInfo File, ZlpFileInfo Destination, Action<string, int> progressCallback)
        {
            const int bufferSize = 1024 * 1024;
            byte[] buffer = new byte[bufferSize], buffer2 = new byte[bufferSize];
            bool swap = false;
            int progress = 0, reportedProgress = 0, read = 0;
            long len = File.Length;
            float flen = len;
            Task writer = null;
            using (var source = File.OpenRead())
            using (var dest = Destination.OpenWrite())
            {
                dest.SetLength(source.Length);
                for (long size = 0; size < len; size += read)
                {
                    if ((progress = ((int)((size / flen) * 100))) != reportedProgress)
                        progressCallback(File.Name, reportedProgress = progress);
                    read = source.Read(swap ? buffer : buffer2, 0, bufferSize);
                    writer?.Wait();
                    writer = dest.WriteAsync(swap ? buffer : buffer2, 0, read);
                    swap = !swap;
                }
                writer?.Wait();
            }
        }
    }

    public delegate void DownloadProgressChangedHandler(object sender, DownloadProgress progress);
    public delegate void ExtractProgressChangedHandler(object sender, ExtractProgress progress);

    public abstract class BaseProgress
    {
        public string FileName;
        public virtual int ProgressPercentage { get; set; }
    }
    public class DownloadProgress : BaseProgress
    {
        public long BytesReceived, TotalBytesToReceive;
        public double Speed;
        public string BytesReceivedAsString, TotalBytesToReceiveAsString;
    }

    public class HashProgress : BaseProgress
    {

    }

    public class ExtractProgress : BaseProgress
    {
    }

    public class CopyProgress : BaseProgress
    {
        public int ItemNumber;
        public int MaxItem;

        public override int ProgressPercentage
        {
            get
            {
                if (MaxItem > 0)
                {
                    return System.Convert.ToInt16(Math.Round(((double)ItemNumber / MaxItem * 100)));
                }

                return 0;
            }
        }
    }

    public class UnPackProgress : CopyProgress
    {

    }

    public class PatchingProgress : CopyProgress
    {

    }

    public class ArchvingProgress : BaseProgress
    {

    }
}
