using Domain.CVS.Domain;

namespace Application.Common.Interfaces.CVS
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }

        IRepository<Client> ClientRepository { get; }

        IRepository<Diagnosis> DiagnosisRepository { get; }

        IRepository<EmergencyPerson> EmergencyPersonRepository { get; }

        public void Save();

        public Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
