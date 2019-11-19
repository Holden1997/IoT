using IoT.Common.Models.Device;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.DevaceListener.Interfaces
{
   public interface IRepository<TDocument> where TDocument : Device
    {
        Task<TDocument> GetAsync(Guid Id);
        Task<IList<string>> GetListDevaces(IList<Guid> devaceSerialNumbers);
        Task<bool> CreateAsync(TDocument model);
        Task<bool> UpdateAsync(TDocument model);
        Task UpdateStatus(string mqttClient, string status);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> IsExist(Guid id);

    }
}
