using Microsoft.Extensions.Logging;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Mqtt.Broker
{
    public class MqttBrokerStoppedHandler : IMqttServerStoppedHandler
    {
        private readonly ILogger _logger;
        public MqttBrokerStoppedHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MqttBrokerStoppedHandler>();
        }
        public Task HandleServerStoppedAsync(EventArgs eventArgs)
        {
            _logger.LogInformation("Mqtt broker stopped");

            return Task.CompletedTask;
        }
    }
}
