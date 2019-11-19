
using IoT.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoT.Domain.Interfaces
{
    public interface IExeptionLogger : ILogger<Exeption>
    {
        Task<IEnumerable<Exeption>> GetLoggi();
    }
}
