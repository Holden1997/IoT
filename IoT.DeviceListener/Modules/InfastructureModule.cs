using IoT.Common.Models.Device;
using IoT.DevaceListener.Configuratios;
using IoT.DevaceListener.Contexts;
using IoT.DevaceListener.Interfaces;
using IoT.DevaceListener.Iterfaces;
using IoT.DevaceListener.Iterfaces.Context;
using IoT.DevaceListener.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoT.DevaceListener.Modules
{
    class InfastructureModule : IMongoDbModule
    {
        public void Register(IServiceCollection services, IConfiguration configuration)
        {

            var config = new ServerConfig();
            // configuration.Bind(config.MongoDB);
            //var x = configuration["MongoDB:DatabaseName"];

            config.MongoDB.Host = configuration["MongoDB:Host"];
            config.MongoDB.Port = int.Parse(configuration["MongoDB:Port"]);
            config.MongoDB.User = configuration["MongoDB:User"];
            config.MongoDB.Password = configuration["MongoDB:Password"];
            config.MongoDB.DatabaseName = configuration["MongoDB:DatabaseName"];


            services.AddSingleton<IDevicesStoreDatabaseConfiguration, DevicesStoreDatabaseConfiguration>();
            services.AddSingleton<IDeviceContext<Device>, DevicesContext>(_ => new DevicesContext(config.MongoDB));
            services.AddSingleton<IRepository<Device>, DeviceRepository>();

      
        }
    }
}
