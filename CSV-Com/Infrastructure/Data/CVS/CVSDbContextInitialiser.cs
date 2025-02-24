using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
    }
}
