using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.CVS
{
    public class CVSDbContext : DbContext
    {
        public DbSet<Gebruiker> Gebruikers { get; set; }

        public DbSet<Cliënt> Cliënten { get; set; }

        public CVSDbContext(DbContextOptions<CVSDbContext> options) : base(options) { }
    }
}
