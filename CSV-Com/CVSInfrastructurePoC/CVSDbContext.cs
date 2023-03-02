using CVSModelPoC;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace CVSInfrastructurePoC
{

    public class CVSDbContext : DbContext
    {
        public DbSet<Gebruiker> Gebruikers { get; set; }

        public CVSDbContext(DbContextOptions<CVSDbContext> options) : base(options) { }
    }
}