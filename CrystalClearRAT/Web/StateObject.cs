using CrystalClearRAT.ZombieModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClearRAT.Web
{
    public class StateObject
    {
        public Zombie Zombie { get; private set; }
        public byte[] Buffer { get; private set; }
        public StateObject(Zombie zombie, byte[] buffer)
        {
            Zombie = zombie;
            Buffer = buffer;
        }

    }
}
