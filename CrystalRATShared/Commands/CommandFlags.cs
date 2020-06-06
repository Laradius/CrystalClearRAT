using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRATShared.Commands
{
    public enum CommandFlags
    {
        GenericCommandOutput = 0,
        RemoteCMD = 1,
        ZombieDownloadFileRequest = 2,
        Screenshot = 3,
    }
}
