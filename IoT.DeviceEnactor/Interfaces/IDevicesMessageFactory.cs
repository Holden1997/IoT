using IoT.Common.SharedMessages.Models;
using MQTTnet;

namespace IoT.DevaceEnactor.Interfaces
{
    public interface IMessageFactory
    {
        Message Create(MqttApplicationMessageReceivedEventArgs eventArgs);
    }
}
