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
    [Topic(TopicType.soilmoisture)]
    public class SoilmoistureMessageCreator : IMessageFactory
    {
       
        public  Message Create(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            var soilmoistureMessage = ParseMessageReceived(eventArgs);
            Soilmoisture soilmoisture = new Soilmoisture
            {
                SerialNumber = soilmoistureMessage.idDevace,
                Moisture = soilmoistureMessage.message,
                MqttClient = eventArgs.GetMqttClientIdMessageReceived(),
                LogMoisture = soilmoistureMessage.logMoisture,
                Status = Status.Online.ToString()
            };

            return new SoilmoistureMessageCommand(soilmoisture);
        }

        private (Guid idDevace, int message, int[] logMoisture) ParseMessageReceived(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            Guid idDevace = eventArgs.GetGuidSerialNumberReceived();
            byte[] payload = eventArgs.ApplicationMessage.Payload;
          
            string[] message = Encoding.UTF8.GetString(payload, 0, payload.Length).Split('/');

            int[] arr = new int[10];
            int index = 0;
            for (int i = message.Length - 2; i > 1; i--)
            {
                arr[index] = Convert.ToInt32(message[i]);
                index++;
            }


            int value;
            bool isParse = int.TryParse(message[1], out value);
            if (!isParse)
                throw new Exception("Value is not valid");

            return (idDevace, value,arr);
        }
    }
}
