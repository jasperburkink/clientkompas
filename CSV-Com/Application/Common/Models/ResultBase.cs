namespace Application.Common.Models
{
    public abstract class ResultBase
    {
        protected ResultBase(bool succeeded, IReadOnlyList<Error>? errors = null)
        {
            if (succeeded && errors is { Count: > 0 })
            {
                throw new ArgumentException("A successful result cannot have errors.", nameof(errors));
            }
            if (!succeeded && (errors == null || errors.Count == 0))
            {
                throw new ArgumentException("A failed result must have at least one error.", nameof(errors));
            }

            Succeeded = succeeded;
            Errors = errors ?? [];
        }

        public bool Succeeded { get; }

        public IReadOnlyList<Error> Errors { get; }

        public string ErrorMessage => string.Join(", ", Errors.Select(error => error.Description));
    }
}
