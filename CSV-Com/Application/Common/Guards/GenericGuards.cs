using Application.Common.Models;
using Ardalis.GuardClauses;

namespace Application.Common.Guards
{
    public static class GenericGuards
    {
        public static readonly Error GuardNotNull = new($"{nameof(GenericGuards)}.{nameof(GuardNotNull)}", "{0} mag niet null zijn.");

        public static Result<T> NotNull<T>(this IGuardClause guardClause, T? value) where T : class
        {
            if (value is null)
            {
                return Result<T>.Failure(GuardNotNull.WithParams(typeof(T).Name));
            }

            return Result<T>.Success(value);
        }
    }
}
