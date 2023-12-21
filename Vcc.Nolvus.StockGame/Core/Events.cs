using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcc.Nolvus.Core.Enums;

namespace Vcc.Nolvus.StockGame.Core
{
    public class StockGameBaseEventArgs : EventArgs
    {
        private int _Value;
        private int _Total;        

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

        public StockGameBaseEventArgs(int Value, int Total)
        {
            _Value = Value;
            _Total = Total;            
        }
    }

    public class ItemProcessedEventArgs : StockGameBaseEventArgs
    {        
        private StockGameProcessStep _Step;
        private string _Name;    

        public StockGameProcessStep Step
        {
            get
            {
                return _Step;
            }
        }

        public string ItemName
        {
            get
            {
                return _Name;
            }
        }

        public ItemProcessedEventArgs(int Value, int Total, StockGameProcessStep Step, string ItemName) 
            : base(Value, Total)
        {            
            _Step = Step;
            _Name = ItemName;
        }
    }

    public class StepProcessedEventArgs : StockGameBaseEventArgs
    {        
        private string _Step;      

        public string Step
        {
            get
            {
                return _Step;
            }
        }

        public StepProcessedEventArgs(int Value, int Total, string Step) 
            : base(Value, Total)
        {
            _Step = Step;
        }
    }

    public delegate void OnItemProcessedHandler(object sender, ItemProcessedEventArgs EventArgs);
    public delegate void OnStepProcessedHandler(object sender, StepProcessedEventArgs EventArgs);
}
