using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;

using System.Xml;
using System.Collections.Generic;

namespace obfsproxy
{

    public partial class Form1 : Form
    {

        private List<Thread> _threadList = new List<Thread>();
        public static int _contrastupdateStatus;

        public static string versionNum;
        bool crlapp = true;
        bool fetchnewsbool = true;
        private delegate void FlushClient();//代理
        private delegate void FlushClient1();//代理
        private System.Timers.Timer timer = new System.Timers.Timer();

        private static bool IsConnectionSuccessful = false;
        private static Exception socketexception;
        private static ManualResetEvent TimeoutObject = new ManualResetEvent(false);


        delegate void set_Text(string s); //定义委托

        public Form1()
        {

            InitializeComponent();

        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileStringW(string section, string key, string val, string filePath);


        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);
        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;
        static bool _settingsReturn, _refreshReturn;
        public string inipath;
        public string path;
        public string _Base32;
        public string _obfsproxyadd;
        public string _obfsproxyport;
        public string _Localserver;
        public string _Localport;
        public string _Squidserveradd;
        public string _Squidport;
        string urlink;
        string newscaption;
        string _fetchfs;

        /// <summary>
        /// //毫秒
        /// </summary>
        private int ms = 00;
        /// <summary>
        /// 秒
        /// </summary>
        private int s = 00;
        /// <summary>
        /// 分钟
        /// </summary>
        private int m = 00;
        /// <summary>
        /// 小时
        /// </summary>
        private int h = 00;
        /// <summary>
        /// 开始计时按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 


        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;
            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_CLOSE)
            { return; }
            base.WndProc(ref m);
        }

        public class obfsproxyconfig
        {
            public string BASE32 { get; set; }
            public string obfsproxyadd { get; set; }
            public string obfsproxyport { get; set; }
            public string Localserver { get; set; }
            public string Localport { get; set; }

        }


        public class data
        {

            public string BASE32 { get; set; }
            public string obfsproxyadd { get; set; }
            public string obfsproxyport { get; set; }


        }


        public class Account
        {
            public string BASE32 { get; set; }
            public string obfsproxyadd { get; set; }
            public string obfsproxyport { get; set; }
            public string Localserver { get; set; }
            public string Localport { get; set; }


            //  public IList<string> Roles { get; set; }
        }

        public class squidjson
        {

            public string Squidserveradd { get; set; }
            public string Squidport { get; set; }

            //  public IList<string> Roles { get; set; }
        }





        public string GetAllTime(int time)
        {
            string hh, mm, ss, fff;

            int f = time % 100; // 毫秒   
            int s = time / 100; // 转化为秒
            int m = s / 60;     // 分
            int h = m / 60;     // 时
            s = s % 60;     // 秒 

            //毫秒格式00
            if (f < 10)
            {
                fff = "0" + f.ToString();
            }
            else
            {
                fff = f.ToString();
            }

            //秒格式00
            if (s < 10)
            {
                ss = "0" + s.ToString();
            }
            else
            {
                ss = s.ToString();
            }

            //分格式00
            if (m < 10)
            {
                mm = "0" + m.ToString();
            }
            else
            {
                mm = m.ToString();
            }

            //时格式00
            if (h < 10)
            {
                hh = "0" + h.ToString();
            }
            else
            {
                hh = h.ToString();
            }

            //返回 hh:mm:ss.ff            
            //    return hh + ":" + mm + ":" + ss + "." + fff;

            return hh + ":" + mm + ":" + ss;
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

        public static System.Net.Sockets.TcpClient Connect(System.Net.IPEndPoint remoteEndPoint, int timeoutMSec)
        {
            TimeoutObject.Reset();
            socketexception = null;

            string serverip = Convert.ToString(remoteEndPoint.Address);
            int serverport = remoteEndPoint.Port;
            System.Net.Sockets.TcpClient tcpclient = new System.Net.Sockets.TcpClient();

            tcpclient.BeginConnect(serverip, serverport,
                new AsyncCallback(CallBackMethod), tcpclient);

            if (TimeoutObject.WaitOne(timeoutMSec, false))
            {
                if (IsConnectionSuccessful)
                {
                    return tcpclient;
                }
                else
                {
                    throw socketexception;
                }
            }
            else
            {
                tcpclient.Close();
                throw new TimeoutException("TimeOut Exception");
            }
        }

        private static void CallBackMethod(IAsyncResult asyncresult)
        {
            try
            {
                IsConnectionSuccessful = false;
                TcpClient tcpclient = asyncresult.AsyncState as TcpClient;

                if (tcpclient.Client != null)
                {
                    tcpclient.EndConnect(asyncresult);
                    IsConnectionSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                IsConnectionSuccessful = false;
                socketexception = ex;
            }
            finally
            {
                TimeoutObject.Set();
            }
        }


     


        private void Savejson()

        {



            string path = System.Environment.CurrentDirectory;


            //IniWriteValue("obfsServer", "BASE32", textBox1.Text);
            //IniWriteValue("obfsServer", "Port", textBox5.Text);
            //IniWriteValue("obfsServer", "Address", textBox3.Text);



            //List<data> _data = new List<data>();
            //_data.Add(new data()
            //{
            //    BASE32 = textBox1.Text,
            //    obfsproxyadd = textBox3.Text,
            //    obfsproxyport = textBox5.Text

            //}

            //);



            //string json = JsonConvert.SerializeObject(_data.ToArray());

            ////write string to file
            //System.IO.File.WriteAllText(@path + "\\" + "osconf1.json", json);


            Account account = new Account
            {
                BASE32 = textBox1.Text,
                obfsproxyadd = textBox3.Text,
                obfsproxyport = textBox5.Text,
                Localport = textBox2.Text,
                Localserver = textBox4.Text,


            };

            string json = JsonConvert.SerializeObject(account, Newtonsoft.Json.Formatting.Indented);
            // {
            //   "Active": true,
            //   "CreatedDate": "2013-01-20T00:00:00Z",
            //   "Roles": [
            //     "User",
            //     "Admin"
            //   ]
            // }

            System.IO.File.WriteAllText(@path + "\\" + "osconf.json", json);





        }

        private void readjson()

        {



            string path = System.Environment.CurrentDirectory;
            // read file into a string and deserialize JSON to a type


            obfsproxyconfig movie1 = JsonConvert.DeserializeObject<obfsproxyconfig>(File.ReadAllText(@path + "\\" + "osconf.json"));


            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(@path + "\\" + "osconf.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                obfsproxyconfig movie2 = (obfsproxyconfig)serializer.Deserialize(file, typeof(obfsproxyconfig));

                _Base32 = movie2.BASE32;
                _obfsproxyadd = movie2.obfsproxyadd;
                _obfsproxyport = movie2.obfsproxyport;
                _Localserver = movie2.Localserver;
                _Localport = movie2.Localport;



            }


        }


      private  void LatestVersionDL()
        {
            //Shadowsocks.updateApp fetchapp = new Shadowsocks.updateApp();

            //fetchapp.Checkweb();

            //fetchapp.CheckUpdate();

            string latestversion = obfsproxy.updateApp.AppdlADD;

            Process.Start(latestversion);

        }

   

        private void readconf()

        {

            obfsproxy.readconf obfsproxy = new obfsproxy.readconf();


            obfsproxy.readjson();

            //  MessageBox.Show(obfsproxy._Localserver);


            textBox4.Text = obfsproxy._Localserver;

            //  textbox1.text = readconf._base32;
            textBox2.Text = obfsproxy._Localport;
            textBox1.Text = obfsproxy._Base32;
            textBox3.Text = obfsproxy._obfsproxyadd;
            textBox5.Text = obfsproxy._obfsproxyport;
        }



        private void updateVersion() {


            obfsproxy.updateApp fetchapp = new obfsproxy.updateApp();

            fetchapp.Checkweb();

            fetchapp.CheckUpdate();


            _contrastupdateStatus = updateApp.addMenu;

            

          versionNum = updateApp.Serverversion;

           // MessageBox.Show("版本"+versionNum);


            if (_contrastupdateStatus == 100)


            {


                StatusInfo.Text = "Obfsproxy mode : obfs4  版本升级提示:请升级到 V " + versionNum;

                UpdateInfo.Text  = "有最新版"+versionNum;
            }

            if (_contrastupdateStatus != 100)



            {

          //      MessageBox.Show("test");

               

                StatusInfo.Text = "Obfsproxy mode : obfs4  版本:已经最新";
            }


        }

        private void _checklocalport()

        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    tcpClient.Connect("127.0.0.1", 7878);
                    MessageBox.Show("雪豹端口7878已经被占用,请检查");

                    Application.DoEvents();
                    System.Environment.Exit(0);


                }
                catch (Exception)
                {

            
                    //pictureBox1.Visible = false;
                    this.ShowInTaskbar = false;

                    obfsproxy.listen listen = new obfsproxy.listen();

                    Thread _threadMain = new Thread(listen.DoWork);
                    _threadMain.IsBackground = true;
                    _threadList.Add(_threadMain);
                    _threadMain.Start();


                }
            }

        }



        private void Form1_Load(object sender, EventArgs e)

        {
            //      this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);

            // _checklocalport();


            updateVersion();

       
            _checklocalport();

         //   StatusInfo.Text= "Obfsproxy mode : obfs4";

            textBox1.Visible = true;
            timer1.Enabled = true;
   

             Killobfsproxy();

           

    
            this.timer1.Enabled = false;
            //textBox4.Enabled = false;
            //textBox2.Enabled = false;

            this.timer1.Interval = 2;

            //   PublicServer.Checked = true;
             obfs4.Checked = true;
             smart.Checked = true;


            label5.Text = "obfsproxy服务器端口";
            label3.Text = "obfsproxy服务器地址";
            label4.Text = "本地监听地址:";
            label2.Text = "本地监听端口:";
            textBox1.Visible = true;
            textBox3.Visible = true;
            textBox5.Visible = true;
            label1.Visible = true;
            label3.Visible = true;
            label5.Visible = true;


            readconf();


            string path = System.Environment.CurrentDirectory;

            string vbsname = path + "\\" + "obfsproxy.exe scramblesuit " + "--dest " + textBox3.Text + ":" + textBox5.Text + " --password " + textBox1.Text + " client " + textBox4.Text + ":" + textBox2.Text;
          //  MessageBox.Show(vbsname);
          
            Execute(vbsname, 1000);




        }





        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Killobfsproxy();

            obfsproxy.proxy proxy = new obfsproxy.proxy();

            proxy.CancelProxySetting();

            notifyIcon1.Icon = null;
            notifyIcon1.Dispose();
            //  MessageBox.Show("TEST");
            Application.DoEvents();
            System.Environment.Exit(0);
        }


  

        private void button2_Click(object sender, EventArgs e)
        {
            Savejson();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Savejson();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            Savejson();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Savejson();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Savejson();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Savejson();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ms < 59)
            {
                ms++;
            }
            else if (s < 59)
            {
                ms = 00;
                s++;
            }
            else if (m < 59)
            {

                s = 00;
                m++;

            }
            else
            {
                m = -1;
                h++;
            }



            //toolStripStatusLabel1.Text = "运行时间:  " + h.ToString() + "小时" + m.ToString() + "分" + s.ToString() + "秒";

        }

        private void label6_MouseClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start(urlink);
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
         //   this.notifyIcon1.Visible = false;

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            }
        }

        private void 智能_Click(object sender, EventArgs e)
        {

          //  proxy.SetSquidProxy("http://127.0.0.1:8888");
        }

        private void smart_CheckStateChanged(object sender, EventArgs e)
        {
          
        }

   

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://127.0.0.1:7878/");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

     
       
        }

        private void smart_Click(object sender, EventArgs e)
        {
            StatusInfo.Text = "Obfsproxy mode : obfs4 模式: 智能";



            obfsproxy.proxy proxy = new obfsproxy.proxy();

            proxy.SetSquidProxy("http://127.0.0.1:7878");


            global.Checked = false;
        }

        private void global_Click(object sender, EventArgs e)
        {
            obfsproxy.proxy proxy = new obfsproxy.proxy();

            StatusInfo.Text = "Obfsproxy mode : obfs4  模式: 全局";

            proxy.AdslSetSquidProxy("127.0.0.1", "1012");


            smart.Checked = false;
        }

      public  void UpdateInfo_Click(object sender, EventArgs e)
        {

            if (_contrastupdateStatus == 100)


            {
                LatestVersionDL();
            }

            if (_contrastupdateStatus != 100)


            {
                MessageBox.Show("当前版本" + versionNum);
              //  UpdateInfo.Text = "有最新版" + versionNum;
            }


          

        }

        private void global_CheckedChanged(object sender, EventArgs e)
        {
          
        }




    }
}