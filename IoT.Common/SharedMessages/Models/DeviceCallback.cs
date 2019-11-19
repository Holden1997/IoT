using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Common.SharedMessages.Models
{
   public class DeviceCallback : IMessage
    {
        public DeviceCallback(IList<string> devices)
        {
            Devices = devices;
        }
        public IList<string> Devices { get;  private set; }

    }
}
