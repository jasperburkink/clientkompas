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
                .HasForeignKey(wc => wc.OrganizationId);
        }
    }
}
