using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Server;
using System.Threading.Tasks;

namespace IoT.Mqtt.Broker
{
    public class MqttClientConnectedHandler : IMqttServerClientConnectedHandler
    {
        private readonly IMqttServer _mqttServer;
        private readonly ILogger _logger;
        public MqttClientConnectedHandler(IMqttServer mqttServer, ILoggerFactory loggerFactory )
        {
            _mqttServer = mqttServer;
            _logger = loggerFactory.CreateLogger<MqttClientConnectedHandler>();
            
        }

        public async Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
        {
            var message = new MqttApplicationMessageBuilder()
              .WithTopic($"connected/{eventArgs.ClientId}")
              .WithPayload(eventArgs.ClientId)
              .WithExactlyOnceQoS()
              .Build();

            await _mqttServer.PublishAsync(message);

            _logger.LogInformation($"Device connected - {eventArgs.ClientId}");
            
            await Task.CompletedTask;
        }
    }
}
