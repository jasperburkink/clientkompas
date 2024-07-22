using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.CVS.Configuration
{
    public class MaritalStatusConfiguration : IEntityTypeConfiguration<MaritalStatus>, ICVSEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<MaritalStatus>());
        }

        public void Configure(EntityTypeBuilder<MaritalStatus> builder)
        {
            builder.Property(ms => ms.Name)
                .HasMaxLength(MaritalStatusConstants.NAME_MAXLENGTH)
                .IsRequired();
        }
    }
}
