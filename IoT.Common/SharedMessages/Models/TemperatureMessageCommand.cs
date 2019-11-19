using IoT.Common.Models.Device;
using NServiceBus;

namespace IoT.Common.SharedMessages.Models
{
    public class TemperatureMessageCommand : DeviceMessage,  ICommand
    {
        public TemperatureMessageCommand(Temperature temperature)
        {
            Temperature = temperature;

        }
        public Temperature Temperature { get; private set; }

    }
}
