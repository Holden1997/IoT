using MQTTnet;
using System;
using System.Text;
using IoT.DevaceEnactor.Interfaces;
using IoT.DevaceEnactor.Attributes;
using IoT.Common.SharedMessages.Models;
using IoT.DevaceEnactor.Extensions;
using IoT.Common.Models.Device;

namespace IoT.DevaceEnactor.MessageFactory
{
    [Topic(TopicType.temperature)]
    public class TemperatureMessageCreator : IMessageFactory
    {

        public Message Create(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
          
            var tepmeratureMessage = ParseMessageReceived(eventArgs);
            Temperature temperature = new Temperature
            {
                SerialNumber = tepmeratureMessage.idDevace,
                Temp = tepmeratureMessage.message,
                LogTemp = tepmeratureMessage.logTemp,
                MqttClient = eventArgs.GetMqttClientIdMessageReceived(),
                Status = Status.Online.ToString()
            };

            return  new TemperatureMessageCommand(temperature);
        }

        private (Guid idDevace, double message, double[] logTemp ) ParseMessageReceived(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            Guid idDevace = eventArgs.GetGuidSerialNumberReceived();
            byte[] payload = eventArgs.ApplicationMessage.Payload;
      
            string[] message = Encoding.UTF8.GetString(payload, 0, payload.Length).Split('/');

            double[] arr = new double[10];
            int index = 0;
            for (int i = message.Length -2;  i > 1; i--)
            {
                arr[index] = Convert.ToDouble( message[i]);
                index++;
            }


            double value;
            bool isParse = double.TryParse(message[1], out value);
            if (!isParse)
                throw new Exception("Value is not valid");

            return (idDevace, value, arr);
        }
    }
}
