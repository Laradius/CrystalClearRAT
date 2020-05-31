using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zombie.Functions;

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
            Console.WriteLine(RemoteCMD.ExecuteCommand("notepad.exe"));
            Console.WriteLine(RemoteCMD.ExecuteCommand("asf"));
            Console.WriteLine(RemoteCMD.ExecuteCommand("echo"));
        }
    }
}
