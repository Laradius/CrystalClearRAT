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
        private bool autoScroll = true;
        private bool firstMsg = true;
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
                Dispatcher.Invoke(() =>
                {

                    if (firstMsg)
                    {
                        chatOutputTextBox.Text += "Zombie: " + args.Message;
                        firstMsg = false;
                    }
                    else
                    {
                        chatOutputTextBox.Text += Environment.NewLine + "Zombie: " + args.Message;
                    }


                });
            }
        }

        private void chatInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && chatInputTextBox.Text != "")
            {
                string text = chatInputTextBox.Text;
                chatInputTextBox.Text = "";
                if (firstMsg)
                {
                    chatOutputTextBox.Text += "Me: " + text;
                    firstMsg = false;
                }
                else
                {
                    chatOutputTextBox.Text += Environment.NewLine + "Me: " + text;
                }
                Server.Send(ChatMessage.Create(text, ID), zombie);
            }
        }



        private void ScrollViewer_ScrollChanged(Object sender, ScrollChangedEventArgs e)
        {

            if (e.ExtentHeightChange == 0)
            {
                if (chatScroll.VerticalOffset == chatScroll.ScrollableHeight)
                {
                    autoScroll = true;
                }
                else
                {
                    autoScroll = false;
                }
            }
            if (autoScroll && e.ExtentHeightChange != 0)
            {
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
