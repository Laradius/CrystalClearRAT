using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClearRAT.ZombieModel
{
    class Zombie
    {
        public static List<Zombie> Zombies { get; private set; } = new List<Zombie>();

        public string IP { get; private set; }
        public int Port { get; private set; }

        public Socket Socket { get; private set; }

        public Zombie(string ip, int port, Socket socket)
        {
            IP = ip;
            Port = port;
            Socket = socket;
            Zombies.Add(this);
        }


    }
}
