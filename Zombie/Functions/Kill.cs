using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zombie.Functions
{
    public static class Kill
    {
        public static bool KillRequested = false;


        public static void Request()
        {
            KillRequested = true;
            Application.Exit();
        }

    }
}
