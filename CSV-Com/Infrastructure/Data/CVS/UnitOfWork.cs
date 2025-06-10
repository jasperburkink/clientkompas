using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;

namespace Infrastructure.Data.CVS
{
    public class UnitOfWork(CVSDbContext context) : IUnitOfWork
    {
        private GenericRepository<User> _userRepository;

        private GenericRepository<Client> _clientRepository;

        private GenericRepository<DriversLicence> _driversLicenceRepository;

        private GenericRepository<Diagnosis> _diagnosisRepository;

        private GenericRepository<MaritalStatus> _maritalStatusRepository;

        private GenericRepository<BenefitForm> _benefitFormRepository;

        private GenericRepository<Organization> _organizationRepository;

        private GenericRepository<WorkingContract> _workingContractRepository;

        private GenericRepository<CoachingProgram> _coachingProgramRepository;

        private GenericRepository<License> _licenceRepository;

        public IRepository<User> UserRepository
        {
            get => _userRepository ??= new GenericRepository<User>(context);
        }

        public IRepository<Diagnosis> DiagnosisRepository
        {
            get => _diagnosisRepository ??= new GenericRepository<Diagnosis>(context);
        }

        public IRepository<Client> ClientRepository
        {
            get => _clientRepository ??= new GenericRepository<Client>(context);
        }

        public IRepository<DriversLicence> DriversLicenceRepository
        {
            get => _driversLicenceRepository ??= new GenericRepository<DriversLicence>(context);
        }

        public IRepository<MaritalStatus> MaritalStatusRepository
        {
            get => _maritalStatusRepository ??= new GenericRepository<MaritalStatus>(context);
        }

        public IRepository<BenefitForm> BenefitFormRepository
        {
            get => _benefitFormRepository ??= new GenericRepository<BenefitForm>(context);
        }

        public IRepository<Organization> OrganizationRepository
        {
            get => _organizationRepository ??= new GenericRepository<Organization>(context);
        }

        public IRepository<WorkingContract> WorkingContractRepository
        {
            get => _workingContractRepository ??= new GenericRepository<WorkingContract>(context);
        }

        public IRepository<CoachingProgram> CoachingProgramRepository
        {
            get => _coachingProgramRepository ??= new GenericRepository<CoachingProgram>(context);
        }

        public IRepository<License> LicenseRepository
        {
            get => _licenceRepository ??= new GenericRepository<License>(context);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
