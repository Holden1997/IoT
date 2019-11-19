using System.Collections.Generic;
using System.Threading.Tasks;
using IoT.Common.Models;
using IoT.Infrastructure.Contexts;
using IoT.Infrastructure.Interfaces;


namespace IoT.Infrastructure.Repositories
{
    public class ExeptionRepository : IExeptionRepository
    {
       
        private readonly ApplicationContext _applicationContext;
        public ExeptionRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<bool> CreateAsync(Exeption model)
        {
              _applicationContext.Exeptions.Add(model);

            await SaveAsync();

            return true;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Exeption> GetAsync(string Id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Exeption>> GetListAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<ICollection<Exeption>> GetListAsync(int limit, int offset)
        {
            throw new System.NotImplementedException();
        }

        public Task SaveAsync()
        {
            _applicationContext.SaveChangesAsync().ConfigureAwait(false);
            return Task.CompletedTask;
        }

        public Task<bool> UpdateAsync(Exeption model)
        {
            throw new System.NotImplementedException();
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
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ExeptionRepository()
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
