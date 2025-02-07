namespace Domain.Authentication.Domain
{
    public interface IAuthenticationRole
    {
        string Id { get; }
        string Name { get; set; }
    }
}
