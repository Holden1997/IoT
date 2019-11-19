using IoT.Common.Models.Device;
using IoT.DevaceListener.Configuratios;
using IoT.DevaceListener.Iterfaces.Context;
using MongoDB.Driver;

namespace IoT.DevaceListener.Contexts
{
    internal class DevicesContext : IDeviceContext<Device>
    {

        private readonly IMongoDatabase _database;
        internal DevicesContext(DevicesStoreDatabaseConfiguration configuration)
        {
            var client = new MongoClient(configuration.ConnectionString);
            _database = client.GetDatabase(configuration.DatabaseName);

        }

        public IMongoCollection<Device> Devices => _database.GetCollection<Device>(nameof(Device));
    }
}
