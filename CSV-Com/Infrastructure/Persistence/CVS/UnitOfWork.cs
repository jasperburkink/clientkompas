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
        private GenericRepository<DriversLicence> driversLicenceRepository;
       

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
        public IRepository<DriversLicence> DriversLicenceRepository
        {
            get
            {
                if (driversLicenceRepository == null)
                {
                    driversLicenceRepository = new GenericRepository<DriversLicence>(context);
                }
                return driversLicenceRepository;
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