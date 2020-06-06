using CrystalClearRAT.Event;
using CrystalRATShared.Commands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CrystalClearRAT.Functions
{
    public static class FunctionManager
    {

        public static event EventHandler GenericCommandReceived;
        public static event EventHandler ImageReceived;


        public static void Process(CommandFlags flag, BinaryReader reader)
        {
            switch (flag)
            {
                case CommandFlags.GenericCommandOutput:
                    GenericCommandReceived?.Invoke(null, new GenericCommandArgs(reader.ReadString()));
                    break;
                case CommandFlags.Screenshot:
                    byte[] img = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
                    ImageReceived?.Invoke(null, new ImageArgs(img));
                    break;
                default:
                    throw new ArgumentException("No command corresponding to the given value.");

            }
        }

    }
}
