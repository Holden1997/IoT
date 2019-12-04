using IoT.Common.Models.Device;
using IoT.Common.SharedMessages.Models;
using IoT.DevaceListener.Interfaces;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace IoT.DevaceListener.Handlers
{
    public class TemperatureHandler : IHandleMessages<TemperatureMessageCommand>
    {
        private readonly IRepository<Device> _repository;
        private readonly ILogger _logger;
        public TemperatureHandler(IRepository<Device> repository,ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<TemperatureHandler>();
        }

        public  async Task Handle(TemperatureMessageCommand message, IMessageHandlerContext context)
        {
            try
            {
                await _repository.UpdateAsync(message.Temperature);
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
