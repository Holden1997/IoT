using IoT.Common.Models.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoT.Infrastructure.Configurations
{
    public class DeviceAndUserConfig : IEntityTypeConfiguration<DeviceAndUser>
    {
        public void Configure(EntityTypeBuilder<DeviceAndUser> builder)
        {
            builder.HasKey(_ => _.UserId);

        }
    }
}
