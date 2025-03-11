namespace Application.Common.Models
{
    public class Result<T> : ResultBase
    {
        private Result(T? value, bool succeeded, IReadOnlyList<Error>? errors = null)
            : base(succeeded, errors)
        {
            Value = value;
        }

        public T? Value { get; }

        public static Result<T> Success(T value)
        {
            return new(value, true);
        }

        public static Result<T> Failure(IReadOnlyList<Error> errors)
        {
            return new(default, false, errors);
        }

        public static Result<T> Failure(Error error)
        {
            return new(default, false, [error]);
        }
    }
}
