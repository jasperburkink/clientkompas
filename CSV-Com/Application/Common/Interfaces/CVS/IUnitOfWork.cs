using Domain.CVS.Domain;

namespace Application.Common.Interfaces.CVS
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }

        IRepository<Client> ClientRepository { get; }

        IRepository<DriversLicence> DriversLicenceRepository { get; }

        IRepository<MaritalStatus> MaritalStatusRepository { get; }

        IRepository<Diagnosis> DiagnosisRepository { get; }

        IRepository<BenefitForm> BenefitFormRepository { get; }

        IRepository<Organization> OrganizationRepository { get; }

        IRepository<WorkingContract> WorkingContractRepository { get; }

        IRepository<CoachingProgram> CoachingProgramRepository { get; }

        public void Save();

        public Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
