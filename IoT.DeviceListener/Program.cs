using IoT.Common.Models.Device;

using IoT.DevaceListener.Handlers;
using IoT.DevaceListener.Interfaces;
using IoT.DevaceListener.Iterfaces;

using IoT.DevaceListener.Modules;
using IoT.DevaceListener.NServiceBus;
using IoT.DeviceListener.Interfaces;
using IoT.DeviceListener.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

using System.IO;
using System.Threading.Tasks;


namespace IoT.DevaceListener
{
    class Program
    {

        static async Task Main(string[] args)
        {
            var builtConfig = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
             .AddCommandLine(args)
             .Build();

            IHost host = new HostBuilder()
               .ConfigureHostConfiguration(hostConfiguration =>
               {
                   hostConfiguration.SetBasePath(Directory.GetCurrentDirectory());
               })
                .ConfigureAppConfiguration((hostContext, hostConfiguration) =>
                {
                    hostConfiguration.AddConfiguration(builtConfig);
                })
                 .ConfigureLogging(configureLogging =>
                 {
                     configureLogging.ClearProviders();
                     configureLogging.AddConsole();
                     configureLogging.AddDebug();
                     configureLogging.SetMinimumLevel(LogLevel.Trace);
                 })
                .ConfigureServices((hostContext, services) =>
                {
                    Register<InfastructureModule>(services, hostContext.Configuration);
                    ServiceProvider provider = services.BuildServiceProvider();
                  
                    services.AddNServiceBus("IoT.DeviceListener", configuration =>
                    {
                        Register<NServiceBusIocModule>(configuration, provider);
                        configuration.UseSerialization<NewtonsoftSerializer>();
                        configuration.EnableInstallers();
                      
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
        private static void Register<T>(EndpointConfiguration configuration, ServiceProvider provider) where T : INServiceBusIocModule, new()
        {
            new T().Register(configuration, provider);
        }
    }
}
