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

        private static readonly Queue<Action> sendRequest = new Queue<Action>();

        private static readonly ManualResetEvent sendingDone = new ManualResetEvent(false);



        public static Socket Socket { get; private set; } = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public static void Start(string ip, int port)
        {
            Socket.Connect(ip, port);
            SendMonitor();


            //new Thread(() =>
            //{
            //    Thread.CurrentThread.IsBackground = true;
            //    SendMonitor();
            //}).Start();

            //  Console.WriteLine();
        }

        private static void SendMonitor()
        {
            Task.Run(async () =>
            {
                while (true)
                {
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
                Socket.BeginSend(dataLength, 0, dataLength.Length, 0, new AsyncCallback(SendCallback), data);
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
