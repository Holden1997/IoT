using IoT.Common.Models.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoT.Infrastructure.Configurations
{
    public class DevaceAndUserConfig : IEntityTypeConfiguration<DevaceAndUser>
    {
        public void Configure(EntityTypeBuilder<DevaceAndUser> builder)
        {
            builder.HasKey(_ => _.UserId);

        }
    }
}
