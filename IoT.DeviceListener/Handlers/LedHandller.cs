using IoT.Common.Models.Device;
using IoT.Common.SharedMessages.Models;
using IoT.DevaceListener.Interfaces;
using NServiceBus;
using System.Threading.Tasks;

namespace IoT.DevaceListener.Handlers
{
    public class LedHandller : IHandleMessages<LedMessageCommand>
    {

        private readonly IRepository<Device> _repository;
        public LedHandller(IRepository<Device> repository)
        {
            _repository = repository;
        }

        public async Task Handle(LedMessageCommand message, IMessageHandlerContext context)
        {

            try
            {
                await _repository.UpdateAsync(message.LedPropets);
            }
            catch
            {
                throw;
            }

            await Task.CompletedTask;
        }
    }
}
