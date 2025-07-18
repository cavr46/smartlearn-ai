using BCrypt.Net;
using SmartLearn.Application.Interfaces;

namespace SmartLearn.Infrastructure.Services;

public class PasswordHasherService : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}