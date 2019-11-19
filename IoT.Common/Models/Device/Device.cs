using IoT.Common.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace IoT.Common.Models.Device
{
   
    public abstract class Device 
    {
       
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid SerialNumber { get; set; }
        public string MqttClient { get; set; }
        public string Status { get; set; }
    }
}
