using NServiceBus;
using RabbitMQ.Client.Impl;
using System.Runtime.ExceptionServices;

namespace IoT.DevaceEnactor.NServiceBus
{
    public class SessionAndConfigurationHolder
    {
        public SessionAndConfigurationHolder(EndpointConfiguration endpointConfiguration)
        {
            EndpointConfiguration = endpointConfiguration;
        }
        public EndpointConfiguration EndpointConfiguration { get; internal set; }

        public IMessageSession MessageSession { get; internal set; }
     
        public ExceptionDispatchInfo StartupException { get; internal set; }

    }
}
