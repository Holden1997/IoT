using IoT.IoC.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace IoT.WebApi.Extentions
{
    public static class RegistrationAssembly
    {
        public static void RegisterModule(this IServiceCollection services, IConfiguration configuration)
        {
        
            var assemblyModule = typeof(IModule).Assembly.ExportedTypes.Where(x => typeof(IModule)
            .IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance).Cast<IModule>()
            .ToList();
             assemblyModule.ForEach(_ => _.ConfigureServices(services, configuration));

        }
    }
}
