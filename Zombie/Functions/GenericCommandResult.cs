using CrystalRATShared.Commands;
using CrystalRATShared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zombie.Functions
{
    public static class GenericCommandResult
    {

        public static byte[] Generate(string commandOutput)
        {
            return CommandDataSerializer.Serialize(CommandFlags.GenericCommandOutput, (writer) => { writer.Write(commandOutput); });
        }

    }
}
