using NServiceBus;

namespace IoT.Common.SharedMessages.Models
{
   public class DevicesPublishCommand : ICommand
    {
        public DevicesPublishCommand(string topic,string message)
        {
            Topic = topic;
            Message = message;
        }

        public string Topic { get; private set; }
        public string Message { get; private set; }

    }
}
