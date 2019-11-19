using NServiceBus;
using System.Runtime.ExceptionServices;

namespace IoT.DevaceEnactor
{
    public class SessionAndConfigurationHolder
    {
        public SessionAndConfigurationHolder(EndpointConfiguration endpointConfiguration)
        {
            EndpointConfiguration = endpointConfiguration;
        }
        public EndpointConfiguration EndpointConfiguration { get; }

        public IMessageSession MessageSession { get; internal set; }

        public ExceptionDispatchInfo StartupException { get; internal set; }

    }
}
