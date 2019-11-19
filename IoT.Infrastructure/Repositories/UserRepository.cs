using IoT.Infrastructure.Contexts;
using IoT.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
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

            var model = await _context.DevaceAndUsers.FindAsync(userId);

            model.DevaceId.Add(new Device { SirialNumber = serialNumber });

            _context.DevaceAndUsers.Update(model);

            await SaveAsync();

            return true;
        }

        public async Task<bool> Create(Guid clientId, Guid serialNumber)
        {
            var listDevaces = new List<Device>();
            listDevaces.Add(new Device { SirialNumber = serialNumber });
            var user = new DevaceAndUser { UserId = clientId, DevaceId = listDevaces };

            await  _context.DevaceAndUsers.AddAsync(user);
            await SaveAsync();


            return true;

        }

        public async Task<DevaceAndUser> GetClient(Guid clientId)
        {
            var isExist = IsExist(clientId);
            if (isExist == false)
                return   null;

           return await _context.DevaceAndUsers.Include(_ => _.DevaceId)
           .FirstOrDefaultAsync(_ => _.UserId == clientId)
           .ConfigureAwait(false);
        }
        

        private bool IsExist(Guid clientId)
        {
            var model=  _context.DevaceAndUsers.Find(clientId);
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