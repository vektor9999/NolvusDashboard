using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.Controls;
using Syncfusion.Windows.Forms;
using Vcc.Nolvus.Core.Services;
using Vcc.Nolvus.Services.Files;

namespace Vcc.Nolvus.GrassCache
{
    public partial class Main : SfForm
    {
        public Main()
        {
            InitializeComponent();

            SkinManager.SetVisualStyle(this, "Office2016Black");
            Style.TitleBar.MaximizeButtonHoverBackColor = Color.DarkOrange;
            Style.TitleBar.MinimizeButtonHoverBackColor = Color.DarkOrange;
            Style.TitleBar.HelpButtonHoverBackColor = Color.DarkOrange;
            Style.TitleBar.CloseButtonHoverBackColor = Color.DarkOrange;
            Style.TitleBar.MaximizeButtonPressedBackColor = Color.DarkOrange;
            Style.TitleBar.MinimizeButtonPressedBackColor = Color.DarkOrange;
            Style.TitleBar.HelpButtonPressedBackColor = Color.DarkOrange;
            Style.TitleBar.CloseButtonPressedBackColor = Color.DarkOrange;

            Style.TitleBar.BackColor = Color.FromArgb(54, 54, 54);
            Style.TitleBar.IconBackColor = Color.FromArgb(54, 54, 54);
            Style.BackColor = Color.FromArgb(54, 54, 54);

            var Seasons = new List<string>();

            Seasons.Add("Spring");
            Seasons.Add("Summer");
            Seasons.Add("Autumn");
            Seasons.Add("Winter");

            DrpDwnLstSeasons.DataSource = Seasons;
            DrpDwnLstSeasons.SelectedIndex = 0;
        }

        private void BtnBrowseGrassCache_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                TxtBxGrassCacheFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void BtnBrowseCombinedGrassCache_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                TxtBxCombinedGrassCacheFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private string GetSeasonSuffix(string Season)
        {
            switch (Season)
            {
                case "Spring":
                    return "SPR";
                case "Summer":
                    return "SUM";
                case "Autumn":
                    return "AUT";
                case "Winter":
                    return "WIN";
            }

            return string.Empty;
        }

        public void Progress(int Value, int Total)
        {
            if (InvokeRequired)
            {
                Invoke((System.Action<int, int>)Progress, Value, Total);
                return;
            }

            ProgressBar.Visible = true;
            ProgressBar.Value = Value;
            ProgressBar.Maximum = Total;
        }

        private Task Rename()
        {
            ProgressBar.Value = 0;

            return Task.Run(() => 
            {                                
                DirectoryInfo GrassCacheDir = new DirectoryInfo(TxtBxGrassCacheFolder.Text);
                DirectoryInfo CombinedGrassCacheDir = new DirectoryInfo(TxtBxCombinedGrassCacheFolder.Text);

                var Files = GrassCacheDir.GetFiles("*.*", SearchOption.AllDirectories);

                var Counter = 0;

                foreach (var File in Files)
                {
                    var GrassFileName = File.Name.Replace(File.Extension, string.Empty);

                    GrassFileName += "." + GetSeasonSuffix(DrpDwnLstSeasons.SelectedValue.ToString()) + File.Extension;

                    var CombinedGrassFilePath = Path.Combine(CombinedGrassCacheDir.FullName, GrassFileName);

                    File.CopyTo(CombinedGrassFilePath, true);

                    Progress(++Counter, Files.Length);
                }                
            });
        }

        private void BtnRename_Click(object sender, EventArgs e)
        {
            if (TxtBxGrassCacheFolder.Text != string.Empty && TxtBxCombinedGrassCacheFolder.Text != string.Empty)
            {
                if (TxtBxGrassCacheFolder.Text == TxtBxCombinedGrassCacheFolder.Text)
                {
                    MessageBox.Show("Grass cache directory is equal to combined grass cache directory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (Directory.Exists(TxtBxGrassCacheFolder.Text) && Directory.Exists(TxtBxCombinedGrassCacheFolder.Text))
                    {
                        BtnRename.Enabled = false;

                        Rename().ContinueWith(T =>
                        {
                            BtnRename.Enabled = true;
                            ProgressBar.Visible = false;

                            if (T.IsFaulted)
                            {
                                MessageBox.Show(T.Exception.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                    else
                    {
                        MessageBox.Show("Grass cache directory and/or combined directory does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Grass cache directory and/or combined directory are missing!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

