using IoT.Common.Models.ApplicationModels;
using IoT.Domain.Interfaces;
using IoT.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoT.Domain.Services
{
    public class DevaceService : IDevaceService
    {
        private readonly IUserRepository _repository;
        public DevaceService(IUserRepository repository)
        {
            _repository = repository;
        }


        public async Task<bool> AddDevaceToUser(Guid clientId, Guid serialNumber)
        {
            var client = await _repository.GetClient(clientId)
                  .ConfigureAwait(false);

            if (client == null)
            {
                bool result = await _repository.Create(clientId, serialNumber)
              .ConfigureAwait(false);

                return result;
            }

            if (client.DevaceId == null)
            {
                bool result = await _repository.AddDevaceToUser(clientId, serialNumber)
               .ConfigureAwait(false);

                return result;
            }

            foreach (var devace in client.DevaceId)
            {
                if (devace.SirialNumber == serialNumber)
                    throw new Exception("This device is already used by another user.");
            }

            return await _repository.AddDevaceToUser(clientId, serialNumber)
                .ConfigureAwait(false);
        }
      


        public async Task<IList<Device>> GetAllDevaces(Guid clientId)
        {
            var client = await _repository.GetClient(clientId)
                 .ConfigureAwait(false);

            if (client == null)
                throw new Exception("Devaces  is not exist");

            if (client.DevaceId == null)
                throw new Exception("Devaces  is not exist");

            return client.DevaceId;
        }

    }
}
