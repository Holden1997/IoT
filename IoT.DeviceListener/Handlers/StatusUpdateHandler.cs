using IoT.Common.Models.Device;
using IoT.Common.SharedMessages.Models;
using IoT.DevaceListener.Interfaces;
using NServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.DevaceListener.Handlers
{
    public class StatusUpdateHandler : IHandleMessages<StatusUpdateMessage>
    {
        private readonly IRepository<Device> _repository;
        public StatusUpdateHandler(IRepository<Device> repository)
        {
            _repository = repository;
        }

        public async Task Handle(StatusUpdateMessage message, IMessageHandlerContext context)
        {
            try
            {
                await _repository.UpdateStatus(message.MqttClientId, message.Status.ToString())
                                   .ConfigureAwait(false);
            }
            catch
            {
                await Task.CompletedTask;
            }
            
               

            await Task.CompletedTask;
        }
    }
}
