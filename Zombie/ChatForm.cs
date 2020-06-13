using CrystalRATShared.EvArgs;
using CrystalRATShared.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zombie.Functions;
using Zombie.Web;

namespace Zombie
{
    public partial class ChatForm : Form
    {

        // Add Closing Prevention


        public static bool IsOpen = false;
        private string id;
        private bool firstMsg = true;

        public ChatForm(string id)
        {
            IsOpen = true;
            this.id = id;
            FunctionManager.MessageReceived += OnMessageReceived;
            InitializeComponent();
        }

        private void OnMessageReceived(object sender, EventArgs e)
        {
            MessageArgs args = e as MessageArgs;
            

            this.Invoke(new MethodInvoker(() => {

                if (firstMsg)
                {
                    chatOutputTextBox.AppendText("Hacker: " + args.Message);
                    firstMsg = false;
                }

                else
                {
                    chatOutputTextBox.AppendText(Environment.NewLine + "Hacker: " + args.Message);
                   
                }
            
            }));

        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FunctionManager.MessageReceived -= OnMessageReceived;
            IsOpen = false;
        }

        private void chatInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(chatInputTextBox.Text) && e.KeyCode == Keys.Enter && chatInputTextBox.Text.Length <= 512)
            {
                string text = chatInputTextBox.Text;
                if (firstMsg)
                {
                    chatOutputTextBox.AppendText("Me: " + text);
                    firstMsg = false;
                }
                else
                {
                    chatOutputTextBox.AppendText(Environment.NewLine + "Me: " + text);
                }
                
                Client.Send(ChatMessage.Create(text, id));
                chatInputTextBox.Text = "";
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
