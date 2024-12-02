namespace Application.Common.Interfaces.Authentication
{
    public interface IHasher
    {
        string HashString(string stringValue, byte[] salt);

        byte[] GenerateSalt(int size = 16);
    }
}
