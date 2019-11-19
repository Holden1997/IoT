using IoT.Mqtt.Broker;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Server;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Mqtt.Server.Broker
{
    public class MqttHost : IHostedService
    {
        IMqttServer _server;
        IMqttServerOptions _serverOptions;
        MqttClientConnectedHandler _mqttClientConnectedHandler;
        MqttClientDisconnectedHandler _mqttClientDisconnectedHandler;
        
        public MqttHost(IMqttServerOptions serverOptions,
            MqttClientConnectedHandler mqttClientConnectedHandler,
            MqttClientDisconnectedHandler mqttClientDisconnected)
        {
            _mqttClientConnectedHandler = mqttClientConnectedHandler;
            _mqttClientDisconnectedHandler = mqttClientDisconnected;
            _serverOptions = serverOptions;

            Init();      
        }

        private void Init()
        {
            _server = new MqttFactory().CreateMqttServer();
            _mqttClientConnectedHandler.MqttServer = _server;
            _mqttClientDisconnectedHandler.MqttServer = _server;
            _server.ClientConnectedHandler = _mqttClientConnectedHandler;
            _server.ClientDisconnectedHandler = _mqttClientDisconnectedHandler;
          
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {

            _server.StartAsync(_serverOptions).GetAwaiter().GetResult();

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
             _server.StopAsync().GetAwaiter().GetResult();

            await Task.CompletedTask;
        }
    }
}
