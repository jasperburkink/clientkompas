using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Authentication.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>, IAuthenticationEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<RefreshToken>());
        }

        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.Ignore(rt => rt.IsExpired);
            builder.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            builder.Property(t => t.ExpiresAt).HasColumnName("ExpiresAt");
            builder.Property(t => t.IsUsed).HasColumnName("IsUsed");
            builder.Property(t => t.IsRevoked).HasColumnName("IsRevoked");
        }
    }
}
