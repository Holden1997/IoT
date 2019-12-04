using IoT.Common.Models.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IoT.Domain.Interfaces
{
    public interface IDeviceService
    {
        Task<bool> AddDeviceToUser(Guid clientId, Guid serialNumber);
        Task<IList<Device>> GetAllDevices(Guid clientId);
    }
}
