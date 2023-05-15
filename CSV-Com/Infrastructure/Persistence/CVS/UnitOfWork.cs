using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.CVS
{
    public class UnitOfWork : IUnitOfWork
    {
        private CVSDbContext context;
        private GenericRepository<Gebruiker> gebruikerRepository;

        public UnitOfWork(CVSDbContext context)
        {
            this.context = context;
        }

        public IRepository<Gebruiker> GebruikerRepository
        {
            get
            {

                if (gebruikerRepository == null)
                {
                    gebruikerRepository = new GenericRepository<Gebruiker>(context);
                }
                return gebruikerRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;        

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