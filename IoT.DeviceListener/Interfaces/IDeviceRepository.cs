using IoT.Common.Models.Device;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoT.DevaceListener.Iterfaces.Repositories
{
   public interface IDeviceRepository<TDevice> where TDevice : Device
    {
        Task<TDevice> GetAsync(Guid Id);
        Task<IEnumerable<TDevice>> GetListAsync();
        
        Task<bool> CreateAsync(TDevice devace);
        Task<bool> UpdateAsync(TDevice devace);
        Task<bool> DeleteAsync(Guid id);
    }
}
