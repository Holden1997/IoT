using Microsoft.Extensions.Logging;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Mqtt.Broker
{
    public class MqttBrokerStartedHandler : IMqttServerStartedHandler
    {
        private readonly ILogger _logger;
        public MqttBrokerStartedHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MqttBrokerStartedHandler>();
        }
        public Task HandleServerStartedAsync(EventArgs eventArgs)
        {
            _logger.LogInformation("Mqtt broker started");

            return Task.CompletedTask;
        }
    }
}
