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
            configuration.Bind(config.MongoDB);


            config.MongoDB.Host = "mongo";
            config.MongoDB.Port = 27017;
            config.MongoDB.User = "root";
            config.MongoDB.Password = "admin";
            config.MongoDB.DatabaseName = "IoTDeviceDataBase";


            services.AddSingleton<IDevicesStoreDatabaseConfiguration, DevicesStoreDatabaseConfiguration>();
            services.AddSingleton<IDeviceContext<Device>, DevicesContext>(_ => new DevicesContext(config.MongoDB));
            services.AddSingleton<IRepository<Device>, DeviceRepository>();

      
        }
    }
}
