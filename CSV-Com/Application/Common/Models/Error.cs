namespace Application.Common.Models
{
    public sealed record Error(string Code, string Description, params object[] Objects)
    {
        public static implicit operator Result(Error error)
        {
            return Result.Failure(error);
        }

        public string FormatDescription(params object[] args)
        {
            return string.Format(Description, args);
        }

        public Error WithParams(params object[] args)
        {
            return new Error(Code, FormatDescription(args));
        }
    }
}
