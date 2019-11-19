using IoT.DevaceEnactor.Extensions;
using MQTTnet;
using MQTTnet.Client.Receiving;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace IoT.DevaceEnactor.Handlers
{

    public class MqttApplicationMessageReceivedHandler : IMqttApplicationMessageReceivedHandler
    {
       
        private  readonly IMessageSession _message;
        public MqttApplicationMessageReceivedHandler(IMessageSession message)
        {
            _message = message;
        }

        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
           
            try
            {
                string topic = eventArgs.ApplicationMessage.Topic;
                if (string.IsNullOrWhiteSpace(topic) == false)
                {
                    
                    var message = eventArgs.CreateMesssage();
                    if (message != null)
                       await _message.Send(message).ConfigureAwait(false);

                }
            }
            catch (Exception exe)
            {
                throw;
            }

            await Task.CompletedTask;
        }
    }
}
