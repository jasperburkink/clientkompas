namespace Application.Common.Models
{
    public class Result : ResultBase
    {
        private Result(bool succeeded, IReadOnlyList<Error>? errors = null)
            : base(succeeded, errors) { }

        public static Result Success()
        {
            return new(true);
        }

        public static Result Failure(IReadOnlyList<Error> errors)
        {
            return new(false, errors);
        }

        public static Result Failure(Error error)
        {
            return new(false, [error]);
        }
    }
}
