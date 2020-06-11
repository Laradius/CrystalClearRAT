using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClearRAT.Event
{
    class ImageArgs : EventArgs
    {
        public byte[] Data;
        public string ID;

        public ImageArgs(byte[] data, string id)
        {
            this.Data = data;
            ID = id;
        }
    }
}
