using IoT.Common.Models.Device;
using NServiceBus;


namespace IoT.Common.SharedMessages.Models
{
    public class TemperatureMessageEvent : DeviceMessage, IEvent
    {
        public TemperatureMessageEvent(Temperature temperature)
        {
            Temeperature = temperature;
        }

        public Temperature Temeperature { get; private set; }

    }
}
