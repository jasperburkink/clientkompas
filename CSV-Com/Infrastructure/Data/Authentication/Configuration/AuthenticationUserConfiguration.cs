using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Authentication.Configuration
{
    public class AuthenticationUserConfiguration : IEntityTypeConfiguration<AuthenticationUser>, IAuthenticationEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<AuthenticationUser>());
        }

        public void Configure(EntityTypeBuilder<AuthenticationUser> builder)
        {
            builder.Property(u => u.CVSUserId)
               .IsRequired(true);

            builder.Property(u => u.Salt)
               .IsRequired(false);

            builder.Property(u => u.HasTemporaryPassword)
                .IsRequired(true);

            builder.Property(u => u.TemporaryPasswordExpiryDate)
                .IsRequired(false);

            builder.Property(u => u.TemporaryPasswordTokenCount)
                .IsRequired(true);
        }
    }
}
