using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.CVS.Configuration
{
    public class DriversLicenceConfiguration : IEntityTypeConfiguration<DriversLicence>, ICVSEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<DriversLicence>());
        }

        public void Configure(EntityTypeBuilder<DriversLicence> builder)
        {
            builder.Property(dl => dl.Category)
                .HasMaxLength(DriversLicenceConstants.CATEGORY_MAXLENGTH)
                .IsRequired();

            builder.Property(dl => dl.Description)
                .HasMaxLength(DriversLicenceConstants.DESCRIPTION_MAXLENGTH)
                .IsRequired();
        }
    }
}
