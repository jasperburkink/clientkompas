using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public static class ContextExtensions
    {
        public static string GetTableName(DbContext dbContext, Type entityType)
        {
            var dbEntityType = dbContext.Model.FindEntityType(entityType);

            if (dbEntityType == null)
            {
                throw new NullReferenceException(nameof(dbEntityType));
            }

            var tableName = dbEntityType.GetTableName();
            return tableName;
        }
    }
}
