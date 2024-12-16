using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Authentication.Configuration
{
    public interface IAuthenticationEntityTypeConfiguration
    {
        void Configure(ModelBuilder modelBuilder);
    }
}
