using System;
using System.IO;
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
        // 二义性的值不必单独存放
        // string BaseGame = "coop"
        int PlayerNumber = 4;
        int IncapMode = 1; // 1 - 普通倒地，2 - 转为黑白，3 - 直接死亡

        public MainWindow()
        {
            InitializeComponent();

            // 读取配置文件（记录游戏目录）
            try
            {
                using (StreamReader sr = new StreamReader("path.dat"))
                {
                    string exeRootPath = sr.ReadLine();
                    if (!File.Exists(exeRootPath + "\\left4dead2.exe"))
                    {
                        HasFoundGame(exeRootPath, false);
                    }
                }
            }
            catch (Exception e)
            {}
        }
        private void HasFoundGame(string GameDir, bool rewrite)
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
        private void RealOnly(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex(@"[^0-9]+[^\.]?[^0-9]*");
            e.Handled = re.IsMatch(e.Text);
        }
        private void AlphabetOnly(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex(@"[^a-z]+");
            e.Handled = re.IsMatch(e.Text);
        }
        private void HypeLink1_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/sakamitz/l4d2-mutation-creator");
        }

        private void HypeLink2_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://space.bilibili.com/374026342/");
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            PlayerNumber = 1;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            PlayerNumber = 4;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            PlayerNumber = 8;
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            IncapMode = 1;
        }

        private void RadioButton_Checked_4(object sender, RoutedEventArgs e)
        {
            IncapMode = 2;
        }

        private void RadioButton_Checked_5(object sender, RoutedEventArgs e)
        {
            IncapMode = 3;
        }

        private void ChkBoomer_Clicked(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded) return;
            if (false == chkBoomer.IsChecked)
            {
                tbxBoomer.IsEnabled = false;
                tbxLimitBoomer.IsEnabled = false;
                tbxLimitBoomer.Text = "0";
            }
            else
            {
                tbxBoomer.IsEnabled = true;
                tbxBoomer.Text = "50";
                tbxLimitBoomer.IsEnabled = true;
                tbxLimitBoomer.Text = "3";
            }
        }

        private void ChkSpitter_Clicked(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded) return;
            if (false == chkSpitter.IsChecked)
            {
                tbxSpitter.IsEnabled = false;
                tbxLimitSpitter.IsEnabled = false;
                tbxLimitSpitter.Text = "0";
            }
            else
            {
                tbxSpitter.IsEnabled = true;
                tbxSpitter.Text = "100";
                tbxLimitSpitter.IsEnabled = true;
                tbxLimitSpitter.Text = "3";
            }
        }

        private void ChkHunter_Clicked(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded) return;
            if (false == chkHunter.IsChecked)
            {
                tbxHunter.IsEnabled = false;
                tbxLimitHunter.IsEnabled = false;
                tbxLimitHunter.Text = "0";
            }
            else
            {
                tbxHunter.IsEnabled = true;
                tbxHunter.Text = "250";
                tbxLimitHunter.IsEnabled = true;
                tbxLimitHunter.Text = "2";
            }
        }

        private void ChkJockey_Clicked(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded) return;
            if (false == chkJockey.IsChecked)
            {
                tbxJockey.IsEnabled = false;
                tbxLimitJockey.IsEnabled = false;
                tbxLimitJockey.Text = "0";
            }
            else
            {
                tbxJockey.IsEnabled = true;
                tbxJockey.Text = "325";
                tbxLimitJockey.IsEnabled = true;
                tbxLimitJockey.Text = "2";
            }
        }

        private void ChkSmoker_Clicked(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded) return;
            if (false == chkSmoker.IsChecked)
            {
                tbxSmoker.IsEnabled = false;
                tbxLimitSmoker.IsEnabled = false;
                tbxLimitSmoker.Text = "0";
            }
            else
            {
                tbxSmoker.IsEnabled = true;
                tbxSmoker.Text = "250";
                tbxLimitSmoker.IsEnabled = true;
                tbxLimitSmoker.Text = "2";
            }
        }

        private void ChkCharger_Clicked(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded) return;
            if (false == chkCharger.IsChecked)
            {
                tbxCharger.IsEnabled = false;
                tbxLimitCharger.IsEnabled = false;
                tbxLimitCharger.Text = "0";
            }
            else
            {
                tbxCharger.IsEnabled = true;
                tbxCharger.Text = "600";
                tbxLimitCharger.IsEnabled = true;
                tbxLimitCharger.Text = "2";
            }
        }

        private void ChkTank_Clicked(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded) return;
            if (false == chkTank.IsChecked)
            {
                tbxTank.IsEnabled = false;
                tbxLimitTank.IsEnabled = false;
                tbxLimitTank.Text = "0";
            }
            else
            {
                tbxTank.IsEnabled = true;
                tbxTank.Text = "1.0";
                tbxLimitTank.IsEnabled = true;
                tbxLimitTank.Text = "1";
            }
        }

        private void ChkCommon_Clicked(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded) return;
            if (false == chkCommon.IsChecked)
            {
                tbxLimitCommon.IsEnabled = false;
                tbxLimitCommon.Text = "0";
            }
            else
            {
                tbxLimitCommon.IsEnabled = true;
                tbxLimitCommon.Text = "30";
            }
        }

        private void BtnGenVPK_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
