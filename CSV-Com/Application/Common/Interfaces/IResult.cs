namespace Application.Common.Interfaces
{
    public interface IResult
    {
        bool Succeeded { get; }

        string[] Errors { get; }
    }

}
