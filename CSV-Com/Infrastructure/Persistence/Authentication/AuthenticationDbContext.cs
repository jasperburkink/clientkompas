using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Authentication
{
    public class AuthenticationDbContext : DbContext
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options) { }
    }
}
