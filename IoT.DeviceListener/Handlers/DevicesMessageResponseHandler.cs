
using IoT.Common.Models.Device;
using IoT.Common.SharedMessages.Models;
using IoT.DevaceListener.Interfaces;
using NServiceBus;
using System.Threading.Tasks;

namespace IoT.DevaceListener.Handlers
{
    public class DevaceMessageResponseHandler : IHandleMessages<UserMessage>
    {

        private readonly IRepository<Device> _repository;
        public DevaceMessageResponseHandler (IRepository<Device> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UserMessage message, IMessageHandlerContext context)
        {
            try
            {
                var listDevaces = await _repository.GetListDevaces(message.DevaceSerialNumbers)
                    .ConfigureAwait(false);

                var devaceCallbackMessage = new DeviceCallback(listDevaces);

                await context.Reply(devaceCallbackMessage)
                    .ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
            await Task.CompletedTask;
        }

    }
}
