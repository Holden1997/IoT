using IoT.Common.Models.Device;
using IoT.Common.SharedMessages.Models;
using IoT.DevaceEnactor.Attributes;
using IoT.DevaceEnactor.Extensions;
using IoT.DevaceEnactor.Interfaces;
using MQTTnet;
using System;
using System.Text;


namespace IoT.DevaceEnactor.MessageFactory
{
    [Topic(TopicType.led)]
    public class LedMessageCreator : IMessageFactory
    {
        public Message Create(MqttApplicationMessageReceivedEventArgs eventArgs)
        {

            var ledMessage = ParseMessageReceived(eventArgs);
            Led led = new Led
            {
                SerialNumber = ledMessage.idDevice,
                Power = ledMessage.message,
                MqttClient = eventArgs.GetMqttClientIdMessageReceived(),
                Status = Status.Online.ToString()
            };

            return new LedMessageCommand(led);
        }

        private (Guid idDevice, string message) ParseMessageReceived(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            Guid idDevice = eventArgs.GetGuidSerialNumberReceived();
            byte[] payload = eventArgs.ApplicationMessage.Payload;

            string message = Encoding.UTF8.GetString(payload, 0, payload.Length).Split('/')[1];

            return (idDevice, message);
        }
    }
}
