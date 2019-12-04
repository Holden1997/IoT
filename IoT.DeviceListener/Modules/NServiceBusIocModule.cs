using IoT.Common.Models.Device;
using IoT.DevaceListener.Handlers;
using IoT.DevaceListener.Interfaces;
using IoT.DeviceListener.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.DeviceListener.Modules
{
    class NServiceBusIocModule : INServiceBusIocModule
    {
        public void Register(EndpointConfiguration configuration, ServiceProvider provider)
        {
            var repositoryService = provider.GetService<IRepository<Device>>();
            var loggerFactory = provider.GetService<ILoggerFactory>();

            configuration.RegisterComponents(conponents =>
            {
                conponents.ConfigureComponent(componentFactory: () =>
                {
                    return new TemperatureHandler(repositoryService,loggerFactory);
                },
                dependencyLifecycle: DependencyLifecycle.InstancePerCall);
                conponents.ConfigureComponent(componentFactory: () =>
                {
                    return new DevicesMessageResponseHandler(repositoryService, loggerFactory);
                },
                dependencyLifecycle: DependencyLifecycle.InstancePerCall);
                conponents.ConfigureComponent(componentFactory: () =>
                {
                    return new SoilmoistureHandler(repositoryService,loggerFactory);
                },
               dependencyLifecycle: DependencyLifecycle.InstancePerCall);
                conponents.ConfigureComponent(componentFactory: () =>
                {
                    return new StatusUpdateHandler(repositoryService,loggerFactory);
                },
                dependencyLifecycle: DependencyLifecycle.InstancePerCall);
                conponents.ConfigureComponent(componentFactory: () =>
                {
                    return new LedHandller(repositoryService, loggerFactory);
                },
              dependencyLifecycle: DependencyLifecycle.InstancePerCall);


            });
        }
    }
}
