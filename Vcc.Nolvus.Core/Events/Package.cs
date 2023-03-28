using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Enums;
using Vcc.Nolvus.Core.Interfaces;

namespace Vcc.Nolvus.Core.Events
{    
    public class ProgressInfo
    {
        private int _PercentDone;
        private string _FileName;

        public int PercentDone { get { return _PercentDone; } }
        public string FileName { get { return _FileName; } }

        public ProgressInfo(string FileName, int PercentDone)
        {
            _FileName = FileName;
            _PercentDone = PercentDone;
        }
    }
    public class HashInfo : ProgressInfo
    {
        public HashInfo(string FileName, int PercentDone)
            : base(FileName, PercentDone)
        {

        }
    }
    public class DownloadInfo : ProgressInfo
    {
        private double _Speed;
        private string _BytesReceivedAsString, _TotalBytesToReceiveAsString;
        private long _BytesReceived, _TotalBytesToReceive;

        public double Speed { get { return _Speed; } }
        public string BytesReceivedAsString { get { return _BytesReceivedAsString; } }
        public string TotalBytesToReceiveAsString { get { return _TotalBytesToReceiveAsString; } }
        public long BytesReceived { get { return _BytesReceived; } }
        public long TotalBytesToReceive { get { return _TotalBytesToReceive; } }

        public DownloadInfo(string FileName, int PercentDone, double Speed, string BytesReceivedAsString, string TotalBytesToReceiveAsString, long BytesReceived, long TotalBytesToReceive)
            : base(FileName, PercentDone)
        {
            _Speed = Speed;
            _BytesReceivedAsString = BytesReceivedAsString;
            _BytesReceived = BytesReceived;
            _TotalBytesToReceiveAsString = TotalBytesToReceiveAsString;
            _TotalBytesToReceive = TotalBytesToReceive;
        }
    }
    public class ExtractInfo : ProgressInfo
    {
        public ExtractInfo(string FileName, int PercentDone)
            : base(FileName, PercentDone)
        {

        }
    }
    public class CopyInfo : ProgressInfo
    {
        private int _Value, _MaxValue;

        public int ItemNumber { get { return _Value; } }
        public int MaxItem { get { return _MaxValue; } }

        public CopyInfo(string FileName, int PercentDone, int ItemNumber, int MaxItem)
            : base(FileName, PercentDone)
        {
            _Value = ItemNumber;
            _MaxValue = MaxItem;
        }
    }
    public class UnPackInfo : CopyInfo
    {
        public UnPackInfo(string FileName, int PercentDone, int ItemNumber, int MaxItem)
            : base(FileName, PercentDone, ItemNumber, MaxItem)
        {
        }
    }
    public class PatchingInfo : CopyInfo
    {
        public PatchingInfo(string FileName, int PercentDone, int ItemNumber, int MaxItem)
            : base(FileName, PercentDone, ItemNumber, MaxItem)
        {
        }
    }
    public class ArchivingInfo : ProgressInfo
    {
        public ArchivingInfo(string FileName, int PercentDone)
            : base(FileName, PercentDone)
        {

        }
    }
    public class ModInstallProgressEvent : EventArgs
    {
        #region Fields    
        private ModInstallStatus _Status;
        private DownloadInfo _DownloadInfo;
        private HashInfo _HashInfo;
        private ExtractInfo _ExtractInfo;
        private CopyInfo _CopyInfo;
        private UnPackInfo _UnPackInfo;
        private PatchingInfo _PatchingInfo;
        private ArchivingInfo _ArchivingInfo;
        #endregion

        #region Properties
        public ModInstallStatus Status { get { return _Status; } }
        public DownloadInfo DownloadInfo { get { return _DownloadInfo; } }
        public HashInfo HashInfo { get { return _HashInfo; } }
        public ExtractInfo ExtractInfo { get { return _ExtractInfo; } }
        public CopyInfo CopyInfo { get { return _CopyInfo; } }
        public UnPackInfo UnPackInfo { get { return _UnPackInfo; } }
        public PatchingInfo PatchingInfo { get { return _PatchingInfo; } }
        public ArchivingInfo ArchivingInfo { get { return _ArchivingInfo; } }
        #endregion

        public ModInstallProgressEvent(DownloadProgress Progress)
        {
            _Status = ModInstallStatus.Downloading;
            _DownloadInfo = new DownloadInfo(Progress.FileName, Progress.ProgressPercentage, Progress.Speed, Progress.BytesReceivedAsString, Progress.TotalBytesToReceiveAsString, Progress.BytesReceived, Progress.TotalBytesToReceive);
            _HashInfo = new HashInfo(string.Empty, 0);
            _ExtractInfo = new ExtractInfo(string.Empty, 0);
            _CopyInfo = new CopyInfo(string.Empty, 0, 0, 0);
            _UnPackInfo = new UnPackInfo(string.Empty, 0, 0, 0);
            _PatchingInfo = new PatchingInfo(string.Empty, 0, 0, 0);
            _ArchivingInfo = new ArchivingInfo(string.Empty, 0);
        }
        public ModInstallProgressEvent(HashProgress Progress)
        {
            _Status = ModInstallStatus.Hashing;
            _DownloadInfo = new DownloadInfo(string.Empty, 0, 0, "0", "0", 0, 0);
            _HashInfo = new HashInfo(Progress.FileName, Progress.ProgressPercentage);
            _ExtractInfo = new ExtractInfo(string.Empty, 0);
            _CopyInfo = new CopyInfo(string.Empty, 0, 0, 0);
            _UnPackInfo = new UnPackInfo(string.Empty, 0, 0, 0);
            _PatchingInfo = new PatchingInfo(string.Empty, 0, 0, 0);
            _ArchivingInfo = new ArchivingInfo(string.Empty, 0);
        }
        public ModInstallProgressEvent(ExtractProgress Progress)
        {
            _Status = ModInstallStatus.Extracting;
            _DownloadInfo = new DownloadInfo(string.Empty, 0, 0, "0", "0", 0, 0);
            _HashInfo = new HashInfo(string.Empty, 0);
            _ExtractInfo = new ExtractInfo(Progress.FileName, Progress.ProgressPercentage);
            _CopyInfo = new CopyInfo(string.Empty, 0, 0, 0);
            _UnPackInfo = new UnPackInfo(string.Empty, 0, 0, 0);
            _PatchingInfo = new PatchingInfo(string.Empty, 0, 0, 0);
            _ArchivingInfo = new ArchivingInfo(string.Empty, 0);
        }
        public ModInstallProgressEvent(CopyProgress Progress)
        {
            _Status = ModInstallStatus.Installing;
            _DownloadInfo = new DownloadInfo(string.Empty, 0, 0, "0", "0", 0, 0);
            _HashInfo = new HashInfo(string.Empty, 0);
            _ExtractInfo = new ExtractInfo(string.Empty, 0);
            _CopyInfo = new CopyInfo(Progress.FileName, Progress.ProgressPercentage, Progress.ItemNumber, Progress.MaxItem);
            _UnPackInfo = new UnPackInfo(string.Empty, 0, 0, 0);
            _PatchingInfo = new PatchingInfo(string.Empty, 0, 0, 0);
            _ArchivingInfo = new ArchivingInfo(string.Empty, 0);
        }
        public ModInstallProgressEvent(UnPackProgress Progress)
        {
            _Status = ModInstallStatus.UnPacking;
            _DownloadInfo = new DownloadInfo(string.Empty, 0, 0, "0", "0", 0, 0);
            _HashInfo = new HashInfo(string.Empty, 0);
            _ExtractInfo = new ExtractInfo(string.Empty, 0);
            _CopyInfo = new CopyInfo(string.Empty, 0, 0, 0);
            _UnPackInfo = new UnPackInfo(Progress.FileName, Progress.ProgressPercentage, Progress.ItemNumber, Progress.MaxItem);
            _PatchingInfo = new PatchingInfo(string.Empty, 0, 0, 0);
            _ArchivingInfo = new ArchivingInfo(string.Empty, 0);
        }
        public ModInstallProgressEvent(PatchingProgress Progress)
        {
            _Status = ModInstallStatus.Patching;
            _DownloadInfo = new DownloadInfo(string.Empty, 0, 0, "0", "0", 0, 0);
            _HashInfo = new HashInfo(string.Empty, 0);
            _ExtractInfo = new ExtractInfo(string.Empty, 0);
            _CopyInfo = new CopyInfo(string.Empty, 0, 0, 0);
            _UnPackInfo = new UnPackInfo(string.Empty, 0, 0, 0);
            _PatchingInfo = new PatchingInfo(string.Empty, Progress.ProgressPercentage, Progress.ItemNumber, Progress.MaxItem);
            _ArchivingInfo = new ArchivingInfo(string.Empty, 0);
        }
        public ModInstallProgressEvent(ArchvingProgress Progress)
        {
            _Status = ModInstallStatus.Archiving;
            _DownloadInfo = new DownloadInfo(string.Empty, 0, 0, "0", "0", 0, 0);
            _HashInfo = new HashInfo(string.Empty, 0);
            _ExtractInfo = new ExtractInfo(string.Empty, 0);
            _CopyInfo = new CopyInfo(string.Empty, 0, 0, 0);
            _UnPackInfo = new UnPackInfo(string.Empty, 0, 0, 0);
            _PatchingInfo = new PatchingInfo(string.Empty, 0, 0, 0);
            _ArchivingInfo = new ArchivingInfo(Progress.FileName, Progress.ProgressPercentage);
        }
    }   
    public class ModInstallSettings
    {        
        public Func<IBrowserInstance> Browser { get; set; }               
        public Action OnModInstalled { get; set; }        
    }    
    public class ModProgress
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public int PercentDone { get; set; }
        public int GlobalDone { get; set; }
        public Image Image { get; set; }
        public bool HasError { get; set; }
        public string Mbs { get; set; }
        public string Action { get; set; }
    }    
}
