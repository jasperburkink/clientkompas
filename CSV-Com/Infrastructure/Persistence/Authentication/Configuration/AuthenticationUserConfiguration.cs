using Domain.Authentication.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Authentication.Configuration
{
    public class AuthenticationUserConfiguration : IEntityTypeConfiguration<AuthenticationUser>, IAuthenticationEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<AuthenticationUser>());
        }

        public void Configure(EntityTypeBuilder<AuthenticationUser> builder)
        {
            // NOTE: Don't use keys configurations etc. which you can solve with EF conventions. https://youtu.be/dK4Yb6-LxAk?t=2210

            /*
             * TODO: Maybe these foreign keys can be solved with conventions. Also maybe cascade delete is not wanted.
             * But it is needed in the current situation, because there are no repositories for diagnoses, driverslicences, etc.
             */
            //builder.HasKey(u => u.CVSUserId)
            //        .WithMany(dl => dl.Clients);
        }
    }
}
