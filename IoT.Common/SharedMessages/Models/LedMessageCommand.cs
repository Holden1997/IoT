using IoT.Common.Models.Device;
using NServiceBus;

namespace IoT.Common.SharedMessages.Models
{
    public class LedMessageCommand : DeviceMessage, ICommand
    {
        public LedMessageCommand(Led ledModel)
        {
           
            LedPropets = ledModel;
        }

        public Led LedPropets { get;  set; }

    }
}
