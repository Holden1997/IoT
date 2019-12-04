using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.WebApi.NServiceBus
{
    public class NServiceBusHost : IHostedService
    {
        readonly SessionAndConfigurationHolder _holder;
        IEndpointInstance _endpoint;
        private readonly ILogger _logger;
        public NServiceBusHost(SessionAndConfigurationHolder holder, ILoggerFactory loggerFactory)
        {
            _holder = holder;
            _logger = loggerFactory.CreateLogger<NServiceBusHost>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _endpoint = await Endpoint.Start(_holder.EndpointConfiguration);
                _logger.LogInformation("NServiceBus host started");
            }
            catch (Exception e)
            {
                _holder.StartupException = ExceptionDispatchInfo.Capture(e);
                _logger.LogError(e, "NServiceBus host error");
                return;
            }

            _holder.MessageSession = _endpoint;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if(_endpoint != null)
            {
                await _endpoint.Stop();
                _logger.LogInformation("NServiceBus host stopped");
            }

            _holder.MessageSession = null;
            _holder.StartupException = null;
        }
    }
}
