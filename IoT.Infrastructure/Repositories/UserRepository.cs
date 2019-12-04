using IoT.Infrastructure.Contexts;
using IoT.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IoT.Common.Models.ApplicationModels;
using Microsoft.EntityFrameworkCore;

namespace IoT.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly ApplicationContext _context;
        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> AddDevaceToUser(Guid userId, Guid serialNumber)
        {

            var model = await _context.DeviceAndUsers.FindAsync(userId);

            model.DeviceId.Add(new Device { SirialNumber = serialNumber });

            _context.DeviceAndUsers.Update(model);

            await SaveAsync();

            return true;
        }

        public async Task<bool> Create(Guid clientId, Guid serialNumber)
        {
            var listDevices = new List<Device>();
            listDevices.Add(new Device { SirialNumber = serialNumber });
            var user = new DeviceAndUser { UserId = clientId, DeviceId = listDevices };

            await  _context.DeviceAndUsers.AddAsync(user);
            await SaveAsync();

            return true;
        }
        
        public async Task<DeviceAndUser> GetClient(Guid clientId)
        {
            var isExist = await IsExist(clientId);
            if (isExist == false)
                return   null;

            return await _context.DeviceAndUsers.Include(_ => _.DeviceId)
            .FirstOrDefaultAsync(_ => _.UserId == clientId);
        }
        

        private async  Task<bool> IsExist(Guid clientId)
        {
            var model= await _context.DeviceAndUsers.FindAsync(clientId);
           
            if (model!=null) return true;

            return false;
        }

        private async Task SaveAsync()
        {
             await _context.SaveChangesAsync();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
                _context.Dispose();
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UserDevaceRepository()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

       
        #endregion

    }
}