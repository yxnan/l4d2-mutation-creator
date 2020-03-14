using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace l4d2_mutation_creator
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Initialize(object sender, StartupEventArgs e)
        {
            MainWindow wnd = new MainWindow();

            // 检查文件完整性（不检查内容）
            if (false == VerifyIntegrity())
            {
                MessageBox.Show("文件不完整，请转到“帮助”页面重新下载", "",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                wnd.Show();
                return;
            }

            // 读取配置文件（记录游戏目录）
            try
            {
                using (StreamReader sr = new StreamReader("path.dat"))
                {
                    string exeRootPath = sr.ReadLine();
                    if (!File.Exists(exeRootPath + "/left4dead2.exe"))
                    {

                        wnd.HasFoundGame(exeRootPath, false);
                    }
                }
            }
            catch (Exception)
            {

            }

            GameOption.InitSI();
            wnd.Show();
        }
        public bool VerifyIntegrity()
        {
            string[] LibFiles =
            {
                "EasyLogic.nut",
                "Entity.nut",
                "FileIO.nut",
                "HUD.nut",
                "Player.nut",
                "RandomItemSpawner.nut",
                "ResponseRules.nut",
                "Timer.nut",
                "Utils.nut"
            };

            string LibDir = "template/scripts/vscripts/VSLib";
            if (false == Directory.Exists(LibDir)
             || false == File.Exists(LibDir + ".nut"))
            {
                return false;
            }

            DirectoryInfo libpath = new DirectoryInfo(LibDir);
            FileSystemInfo[] fileinfo = libpath.GetFileSystemInfos();

            foreach (FileSystemInfo f in fileinfo)
            {
                if (f is DirectoryInfo)
                {
                    // don't care this
                }
                else
                {
                    if (false == LibFiles.Contains(Path.GetFileName(f.ToString())))
                    {
                        return false;
                    }
                }
            }
            return true;

        }
        public void DelectOldModes(string rootPath)
        {
            if (false == Directory.Exists(rootPath))
            {
                return;
            }
            try
            {
                DirectoryInfo dir = new DirectoryInfo(rootPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();
                foreach (FileSystemInfo f in fileinfo)
                {
                    if (f is DirectoryInfo)
                    {
                        //DirectoryInfo subdir = new DirectoryInfo(f.FullName);
                        //subdir.Delete(true);
                    }
                    else
                    {
                        File.Delete(f.FullName);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        public void WriteGameMode(string GameName, string GameID, string BaseGame,
            int PlayerNumber, string GameSummary, string GameAuthor)
        {
            string ModeDir = "template/modes/";
            if (false == Directory.Exists(ModeDir))
            {
                Directory.CreateDirectory(ModeDir);
            }

            using (StreamWriter fileMode = new StreamWriter(ModeDir + GameID + ".txt"))
            {
                string strMode = "\"{0}\" {{\r\n" +
                    "\"base\" \"{1}\"\r\n" +
                    "\"maxplayers\"    \"{2}\"\r\n" +
                    "\"DisplayTitle\"  \"{3}\"\r\n" +
                    "\"Description\"   \"{4}\"\r\n" +
                    "\"Image\"     \"maps/any\"\r\n" +
                    "\"Author\"    \"{5}\"}}";

                strMode = string.Format(strMode, GameID, BaseGame, PlayerNumber,
                    GameName.Trim(), GameSummary.Trim(), GameAuthor.Trim());
                fileMode.Write(strMode);
            }
        }

        public void WriteGameInfo(string GameName, string GameAuthor, string GameSummary)
        {
            using (StreamWriter fileInfo = new StreamWriter("template/addoninfo.txt"))
            {
                string strInfo = "\"AddonInfo\" {{\r\n" +
                    "addonSteamAppID     550\r\n" +
                    "addontitle          \"{0}\"\r\n" +
                    "addonContent_Script 1\r\n" +
                    "addonversion        1.0\r\n" +
                    "addonauthor         \"{1}\"\r\n" +
                    "addonDescription    \"{2}\"}}";

                strInfo = string.Format(strInfo, GameName.Trim(),
                    GameAuthor.Trim(), GameSummary.Trim());
                fileInfo.Write(strInfo);
            }
        }

    }
    public class GameOption
    {
        // 游戏选项
        public static string BaseGame = "coop";
        public static int    PlayerNumber = 4;
        public static int    IncapMode = 1; // 1 - 普通倒地，2 - 转为黑白，3 - 直接死亡

        // 特感字典
        public static Dictionary<string, Zombie> SI = new Dictionary<string, Zombie>();

        public static void InitSI()
        {
            // 初始化特感
            SI.Add("Boomer",  new Zombie(50,  3));
            SI.Add("Spitter", new Zombie(100, 3));
            SI.Add("Hunter",  new Zombie(250, 2));
            SI.Add("Jockey",  new Zombie(325, 2));
            SI.Add("Smoker",  new Zombie(250, 2));
            SI.Add("Charger", new Zombie(600, 2));
            SI.Add("Tank",    new Zombie(4000, 1));
        }
    }
    public class Zombie
    {
        private int Health;
        private int Limit;
        public Zombie(int zombieHealth, int zombieLimit)
        {
            this.Health = zombieHealth;
            this.Limit  = zombieLimit;
            MessageBox.Show("registered");
        }
        public int GetHealth()
        {
            return this.Health;
        }
        public int GetLimit()
        {
            return this.Limit;
        }
        public void SetHealth(int newHealth)
        {
            this.Health = newHealth;
            MessageBox.Show(newHealth.ToString());
        }
        public void SetLimit(int newLimit)
        {
            this.Limit = newLimit;
            MessageBox.Show(newLimit.ToString());
        }
    }
}
