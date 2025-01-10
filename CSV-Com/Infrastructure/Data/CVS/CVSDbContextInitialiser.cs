using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestData;
using TestData.Client;
using TestData.CoachingProgram;

namespace Infrastructure.Data.CVS
{
    public class CVSDbContextInitialiser(ILogger<CVSDbContextInitialiser> logger, CVSDbContext context)
    {
        private const int INITIAL_NUMBER_OF_CLIENTS = 10, INITIAL_NUMBER_OF_COACHINGPROGRAMS = 2;

        public async Task InitialiseAsync()
        {
            try
            {
                if (context.Database.IsMySql())
                {
                    await context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initialising the database.");
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
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
        public async Task TrySeedAsync()
        {
            // Default data
            // Seed, if necessary
            // TODO: Maybe only when debugging
            if (!context.Clients.Any())
            {
                ITestDataGenerator<Client> testDataGeneratorClient = new ClientDataGenerator();
                ITestDataGenerator<CoachingProgram> testDataGeneratorCoachingProgram = new CoachingProgramDataGenerator();

                for (var i = 0; i < INITIAL_NUMBER_OF_CLIENTS; i++)
                {
                    var client = testDataGeneratorClient.Create();
                    context.Clients.Add(client);
                }

                await context.SaveChangesAsync();

                var clients = context.Clients.ToList();

                foreach (var client in clients)
                {
                    for (var j = 0; j < INITIAL_NUMBER_OF_COACHINGPROGRAMS; j++)
                    {
                        var coachingProgram = testDataGeneratorCoachingProgram.Create();
                        coachingProgram.ClientId = client.Id;
                        coachingProgram.Client = null;
                        context.CoachingPrograms.Add(coachingProgram);
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
