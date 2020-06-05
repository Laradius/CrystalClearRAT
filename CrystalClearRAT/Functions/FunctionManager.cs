using CrystalClearRAT.Event;
using CrystalRATShared.Commands;
using System;
using System.Collections.Generic;
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


        public static void Process(CommandFlags flag, BinaryReader reader)
        {
            switch (flag)
            {
                case CommandFlags.GenericCommandOutput:
                    GenericCommandReceived(null, new GenericCommandArgs(reader.ReadString()));
                    break;
                case CommandFlags.RemoteCMD:
                    Console.WriteLine(reader.ReadString());
                    break;
                default:
                    throw new ArgumentException("No command corresponding to the given value.");

            }
        }

    }
}
