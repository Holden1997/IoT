using IoT.Common.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace IoT.Infrastructure.Contexts
{
    public class ApplicationIdentityContext : IdentityDbContext<AppUser>
    {
       

        public ApplicationIdentityContext(DbContextOptions<ApplicationIdentityContext> options)
           : base(options)
        {
          
        }
       

      
    }
}
