using CVSModelPoC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVSInfrastructurePoC.Repositories
{
    public class GebruikerRepository : IGebruikerRepository, IDisposable
    {
        private CVSDbContext context;
        private bool disposed = false;

        public GebruikerRepository(CVSDbContext context)
        {
            this.context = context;
        }

        public Task<List<Gebruiker>> GetGebruikersAsync()
        {
            return context.Gebruikers.ToListAsync();
        }

        public Task InsertGebruikerAsync(Gebruiker gebruiker)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
