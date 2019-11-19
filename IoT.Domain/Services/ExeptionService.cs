using IoT.Common.Models;
using IoT.Domain.Interfaces;
using IoT.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoT.Domain.Services
{
    public class ExeptionService : IExeptionLogger
    {
        private readonly IExeptionRepository _exeptionRepository;
        public ExeptionService(IExeptionRepository exeptionRepository)
        {
            _exeptionRepository = exeptionRepository;
        }

        public async Task<IEnumerable<Exeption>> GetLoggi()
        {
            return  await _exeptionRepository.GetListAsync();
        }

        void ILogger<Exeption>.Logger(Exeption model)
        {
            _exeptionRepository.CreateAsync(model).ConfigureAwait(false).GetAwaiter();
            _exeptionRepository.SaveAsync().ConfigureAwait(false).GetAwaiter();
        }
    }
}
