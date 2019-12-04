
using IoT.Common.Models;
using IoT.Common.Models.ApplicationModels;
using IoT.Infrastructure.Configurations;
using IoT.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IoT.Infrastructure.Contexts
{
   public class ApplicationContext :DbContext 
    {
        public ApplicationContext(DbContextOptions options): base(options)
        {
            
        }
        public virtual DbSet<Exeption> Exeptions { get; set; }
        
        public virtual DbSet<DeviceAndUser> DeviceAndUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ExeptionConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new DeviceAndUserConfig());
            
            base.OnModelCreating(modelBuilder);

        }
    }
}
