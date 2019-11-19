using IoT.Common.SharedMessages.Models;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using NServiceBus;
using System.Threading.Tasks;

namespace IoT.DevaceEnactor
{
    public class DevicesPublishHander : IHandleMessages<DevicesPublishCommand>
    {
        private readonly IMqttClient _mqttClient;
        private readonly IMqttClientOptions _mqttClientOptions;
        int counter = 0;
        
        public DevicesPublishHander(IMqttClient mqttClient, IMqttClientOptions mqttClientOptions)
        {
            _mqttClient = mqttClient;
            _mqttClientOptions = mqttClientOptions;
        }

        public async Task Handle(DevicesPublishCommand message, IMessageHandlerContext context)
        {
            if (_mqttClient.IsConnected)
                 await PublishMessage(message.Topic, message.Message);

            if (!_mqttClient.IsConnected)
            {
                bool connectionResult = Reconnect();
                if (connectionResult == true)
                   await PublishMessage(message.Topic, message.Message);

            }

            await Task.CompletedTask;
        }

        private bool Reconnect()
        {   
            _mqttClient.ConnectAsync(_mqttClientOptions).GetAwaiter().GetResult();
            if (_mqttClient.IsConnected)
                return true;
            if(!_mqttClient.IsConnected)
            {
                if (counter < 5)
                {
                    counter++;
                    Reconnect();          
                }
            }
            counter = 0;
            return false;
        }

        private async Task PublishMessage(string topic,string message)
        {
            await _mqttClient.PublishAsync(topic, message);

            await Task.CompletedTask;
        }
    }
}
