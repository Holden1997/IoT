
using IoT.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoT.Infrastructure.Models
{
    class ExeptionConfig : IEntityTypeConfiguration<Exeption>
    {
        public void Configure(EntityTypeBuilder<Exeption> builder)
        {
            builder.HasKey(_ => _.ExeptionId);
        }
    }
}
