using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using MQTTnet.AspNetCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IoT.Mqtt.Broker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    options.ListenAnyIP(1884, l => l.UseMqtt());
                })
                .ConfigureLogging(configureLogging =>
                {
                    configureLogging.ClearProviders();
                    configureLogging.AddConsole();
                    configureLogging.AddDebug();
                    configureLogging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseStartup<Startup>()
                .Build();
    }
}