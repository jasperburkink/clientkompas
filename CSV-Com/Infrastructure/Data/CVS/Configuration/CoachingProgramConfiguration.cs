using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.CVS.Configuration
{
    public class CoachingProgramConfiguration : IEntityTypeConfiguration<CoachingProgram>, ICVSEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<CoachingProgram>());
        }

        public void Configure(EntityTypeBuilder<CoachingProgram> builder)
        {
            builder.HasOne(cp => cp.Client)
                .WithMany()
                .HasForeignKey(cp => cp.ClientId)
                .IsRequired();

            builder.Property(cp => cp.Title)
                .HasMaxLength(CoachingProgramConstants.TITLE_MAXLENGTH)
                .IsRequired();

            builder.Property(cp => cp.OrderNumber)
                .HasMaxLength(CoachingProgramConstants.ORDERNUMBER_MAXLENGTH)
                .IsRequired(false);

            builder.HasOne(cp => cp.Organization)
                .WithMany()
                .HasForeignKey(cp => cp.OrganizationId)
                .IsRequired(false);

            builder.Property(cp => cp.CoachingProgramType)
                .IsRequired();

            builder.Property(cp => cp.BeginDate)
                .IsRequired();

            builder.Property(cp => cp.EndDate)
                .IsRequired();

            builder.Property(cp => cp.BudgetAmmount)
                .IsRequired(false);

            builder.Property(cp => cp.HourlyRate)
                .IsRequired();
        }
    }
}
