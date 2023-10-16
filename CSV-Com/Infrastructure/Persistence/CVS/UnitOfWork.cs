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
        private GenericRepository<User> userRepository;
        private GenericRepository<Client> clientRepository;
        private GenericRepository<BenefitForm> benefitFormRepository;
        

        public UnitOfWork(CVSDbContext context)
        {
            this.context = context;
        }

        public IRepository<User> UserRepository
        {
            get
            {

                if (userRepository == null)
                {
                    userRepository = new GenericRepository<User>(context);
                }
                return userRepository;
            }
        }

        public IRepository<Client> ClientRepository
        {
            get
            {
                if (clientRepository == null)
                {
                    clientRepository = new GenericRepository<Client>(context);
                }
                return clientRepository;
            }
        }

        public IRepository<BenefitForm> BenefitFormRepository
        {
            get
            {
                if (benefitFormRepository == null)
                {
                    benefitFormRepository = new GenericRepository<BenefitForm>(context);
                }
                return benefitFormRepository;
            }
        }

        

        public void Save()
        {
            context.SaveChanges();
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
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