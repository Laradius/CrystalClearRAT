using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClearRAT.ZombieModel
{
    public class Zombie
    {
        public static ObservableCollection<Zombie> Zombies { get; private set; } = new ObservableCollection<Zombie>();

        public event EventHandler Disconnected;

        public string IP { get; private set; }
        public int Port { get; private set; }

        public Socket Socket { get; private set; }

        // public bool Disconnected { get; private set; }



        public Zombie(string ip, int port, Socket socket)
        {
            IP = ip;
            Port = port;
            Socket = socket;

            App.Current.Dispatcher.Invoke(() =>
            {
                Zombies.Add(this);
            });


        }

        public void Destroy()
        {
            //  Disconnected = true;

            App.Current.Dispatcher.Invoke(() =>
            {
                Zombies.Remove(this);
            });

            Disconnected?.Invoke(this, EventArgs.Empty);
        }


    }
}
