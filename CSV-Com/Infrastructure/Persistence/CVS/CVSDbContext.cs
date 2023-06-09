using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.CVS
{
    public class CVSDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Client> Clients { get; set; }

        public CVSDbContext(DbContextOptions<CVSDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
     .Entity<Client>()
     .Property(e => e.DriversLicence)
     .HasConversion(
         v => string.Join(",", v.Select(e => e.ToString("D")).ToArray()),
         v => v.Split(new[] { ',' })
           .Select(e => Enum.Parse(typeof(DriversLicence), e))
           .Cast<DriversLicence>()
           .ToList());

        //    modelBuilder.Entity<Client>()
        //.Property(e => e.DriversLicence)
        //.HasConversion(
        //    v => string.Join(',', v),
        //    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<DriversLicence>())
        //.Metadata.SetValueComparer(valueComparer);


            //        modelBuilder
            //.Entity<Client>()
            //.Property(c => c.DriversLicence)
            //.HasConversion(
            //  v => v.ToString(),
            //  v => (DriversLicence)Enum.Parse(typeof(DriversLicence), v));
        }
    }
}
