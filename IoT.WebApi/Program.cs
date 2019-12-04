using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace IoT.WebApi
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            return CreateWebHostBuilder(args).Build().RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseUrls("http://0.0.0.0:5090")
            .ConfigureLogging(configureLogging =>
            {
                configureLogging.ClearProviders();
                configureLogging.AddConsole();
                configureLogging.AddDebug();
                configureLogging.SetMinimumLevel(LogLevel.Trace);
            })
            .UseKestrel()
            .UseStartup<Startup>();
    }
}
