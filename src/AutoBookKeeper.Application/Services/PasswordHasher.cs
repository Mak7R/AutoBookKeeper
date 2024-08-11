using System.Security.Cryptography;
using AutoBookKeeper.Application.Interfaces;

namespace AutoBookKeeper.Application.Services;

public class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 32; 
    private const int KeySize = 64;
    private const int Iterations = 10000;

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);
        
        var hashBytes = new byte[SaltSize + KeySize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, KeySize);

        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        var hashBytes = Convert.FromBase64String(hashedPassword);
        var salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);

        for (int i = 0; i < KeySize; i++)
            if (hashBytes[i + SaltSize] != hash[i])
                return false;

        return true;
    }
}