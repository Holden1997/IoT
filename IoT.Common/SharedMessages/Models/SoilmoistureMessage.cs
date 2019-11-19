using IoT.Common.Models.Device;
using NServiceBus;

namespace IoT.Common.SharedMessages.Models
{
    public class SoilmoistureMessageCommand : DeviceMessage,  ICommand
    {
       public  SoilmoistureMessageCommand(Soilmoisture soilmoisture)
        {
            Soilmoisture = soilmoisture;
        }

        public Soilmoisture Soilmoisture { get; private set; }
    }
}
