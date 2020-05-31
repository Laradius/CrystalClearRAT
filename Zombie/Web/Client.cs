using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zombie.Web
{
    public static class Client
    {
        private static ManualResetEvent doneSending = new ManualResetEvent(true);
        public static Socket Socket { get; private set; } = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public static void Start(string ip, int port)
        {
            Socket.Connect(ip, port);
        }



        public static void Send(byte[] data)
        {

            // Fix race condition

            byte[] dataLength = BitConverter.GetBytes(data.Length);
            Socket.BeginSend(dataLength, 0, dataLength.Length, 0, new AsyncCallback(SendCallback), data);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            Socket.EndSend(ar);
            Socket.Send((byte[])ar.AsyncState);
        }
    }
}
