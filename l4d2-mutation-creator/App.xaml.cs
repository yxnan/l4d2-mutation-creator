using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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
                MessageBox.Show("文件不完整，请到“帮助”页面重新下载本软件", "求生之路2变异模式生成器",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                wnd.grdFirstTab.IsEnabled = false;
                wnd.grdSecondTab.IsEnabled = false;
                wnd.grdThirdTab.IsEnabled = false;
                wnd.lblHasFindGame.Content = "缺失必要文件";
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

        private bool VerifyIntegrity()
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

        public static void DelectOldModes(string rootPath)
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

        public static void WriteGameMode(string GameName, string GameID,
            string GameSummary, string GameAuthor)
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

                strMode = string.Format(strMode, GameID, GameOption.BaseGame,
                    GameOption.PlayerNumber, GameName.Trim(),
                    GameSummary.Trim(), GameAuthor.Trim());
                fileMode.Write(strMode);
            }
        }

        public static void WriteGameInfo(string GameName, string GameAuthor, string GameSummary)
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

        // @TODO
        public static void WriteGameScript(string GameID, string DirectorOptions, string HookFuncs)
        {
            using (StreamWriter fileScript = new StreamWriter("template/scripts/vscripts/" + GameID + ".nut"))
            {
                fileScript.WriteLine("IncludeScript(\"VSLib\");");
                fileScript.Write(DirectorOptions);
                fileScript.Write(HookFuncs);
            }
        }
    }
    public class GameOption
    {
        // 游戏路径
        public static string RootPath = "";
        // 游戏选项
        public static string BaseGame = "coop";
        public static int PlayerNumber = 4;

        // 感染者字典
        public static Dictionary<string, Zombie> SI = new Dictionary<string, Zombie>();
        public static Dictionary<string, ProfZombie> DefaultSI;

        public static void InitSI()
        {
            // 初始化感染者
            SI.Add("Boomer", new Zombie(50, 3));
            SI.Add("Spitter", new Zombie(100, 3));
            SI.Add("Hunter", new Zombie(250, 2));
            SI.Add("Jockey", new Zombie(325, 2));
            SI.Add("Smoker", new Zombie(250, 2));
            SI.Add("Charger", new Zombie(600, 2));
            SI.Add("Tank", new Zombie(4000, 1));
            SI.Add("Common", new Zombie(0, 20));

            DefaultSI = new Dictionary<string, ProfZombie>();
            foreach (KeyValuePair<string, Zombie> kv in SI)
            {
                DefaultSI.Add(kv.Key, kv.Value);
            }
        }
    }

    public class BaseZombie
    {
        protected int Health;
        protected int Limit;
        protected BaseZombie(int zombieHealth, int zombieLimit)
        {
            this.Health = zombieHealth;
            this.Limit  = zombieLimit;
        }
        public int GetHealth()
        {
            return this.Health;
        }
        public int GetLimit()
        {
            return this.Limit;
        }
    }


    public class Zombie : BaseZombie
    {
        public Zombie(int a, int b) : base(a, b) { }

        public void SetHealth(int newHealth)
        {
            this.Health = newHealth;
        }
        public void SetLimit(int newLimit)
        {
            this.Limit = newLimit;
        }

        public static implicit operator ProfZombie(Zombie z)
        {
            return new ProfZombie(z.GetHealth(), z.GetLimit());
        }
    }

    public class ProfZombie : BaseZombie
    {
        public ProfZombie(int a, int b) : base(a, b) { }
    }
}
