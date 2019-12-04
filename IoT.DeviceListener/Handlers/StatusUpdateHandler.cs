using IoT.Common.Models.Device;
using IoT.Common.SharedMessages.Models;
using IoT.DevaceListener.Interfaces;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;
using System;

namespace IoT.DevaceListener.Handlers
{
    public class StatusUpdateHandler : IHandleMessages<StatusUpdateMessage>
    {
        private readonly IRepository<Device> _repository;
        private readonly ILogger _logger;
        public StatusUpdateHandler(IRepository<Device> repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<StatusUpdateHandler>();
        }

        public async Task Handle(StatusUpdateMessage message, IMessageHandlerContext context)
        {
            try
            {
                await _repository.UpdateStatus(message.MqttClientId, message.Status.ToString());
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
               
                throw;
            }

            await Task.CompletedTask;
        }
    }
}
