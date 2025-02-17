namespace Domain.Authentication.Domain
{
    public interface IAuthenticationUserRole
    {
        string UserId { get; }

        string RoleId { get; }
    }
}
