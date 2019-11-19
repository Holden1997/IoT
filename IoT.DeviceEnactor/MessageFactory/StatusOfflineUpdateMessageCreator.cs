using IoT.Common.SharedMessages.Models;
using IoT.DevaceEnactor.Attributes;
using IoT.DevaceEnactor.Extensions;
using IoT.DevaceEnactor.Interfaces;
using MQTTnet;

namespace IoT.DevaceEnactor.MessageFactory
{
    [Topic(TopicType.disconnected)]
    public class StatusOfflineUpdateMessageCreator : IMessageFactory
    {
        public Message Create(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            return new StatusUpdateMessage(eventArgs.GetMqttClientIdMessageReceived(), Status.Offline);
        }
    }
}
