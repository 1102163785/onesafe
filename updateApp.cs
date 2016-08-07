using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace obfsproxy
{

    class updateApp
    {
        public static int port;
        public static string Serverversion;
        public static Version newVersion = null;
        public static string obfs4config;
        public static string versionADD;
        public static string AppdlADD;
        public static int addMenu;

        public void Checkweb()
        {


            string[][] xinxi = new string[4][];

            // Initialize the elements:
            xinxi[0] = new string[2] { "76.32.15.44", "22.63.12.18", };
            xinxi[1] = new string[1] { "80" };
            xinxi[2] = new string[2] { "http://76.32.15.44/VersionInfo_obfs4.xml", "http://22.63.12.18/VersionInfo_obfs4.xml" };
            xinxi[3] = new string[2] { "https://github.com/squidproxy/onesafe/releases", "https://github.com/squidproxy/onesafe/releases" };


            Console.WriteLine("正在给交错数组赋值！");
            Console.WriteLine(xinxi[3][1]);
            Console.WriteLine(xinxi[3][0]);
            Console.WriteLine("现在准备输出！");
            Console.WriteLine("******************************");
            for (int i = 0; i < xinxi.Length; i++)//先得到行索引
            {
                for (int j = 0; j < xinxi[i].Length; j++)//得到行中的列索引
                {
                    if (i == 1)

                        continue;

                    if (i == 2)

                        continue;

                    if (i == 3)

                        continue;



                    string serverip = xinxi[i][j];

                    // MessageBox.Show("解密地址"+Decryptaddress);



                    //  MessageBox.Show(serverip);

                    try
                    {

                        port = Int32.Parse(xinxi[1][0]);

                        var client = new TcpClient();

                        var result = client.BeginConnect(serverip, port, null, null);

                        var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

                        if (success)


                        {

                            if ("76.32.15.44" == serverip)


                            {
                                versionADD = xinxi[2][0];

                                AppdlADD = xinxi[3][0];


                                //MessageBox.Show(versionADD);

                                //MessageBox.Show(AppdlADD);


                                break;

                            }

                            if ("22.63.12.18" == serverip)


                            {

                                versionADD = xinxi[2][1];
                                AppdlADD = xinxi[3][1];



                               //MessageBox.Show("ver"+versionADD);

                                //MessageBox.Show("dl"+AppdlADD);

                                break;

                            }

                        }



                        //  MessageBox.Show("Port is open");

                        if (!success)
                        {
                            // break;


                        }
                    }


                    catch (System.Net.Sockets.SocketException ex)
                    {
                        if (ex.ErrorCode == 10061)  // Port is unused and could not establish connection 

                        {



                        }

                        else

                        {
                            //   MessageBox.Show(ex.Message);
                        }

                    }

                    //if (i == 1)

                    //    continue;

                    //if (xinxi[i][j] != null)                      //如果此元素不为空时输出元素值
                    //{
                    //    Console.Write(xinxi[i][j]);
                    //}
                    //else
                    //{
                    //    Console.Write("-------");             //如果此元素为空时输出“-------”
                    //}
                }
                Console.WriteLine();
            }

        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)

        {

            MessageBox.Show("新客户端更新完成,请到原版客户端目录启动");

        }





        public void CheckUpdate()

        {


            //  Apache2 = xuebao.CheckApache2.apache2ServerIP;

            //     StrxmlURL = xuebao.CheckApache2.apache2Downloadurl;



            //  MessageBox.Show("found you "+StrxmlURL);

            // version info from xml file  

            // and in this variable we will put the url we  
            // would like to open so that the user can  
            // download the new version  
            // it can be a homepage or a direct  
            // link to zip/exe file  
            string url = "";
            XmlTextReader reader = null;
            try
            {
                // provide the XmlTextReader with the URL of  
                // our xml document  



                // MessageBox.Show(StrxmlURL);

                reader = new XmlTextReader(versionADD);
               // MessageBox.Show(versionADD);

                // simply (and easily) skip the junk at the beginning  
                reader.MoveToContent();
                // internal - as the XmlTextReader moves only  
                // forward, we save current xml element name  
                // in elementName variable. When we parse a  
                // text node, we refer to elementName to check  
                // what was the node name  
                string elementName = "";
                // we check if the xml starts with a proper  
                // "ourfancyapp" element node  
                if ((reader.NodeType == XmlNodeType.Element) &&
                    (reader.Name == "SquidproxyAPP"))
                {
                    while (reader.Read())
                    {
                        // when we find an element node,  
                        // we remember its name  
                        if (reader.NodeType == XmlNodeType.Element)
                            elementName = reader.Name;
                        else
                        {
                            // for text nodes...  
                            if ((reader.NodeType == XmlNodeType.Text) &&
                                (reader.HasValue))
                            {
                                // we check what the name of the node was  
                                switch (elementName)
                                {
                                    case "version":
                                        // thats why we keep the version info  
                                        // in xxx.xxx.xxx.xxx format  
                                        // the Version class does the  
                                        // parsing for us  
                                        newVersion = new Version(reader.Value);

                                        Serverversion = reader.Value;

                                       // MessageBox.Show(Serverversion);
                                        break;

                                    case "url":
                                        url = reader.Value;

                                        //  MessageBox.Show(url);

                                        //    FetchServerIP = url;

                                        break;


                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            Version curVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            // compare the versions  
            if (curVersion.CompareTo(newVersion) < 0)
            {

                addMenu = 100;

               


            }
            else

            {
                
            }



        }
    }
}
