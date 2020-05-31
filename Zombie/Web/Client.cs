using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zombie.Web
{
    public static class Client
    {

        private static readonly Queue<Action> sendRequest = new Queue<Action>();
        private static readonly ManualResetEvent sendingDone = new ManualResetEvent(false);

        private static string _ip;
        private static int _port;
        private static bool _inited = false;



        public static Socket Socket { get; private set; }



        private static void RestartSocket()
        {
            _inited = false;
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
            sendRequest.Clear();
            sendingDone.Set();
            Thread.Sleep(500);
            sendingDone.Reset();

            SocketInit();
            SendMonitor();
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
                    break;
                }

                catch (SocketException)
                {
                    Console.WriteLine("Connection failed. Retrying in 10 seconds.");
                    Thread.Sleep(10000);
                }
            }
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
            Task.Run(async () =>
            {
                while (true)
                {
                    if (!_inited)
                    {
                        break;
                    }
                    await Task.Delay(10);
                    if (sendRequest.Count > 0)
                    {
                        sendRequest.Dequeue()();
                        sendingDone.WaitOne();
                    }
                }
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


        }

        private static void SendCallback(IAsyncResult ar)
        {
            Socket.EndSend(ar);
            Socket.Send((byte[])ar.AsyncState);
            sendingDone.Set();
        }
    }
}
