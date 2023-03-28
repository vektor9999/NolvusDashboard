using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcc.Nolvus.StockGame.Core
{
    public class ItemProcessedEventArgs : EventArgs
    {
        private int _Value;
        private int _Total;
        private string _Step;

        public int Value
        {
            get
            {
                return _Value;
            }
        }

        public int Total
        {
            get
            {
                return _Total;
            }
        }

        public string Step
        {
            get
            {
                return _Step;
            }
        }

        public ItemProcessedEventArgs(int Value, int Total, string Step)
        {
            _Value = Value;
            _Total = Total;
            _Step = Step;
        }
    }

    public delegate void OnItemProcessedHandler(object sender, ItemProcessedEventArgs EventArgs);
    public delegate void OnStepProcessedHandler(object sender, ItemProcessedEventArgs EventArgs);
}
