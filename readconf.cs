using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace obfsproxy
{
    class readconf
    {

        public string _Base32;
        public string _obfsproxyadd;
        public string _obfsproxyport;
        public string _Localserver;
        public string _Localport;
        public string _Squidserveradd;
        public string _Squidport;

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


      


       public void readjson()

        {



            string path = System.Environment.CurrentDirectory;
            // read file into a string and deserialize JSON to a type


            obfsproxyconfig movie1 = JsonConvert.DeserializeObject<obfsproxyconfig>(File.ReadAllText(@path + "\\" + "osconf.json"));


            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(@path + "\\" + "osconf.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                obfsproxyconfig movie2 = (obfsproxyconfig)serializer.Deserialize(file, typeof(obfsproxyconfig));

               // MessageBox.Show(movie2.BASE32);

             //   MessageBox.Show(movie2.obfsproxyadd);
                _Base32 = movie2.BASE32;
                
                _obfsproxyadd = movie2.obfsproxyadd;
                _obfsproxyport = movie2.obfsproxyport;
                _Localserver = movie2.Localserver;
                _Localport = movie2.Localport;


            }


        }



    }
}
