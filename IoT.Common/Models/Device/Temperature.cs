
using MongoDB.Bson.Serialization.Attributes;

namespace IoT.Common.Models.Device
{

    public class Temperature : Sensor
    {
        public double  Temp { get; set; }
        public double[] LogTemp { get; set; }
    }
}
