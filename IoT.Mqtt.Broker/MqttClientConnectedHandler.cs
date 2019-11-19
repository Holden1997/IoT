using MQTTnet;
using MQTTnet.Server;
using System.Threading.Tasks;

namespace IoT.Mqtt.Broker
{
    public class MqttClientConnectedHandler : IMqttServerClientConnectedHandler
    {
       public IMqttServer MqttServer { get; set; }

        public async Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
        {
            var message = new MqttApplicationMessageBuilder()
              .WithTopic($"connected/{eventArgs.ClientId}")
              .WithPayload(eventArgs.ClientId)
              .WithExactlyOnceQoS()
              .Build();

            await MqttServer.PublishAsync(message);

            await Task.CompletedTask;
        }
    }
}
