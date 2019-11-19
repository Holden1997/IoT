using Microsoft.Extensions.DependencyInjection;
using MQTTnet.AspNetCore;
using MQTTnet.Server;
using System;

namespace IoT.Mqtt.Server.Broker
{
    public static class AddServiceMqttBrokerCollectionExtension
    {

        public static IServiceCollection AddMqttServer(this IServiceCollection services, Action<MqttServerOptionsBuilder> configuration)
        {
            MqttServerOptionsBuilder mqttServerOptions = new MqttServerOptionsBuilder();
            configuration(mqttServerOptions);

            return services.AddMqttServer(mqttServerOptions);
        }

        private static IServiceCollection AddMqttServer(this IServiceCollection services, MqttServerOptionsBuilder configuration)
        {

            var builder = configuration.Build();
            services.AddSingleton(configuration);
            services.AddSingleton(builder);


            //services.AddHostedMqttServer(builder);
            services.AddHostedService<MqttHost>();
           
            return services;
        }
    }
}
