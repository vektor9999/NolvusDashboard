using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vcc.Nolvus.Core.Errors;

namespace Vcc.Nolvus.Dashboard.Controls
{
    public partial class ErrorsPanel : UserControl
    {        
        public ErrorsPanel()
        {
            InitializeComponent();
        }

        public void LoadMods(List<FaultyMod> Mods)
        {
            int Top = 5;
            int Left = 5;

            Controls.Clear();

            foreach (FaultyMod FaultyMod in Mods)
            {                
                var ErrorPanel = new ErrorPanel();

                ErrorPanel.ModName = string.Format("{0} (v{1})", FaultyMod.Mod.Name, FaultyMod.Mod.Version);
                ErrorPanel.ErrorText = FaultyMod.Error.Message;
                ErrorPanel.SetImage(FaultyMod.Mod.Progress.Image);                

                ErrorPanel.Top = Top;
                ErrorPanel.Left = Left;

                ErrorPanel.BackColor = Color.FromArgb(50, 54, 54, 54);
                ErrorPanel.Width = this.Width;


                Top += 40;

                Controls.Add(ErrorPanel);
            }
        }
    }
}
