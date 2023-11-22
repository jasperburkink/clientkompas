namespace Application.Common.Interfaces.Authentication
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
    }
}
