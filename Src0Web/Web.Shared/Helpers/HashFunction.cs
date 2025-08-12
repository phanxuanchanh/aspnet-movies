using System.Security.Cryptography;
using System.Text;

namespace Common.Hash
{
    public static class HashFunction
    {
        public static string MD5_Hash(string text)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder hashSb = new StringBuilder();
            foreach (byte b in hash)
            {
                hashSb.Append(b.ToString("x2"));
            }
            return hashSb.ToString();
        }

        public static string PBKDF2_Hash(string input, string salt, int length)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, Encoding.ASCII.GetBytes(salt), iterations: 5000);
            StringBuilder hashSb = new StringBuilder();
            byte[] hash = pbkdf2.GetBytes(length);
            foreach (byte b in hash)
            {
                hashSb.Append(b.ToString("x2"));
            }
            return hashSb.ToString();
        }
    }
}
