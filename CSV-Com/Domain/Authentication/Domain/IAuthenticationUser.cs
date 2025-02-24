namespace Domain.Authentication.Domain
{
    public interface IAuthenticationUser
    {
        string Id { get; }

        string UserName { get; set; }

        string Email { get; set; }

        bool EmailConfirmed { get; set; }

        bool TwoFactorEnabled { get; set; }

        int CVSUserId { get; set; }

        byte[]? Salt { get; set; }

        bool HasTemporaryPassword { get; set; }

        DateTime? TemporaryPasswordExpiryDate { get; set; }

        int TemporaryPasswordTokenCount { get; set; }
    }
}
