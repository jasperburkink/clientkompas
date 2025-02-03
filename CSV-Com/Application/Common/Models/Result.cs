using Application.Common.Interfaces;

namespace Application.Common.Models
{
    public class Result : IResult
    {
        private Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        public bool Succeeded { get; }
        public string[] Errors { get; }

        public static Result Success()
        {
            return new Result(true, []);
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }

        public static Result Failure(string error)
        {
            return new Result(false, [error]);
        }

        public static Result<T> Success<T>(T value)
        {
            return Result<T>.Success(value);
        }
    }

    public class Result<T> : IResult
    {
        private Result(T? value, bool succeeded, IEnumerable<string> errors)
        {
            Value = value;
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        public T? Value { get; }
        public bool Succeeded { get; }
        public string[] Errors { get; }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value, true, []);
        }

        public static Result<T> Failure(IEnumerable<string> errors)
        {
            return new Result<T>(default, false, errors);
        }

        public static Result<T> Failure(string error)
        {
            return new Result<T>(default, false, [error]);
        }
    }

}
