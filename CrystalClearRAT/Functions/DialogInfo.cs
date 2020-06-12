using CrystalRATShared.Commands;
using CrystalRATShared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClearRAT.Functions
{
    public static class DialogInfo
    {

        public static byte[] Create(string text, string caption, int button, int icon)
        {
            return CommandDataSerializer.Serialize(CommandFlags.MessageBox, (writer) =>
            {
                writer.Write(text);
                writer.Write(caption);
                writer.Write(button);
                writer.Write(icon);
            });
        }

    }
}
