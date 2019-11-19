using IoT.Common.SharedMessages.Models;
using IoT.DevaceEnactor.Attributes;
using IoT.DevaceEnactor.Interfaces;
using MQTTnet;
using IoT.DevaceEnactor.Extensions;

namespace IoT.DevaceEnactor.MessageFactory
{
    [Topic(TopicType.connected)]
    public class StatusOnlineUpdateMessageCreator : IMessageFactory
    {
        public Message Create(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            return new StatusUpdateMessage(eventArgs.GetMqttClientIdMessageReceived(), Status.Online);   
        }
    }
}
