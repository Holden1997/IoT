
namespace IoT.DevaceListener.Iterfaces
{
     internal interface IDevicesStoreDatabaseConfiguration
    {
        string Host { get; set; }
        int Port { get; set; }
        string User { get; set; }
        string Password { get; set; }
        string DeviceCollectionName { get; set; }
        string ConnectionString
        {
            get;

        }
        string DatabaseName { get; set; }

    }
}
