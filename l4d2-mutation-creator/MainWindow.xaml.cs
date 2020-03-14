using System;
using System.IO;
using Path = System.IO.Path;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinForm = System.Windows.Forms;
using System.Text.RegularExpressions;

namespace l4d2_mutation_creator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void HasFoundGame(string GameDir, bool rewrite)
        {
            tbxGameDir.Text = GameDir;
            tbxGameDir.IsEnabled = false;
            lblHasFindGame.Content = "已找到游戏";
            lblHasFindGame.Foreground = Brushes.Green;
            btnGenVPK.IsEnabled = true;
            btnExport.IsEnabled = true;

            if (rewrite)
            {
                using (StreamWriter sw = new StreamWriter("path.dat"))
                {
                    sw.WriteLine(GameDir);
                }
            }
        }
        private void IntegerOnly(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex(@"[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }
        // @TODO
        //private void RealOnly(object sender, TextCompositionEventArgs e)
        //{
        //    Regex re = new Regex(@"[^0-9]+[^\.]?[^0-9]*");
        //    e.Handled = re.IsMatch(e.Text);
        //}
        private void AlphabetOnly(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex(@"[^a-z]+");
            e.Handled = re.IsMatch(e.Text);
        }
        private void HypeLink_OnClick(object sender, RoutedEventArgs e)
        {
            Hyperlink hpl = (Hyperlink)sender;
            System.Diagnostics.Process.Start(hpl.TargetName);
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            WinForm.FolderBrowserDialog dialog = new WinForm.FolderBrowserDialog();
            dialog.Description = "请选择Left 4 Dead 2的根目录";
            if (WinForm.DialogResult.OK == dialog.ShowDialog())
            {
                string exepath = dialog.SelectedPath + "\\left4dead2.exe";
                if (!File.Exists(exepath))
                {
                    HasFoundGame(dialog.SelectedPath, true);
                }
            }
        }

        private void RdbPlayerNumber_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                RadioButton rdb = (RadioButton)sender;
                GameOption.PlayerNumber = Convert.ToInt32(rdb.Tag);
            }
        }

        private void RdbIncapMode_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                RadioButton rdb = (RadioButton)sender;
                GameOption.IncapMode = Convert.ToInt32(rdb.Tag);
            }
        }

        private void GenVPK(string targetDir)
        {
            // addoninfo.txt
            // @TODO : vpk description
            App.WriteGameInfo(tbxMutName.Text, tbxAuthor.Text, tbxSummary.Text);

            // modes\<mutname.txt>
            App.DelectOldModes("template/modes");
            App.WriteGameMode(tbxMutName.Text, tbxMutID.Text,
                tbxSummary.Text, tbxAuthor.Text);
            MessageBox.Show("VPK gen on " + targetDir);
        }
        private void BtnGenVPK_Click(object sender, RoutedEventArgs e)
        {
            GenVPK("");
        }
        private void ChkCoop_Checked(object sender, RoutedEventArgs e)
        {
            GameOption.BaseGame = "coop";
        }

        private void ChkRealism_Checked(object sender, RoutedEventArgs e)
        {
            GameOption.BaseGame = "realism";
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            GenVPK(dir);
        }

        private void TbxHealth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                TextBox tbx = (TextBox)sender;
                GameOption.SI[tbx.Tag.ToString()].SetHealth(Convert.ToInt32(tbx.Text));
            }
        }

        private void TbxLimit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                TextBox tbx = (TextBox)sender;
                GameOption.SI[tbx.Tag.ToString()].SetLimit(Convert.ToInt32(tbx.Text));
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] tbxHealth = {
                tbxBoomer, tbxSpitter, tbxHunter, tbxJockey,
                tbxSmoker, tbxCharger, tbxTank
            };
            TextBox[] tbxLimit = {
                tbxLimitBoomer, tbxLimitSpitter, tbxLimitHunter, tbxLimitJockey,
                tbxLimitSmoker, tbxLimitCharger, tbxLimitTank
            };

            for (int i = 0; i < 7; i++)
            {
                tbxHealth[i].Text = GameOption.DefaultSI[tbxHealth[i].Tag.ToString()]
                    .GetHealth().ToString();
                tbxLimit[i].Text = GameOption.DefaultSI[tbxLimit[i].Tag.ToString()]
                    .GetLimit().ToString();
            }
            tbxLimitCommon.Text = GameOption.DefaultSI["Common"].GetLimit().ToString();
        }
    }

}
