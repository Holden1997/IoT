using IoT.Common.Models.Device;

using IoT.DevaceListener.Handlers;
using IoT.DevaceListener.Interfaces;
using IoT.DevaceListener.Iterfaces;

using IoT.DevaceListener.Modules;
using IoT.DevaceListener.NServiceBus;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

using System;
using System.IO;
using System.Threading.Tasks;

namespace IoT.DevaceListener
{
    class Program
    {

        static async Task Main(string[] args)
        {

            IHost host = new HostBuilder()
               .ConfigureHostConfiguration(hostConfiguration =>{
                   hostConfiguration.SetBasePath(Directory.GetCurrentDirectory());
               })
                .ConfigureAppConfiguration((hostContext, hostConfiguration) =>
                {
                    hostConfiguration.AddJsonFile("appsettings.json", optional: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    Register<InfastructureModule>(services, hostContext.Configuration);
                    var provider = services.BuildServiceProvider();
                    var repositoryService = provider.GetService<IRepository<Device>>();

                    services.AddNServiceBus("IoT.DeviceListener", configuration =>
                    {
                        configuration.UseSerialization<NewtonsoftSerializer>();
                        configuration.EnableInstallers();

                        #region DI NServiceBus
                        configuration.RegisterComponents(conponents =>
                        {
                            conponents.ConfigureComponent(componentFactory: () =>
                            {
                                return new TemperatureHandler(repositoryService);
                            },
                            dependencyLifecycle: DependencyLifecycle.InstancePerCall);
                            conponents.ConfigureComponent(componentFactory: () =>
                            {
                                return new DevaceMessageResponseHandler(repositoryService);
                            },
                            dependencyLifecycle: DependencyLifecycle.InstancePerCall);
                            conponents.ConfigureComponent(componentFactory: () =>
                            {
                                return new SoilmoistureHandler(repositoryService);
                            },
                           dependencyLifecycle: DependencyLifecycle.InstancePerCall);
                           conponents.ConfigureComponent(componentFactory: () =>
                            {
                                return new StatusUpdateHandler(repositoryService);
                            },
                           dependencyLifecycle: DependencyLifecycle.InstancePerCall);
                            conponents.ConfigureComponent(componentFactory: () =>
                            {
                                return new LedHandller(repositoryService);
                            },
                          dependencyLifecycle: DependencyLifecycle.InstancePerCall);


                        });
                        #endregion

                        var transport = configuration.UseTransport<RabbitMQTransport>();
                        configuration.UseTransport<RabbitMQTransport>();

                        transport.UseDirectRoutingTopology();
                        transport.ConnectionString("host=rabbitmq");

                        #region routing
                        var routing = transport.Routing();
                        #endregion
                    });
                })
                .Build();

            await host.RunAsync();
        }

        private static void Register<T>(IServiceCollection services, IConfiguration configuration) where T : IMongoDbModule, new()
        {
            new T().Register(services, configuration);
        }
    }
}
