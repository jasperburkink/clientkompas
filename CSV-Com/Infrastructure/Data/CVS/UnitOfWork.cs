using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;

namespace Infrastructure.Data.CVS
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CVSDbContext _context;

        private GenericRepository<User> _userRepository;

        private GenericRepository<Client> _clientRepository;

        private GenericRepository<DriversLicence> _driversLicenceRepository;

        private GenericRepository<Diagnosis> _diagnosisRepository;

        private GenericRepository<MaritalStatus> _maritalStatusRepository;

        private GenericRepository<BenefitForm> _benefitFormRepository;

        private GenericRepository<Organization> _organizationRepository;

        private GenericRepository<WorkingContract> _workingContractRepository;

        private GenericRepository<CoachingProgram> _coachingProgramRepository;

        public UnitOfWork(CVSDbContext context)
        {
            _context = context;
        }

        public IRepository<User> UserRepository
        {
            get => _userRepository ??= new GenericRepository<User>(_context);
        }

        public IRepository<Diagnosis> DiagnosisRepository
        {
            get => _diagnosisRepository ??= new GenericRepository<Diagnosis>(_context);
        }

        public IRepository<Client> ClientRepository
        {
            get => _clientRepository ??= new GenericRepository<Client>(_context);
        }

        public IRepository<DriversLicence> DriversLicenceRepository
        {
            get => _driversLicenceRepository ??= new GenericRepository<DriversLicence>(_context);
        }

        public IRepository<MaritalStatus> MaritalStatusRepository
        {
            get => _maritalStatusRepository ??= new GenericRepository<MaritalStatus>(_context);
        }

        public IRepository<BenefitForm> BenefitFormRepository
        {
            get => _benefitFormRepository ??= new GenericRepository<BenefitForm>(_context);
        }

        public IRepository<Organization> OrganizationRepository
        {
            get => _organizationRepository ??= new GenericRepository<Organization>(_context);
        }

        public IRepository<WorkingContract> WorkingContractRepository
        {
            get => _workingContractRepository ??= new GenericRepository<WorkingContract>(_context);
        }

        public IRepository<CoachingProgram> CoachingProgramRepository
        {
            get => _coachingProgramRepository ??= new GenericRepository<CoachingProgram>(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
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
