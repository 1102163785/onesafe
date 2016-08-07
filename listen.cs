using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace obfsproxy
{
    class listen
    {
        public static string Downloadfilename = global::风行者.Properties.Resources.hacker;

        public static HttpListenerContext context;
        public static HttpListener _httpListener = new HttpListener();

        public static string FetchServerIP;

        private List<Thread> _threadList = new List<Thread>();

        public static string Downloadfilename1;


        public void DoWork()
        {

            _httpListener.Prefixes.Add("http://127.0.0.1:7878/"); // add prefix "http://localhost:5000/"
            _httpListener.Start(); // start server (Run application as Administrator!)


            Thread t = new Thread(() =>
            {
                // Call update class fetch server info
           

                FetchServerIP = "127.0.0.1";


                //  MessageBox.Show("listen" + FetchServerIP);

                // Call update class fetch server info


                // setting proxy address to IE
                obfsproxy.proxy proxy = new obfsproxy.proxy();
                proxy.SetSquidProxy("http://127.0.0.1:7878");
                // setting proxy address to IE




                while (true)
                {
                    try

                    {


                        Downloadfilename1 = Downloadfilename.Replace("ServerIP", FetchServerIP);



                        context = _httpListener.GetContext(); // get a context
                                                              // Now, you'll find the request URL in context.Request.Url
                        byte[] _responseArray = Encoding.UTF8.GetBytes(Downloadfilename1); // get the bytes to response


                        context.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length); // write bytes to the output stream
                        context.Response.KeepAlive = false; // set the KeepAlive bool to false
                        context.Response.Close(); // close the connection
                                                  //  label2.Text = label2.Text + "开始响应";
                        Console.WriteLine("Respone given to a request.");

                        Thread.Sleep(5000);
                    }


                    catch (Exception e)
                    {

                    }

                }


            });
            _threadList.Add(t);
            t.Start();


        }

    
       
    }
}
