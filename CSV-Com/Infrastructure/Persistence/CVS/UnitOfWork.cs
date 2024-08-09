using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;

namespace Infrastructure.Persistence.CVS
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CVSDbContext _context;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private GenericRepository<User> userRepository;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private GenericRepository<Client> clientRepository;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private GenericRepository<DriversLicence> driversLicenceRepository;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private GenericRepository<Diagnosis> diagnosisRepository;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private GenericRepository<MaritalStatus> maritalStatusRepository;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private GenericRepository<BenefitForm> benefitFormRepository;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private GenericRepository<Organization> organizationRepository;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private GenericRepository<WorkingContract> workingContractRepository;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private GenericRepository<CoachingProgram> coachingProgramRepository;

        public UnitOfWork(CVSDbContext context)
        {
            _context = context;
        }

        public IRepository<User> UserRepository
        {
            get
            {

                userRepository ??= new GenericRepository<User>(_context);
                return userRepository;
            }
        }

        public IRepository<Diagnosis> DiagnosisRepository
        {
            get
            {

                diagnosisRepository ??= new GenericRepository<Diagnosis>(_context);
                return diagnosisRepository;
            }
        }

        public IRepository<Client> ClientRepository
        {
            get
            {
                clientRepository ??= new GenericRepository<Client>(_context);
                return clientRepository;
            }
        }

        public IRepository<DriversLicence> DriversLicenceRepository
        {
            get
            {
                driversLicenceRepository ??= new GenericRepository<DriversLicence>(_context);
                return driversLicenceRepository;
            }
        }

        public IRepository<MaritalStatus> MaritalStatusRepository
        {
            get
            {
                maritalStatusRepository ??= new GenericRepository<MaritalStatus>(_context);
                return maritalStatusRepository;
            }
        }

        public IRepository<BenefitForm> BenefitFormRepository
        {
            get
            {
                benefitFormRepository ??= new GenericRepository<BenefitForm>(_context);
                return benefitFormRepository;
            }
        }

        public IRepository<Organization> OrganizationRepository
        {
            get
            {

                organizationRepository ??= new GenericRepository<Organization>(_context);
                return organizationRepository;
            }
        }

        public IRepository<WorkingContract> WorkingContractRepository
        {
            get
            {

                workingContractRepository ??= new GenericRepository<WorkingContract>(_context);
                return workingContractRepository;
            }
        }

        public IRepository<CoachingProgram> CoachingProgramRepository
        {
            get
            {
                coachingProgramRepository ??= new GenericRepository<CoachingProgram>(_context);
                return coachingProgramRepository;
            }
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
