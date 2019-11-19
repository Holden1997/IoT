
namespace IoT.Common.Models.Device
{
    public class Soilmoisture : Sensor
    {
        public int Moisture { get; set; }
        public int[] LogMoisture { get; set; }
    }
}
