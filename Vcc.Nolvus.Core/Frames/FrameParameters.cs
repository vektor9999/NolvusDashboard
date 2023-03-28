using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.Core.Frames
{
    public class FrameParameter
    {
        public string Key;
        public object Value;

        public static FrameParameter Create(string Name, object Value)
        {
            return new FrameParameter() { Key = Name, Value = Value };
        }
    }
    public class FrameParameters
    {
        Dictionary<string, object> KeyValues = new Dictionary<string, object>();

        public FrameParameters(params FrameParameter[] Args)
        {
            
            foreach(var Arg in Args)
            {
                KeyValues.Add(Arg.Key, Arg.Value);
            }
        }

        public object this[string Key]
        {
            get
            {
                return KeyValues[Key];
            }
        }

        public bool IsEmpty
        {
            get
            {
                return KeyValues.Count == 0;
            }            
        }
    }
}
