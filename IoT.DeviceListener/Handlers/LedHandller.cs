using IoT.Common.Models.Device;
using IoT.Common.SharedMessages.Models;
using IoT.DevaceListener.Interfaces;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;
using System;

namespace IoT.DevaceListener.Handlers
{
    public class LedHandller : IHandleMessages<LedMessageCommand>
    {

        private readonly IRepository<Device> _repository;
        private readonly ILogger _logger;
        public LedHandller(IRepository<Device> repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<LedHandller>();
        }

        public async Task Handle(LedMessageCommand message, IMessageHandlerContext context)
        {
            try
            {
                await _repository.UpdateAsync(message.LedPropets);
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
