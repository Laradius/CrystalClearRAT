﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalRATShared.Commands
{
    public enum CommandFlags
    {
        DataCorrupted = 0,
        GenericCommandOutput = 1,
        RemoteCMD = 2,
        ZombieDownloadFileRequest = 3,
        Screenshot = 4,
        Click = 5,
        KeyboardKey = 6,
        Kill = 7,
        MessageBox = 8,
        ChatMessage = 9,
        CloseChat = 10,
    }
}
