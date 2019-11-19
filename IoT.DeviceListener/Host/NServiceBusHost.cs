using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.DevaceEnactor.NServiceBus
{
    public class NServiceBusHost  : IHostedService
    {
        readonly SessionAndConfigurationHolder _holder;
        IEndpointInstance _endpoint;

        public NServiceBusHost(SessionAndConfigurationHolder holder)
        {
            _holder = holder;
           
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _endpoint = await Endpoint.Start(_holder.EndpointConfiguration);
            }
            catch (Exception e)
            {
                _holder.StartupException = ExceptionDispatchInfo.Capture(e);

                return;
            }

            _holder.MessageSession = _endpoint;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_endpoint != null)
            {
                await _endpoint.Stop();
            }

            _holder.MessageSession = null;
            _holder.StartupException = null;
        }
    }
}
