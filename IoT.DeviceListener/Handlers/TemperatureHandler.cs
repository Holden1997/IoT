using IoT.Common.Models.Device;
using IoT.Common.SharedMessages.Models;
using IoT.DevaceListener.Interfaces;
using NServiceBus;
using System.Threading.Tasks;

namespace IoT.DevaceListener.Handlers
{
    public class TemperatureHandler : IHandleMessages<TemperatureMessageCommand>
    {
        private readonly IRepository<Device> _repository;
        public TemperatureHandler(IRepository<Device> repository)
        {
            _repository = repository;
        }

        public  async Task Handle(TemperatureMessageCommand message, IMessageHandlerContext context)
        {

            try
            {
                await _repository.UpdateAsync(message.Temperature);
            }
            catch
            {
                throw;
            }
                
           await Task.CompletedTask; 
        }
      
    }
}
