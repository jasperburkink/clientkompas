using CVSModelPoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVSInfrastructurePoC.Services
{
    public class GebruikerService : IGebruikerService
    {
        private readonly CVSDbContext dbContext;

        public GebruikerService(CVSDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Gebruiker> GetGebruikers()
        {
            return dbContext.Gebruikers.ToList();
        }
    }
}
