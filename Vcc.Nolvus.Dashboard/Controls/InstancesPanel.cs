using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Interfaces;

namespace Vcc.Nolvus.Dashboard.Controls
{
    public partial class InstancesPanel : UserControl
    {                
        public InstancesPanel()
        {
            InitializeComponent();            
        }

        public IDashboardFrame ContainerFrame { get; set; }

        public void LoadInstances(List<INolvusInstance> Instances)
        {
            int Top = 5;
            int Left = 5;
                    
            foreach (INolvusInstance Instance in Instances)
            {
                InstancePanel InstancePanel = new InstancePanel(this);                

                InstancePanel.LoadInstance(Instance);

                InstancePanel.Top = Top;
                InstancePanel.Left = Left;

                Top += 175;

                Controls.Add(InstancePanel);
            }            
        }                
    }
}
