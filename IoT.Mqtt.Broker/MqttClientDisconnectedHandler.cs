
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Diagnostics;
using MQTTnet.Server;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IoT.Mqtt.Broker
{
    public class MqttClientDisconnectedHandler : IMqttServerClientDisconnectedHandler
    {
        private readonly IMqttServer _mqttServer;
        private readonly ILogger _logger;
        public MqttClientDisconnectedHandler(IMqttServer mqttServer, ILoggerFactory loggerFactory)
        {
            _mqttServer = mqttServer;
            _logger = loggerFactory.CreateLogger<MqttClientDisconnectedHandler>();
        }

        public async Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs)
        {
            if (string.IsNullOrEmpty(eventArgs.ClientId))
                await Task.CompletedTask;

            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"disconnected/{eventArgs.ClientId}")
                .WithPayload(eventArgs.ClientId)
                .WithExactlyOnceQoS()
                .Build();

            await _mqttServer.PublishAsync(message);

            _logger.LogInformation($"Device disconnected - {eventArgs.ClientId}, {eventArgs.DisconnectType} ");

            await Task.CompletedTask;
        }
    }
}
