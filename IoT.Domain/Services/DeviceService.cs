using IoT.Common.Models.ApplicationModels;
using IoT.Domain.Interfaces;
using IoT.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoT.Domain.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IUserRepository _repository;
        public DeviceService(IUserRepository repository)
        {
            _repository = repository;
        }


        public async Task<bool> AddDeviceToUser(Guid clientId, Guid serialNumber)
        {
            var client = await _repository.GetClient(clientId);

            if (client == null)
            {
                bool result = await _repository.Create(clientId, serialNumber);

                return result;
            }

            if (client.DeviceId == null)
            {
                bool result = await _repository.AddDevaceToUser(clientId, serialNumber);

                return result;
            }

            foreach (var device in client.DeviceId)
            {
                if (device.SirialNumber == serialNumber)
                    throw new Exception("This device is already used by another user.");
            }

            return await _repository.AddDevaceToUser(clientId, serialNumber);
        }



        public async Task<IList<Device>> GetAllDevices(Guid clientId)
        {

            var client = await _repository.GetClient(clientId);

            if (client == null)
                throw new Exception("Devices  is not exist");

            if (client.DeviceId == null)
                throw new Exception("Devices  is not exist");

            return client.DeviceId;

        }

    }
}
