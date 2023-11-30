using Domain.CVS.Domain;

namespace Application.Common.Interfaces.CVS
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }

        IRepository<Client> ClientRepository { get; }

        IRepository<MaritalStatus> MaritalStatusRepository { get; }

        IRepository<Diagnosis> DiagnosisRepository { get; }

        public void Save();

        public Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
