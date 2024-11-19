using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Authentication.Configuration
{
    public class TwoTokenConfiguration : IEntityTypeConfiguration<TwoFactorPendingToken>, IAuthenticationEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<TwoFactorPendingToken>());
        }

        public void Configure(EntityTypeBuilder<TwoFactorPendingToken> builder)
        {
            builder.Ignore(rt => rt.IsExpired);
            builder.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            builder.Property(t => t.ExpiresAt).HasColumnName("ExpiresAt");
            builder.Property(t => t.IsUsed).HasColumnName("IsUsed");
            builder.Property(t => t.IsRevoked).HasColumnName("IsRevoked");
        }
    }
}
