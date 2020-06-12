using CrystalClearRAT.Functions;
using CrystalClearRAT.Web;
using CrystalClearRAT.ZombieModel;
using CrystalRATShared.EvArgs;
using CrystalRATShared.Functions;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CrystalClearRAT.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : MetroWindow
    {

        public static List<Zombie> ChatOpenedZombies = new List<Zombie>();

        private Zombie zombie;
        private string ID;
        public ChatWindow(Zombie zombie)
        {
            this.zombie = zombie;
            zombie.Disconnected += OnDisconnected;
            FunctionManager.MessageReceived += OnMessageReceived;
            ChatOpenedZombies.Add(zombie);
            ID = Guid.NewGuid().ToString();
            InitializeComponent();
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() => { this.Close(); });
        }

        private void OnMessageReceived(object sender, EventArgs e)
        {
            MessageArgs args = e as MessageArgs;

            if (args.ID == ID)
            {
                Dispatcher.Invoke(() => { chatOutputTextBox.Text += "Zombie " + args.Message + Environment.NewLine; });
            }
        }

        private void chatInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && chatInputTextBox.Text != "")
            {
                string text = chatInputTextBox.Text;
                chatInputTextBox.Text = "";
                chatOutputTextBox.Text += "Me: " + text + Environment.NewLine;
                Server.Send(ChatMessage.Create(text, ID), zombie);
            }
        }

        private Boolean AutoScroll = true;

        private void ScrollViewer_ScrollChanged(Object sender, ScrollChangedEventArgs e)
        {
            // User scroll event : set or unset auto-scroll mode
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (chatScroll.VerticalOffset == chatScroll.ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set auto-scroll mode
                    AutoScroll = true;
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset auto-scroll mode
                    AutoScroll = false;
                }
            }

            // Content scroll event : auto-scroll eventually
            if (AutoScroll && e.ExtentHeightChange != 0)
            {   // Content changed and auto-scroll mode set
                // Autoscroll
                chatScroll.ScrollToVerticalOffset(chatScroll.ExtentHeight);
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FunctionManager.MessageReceived -= OnMessageReceived;
            ChatOpenedZombies.Remove(zombie);
        }
    }
}
