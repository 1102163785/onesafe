using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace obfsproxy
{
    class proxy
    {


        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;
        static bool _settingsReturn, _refreshReturn;

        public static void NotifyIE()
        {
            // These lines implement the Interface in the beginning of program 
            // They cause the OS to refresh the settings, causing IP to realy update
            _settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            _refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        public void CancelProxySetting()
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
                //  MessageBox.Show(e.ToString());
            }
        }

        public void SetSquidProxy(string ServerAdress)
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
                //  MessageBox.Show(ee.Message.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                //  MessageBox.Show(e.Message.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       public  void AdslSetSquidProxy(string SquidGobal, string port)

        {
            RegistryKey loca = Registry.CurrentUser;
            RegistryKey run = loca.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings");
            //  Software\Microsoft\\Windows\CurrentVersion\Internet Settings\Connections",


            try

            {
                run.SetValue("ProxyEnable", 1);
                run.SetValue("ProxyServer", SquidGobal + ":" + port);
                run.SetValue("AutoConfigURL", "");

                NotifyIE();
                //Must Notify IE first, or the connections do not chanage

                IEAutoDetectProxy(false);
                //Must Notify IE first, or the connections do not chanage
                CopyProxySettingFromLan();

            }

            catch (Exception ee)

            {
             //   MessageBox.Show(ee.Message.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



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

    }
}

