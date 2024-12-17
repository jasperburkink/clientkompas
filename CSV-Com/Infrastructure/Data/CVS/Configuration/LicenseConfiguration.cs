using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.CVS.Configuration
{
    public class LicenseConfiguration : IEntityTypeConfiguration<License>, ICVSEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<License>());
        }

        public void Configure(EntityTypeBuilder<License> builder)
        {
            builder.Property(l => l.Status)
                .IsRequired();

            builder.Property(l => l.CreatedAt)
                .IsRequired();

            builder.Property(l => l.ValidUntil)
                .IsRequired(false);

            builder.HasOne(l => l.Organization)
                .WithMany()
                .IsRequired();

            builder.HasOne(l => l.LicenseHolder)
                .WithMany()
                .IsRequired();

            builder.Property(l => l.Status)
                .IsRequired();
        }
    }
}
