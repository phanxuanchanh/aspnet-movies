using System.Security.Cryptography;
using System.Text;

namespace MediaSrv.ApiKeyGenerator;

public class Helper
{
    public static string GenerateClientId()
    {
        string guid = Guid.NewGuid().ToString();

        byte[] bytes = new byte[12];
        RandomNumberGenerator.Fill(bytes);
        string randomHexStr = Convert.ToHexString(bytes).ToLowerInvariant();

        return $"{guid}-{randomHexStr}";
    }

    public static string GenerateClientSecret()
    {
        byte[] secretBytes = new byte[256];
        RandomNumberGenerator.Fill(secretBytes);
        return Convert.ToBase64String(secretBytes);
    }

    public static string HashSecret(string secret)
    {
        using var sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(secret));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
    }
}
