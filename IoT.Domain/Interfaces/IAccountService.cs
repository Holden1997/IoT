using IoT.Common.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace IoT.Domain.Interfaces
{
    public  interface IAccountService 
    {
        IConfiguration Configuration { get;  }
        Task<string> SignInAsync(User userModel);
        Task<object> CreateAccountAsync(User user);
    }
}
