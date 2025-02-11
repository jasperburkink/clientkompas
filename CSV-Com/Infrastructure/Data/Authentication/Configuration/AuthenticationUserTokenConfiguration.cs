using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Authentication.Configuration
{
    public class AuthenticationUserTokenConfiguration : IEntityTypeConfiguration<AuthenticationUserToken>, IAuthenticationEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<AuthenticationUserToken>());
        }

        public void Configure(EntityTypeBuilder<AuthenticationUserToken> builder)
        {
            builder.HasKey(rt => new { rt.UserId, rt.LoginProvider, rt.Name });

            builder.Ignore(rt => rt.IsExpired);

            builder.Property(t => t.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired(true);

            builder.Property(t => t.ExpiresAt)
                .HasColumnName("ExpiresAt")
                .IsRequired(true);

            builder.Property(t => t.IsUsed)
                .HasColumnName("IsUsed")
                .IsRequired(true);

            builder.Property(t => t.IsRevoked)
                .HasColumnName("IsRevoked")
                .IsRequired(true);

            builder.HasDiscriminator<string>("Discriminator")
                .HasValue<RefreshToken>(nameof(RefreshToken))
                .HasValue<TemporaryPasswordToken>(nameof(TemporaryPasswordToken))
                .HasValue<TwoFactorPendingToken>(nameof(TwoFactorPendingToken));
        }
    }
}
