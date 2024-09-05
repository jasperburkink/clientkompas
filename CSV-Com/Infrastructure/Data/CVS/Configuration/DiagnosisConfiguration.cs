using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.CVS.Configuration
{
    public class DiagnosisConfiguration : IEntityTypeConfiguration<Diagnosis>, ICVSEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<Diagnosis>());
        }

        public void Configure(EntityTypeBuilder<Diagnosis> builder)
        {
            builder.Property(d => d.Name)
                .HasMaxLength(DiagnosisConstants.NAME_MAXLENGTH)
                .IsRequired();
        }
    }
}
