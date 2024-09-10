using Domain.Authentication.Domain;
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
            builder.Property(u => u.CVSUserId) // TODO: Perhaps this prop must be required when adding the logics for copuling the CVS user to this user. A foreign key is not posible between different contexts.
               .IsRequired(false);
        }
    }
}
