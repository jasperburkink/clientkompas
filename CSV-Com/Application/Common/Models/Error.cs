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
            return args.Count() > 0 ? string.Format(Description, args) : Description;
        }

        public Error WithParams(params object[] args)
        {
            return new Error(Code, FormatDescription(args));
        }
    }
}
