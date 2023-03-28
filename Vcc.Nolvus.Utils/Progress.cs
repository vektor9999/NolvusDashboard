using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Utils
{
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

    public class HashProgress: BaseProgress
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
                if ( MaxItem > 0)
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
