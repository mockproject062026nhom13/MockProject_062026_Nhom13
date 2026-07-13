using BCrypt.Net;
namespace NursingHome.Application.Abstractions;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        // Implement your hashing logic here
        // For example, you can use a library like BCrypt or PBKDF2
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string hash)
    {
        // Implement your verification logic here
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}