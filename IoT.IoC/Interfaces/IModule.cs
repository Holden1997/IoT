using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.IoC.Interfaces
{

    public interface IModule
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }

   
       
}