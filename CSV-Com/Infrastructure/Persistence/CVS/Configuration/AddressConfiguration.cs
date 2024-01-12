using Domain.CVS.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.CVS.Configuration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>, ICVSEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<Address>());
        }

        public void Configure(EntityTypeBuilder<Address> builder)
        {
            //builder.HasNoKey();
        }
    }
}
