using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClearRAT.Event
{
    public class GenericCommandArgs : EventArgs
    {

        public GenericCommandArgs(string command)
        {
            Command = command;
        }

        public string Command;
    }
}
