using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoT.Common.SharedMessages.Models
{
    public enum Status
    {
        Online,
        Offline
    }
    public class StatusUpdateMessage : Message, ICommand
    {
        public StatusUpdateMessage(string mqttClientId, Status status)
        {
            MqttClientId = mqttClientId;
            Status = status.ToString();

        }
        public string MqttClientId { get; private set; }
        public string Status { get; private set; }
    }
}
