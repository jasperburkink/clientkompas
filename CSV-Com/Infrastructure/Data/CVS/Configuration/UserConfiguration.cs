using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.CVS.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>, ICVSEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<User>());
        }

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FirstName)
                .HasMaxLength(UserConstants.FirstNameMaxLength)
                .IsRequired();

            builder.Property(u => u.PrefixLastName)
                .HasMaxLength(UserConstants.PrefixLastNameMaxLength)
                .IsRequired(false);

            builder.Property(u => u.LastName)
                .HasMaxLength(UserConstants.LastNameMaxLength)
                .IsRequired();

            builder.Property(c => c.FullName)
                .HasMaxLength(UserConstants.FullnameMaxLength);

            builder.HasIndex(c => c.FullName)
                .IsFullText();

            builder.Property(u => u.EmailAddress)
                .HasMaxLength(UserConstants.EmailAddressMaxLength)
                .IsRequired();

            builder.Property(u => u.TelephoneNumber)
                .HasMaxLength(UserConstants.TelephoneNumberMaxLength)
                .IsRequired();

            builder.Property(u => u.DeactivationDateTime)
                .IsRequired(false);

            builder.HasOne(u => u.CreatedByUser)
                .WithMany()
                .HasForeignKey(u => u.CreatedByUserId)
                .IsRequired(false);
        }
    }
}
