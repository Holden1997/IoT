using IoT.Domain.Interfaces;
using IoT.Domain.Services;
using IoT.IoC.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoT.IoC.Modules
{
    public class Module : IModule
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IExeptionLogger, ExeptionService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IDevaceService, DevaceService>();
        
        }
    }
}
