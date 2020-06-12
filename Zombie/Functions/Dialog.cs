using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zombie.Functions
{
    public static class Dialog
    {
        public static void Show(string text, string caption, MessageBoxButtons button, MessageBoxIcon icon)
        {

            

            MessageBox.Show(text, caption, button, icon, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }
    }
}
