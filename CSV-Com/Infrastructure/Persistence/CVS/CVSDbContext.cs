using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            modelBuilder.Entity<Client>().HasMany(c => c.Diagnoses)
                .WithOne(d => d.Client)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>().HasMany(c => c.DriversLicences)
                .WithOne(dl => dl.Client)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>().HasMany(c => c.EmergencyPeople)
                .WithOne(ep => ep.Client)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>().HasMany(c => c.WorkingContracts)
                .WithOne(wc => wc.Client)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
