using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRATShared.Helper
{
    public class ClientSettings
    {

        public static string SettingsResourceName = "zombieSettings.json";
        public string IP { get; private set; }
        public int Port { get; private set; }

        public ClientSettings(string ip, int port)
        {
            IP = ip;
            Port = port;
        }

    }
}
