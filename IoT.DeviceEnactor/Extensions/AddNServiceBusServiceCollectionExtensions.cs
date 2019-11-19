using System;
using NServiceBus;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using IoT.Common.SharedMessages.Models;
using ServiceControl.Features;
using IoT.DevaceEnactor.NServiceBus;

namespace IoT.DevaceEnactor.Extensions
{
    public static class AddNServiceBusServiceCollectionExtensions
    {
   
       public static UpdateableServiceProvider AddNServiceBus(this IServiceCollection services,string endpointName)
        {
            UpdateableServiceProvider container = null;
            EndpointConfiguration configuration = new EndpointConfiguration(endpointName);
            configuration.UseSerialization<NewtonsoftSerializer>();
            configuration.EnableInstallers();
            configuration.DisableFeature<Heartbeats>();

            var transport = configuration.UseTransport<RabbitMQTransport>();
            configuration.UseTransport<RabbitMQTransport>();

            transport.UseDirectRoutingTopology();
            transport.ConnectionString("host=rabbitmq");
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(Message).Assembly, "IoT.DeviceListener");
           
            services.AddSingleton(configuration);
            var sessionConfiguration = new SessionAndConfigurationHolder(configuration);

            services.AddSingleton(sessionConfiguration);

            configuration.UseContainer<ServicesBuilder>(customizations =>
            {
                customizations.ExistingServices(services);
                customizations.ServiceProviderFactory(sc =>
                {
                    container = new UpdateableServiceProvider(sc);
                    return container;
                });
            });

            //container.AddHostedService<NServiceBusHost>();
            var endpointInstanse = Endpoint.Start(configuration).GetAwaiter().GetResult();
            sessionConfiguration.MessageSession = endpointInstanse;

            container.AddSingleton(provider =>
            {
                var management = provider.GetService<SessionAndConfigurationHolder>();

                if (management.MessageSession != null)    return management.MessageSession;
                
                var timeout = TimeSpan.FromSeconds(30);

                if (!SpinWait.SpinUntil(() => management.MessageSession != null || management.StartupException != null,
                    timeout))
                {
                    throw new TimeoutException($"Unable to resolve the message session within '{timeout.ToString()}'. If you are trying to resolve the session within hosted services it is encouraged to use `Lazy<IMessageSession>` instead of `IMessageSession` directly");
                }

                management.StartupException?.Throw();

                return management.MessageSession;
            });
            container.AddSingleton(provider => new Lazy<IMessageSession>(provider.GetService<IMessageSession>));

            return container;
        }

    }
}
