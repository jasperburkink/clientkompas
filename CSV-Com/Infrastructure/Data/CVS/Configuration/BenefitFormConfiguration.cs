using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.CVS.Configuration
{
    public class BenefitFormConfiguration : IEntityTypeConfiguration<BenefitForm>, ICVSEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<BenefitForm>());
        }

        public void Configure(EntityTypeBuilder<BenefitForm> builder)
        {
            builder.Property(bf => bf.Name)
                .HasMaxLength(BenefitFormConstants.NAME_MAXLENGTH)
                .IsRequired();
        }
    }
}
