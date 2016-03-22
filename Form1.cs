using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace obfsproxy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;
        static bool _settingsReturn, _refreshReturn;
        string globalURL;
        public string inipath;
        string PacURL;


        private void smartproxy(string ServerAdress)
        {
            RegistryKey loca = Registry.CurrentUser;
            RegistryKey run = loca.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings");
            //  Software\Microsoft\\Windows\CurrentVersion\Internet Settings\Connections",


            try

            {

                run.SetValue("ProxyEnable", 0);
                run.SetValue("ProxyServer", "");
                run.SetValue("AutoConfigURL", ServerAdress);

                NotifyIE();
                //Must Notify IE first, or the connections do not chanage

                IEAutoDetectProxy(false);
                //Must Notify IE first, or the connections do not chanage
                CopyProxySettingFromLan();

                //Must Notify IE first, or the connections do not chanage

                loca.Close();
            }

            catch (Exception ee)

            {
                MessageBox.Show(ee.Message.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void AdslSetSquidProxy(string SquidGobal,string port)

        {
            RegistryKey loca = Registry.CurrentUser;
            RegistryKey run = loca.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings");
            //  Software\Microsoft\\Windows\CurrentVersion\Internet Settings\Connections",


            try

            {
                run.SetValue("ProxyEnable", 1);
                run.SetValue("ProxyServer", SquidGobal + ":"+port);
                run.SetValue("AutoConfigURL", "");

                NotifyIE();
                //Must Notify IE first, or the connections do not chanage

                IEAutoDetectProxy(false);
                //Must Notify IE first, or the connections do not chanage
                CopyProxySettingFromLan();

            }

            catch (Exception ee)

            {
                MessageBox.Show(ee.Message.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }


        private static void CopyProxySettingFromLan()
        {
            RegistryKey registry =
                Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections",
                    true);
            var defaultValue = registry.GetValue("DefaultConnectionSettings");
            try
            {
                var connections = registry.GetValueNames();
                foreach (String each in connections)
                {
                    if (!(each.Equals("DefaultConnectionSettings")
                        || each.Equals("LAN Connection")
                        || each.Equals("SavedLegacySettings")))
                    {
                        //set all the connections's proxy as the lan
                        registry.SetValue(each, defaultValue);
                    }
                }
                NotifyIE();
                //Must Notify IE first, or the connections do not chanage

            }
            catch (System.IO.IOException e)
            {
                MessageBox.Show(e.Message.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public static void NotifyIE()
        {
            // These lines implement the Interface in the beginning of program 
            // They cause the OS to refresh the settings, causing IP to realy update
            _settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            _refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        private static void IEAutoDetectProxy(bool set)
        {
            RegistryKey registry =
                Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Connections",
                    true);
            byte[] defConnection = (byte[])registry.GetValue("DefaultConnectionSettings");
            byte[] savedLegacySetting = (byte[])registry.GetValue("SavedLegacySettings");
            if (set)
            {
                defConnection[8] = Convert.ToByte(defConnection[8] & 8);
                savedLegacySetting[8] = Convert.ToByte(savedLegacySetting[8] & 8);
            }
            else
            {
                defConnection[8] = Convert.ToByte(defConnection[8] & ~8);
                savedLegacySetting[8] = Convert.ToByte(savedLegacySetting[8] & ~8);
            }
            registry.SetValue("DefaultConnectionSettings", defConnection);
            registry.SetValue("SavedLegacySettings", savedLegacySetting);
        }


        private void CancelProxySetting()
        {

            try
            {
                RegistryKey registry =
                    Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings",
                        true);

                registry.SetValue("ProxyEnable", 0);
                registry.SetValue("ProxyServer", "");
                registry.SetValue("AutoConfigURL", "");

                //Set AutoDetectProxy Off
                IEAutoDetectProxy(false);
                NotifyIE();
                //Must Notify IE first, or the connections do not chanage
                CopyProxySettingFromLan();
            }
            catch (Exception e)
            {

                // TODO this should be moved into views
                MessageBox.Show("Failed to update registry");
            }
        }

        private void Killobfsproxy()
        {

            try
            {
                foreach (Process proc in Process.GetProcessesByName("obfsproxy"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string RunCmd(string command)
        {
            //實例一個Process類，啟動一個獨立進程
            Process p = new Process();

            //Process類有一個StartInfo屬性，這個是ProcessStartInfo類，包括了一些屬性和方法，下面我們用到了他的幾個屬性：

            p.StartInfo.FileName = "cmd.exe";           //設定程序名
            p.StartInfo.Arguments = "/c " + command;    //設定程式執行參數
            p.StartInfo.UseShellExecute = false;        //關閉Shell的使用
            p.StartInfo.RedirectStandardInput = true;   //重定向標準輸入
            p.StartInfo.RedirectStandardOutput = true;  //重定向標準輸出
            p.StartInfo.RedirectStandardError = true;   //重定向錯誤輸出
            p.StartInfo.CreateNoWindow = true;          //設置不顯示窗口

            p.Start();   //啟動

            //p.StandardInput.WriteLine(command);       //也可以用這種方式輸入要執行的命令
            //p.StandardInput.WriteLine("exit");        //不過要記得加上Exit要不然下一行程式執行的時候會當機

            return p.StandardOutput.ReadToEnd();        //從輸出流取得命令執行結果

        }
        public static string Execute(string command, int seconds)
        {
            string output = ""; //输出字符串  
            if (command != null && !command.Equals(""))
            {
                Process process = new Process();//创建进程对象  
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//设定需要执行的命令  
                startInfo.Arguments = "/C " + command;//“/C”表示执行完命令后马上退出  
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动  
                startInfo.RedirectStandardInput = false;//不重定向输入  
                startInfo.RedirectStandardOutput = true; //重定向输出  
                startInfo.CreateNoWindow = true;//不创建窗口  
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//开始进程  
                    {
                        //if (seconds == 0)
                        //{
                        process.WaitForExit(seconds); //等待进程结束，等待时间为指定的毫秒  


                        //   process.WaitForExit();//这里无限等待进程结束  
                        //}
                        //else
                        //{
                        //    process.WaitForExit(seconds); //等待进程结束，等待时间为指定的毫秒  
                        //}
                        //output = process.StandardOutput.ReadToEnd();//读取进程的输出  
                    }
                }
                catch
                {
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
            return output;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "SBSB4444FANGBINXING4SBSBSBSBSBSB";
            textBox5.Text = "1010";
            textBox3.Text = "127.0.0.1";
            textBox4.Text = "127.0.0.1";
            textBox2.Text = "22222";
        }

        private void button3_Click(object sender, EventArgs e)
        {
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
          //  killfinaspeed();
            Killobfsproxy();
            CancelProxySetting();
            //notifyIcon1.Icon = null;
            //notifyIcon1.Dispose();
            Application.DoEvents();
            System.Environment.Exit(0);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            CancelProxySetting();
        }

        private void button1_Click(object sender, EventArgs e)
        {



            string path = System.Environment.CurrentDirectory;
            string vbsname = path + "\\" + "obfsproxy.exe" + " scramblesuit --password=" + textBox1.Text + " --dest=" + textBox3.Text + ":" + textBox5.Text + " client " + textBox4.Text + ":" + textBox2.Text;
          //  MessageBox.Show(vbsname);
            Execute(vbsname, 1000);

            AdslSetSquidProxy(textBox4.Text, textBox2.Text);

        }
    }
}
