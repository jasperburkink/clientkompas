using Application.Common.Models;
using Ardalis.GuardClauses;

namespace Application.Common.Guards
{
    public static class GenericGuards
    {
        public static Result<T> NotNull<T>(this IGuardClause guardClause, T? value, string? errorMessage = null) where T : class
        {
            if (value is null)
            {
                var defaultMessage = $"{typeof(T).Name} cannot be null.";
                return Result<T>.Failure(errorMessage ?? defaultMessage);
            }

            return Result<T>.Success(value);
        }
    }
}
