using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Utils.Events
{
    public class DirectoryProgressEvent : EventArgs
    {
        private int _ProgressPercentage;
        public int ProgressPercentage
        {
            get { return _ProgressPercentage; }
        }


        public DirectoryProgressEvent(int Value)
        {
            _ProgressPercentage = Value;
        }
    }

    public class DirectoryDeletingProgressEvent : DirectoryProgressEvent
    {
        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
        }


        public DirectoryDeletingProgressEvent(int Value, string FileName) :
            base(Value)
        {
            _FileName = FileName;
        }
    }

    public delegate void DirectoryCopyingEventHandler(object sender, DirectoryProgressEvent EventArgs);
    public delegate void DirectoryDeletingEventHandler(object sender, DirectoryDeletingProgressEvent EventArgs);
}
