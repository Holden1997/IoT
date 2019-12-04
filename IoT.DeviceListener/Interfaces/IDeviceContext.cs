using MongoDB.Driver;

namespace IoT.DevaceListener.Iterfaces.Context
{
    public interface IDeviceContext<TDocument>
    {
        IMongoCollection<TDocument> Devices { get; }
    }
}
