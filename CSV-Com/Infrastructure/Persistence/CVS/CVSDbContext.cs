using System.Reflection;
using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.CVS
{
    public class CVSDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Client> Clients { get; set; }
        public DbSet<DriversLicence> DriversLicence { get; set; }

        public DbSet<Diagnosis> Diagnosis { get; set; }

        public CVSDbContext(DbContextOptions<CVSDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Execute configurations
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
