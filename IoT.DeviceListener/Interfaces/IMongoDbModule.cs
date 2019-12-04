using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoT.DevaceListener.Iterfaces
{
    internal interface IMongoDbModule
    {
        void Register(IServiceCollection services, IConfiguration configuration);
    }
}
