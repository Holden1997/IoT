using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.DevaceListener.Configuratios
{
    class ServerConfig
    {
        internal DevicesStoreDatabaseConfiguration MongoDB { get; set; } = new DevicesStoreDatabaseConfiguration();
    }

}
