using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.WebApi.NServiceBus
{
    public static class AddNServiceBusServiceCollectionExtensions
    {

        public static IServiceCollection AddNServiceBus(this IServiceCollection services, string endpointName, Action<EndpointConfiguration> configuration)
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            configuration(endpointConfiguration);
            return services.AddNServiceBus(endpointConfiguration);
        }
        static IServiceCollection AddNServiceBus(this IServiceCollection services, EndpointConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddSingleton(new SessionAndConfigurationHolder(configuration));
            services.AddHostedService<NServiceBusHost>();
            services.AddSingleton(provider =>
            {
                var management = provider.GetService<SessionAndConfigurationHolder>();
                if (management.MessageSession != null)
                {
                    return management.MessageSession;
                }

                var timeout = TimeSpan.FromSeconds(30);
           
                if (!SpinWait.SpinUntil(() => management.MessageSession != null || management.StartupException != null,
                    timeout))
                {
                    throw new TimeoutException($"Unable to resolve the message session within '{timeout.ToString()}'. If you are trying to resolve the session within hosted services it is encouraged to use `Lazy<IMessageSession>` instead of `IMessageSession` directly");
                }

                management.StartupException?.Throw();

                return management.MessageSession;
            });
            services.AddTransient(provider => new Lazy<IMessageSession>(provider.GetService<IMessageSession>));
            return services;
        }

    }
}
