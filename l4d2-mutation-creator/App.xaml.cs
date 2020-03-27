using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Path = System.IO.Path;

namespace l4d2_mutation_creator
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static string ver = "v1.2";

        public static WndTempoHelp wndTempoHelp;
        private void Initialize(object sender, StartupEventArgs e)
        {
            wndTempoHelp = new WndTempoHelp();
            MainWindow wnd = new MainWindow();

            // 检查文件完整性（不检查内容）
            if (false == VerifyIntegrity())
            {
                MessageBox.Show("文件不完整，请到“帮助”页面重新下载本软件", "求生之路2变异模式生成器",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                wnd.grdFirstTab.IsEnabled = false;
                wnd.grdSecondTab.IsEnabled = false;
                wnd.grdThirdTab.IsEnabled = false;
                wnd.grdForthTab.IsEnabled = false;
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
                    if (File.Exists(exeRootPath + "/left4dead2.exe"))
                    {
                        wnd.HasFoundGame(exeRootPath, false);
                    }
                }
            }
            catch (Exception)
            {
                // do nothing if not found conf
            }

            GameOption.InitGameOption();

            //显示版本
            wnd.Title = string.Format(wnd.Title, App.ver);
            wnd.lblVersion.Content = string.Format(wnd.lblVersion.Content.ToString(), App.ver);
            wnd.Show();
        }

        private bool VerifyIntegrity()
        {
            // 检查"gamemodes.txt"
            if (false == File.Exists("template/raw_gamemodes.txt"))
            {
                return false;
            }

            // 检查VPK Pack Tool
            string[] BinFiles =
            {
                "filesystem_stdio.dll",
                "tier0.dll",
                "tier0_s.dll",
                "vpk.exe",
                "vstdlib.dll",
                "vstdlib_s.dll"
            };


            // 检查VSLib库
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
            if (false == File.Exists(LibDir + ".nut")
             || false == CheckInFolder(LibDir, LibFiles)
             || false == CheckInFolder("bin", BinFiles))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckInFolder(string DirPath, string[] FileList)
        {
            if (false == Directory.Exists(DirPath))
            {
                return false;
            }

            DirectoryInfo dir = new DirectoryInfo(DirPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();

            foreach (FileSystemInfo f in fileinfo)
            {
                if (f is DirectoryInfo)
                {
                    // don't care this
                }
                else
                {
                    if (false == FileList.Contains(Path.GetFileName(f.ToString())))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static void DelectOldFiles()
        {
            string ModePath = "template/modes";
            string ScriptPath = "template/scripts/vscripts";

            if (false == Directory.Exists(ModePath))
            {
                return;
            }
            try
            {
                // 清理 ModePath
                DirectoryInfo dir1 = new DirectoryInfo(ModePath);
                FileSystemInfo[] fileinfo1 = dir1.GetFileSystemInfos();
                foreach (FileSystemInfo f in fileinfo1)
                {
                    if (f is DirectoryInfo)
                    {
                        DirectoryInfo subdir = new DirectoryInfo(f.FullName);
                        subdir.Delete(true);
                    }
                    else
                    {
                        File.Delete(f.FullName);
                    }
                }

                //清理 ScriptPath
                DirectoryInfo dir2 = new DirectoryInfo(ScriptPath);
                FileSystemInfo[] fileinfo2 = dir2.GetFileSystemInfos();
                foreach (FileSystemInfo f in fileinfo2)
                {
                    if (f is DirectoryInfo)
                    {
                        // do nothing
                    }
                    else
                    {
                        if (f.Name != "VSLib.nut")
                        {
                            File.Delete(f.FullName);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public static void WriteTest(string str)
        {
            using (StreamWriter f = new StreamWriter("test.txt"))
            {
                f.Write(str);
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
                    "\"Author\"    \"{5}\"}}\r\n";

                strMode = string.Format(strMode, GameID, GameOption.BaseGame,
                    GameOption.PlayerNumber, GameName.Trim(),
                    GameSummary.Trim(), GameAuthor.Trim());

                string Convars = "convar{\r\n";
                foreach (var cvar in GameOption.Convars)
                {
                    Convars += (cvar.Key +" "+cvar.Value + "\r\n");
                }
                Convars += "}\r\n";

                fileMode.Write(strMode + Convars);
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

        public static void WriteGameScript(string GameID, string DirectorOptions, string HookFuncs)
        {
            using (StreamWriter fileScript = new StreamWriter("template/scripts/vscripts/" + GameID + ".nut"))
            {
                fileScript.WriteLine("IncludeScript(\"VSLib\");");
                fileScript.WriteLine("Utils.PrecacheCSSWeapons();");
                fileScript.Write(DirectorOptions);
                fileScript.Write(HookFuncs);
            }
        }

        public static void CallExeByProcess(string ExePath, string argv)
        {
            // 开启新线程
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            // 调用的exe的名称
            process.StartInfo.FileName = ExePath;
            // 传递进exe的参数
            process.StartInfo.Arguments = argv;
            process.StartInfo.UseShellExecute = false;
            // 不显示exe的界面
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();

            process.StandardInput.AutoFlush = true;
            process.WaitForExit();
        }
    }

    public class GameOption
    {
        // 游戏路径
        public static string RootPath = "";
        // 游戏选项
        public static string BaseGame = "coop";
        public static int PlayerNumber = 4;
        public static Dictionary<string, string> Convars;

        // 感染者字典
        public static Dictionary<string, Zombie> SI;
        public static Dictionary<string, ProfZombie> DefaultSI;

        // 禁止的物资
        public static ArrayList WeaponToRemove;

        // 游戏节奏
        public static Dictionary<string, int> Tempo;

        // 初始装备
        public static string[] InitCarries = new string[5];

        public static void InitGameOption()
        {
            WeaponToRemove = new ArrayList();
            Convars = new Dictionary<string, string>();
            InitSI();
            InitTempo();
        }
        
        private static void InitSI()
        {
            // 初始化感染者
            SI = new Dictionary<string, Zombie>();
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

        private static void InitTempo()
        {
            Tempo = new Dictionary<string, int>
            {
                { "MobSpawnMaxTime", 180 },
                { "MobSpawnMinTime", 90 },
                { "MobMaxSize", 30 },
                { "MobMinSize", 10 },
                { "SustainPeakMaxTime", 10 },
                { "SustainPeakMinTime", 5 },
                { "RelaxMaxInterval", 20 },
                { "RelaxMinInterval", 10 }
            };
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
