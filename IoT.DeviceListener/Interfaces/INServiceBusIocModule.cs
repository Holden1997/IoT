using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.DeviceListener.Interfaces
{
    interface INServiceBusIocModule
    {
        void Register(EndpointConfiguration configuration, ServiceProvider provider);
    }
}
