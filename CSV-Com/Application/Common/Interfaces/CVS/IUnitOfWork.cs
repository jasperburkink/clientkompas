using Domain.CVS.Domain;

namespace Application.Common.Interfaces.CVS
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }

        IRepository<Client> ClientRepository { get; }

        public void Save();

        public Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
