using System.Security.Cryptography;
using System.Text;
using GameGather.Application.Utils;
using Konscious.Security.Cryptography;

namespace GameGather.Infrastructure.Utils;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int DegreeOfParallelism = 8;
    private const int MemorySize = 1024 * 1024;
    private const int Iterations = 4;

    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;
    
    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = HashPassword(password, salt);
        
        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    public bool Verify(string password, string passwordHash)
    {
        var hashParts = passwordHash.Split('-');
        var hash = Convert.FromHexString(hashParts[0]);
        var salt = Convert.FromHexString(hashParts[1]);
        
        var inputHash = HashPassword(password, salt);
        
        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }
    
    private byte[] HashPassword(string password, byte[] salt)
    {
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            Iterations = Iterations,
            MemorySize = MemorySize
        };

        return argon2.GetBytes(KeySize);
    }
}