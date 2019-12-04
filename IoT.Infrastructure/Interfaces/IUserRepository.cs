using IoT.Common.Models.ApplicationModels;
using System;
using System.Threading.Tasks;

namespace IoT.Infrastructure.Interfaces
{
    public  interface IUserRepository
    {
        Task<bool> AddDevaceToUser(Guid clientId, Guid serialNumber );  
        Task<DeviceAndUser> GetClient(Guid clientId);
        Task<bool> Create(Guid clientId, Guid serialNumber);

    }
}
