using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoT.Infrastructure.Interfaces
{
    public interface IRepository<TModel> where TModel : class 
    {
        Task<TModel> GetAsync(string Id);
        Task<IEnumerable<TModel>> GetListAsync();
        Task<ICollection<TModel>> GetListAsync(int limit, int offset);
        Task<bool> CreateAsync(TModel model);
        Task<bool> UpdateAsync(TModel model);
        Task<bool> DeleteAsync(string id);
        Task SaveAsync();
    }
}
