using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using System;
using IoT.DevaceEnactor.Handlers;
using IoT.DevaceEnactor.Extensions;

namespace IoT.DevaceEnactor
{
    class Program
    {

        static async Task Main(string[] args)
        {
             await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices(async (hostContext, services) =>
            {
                services.AddTransient<IMqttApplicationMessageReceivedHandler, MqttApplicationMessageReceivedHandler>();
                var factory = new MqttFactory();
                var mqttClient = factory.CreateMqttClient();
                var options = new MqttClientOptionsBuilder()
                .WithClientId("Device Enactor")
                .WithTcpServer("iot.mqtt.broker", 1884).Build();

                mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(async e =>
                {
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("temperature/#").Build());
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("soilmoisture/#").Build());
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("connected/#").Build());
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("disconnected/#").Build());
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("led/#").Build());
                });
                mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(async e =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    try
                    {
                        await mqttClient.ConnectAsync(options, CancellationToken.None);
                    }
                    catch { }
                    
                });

                services.AddSingleton(mqttClient);
                services.AddSingleton(options);

                var provider = services.AddNServiceBus("IoT.DeviceEnactor");
                var mqttApplicationMessageReceivedHandler = provider.GetService<IMqttApplicationMessageReceivedHandler>();

                mqttClient.UseApplicationMessageReceivedHandler(mqttApplicationMessageReceivedHandler);
                try
                {
                    await mqttClient.ConnectAsync(options, CancellationToken.None);
                }
                catch { }
            });
    }
}
