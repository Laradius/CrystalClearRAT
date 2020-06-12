using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRATShared.EvArgs
{
    public class MessageArgs : EventArgs
    {
        public string Message { get; private set; }
        public string ID { get; private set; }
        public MessageArgs(string message, string id = "")
        {
            Message = message;
            ID = id;
        }

    }
}
