using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Common.SharedMessages.Models
{
    public class UserMessage : IMessage
    {
        public UserMessage(IList<Guid> devaceSerialNumbers)
        {
            DevaceSerialNumbers = devaceSerialNumbers;
        }

        public IList<Guid> DevaceSerialNumbers { get; private set; }
    }
}
