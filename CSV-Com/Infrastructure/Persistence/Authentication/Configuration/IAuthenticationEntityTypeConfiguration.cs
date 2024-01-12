using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Authentication.Configuration
{
    public interface IAuthenticationEntityTypeConfiguration
    {
        void Configure(ModelBuilder modelBuilder);
    }
}
