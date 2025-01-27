namespace Application.Common.Interfaces.Authentication
{
    public interface IPasswordService
    {
        string GeneratePassword(int length);

        bool IsValidPassword(string password);
    }
}
