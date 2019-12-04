
using IoT.Common.Models.Device;
using IoT.Common.SharedMessages.Models;
using IoT.DevaceListener.Interfaces;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.DevaceListener.Handlers
{
    public class DevicesMessageResponseHandler : IHandleMessages<UserMessage>
    {

        private readonly IRepository<Device> _repository;
        private readonly ILogger _logger;
        public DevicesMessageResponseHandler (IRepository<Device> repository,ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<DevicesMessageResponseHandler>();
        }

        public async Task Handle(UserMessage message, IMessageHandlerContext context)
        {
            try
            {
                var listDevices = await _repository.GetListDevaces(message.DevaceSerialNumbers);
              
                var deviceCallbackMessage = new DeviceCallback(listDevices);

                await context.Reply(deviceCallbackMessage);
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
