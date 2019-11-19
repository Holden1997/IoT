using IoT.Common.Models.Device;
using IoT.Common.SharedMessages.Models;
using IoT.DevaceListener.Interfaces;
using NServiceBus;
using System.Threading.Tasks;

namespace IoT.DevaceListener.Handlers
{
    public class SoilmoistureHandler : IHandleMessages<SoilmoistureMessageCommand>
    {
        private readonly IRepository<Device> _repository;
        public SoilmoistureHandler(IRepository<Device> repository)
        {
            _repository = repository;
        }

        public async Task Handle(SoilmoistureMessageCommand message, IMessageHandlerContext context)
        {
            try
            {
                await _repository.UpdateAsync(message.Soilmoisture);
            }
            catch
            {
                throw;
            }

            await Task.CompletedTask;

        }
    }
}
