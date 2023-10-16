using Domain.CVS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.CVS
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }

        IRepository<Client> ClientRepository { get; }

        IRepository<BenefitForm> BenefitFormRepository { get; }
        
        public void Save();

        public Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
