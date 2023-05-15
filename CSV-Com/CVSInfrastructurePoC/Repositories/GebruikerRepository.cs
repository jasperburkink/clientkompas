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

        public async Task InsertGebruikerAsync(Gebruiker gebruiker)
        {
            await context.Gebruikers.AddAsync(gebruiker);            
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
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

        public async Task<Gebruiker> GetGebruikerByEmailAsync(string email)
        {
            return await context.Gebruikers.FirstAsync(g => g.Email == email);
        }

        public async Task<Gebruiker> GetGebruikerAsync(int id)
        {
            return await context.Gebruikers.FindAsync(id);
        }

        public async Task UpdateGebruikerAsync(Gebruiker gebruiker)
        {
            await Task.Run(() =>
            {
                context.Gebruikers.Update(gebruiker);
                context.SaveChanges();
            });
            
        }
    }
}
