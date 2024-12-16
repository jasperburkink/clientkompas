using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestData;
using TestData.Client;
using TestData.CoachingProgram;

namespace Infrastructure.Data.CVS
{
    public class CVSDbContextInitialiser
    {
        private const int INITIAL_NUMBER_OF_CLIENTS = 10, INITIAL_NUMBER_OF_COACHINGPROGRAMS = 2;
        private readonly ILogger<CVSDbContextInitialiser> _logger;
        private readonly CVSDbContext _context;
        public CVSDbContextInitialiser(ILogger<CVSDbContextInitialiser> logger, CVSDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsMySql())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }
        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
        public async Task TrySeedAsync()
        {
            // Default data
            // Seed, if necessary
            // TODO: Maybe only when debugging
            if (!_context.Clients.Any())
            {
                ITestDataGenerator<Client> testDataGeneratorClient = new ClientDataGenerator();
                ITestDataGenerator<CoachingProgram> testDataGeneratorCoachingProgram = new CoachingProgramDataGenerator();

                for (var i = 0; i < INITIAL_NUMBER_OF_CLIENTS; i++)
                {
                    var client = testDataGeneratorClient.Create();
                    _context.Clients.Add(client);
                }

                await _context.SaveChangesAsync();

                var clients = _context.Clients.ToList();

                foreach (var client in clients)
                {
                    for (var j = 0; j < INITIAL_NUMBER_OF_COACHINGPROGRAMS; j++)
                    {
                        var coachingProgram = testDataGeneratorCoachingProgram.Create();
                        coachingProgram.ClientId = client.Id;
                        coachingProgram.Client = null;
                        _context.CoachingPrograms.Add(coachingProgram);
                    }
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
