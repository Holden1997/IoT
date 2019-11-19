
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
        public IMqttServer MqttServer { get; set; }

        public async Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs)
        {
            if (string.IsNullOrEmpty(eventArgs.ClientId))
                await Task.CompletedTask;


            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"disconnected/{eventArgs.ClientId}")
                .WithPayload(eventArgs.ClientId)
                .WithExactlyOnceQoS()
                .Build();

            await MqttServer.PublishAsync(message);

            await Task.CompletedTask;
        }
    }
}
