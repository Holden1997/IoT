using IoT.Common.Models.Device;
using IoT.Common.SharedMessages.Models;
using IoT.DevaceListener.Interfaces;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace IoT.DevaceListener.Handlers
{
    public class SoilmoistureHandler : IHandleMessages<SoilmoistureMessageCommand>
    {
        private readonly IRepository<Device> _repository;
        private readonly ILogger _logger;
        public SoilmoistureHandler(IRepository<Device> repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<SoilmoistureHandler>();
        }

        public async Task Handle(SoilmoistureMessageCommand message, IMessageHandlerContext context)
        {
            try
            {
                await _repository.UpdateAsync(message.Soilmoisture);
            }
            catch(Exception e)
            {
                _logger.LogInformation(e, e.Message);

                throw;
            }

            await Task.CompletedTask;

        }
    }
}
