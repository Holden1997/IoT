using NServiceBus;

namespace IoT.Common.SharedMessages.Models
{
    public enum TopicType
    {
        temperature,
        soilmoisture,
        connected,
        disconnected,
        led,
    }

    public abstract class DeviceMessage :Message
    {
       
    }
}
