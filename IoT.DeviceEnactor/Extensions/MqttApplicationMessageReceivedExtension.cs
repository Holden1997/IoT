using MQTTnet;
using System;

using System.Text;

namespace IoT.DevaceEnactor.Extensions
{
   public static class MqttApplicationMessageReceivedExtension
    {
        private static int _guidSize = 36; 
        public static Guid GetGuidSerialNumberReceived(this MqttApplicationMessageReceivedEventArgs args)
        {

           string serialNumber =  args.ApplicationMessage.Topic.Split('/')[1];
            if (serialNumber.Length < _guidSize)
                throw new ArgumentException("Serial number is not valid");

            Guid serialNumberDevice = Guid.Empty;
            Guid.TryParse(serialNumber, out serialNumberDevice);

            return serialNumberDevice;
        }

        public static string GetMqttClientIdMessageReceived(this MqttApplicationMessageReceivedEventArgs args)
        {
            string mqttClient = Encoding.UTF8.GetString(args.ApplicationMessage.Payload).Split('/')[0];
            if (string.IsNullOrEmpty(mqttClient))
                throw new ArgumentException("Mqtt client id is not found");
          

            return mqttClient;
        }
    }
}
