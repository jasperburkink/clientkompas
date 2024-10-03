namespace Application.Common.Interfaces.Authentication
{
    public interface IHasher
    {
        string HashPassword(string password, byte[] salt);

        byte[] GenerateSalt(int size = 16);
    }
}
