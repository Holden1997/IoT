using IoT.Common.Models.Device;
using NServiceBus;
using System.Collections.Generic;


namespace IoT.Common.SharedMessages.Models
{
   public class DeviceMessageResponse :  IMessage
    {
       public DeviceMessageResponse(IList<Device> device)
        {
            Devices = device;
        }

        public IList<Device> Devices { get; private set; }

    }
}
