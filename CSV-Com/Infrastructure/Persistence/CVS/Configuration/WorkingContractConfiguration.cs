using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.CVS.Configuration
{
    public class WorkingContractConfiguration : IEntityTypeConfiguration<WorkingContract>, ICVSEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<WorkingContract>());
        }

        public void Configure(EntityTypeBuilder<WorkingContract> builder)
        {
            builder.HasOne(wc => wc.Organization)
                .WithMany()
                .HasForeignKey(wc => wc.OrganizationId)
                .IsRequired();

            builder.Property(wc => wc.Function)
                .HasMaxLength(WorkingContractConstants.FUNCTION_MAXLENGTH)
                .IsRequired();

            builder.Property(wc => wc.ContractType)
                .IsRequired();

            builder.Property(wc => wc.FromDate)
                .IsRequired();

            builder.Property(wc => wc.ToDate)
                .IsRequired();
        }
    }
}
