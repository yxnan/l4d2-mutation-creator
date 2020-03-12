using System;
using System.IO;
using System.IO.Compression;
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

            // 读取配置文件（记录游戏目录）

            // 解压vpk资源文件
            string extractPath = Environment.GetEnvironmentVariable("temp");
            lblDebug.Content = extractPath;
            //ZipFile.ExtractToDirectory(currentPath + "vpk.source", currentPath);
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
                    tbxGameDir.Text = dialog.SelectedPath;
                    tbxGameDir.IsEnabled = false;
                    lblHasFindGame.Content = "已找到游戏";
                    lblHasFindGame.Foreground = Brushes.Green;
                }
            }
        }
    }

}
