using IoT.Common.Models;
using IoT.Infrastructure.Contexts;
using IoT.IoC.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using IoT.Infrastructure.Repositories;
using IoT.Infrastructure.Interfaces;

namespace IoT.IoC.Modules
{
    public class IfastructureIdentityModule : IModule
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var optionBuilder = new DbContextOptionsBuilder<ApplicationIdentityContext>();
            
            optionBuilder.UseSqlServer(configuration.GetConnectionString("IotIdentityContext"));

            using (ApplicationIdentityContext context = new ApplicationIdentityContext(optionBuilder.Options)) context.Database.EnsureCreated();
            services.AddTransient(_ => new ApplicationIdentityContext(optionBuilder.Options));

            services.AddIdentity<AppUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationIdentityContext>();

        }
    }

    public class IfastructureModule : IModule
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("IotApplicationContext"));
            using (var context = new ApplicationContext(optionsBuilder.Options))  context.Database.EnsureCreated();
            
            services.AddTransient(_ => new ApplicationContext(optionsBuilder.Options));

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IExeptionRepository, ExeptionRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

    

        }
    }

}
