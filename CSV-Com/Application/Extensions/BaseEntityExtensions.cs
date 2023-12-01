using Application.Common.Exceptions;
using Domain.Common;


namespace Application.Extensions
{
    public static class BaseEntityExtensions
    {
        public static void AssertNotNull(this BaseEntity entity)
        {
            if (entity is null)
            {
                throw new NotFoundException(nameof(DriversLicences), entity.Id);
            }
        }
    }
}
