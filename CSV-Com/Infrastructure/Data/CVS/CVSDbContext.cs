using Domain.CVS.Domain;
using Infrastructure.Data.CVS.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.CVS
{
    public class CVSDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<DriversLicence> DriversLicence { get; set; }

        public DbSet<Diagnosis> Diagnosis { get; set; }

        public DbSet<MaritalStatus> MaritalStatus { get; set; }

        public DbSet<BenefitForm> BenefitForm { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<CoachingProgram> CoachingPrograms { get; set; }

        public CVSDbContext(DbContextOptions<CVSDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var configurations = GetType().Assembly.GetTypes()
                .Where(type => type.GetInterfaces().Any(interfaceType =>
                    interfaceType == typeof(ICVSEntityTypeConfiguration)))
                .Select(type => Activator.CreateInstance(type) as ICVSEntityTypeConfiguration);

            foreach (var configuration in configurations)
            {
                configuration!.Configure(builder);
            }

            base.OnModelCreating(builder);
        }
    }
}
